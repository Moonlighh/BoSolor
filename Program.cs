using Microsoft.AspNetCore.Authentication.Cookies; // Para controlador acceso part1
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Agregamos la autenticacion al proyecto usando cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

        //options.LogoutPath = "/Acceso/CerrarSesion";
        options.AccessDeniedPath = "/Home/Index";
    });

// Para los roles
builder.Services.AddAuthorization(builder =>
{
    var defaultAuthBuilder = new AuthorizationPolicyBuilder();
    var defaultAuthPolicy = defaultAuthBuilder
        .RequireAuthenticatedUser()
        .RequireClaim(ClaimTypes.Role)
        .Build();
    builder.DefaultPolicy = defaultAuthPolicy;
});


// Add services to the container.
builder.Services.AddControllersWithViews();

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

// Activar para autenticacion
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
