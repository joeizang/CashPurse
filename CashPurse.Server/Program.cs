using System.Text.Json;
using System.Text.Json.Serialization;
using CashPurse.Server;
using CashPurse.Server.ApiModels;
using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.ApiModels.ExpensesApiModels;
using CashPurse.Server.BusinessLogic.DataServices;
using CashPurse.Server.BusinessLogic.Validators;
using CashPurse.Server.Data;
using CashPurse.Server.Endpoints;

// using CashPurse.Server.Endpoints;
using CashPurse.Server.Models;
using CashPurse.Server.SwaggerConfig;
using dotenv.net;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextPool<CashPurseDbContext>(options =>
{
    options.UseNpgsql(Environment.GetEnvironmentVariable("postgresconnectionstring")!);
});
builder.Services.AddOutputCache(options => {
    options.AddBasePolicy(b => b.Expire(TimeSpan.FromSeconds(12)));
    options.AddPolicy("CacheDataPage", b => b.Expire(TimeSpan.FromSeconds(30)));
});

builder.Services.Configure<JsonOptions>(opt => 
{
    opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// builder.Services.AddScoped<UserManager<ApplicationUser>>();
// builder.Services.AddScoped<IValidator<CreateExpenseRequest>, ExpenseValidator>();
// builder.Services.AddScoped<IValidator<UpdateExpenseRequest>, ExpenseUpdateValidator>();
// builder.Services.AddScoped<IValidator<CreateBudgetListRequest>, BudgetListCreateValidator>();
// builder.Services.AddScoped<IValidator<UpdateBudgetListRequest>, BudgetListUpdateValidator>();
// builder.Services.AddScoped<IValidator<CreateBudgetListItemRequest>, BudgetListItemValidator>();
// builder.Services.AddScoped<IValidator<UpdateBudgetListItemRequest>, UpdateBudgetListItemValidator>();

// builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
//     .AddEntityFrameworkStores<CashPurseDbContext>();

// builder.Services.AddAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

var app = builder.Build();
app.UseCors("AllowAll");
app.UseOutputCache();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.MapGroup("/auth").MapIdentityApi<ApplicationUser>();
app.MapGet("/api/health", async ([FromServices] CashPurseDbContext context,
    IHttpClientFactory factory) =>
{
    var client = factory.CreateClient();
    client.BaseAddress = new Uri("http://localhost:5086");
    var budgetListsChecked = await context.BudgetLists.AsNoTracking().AnyAsync().ConfigureAwait(false);
    var test = await context.Expenses.AsNoTracking().AnyAsync().ConfigureAwait(false);
    var test1 = await client
        .GetAsync("/api/budgetlists")
        .ConfigureAwait(false);
    var test2 = await client
        .GetAsync("/api/expenses")
        .ConfigureAwait(false);
    if (budgetListsChecked && test && test1.IsSuccessStatusCode && test2.IsSuccessStatusCode)
        return Results.Ok("API is up and running!");
    return Results.Problem("API is up and running but there is a problem with the data!");
});

app.MapExpenseEndpoints();
app.MapBudgetListEndpoints();

app.Run();

