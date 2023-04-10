using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using GoalTrackerAPI.MappingProfiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc();
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAutoMapper(typeof(AppGoalMappingProfile), typeof(AppUserMappingProfile));
builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
