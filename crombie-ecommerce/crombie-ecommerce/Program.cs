using crombie_ecommerce.Contexts;
using crombie_ecommerce.Services;
<<<<<<< HEAD
using System.Text.Json.Serialization;
=======
>>>>>>> 141dfa9bf68ed9a9243d0405caf993792e57ee00

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
builder.Services.AddScoped<ProductService>();
builder.Services.AddSqlServer<ShopContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
<<<<<<< HEAD
builder.Services.AddScoped<UserService>();
=======
builder.Services.AddScoped<WishlistService>();
>>>>>>> 141dfa9bf68ed9a9243d0405caf993792e57ee00

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
