using CashPurse.Server;
using CashPurse.Server.ApiModels;
using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.BusinessLogic.Validators;
using CashPurse.Server.Data;
using CashPurse.Server.Endpoints;
using CashPurse.Server.Models;
using CashPurse.Server.SwaggerConfig;
using dotenv.net;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL.NodaTime;
using Swashbuckle.AspNetCore.SwaggerGen;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextPool<CashPurseDbContext>(options =>
{
    options.UseSqlite("Data Source=../../appdb1.sqlite");
});
builder.Services.AddOutputCache(options => {
    options.AddBasePolicy(b => b.Expire(TimeSpan.FromSeconds(12)));
    options.AddPolicy("CacheDataPage", b => b.Expire(TimeSpan.FromSeconds(30)));
});

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<IValidator<CreateExpenseRequest>, ExpenseValidator>();
builder.Services.AddScoped<IValidator<UpdateExpenseRequest>, ExpenseUpdateValidator>();
builder.Services.AddScoped<IValidator<CreateBudgetListRequest>, BudgetListCreateValidator>();
builder.Services.AddScoped<IValidator<UpdateBudgetListRequest>, BudgetListUpdateValidator>();
builder.Services.AddScoped<IValidator<CreateBudgetListItemRequest>, BudgetListItemValidator>();
builder.Services.AddScoped<IValidator<UpdateBudgetListItemRequest>, UpdateBudgetListItemValidator>();

builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<CashPurseDbContext>();

builder.Services.AddAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

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
app.MapBudgetListEndpoints();

app.Run();

