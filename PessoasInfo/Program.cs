using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using PessoasInfo.Repositories;
using PessoasInfo.Services;
using ReflectionIT.Mvc.Paging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DI DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// DI Dapper
builder.Services.AddScoped<IDbConnection>(x =>
    new SqlConnection(builder.Configuration.GetConnectionString("Default")));

// DI Respositories
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<IDetalheRepository, DetalheRepository>();
builder.Services.AddScoped<ITelefoneRepository, TelefoneRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();

// DI Services
builder.Services.AddScoped<IReportService, ReportService>();

// Paging Service Injection
builder.Services.AddScoped<IPagingService, PagingService>();

// Paging
builder.Services.AddPaging(options =>
{
    options.ViewName = "Bootstrap5";
    options.HtmlIndicatorDown = " <span>&darr;</span>";
    options.HtmlIndicatorUp = " <span>&uarr;</span>";
});

// DI Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Identity Service - Senha,lockout e usuario
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
    options.Lockout.MaxFailedAccessAttempts = 5;

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
});

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

app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();