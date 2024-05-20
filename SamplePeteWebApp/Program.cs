using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SamplePeteService;
using SamplePeteService.Models;
using SamplePeteWebAppServiceCollection;

var builder = WebApplication.CreateBuilder(args);

bool UseInMemoryDatabase = builder.Configuration.GetSection("PeteSampleAppSettings:UseInMemoryDatabase").Value != null && bool.Parse(builder.Configuration.GetSection("PeteSampleAppSettings:UseInMemoryDatabase").Value);

if (UseInMemoryDatabase)
{
    builder.Services.AddDbContext<Context>(config => config.UseInMemoryDatabase("MemoryBaseDataBase"));
}
else
{
    builder.Services.AddDbContext<Context>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("PETESAMPLEAPPCONNECTION")));
}

builder.Services.AddServices();

builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

/*
 Seed the application with a some sample Projects
 */
if (builder.Configuration.GetSection("PeteSampleAppSettings:SeedSampleProjects").Value != null && bool.Parse(builder.Configuration.GetSection("PeteSampleAppSettings:SeedSampleProjects").Value))
{
    SeedInMemoryDatabase();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

async void SeedInMemoryDatabase()
{
    using IServiceScope scope = app.Services.CreateScope();
    IProjectService projectService = (IProjectService)scope.ServiceProvider.GetService(typeof(IProjectService));

    TblProject tblProject1 = new()
    {
        ProjectName = "Sample Project 1",
        StartDate = new System.DateTime(2020, 11, 2),
        EndDate = new System.DateTime(2020, 11, 29)
    };

    await projectService.CreateProjectAsync(tblProject1).ConfigureAwait(false);

    TblProject tblProject2 = new()
    {
        ProjectName = "Sample Project 2",
        StartDate = new System.DateTime(2021, 10, 21),
        EndDate = new System.DateTime(2021, 11, 1)
    };

    await projectService.CreateProjectAsync(tblProject2).ConfigureAwait(false);

    TblProject tblProject3 = new()
    {
        ProjectName = "Sample Project 3",
        StartDate = new System.DateTime(2019, 7, 15),
        EndDate = new System.DateTime(2020, 1, 5)
    };

    await projectService.CreateProjectAsync(tblProject3).ConfigureAwait(false);

    TblProject tblProject4 = new()
    {
        ProjectName = "Sample Project 4",
        StartDate = new System.DateTime(2021, 1, 25),
        EndDate = new System.DateTime(2021, 6, 6)
    };

    await projectService.CreateProjectAsync(tblProject4).ConfigureAwait(false);
}

namespace SamplePeteWebAppServiceCollection
{
    public static class SamplePeteWebAppServiceCollection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            return services;
        }
    }
}