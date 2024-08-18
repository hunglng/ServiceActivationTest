using ServiceActivationTest.Services;
using ServiceActivationTest.Tenant;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddSingleton<IWeatherForecastService, WeatherForecastService>();
//builder.Services.AddActivatedSingleton<IWeatherForecastService, WeatherForecastService>(); //.net9 dm
//builder.Services.AddHostedService<TimerService>();


//var serviceProvider = builder.Services.BuildServiceProvider();
//var mySingleton = serviceProvider.GetRequiredService<IWeatherForecastService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<ITenantServiceProvider, TenantServiceProvider>();
builder.Services.AddTenantServiceProvider();



var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<TenantMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
