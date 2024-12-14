var builder = WebApplication.CreateBuilder(args);


builder.WebHost.UseUrls("http://*:80");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

// Commented out for the Task's purpose as there's no SSL license

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
