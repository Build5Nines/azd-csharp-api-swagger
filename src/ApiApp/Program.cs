var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

// Add a singleton service to manage TODO items
builder.Services.AddSingleton<ApiApp.Services.TodoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles(); // Serve static files
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
    options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});
app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    // Additional development-specific configurations can go here
}

app.UseHttpsRedirection();


// Map the TodoService endpoints as "/todos/*"
ApiApp.Services.TodoService.Map("/todos", app);

app.Run();

