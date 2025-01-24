using Amazon.CognitoIdentityProvider;
using crombie_ecommerce.Contexts;
using crombie_ecommerce.Services;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;


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
    .AddScoped<TagsService>()
    .AddScoped<UserService>()
    .AddScoped<CategoryService>()
    .AddScoped<OrderDetailsService>()
    .AddScoped<OrderService>()
    .AddScoped<CognitoAuthService>();

builder.Services.AddSqlServer<ShopContext>(builder.Configuration["ConnectionString"]);

builder.Services.AddAuthentication(options =>
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

app.UseAuthorization();

app.MapControllers();

app.Run();
