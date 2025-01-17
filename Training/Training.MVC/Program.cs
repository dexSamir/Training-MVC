using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using Training.BL.Extensions;
using Training.Core.Entities;
using Training.DAL.Context;

namespace Training.MVC; 
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<AppdbContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("MsSQL")));

        builder.Services.AddIdentity<User, IdentityRole>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequiredLength = 8;
            opt.Password.RequireDigit = false;
            opt.Password.RequireLowercase = true;
            opt.Password.RequireUppercase = false;

        }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppdbContext>();

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
        app.UseUserSeed(); 
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "register",
            pattern: "register",
            defaults: new { controller = "Account", action = "Register" }
        );

        app.MapControllerRoute(
             name: "areas",
             pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"); 

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.Run();
    }
}
