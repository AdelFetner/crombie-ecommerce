using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.BusinessLogic;
using Amazon.CognitoIdentityProvider;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Amazon;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Formatting.Compact;
using CrombieEcommerce.Middleware;
using Serilog.Events;
using Microsoft.AspNetCore.Components;


var builder = WebApplication.CreateBuilder(args);

// logic for log 

builder.Host.UseSerilog((context, config) =>
{
    config
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Async(async =>
        {
            async.Console(
                new CompactJsonFormatter(),
                restrictedToMinimumLevel: LogEventLevel.Warning
            );
        });
});

// Add services to the container.

builder.Services.AddControllers()
 .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<WishlistService>()
    .AddScoped<ProductService>()
    .AddScoped<BrandService>()
    .AddScoped<TagService>()
    .AddScoped<UserService>()
    .AddScoped<CategoryService>()
    .AddScoped<OrderDetailService>()
    .AddScoped<OrderService>()
    .AddScoped<CognitoAuthService>()
    .AddScoped<s3Service>()
    .AddScoped<NotificationService>()
    .AddScoped<StockService>()
    .AddScoped<RoleService>();

builder.Services.AddSqlServer<ShopContext>(builder.Configuration["ConnectionString"]);

builder.Services.AddScoped<CognitoAuthService, CognitoAuthService>();


//aws cognito
builder.Services.AddSingleton<IAmazonCognitoIdentityProvider, AmazonCognitoIdentityProviderClient>(sp=>
{
    var config = new AmazonCognitoIdentityProviderConfig
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(builder.Configuration["AWS:Region"])
    };
    return new AmazonCognitoIdentityProviderClient(
        builder.Configuration["AWS:AccessKey"],
        builder.Configuration["Aws:SecretKey"],
        config
    );
});
builder.Services.AddSingleton<CognitoUserPool>(sp =>
{
    var provider = sp.GetRequiredService<IAmazonCognitoIdentityProvider>();
    return new CognitoUserPool(
        builder.Configuration["AWS:UserPoolId"],
        builder.Configuration["AWS:ClientId"],
        provider
    );
}
);

builder.Services.AddAWSService<IAmazonCognitoIdentityProvider>();
builder.Services.AddSingleton<CognitoUserPool>(provider =>
{
    var clientId = builder.Configuration["AWS:ClientId"];
    var userPoolId = builder.Configuration["AWS:UserPoolId"];
    var providerService = provider.GetRequiredService<IAmazonCognitoIdentityProvider>();
    return new CognitoUserPool(userPoolId, clientId, providerService);
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://cognito-idp.{builder.Configuration["AWS:Region"]}.amazonaws.com/{builder.Configuration["AWS:UserPoolId"]}";
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://cognito-idp.{builder.Configuration["AWS:Region"]}.amazonaws.com/{builder.Configuration["AWS:UserPoolId"]}",
            ValidateAudience = false, // Disable default audience validation
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var claims = context.Principal.Claims;
                var clientIdClaim = claims.FirstOrDefault(c => c.Type == "client_id")?.Value;

                if (string.IsNullOrEmpty(clientIdClaim))
                {
                    Console.WriteLine("Missing client_id claim");
                    context.Fail("Missing client_id claim");
                    await Task.CompletedTask;
                    return;
                }

                if (clientIdClaim != builder.Configuration["AWS:ClientId"])
                {
                    context.Fail("Invalid client_id");
                    return;
                }

                await Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options=>
{
    options.AddPolicy("RequiresAutgenticatedUser", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddScoped<CognitoAuthService, CognitoAuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Crombie-ecommerce", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Use JWT token in this format: Bearer <token>",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference =new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});




var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// middleware for exception handling
app.UseMiddleware<ExceptionMiddleware>();

// Replace sync database check with async:
await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ShopContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging(options =>
{
    // This logs only responses with status code 400 and higher, E,G: not found, unauthorized. And logs requests that took + 2secs
    options.GetLevel = (ctx, elapsed, exception) =>
    {
        if (exception != null) return LogEventLevel.Error;
        if (ctx.Response.StatusCode >= 400) return LogEventLevel.Warning;
        return LogEventLevel.Information;
    };

    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("http.route", httpContext.GetEndpoint()?.Metadata.GetMetadata<RouteAttribute>()?.Template);
        diagnosticContext.Set("http.client_ip", httpContext.Connection.RemoteIpAddress?.ToString());
        diagnosticContext.Set("http.user_agent", httpContext.Request.Headers.UserAgent);
        diagnosticContext.Set("error.type", httpContext.Response.StatusCode >= 500 ? "server_error" : "client_error");
    };
});

app.MapControllers();

app.Run();
