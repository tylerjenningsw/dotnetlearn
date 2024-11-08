using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITaskService, InMemoryTaskService>();

var app = builder.Build();

app.UseRewriter(new RewriteOptions().AddRedirect("api/tasks/(.*)", "api/todos/$1"));
app.Use(async (context, next) =>
{
    context.Request.Path = "/api/todos" + context.Request.Path;
    Console.WriteLine($"[{context.Request.Method}] {context.Request.Path} {DateTime.Now} START");
    await next(context);
    Console.WriteLine($"[{context.Request.Method}] {context.Request.Path} {DateTime.Now} END");
});

// Basic GET endpoint
app.MapGet("/", () => "Hello World!");

// Example of additional endpoints
app.MapGet("/api/greet/{name}", (string name) => 
    $"Hello, {name}!");

app.MapPost("/api/data", (Data data) => 
    Results.Ok(new { message = $"Received: {data.Value}" }));

var todos = new List<Todo>
{
    new(1, "Buy milk", DateTime.Now, false),
    new(2, "Buy bread", DateTime.Now, false),
};
app.MapGet("/api/todos", async (ITaskService taskService) => await taskService.GetTasks());
app.MapGet("/api/todos/{id}", IResult (int id, ITaskService taskService) =>
{
    var todo = taskService.GetTaskById(id);
    return todo != null ? TypedResults.Ok(todo) : TypedResults.NotFound();
});

app.MapPost("/api/todos", (Todo todo, ITaskService taskService) => 
{
    var createdTask = taskService.CreateTask(todo);
    return TypedResults.Created($"/api/todos/{createdTask.Id}", createdTask);
})
.AddEndpointFilter(async (context, next) =>
{
    var taskArgument = context.GetArgument<Todo>(0);
    var errors = new Dictionary<string, string[]>();
    if (taskArgument.DueDate < DateTime.Now)
        errors["DueDate"] = ["Due date cannot be in the past"];
    if (taskArgument.IsDone)
        errors["IsDone"] = ["IsDone cannot be true"];
    if (taskArgument.Title == null)
        errors["Title"] = ["Title is required"];
    if (errors.Count > 0)
        return TypedResults.ValidationProblem(errors);
    return await next(context);
});

app.MapDelete("/api/todos/{id}", IResult (int id, ITaskService taskService) =>
{
    var success = taskService.DeleteTaskById(id);
    return success != null ? TypedResults.NoContent() : TypedResults.NotFound();
});


app.Run();

// Example data class
public class Data
{
    public string? Value { get; set; }
}


public record Todo(int Id, string Title, DateTime DueDate, bool IsDone);
