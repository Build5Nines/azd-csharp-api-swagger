namespace ApiApp.Services;

using ApiApp.Models;

public class TodoService
{
    /// <summary>
    ///  Maps the TodoService endpoints to the specified path in the WebApplication.
    ///  This method is used to define the API endpoints for managing TODO items.
    ///  It includes endpoints for getting all TODO items, getting a TODO item by ID,
    /// </summary>
    /// <param name="path">The base path for the endpoints.</param>
    /// <param name="app"></param>
    public static void Map(string path, WebApplication app){
        app.MapGet(path, (TodoService service) => service.GetAllTodos())
        .WithName("GetTodos")
        .WithDescription("Get all TODO items");

        app.MapGet("{path}/{id}", (TodoService service, int id) =>
        {
            var todo = service.GetTodoById(id);
            return todo is not null ? Results.Ok(todo) : Results.NotFound();
        }).WithName("GetTodoById")
        .WithDescription("Get a TODO item by ID");

        app.MapPost(path, (TodoService service, Todo todo) =>
        {
            service.AddTodo(todo);
            return Results.Created($"/todos/{todo.Id}", todo);
        }).WithName("CreateTodo")
        .WithDescription("Create a new TODO item");

        app.MapPut("{path}/{id}", (TodoService service, int id, Todo updatedTodo) =>
        {
            var result = service.UpdateTodo(id, updatedTodo);
            return result ? Results.NoContent() : Results.NotFound();
        }).WithName("UpdateTodo")
        .WithDescription("Update an existing TODO item");   

        app.MapDelete("{path}/{id}", (TodoService service, int id) =>
        {
            var result = service.DeleteTodo(id);
            return result ? Results.NoContent() : Results.NotFound();
        }).WithName("DeleteTodo")
        .WithDescription("Delete a TODO item");
    }

    private readonly List<Todo> _todos = new();
    private int _nextId = 1;

    public IEnumerable<Todo> GetAllTodos() => _todos;

    public Todo? GetTodoById(int id) => _todos.FirstOrDefault(t => t.Id == id);

    public void AddTodo(Todo todo)
    {
        todo = todo with { Id = _nextId++ };
        _todos.Add(todo);
    }

    public bool UpdateTodo(int id, Todo updatedTodo)
    {
        var index = _todos.FindIndex(t => t.Id == id);
        if (index == -1) return false;

        _todos[index] = updatedTodo with { Id = id };
        return true;
    }

    public bool DeleteTodo(int id)
    {
        var todo = GetTodoById(id);
        return todo is not null && _todos.Remove(todo);
    }
}
