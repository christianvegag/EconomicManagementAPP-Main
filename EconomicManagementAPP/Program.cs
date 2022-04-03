using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRepositorieAccountTypes, RepositorieAccountTypes>();
builder.Services.AddTransient<IRepositorieAccounts, RepositorieAccounts>();
builder.Services.AddTransient<IRepositorieCategories, RepositorieCategories>();
builder.Services.AddTransient<IRepositorieOperationTypes, RepositorieOperationTypes>();
builder.Services.AddTransient<IRepositorieTransactions, RepositorieTransactions>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IReportServices, ReportServices>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IRepositorieUsers, RepositorieUsers>();
builder.Services.AddTransient<IUserStore<User>, UserStore>();
builder.Services.AddTransient<SignInManager<User>>();
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
}).AddCookie(IdentityConstants.ApplicationScheme);

builder.Services.AddTransient<IUserServices, UserServices>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Transactions}/{action=Index}/{id?}");

app.Run();
