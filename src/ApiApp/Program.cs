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
app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    // Additional development-specific configurations can go here
}

app.UseHttpsRedirection();

// Redirect root "/" to Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

// Map the TodoService endpoints as "/todos/*"
ApiApp.Services.TodoService.Map("/todos", app);

app.Run();

