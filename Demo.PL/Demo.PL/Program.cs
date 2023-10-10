using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;

using Demo.DAL.Context;
using Demo.DAL.Entities;
using Demo.PL.Controllers;
using Demo.PL.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL
{
    public class Program
    {
        public static void Main()
        {
            //WebApplication:A builder for web applications and services.
            //CreateBuilder:Initializes a new instance of the WebApplicationBuilder class with preconfigured defaults.


            var builder = WebApplication.CreateBuilder();
            builder.Services.AddDbContext<MVCAppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"))
            );
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(map => map.AddProfile(new MappingProfile()));


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
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}