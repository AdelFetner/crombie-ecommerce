using crombie_ecommerce.Contexts;
using crombie_ecommerce.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;


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
    .AddScoped<AuthService>();

builder.Services.AddSqlServer<ShopContext>(builder.Configuration["ConnectionString"]);




builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SECRET"])),
        ValidIssuer = builder.Configuration["JWT:ISSUER"],
        ValidAudience = builder.Configuration["JWT:AUDIENCE"],
        RoleClaimType = "Role",
        ClockSkew = TimeSpan.Zero,

    };

});

builder.Services.AddAuthorization();


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
