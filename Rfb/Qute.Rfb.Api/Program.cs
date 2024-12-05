using Qute.Rfb.Api.Helpers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddQuteServices(builder.Configuration);

var app = builder.Build();
app.UseQute(builder.Configuration);
app.Run();
