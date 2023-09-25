using CashPurse.Server.Data;
using CashPurse.Server.Endpoints;
using CashPurse.Server.Models;
using dotenv.net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.NodaTime;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextPool<CashPurseDbContext>(options =>
{
    options.UseSqlite("Data Source=../../appdb1.sqlite");
});
builder.Services.AddOutputCache(options => {
    options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromSeconds(12)));
    options.AddPolicy("CacheDataPage", builder => builder.Expire(TimeSpan.FromSeconds(30)));
});

builder.Services.AddScoped<UserManager<ApplicationUser>>();

builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<CashPurseDbContext>();

builder.Services.AddAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseOutputCache();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapGroup("/auth").MapIdentityApi<ApplicationUser>();

app.MapExpenseEndpoints();

app.Run();

