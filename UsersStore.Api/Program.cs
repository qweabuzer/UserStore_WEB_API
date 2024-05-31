using Microsoft.EntityFrameworkCore;
using UsersStore.Application.Services;
using UsersStore.Core.Interfaces;
using UsersStore.DataAccess;
using UsersStore.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*builder.Services.AddDbContext<UsersStoreDbContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(UsersStoreDbContext)));
    });*/

builder.Services.AddSingleton<UsersStoreDbContext>();
builder.Services.AddDbContextPool<UsersStoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(UsersStoreDbContext))));

builder.Services.AddSingleton<IUsersRepository, UsersRepository>();
builder.Services.AddSingleton<IUsersService, UsersService>(); 
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IStartUpService, StartUpService>();


builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);


var app = builder.Build();

var startUpService = app.Services.GetRequiredService<IStartUpService>();
if (startUpService != null)
    await startUpService.InitializeAdminUser();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

    