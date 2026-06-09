using System.Text.Json.Serialization;
using Issue_Tracker.Data;
using Issue_Tracker.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

//Controller + JSON Enum 문자열 처리
builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

//SQLite 연결
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//Service & Swagger 등록
builder.Services.AddScoped<IIssueService, IssueService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

//SQLite DB 자동 생성
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

//Swagger 사용
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();