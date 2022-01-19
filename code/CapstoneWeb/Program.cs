var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => new CapstoneWeb.Pages.Index());

app.Run();