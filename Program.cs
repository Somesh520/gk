var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🛑 IMPORTANT: Maine ye line hata di hai taaki HTTPS ka error na aaye
// app.UseHttpsRedirection(); 

app.UseAuthorization();

app.MapControllers();

// Server start message
Console.WriteLine("🚀 Server is starting...");

app.Run();