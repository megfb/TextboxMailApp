using TextboxMailApp.Api;
using TextboxMailApp.Api.Extensions;
using TextboxMailApp.Application.Contracts.Api;
using TextboxMailApp.Application.Extensions;
using TextboxMailApp.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRepositories(builder.Configuration).AddServices(builder.Configuration)
    .AddMailKitService(builder.Configuration).AddTokenService(builder.Configuration).AddSwaggerService(builder.Configuration).AddJwtService(builder.Configuration);
//builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
var app = builder.Build();

// Configure the HTTP request pipeline.  
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
