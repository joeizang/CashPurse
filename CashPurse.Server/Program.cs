using System.Text.Json;
using System.Text.Json.Serialization;
using CashPurse.Server.Data;
using CashPurse.Server.Endpoints;

// using CashPurse.Server.Endpoints;
using dotenv.net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

app.MapExpenseEndpoints();
app.MapBudgetListEndpoints();

app.Run();

