using Microsoft.EntityFrameworkCore;
using TarkovAssistant.Data;
using TarkovAssistant.Server.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.Configure<AppOptions>(builder.Configuration.GetSection("Settings"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IMapService, MapService>();
builder.Services.AddTransient<ILayerService, LayerService>();
builder.Services.AddTransient<IProfileService, ProfileService>();
builder.Services.AddTransient<IMarkerService, MarkerService>();
builder.Services.AddTransient<IMarkerStateService, MarkerStateService>();

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
