using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.BusinessLogic;
using Amazon.CognitoIdentityProvider;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using crombie_ecommerce.BusinessLogic;


var builder = WebApplication.CreateBuilder(args);

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
    .AddScoped<OrderDetailsService>()
    .AddScoped<OrderService>()
    .AddScoped<CognitoAuthService>()
    .AddScoped<s3Service>()
    .AddScoped<NotificationService>()
    .AddScoped<CartService>();


builder.Services.AddSqlServer<ShopContext>(builder.Configuration["ConnectionString"]);

/*builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var cognitoIssuer = $"https://cognito-idp.{builder.Configuration["AWS:Region"]}.amazonaws.com/{builder.Configuration["AWS:UserPoolId"]}";

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = cognitoIssuer,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["AWS:AppClientId"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

});*/

builder.Services.AddAuthorization();
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
            OnTokenValidated = context =>
            {
                var claims = context.Principal.Claims;
                var clientIdClaim = claims.FirstOrDefault(c => c.Type == "client_id")?.Value;
                if (string.IsNullOrEmpty(clientIdClaim))
                {
                    Console.WriteLine("Missing client_id claim");
                    context.Fail("Missing client_id claim");
                }

                if (clientIdClaim != builder.Configuration["AWS:ClientId"])
                {
                    context.Fail("Invalid client_id");
                }

                return Task.CompletedTask;
            }
        };
    });





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ShopContext>();

if (dbContext.Database.CanConnect())
    dbContext.Database.EnsureCreated();
else
    Console.WriteLine("Couldn't connect to database");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
