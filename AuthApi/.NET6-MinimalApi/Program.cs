var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/auth", ([FromServices] ILogger<Program> logger) =>
{
    logger.LogInformation("get");
    return Results.Ok("UP!");
}).WithName("Get");

app.MapPost("/auth/user", ([FromForm] UserAuthRequest request, [FromServices] ILogger<Program> logger) =>
{
    var tags = new[] { "administrator", "management", "monitoring" };

    var log = $"user : {request.Username}, pass : {request.Password}";
    logger.LogInformation(log);

    if (request.Username == "admin" && request.Password == "admin")
        return AuthResult.Allow(tags);
    else
        return AuthResult.Deny();
}).WithName("CheckUser");

app.MapPost("/auth/vhost", ([FromForm] VhostAuthRequest request, [FromServices] ILogger<Program> logger) =>
{
    var log = $"user : {request.Username}, ip : {request.Ip}";
    logger.LogInformation(log);
    return AuthResult.Allow();
}).WithName("CheckVhost");

app.MapPost("/auth/resource", ([FromForm] ResourceAuthRequest request, [FromServices] ILogger<Program> logger) =>
{
    var log = $"user : {request.Username}, vhost : {request.Vhost}, resource : {request.Resource}, name : {request.Name}, permission : {request.Permission}";
    logger.LogInformation(log);
    return AuthResult.Allow();
}).WithName("CheckResource");

app.MapPost("/auth/topic", ([FromForm] TopicAuthRequest request, [FromServices] ILogger<Program> logger) =>
{
    var log = $"user : {request.Username}, vhost : {request.Vhost}, resource : {request.Resource}, name : {request.Name}, routing key: {request.RoutingKey}, permission : {request.Permission}"; logger.LogInformation(log);
    logger.LogInformation(log);
    return AuthResult.Allow();
}).WithName("CheckTopic");


app.Run();

record AuthResult
{
    public static IResult Allow()
    {
        return Results.Ok("allow");
    }

    public static IResult Allow(params string[] tags)
    {
        return Results.Ok($"allow {string.Join(" ", tags)}");
    }

    public static IResult Deny()
    {
        return Results.Ok("deny");
    }
}

record UserAuthRequest(string Username, string Password);

record VhostAuthRequest(string Username, string Vhost, string Ip);

record TopicAuthRequest(string Username, string Vhost, string Name, Resource Resource, TopicPermission Permission, [ModelBinder(Name = "routing_key")] string RoutingKey);

record ResourceAuthRequest(string Username, string Vhost, string Name, Resource Resource, ResourcePermission Permission);

enum Resource { Exchange, Queue, Topic }
enum TopicPermission { Write, Read }
enum ResourcePermission { Configure, Write, Read }