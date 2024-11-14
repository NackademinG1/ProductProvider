using ProductProvider.Business.Interfaces;
using ProductProvider.Business.Services;
using ProductProvider.Data.Interfaces;
using ProductProvider.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Register services
var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
var filePath = Path.Combine(baseDirectory, "products.json");

builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IFileService>(new FileService(filePath));

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS policy
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
