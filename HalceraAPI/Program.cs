using HalceraAPI.Utilities.Extensions;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

/// DBContext Intializer
builder.Services.ConfigureDbContextAsync(builder.Configuration);
builder.Services.ConfigureOperationsInjection();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport();

// Automapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

// app.MapControllers();

app.MapControllerRoute(
      name: "default",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
