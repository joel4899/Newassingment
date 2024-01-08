using DataAccess.DataContext;
using DataAccess.Repositories;
using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
 

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<AirlineDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AirlineDbContext>();
            builder.Services.AddControllersWithViews();
         
            string absolutePath = @"C:\Users\joelc\Downloads\enterprise\myonlyassign\EP2023_SWD62B-main\Solution1\Presentation\Data\json.json";

            builder.Services.AddSingleton<ITicketRepository, TicketFileRepository>(_ =>
       new TicketFileRepository(absolutePath));


            builder.Services.AddScoped<IFlightRepository, FlightDbRepository>();
            builder.Services.AddScoped<ITicketRepository, TicketDbRepository>();

            //addscoped = it will create ONE instance per request
            //addtransient = it will create one instance per call
            //addsingleton = it will create ONE instance for all requests by all users;
            //which means that that one instance if being used by user a, user b has to wait until
            //                 that one instance is released


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
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
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}