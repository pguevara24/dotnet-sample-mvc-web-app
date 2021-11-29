using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SamplePeteService;
using SamplePeteService.Models;

namespace SamplePeteWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            /*
             Seed the application with a some sample Projects
             */
            if (Configuration.GetSection("PeteSampleAppSettings:SeedSampleProjects").Value != null && bool.Parse(Configuration.GetSection("PeteSampleAppSettings:SeedSampleProjects").Value))
            {
                TblProject tblProject1 = new()
                {
                    ProjectName = "Sample Project 1",
                    StartDate = new System.DateTime(2020, 11, 2),
                    EndDate = new System.DateTime(2020, 11, 29)
                };

                _ = ProjectService.CreateProjectAsync(tblProject1);

                TblProject tblProject2 = new()
                {
                    ProjectName = "Sample Project 2",
                    StartDate = new System.DateTime(2021, 10, 21),
                    EndDate = new System.DateTime(2021, 11, 1)
                };

                _ = ProjectService.CreateProjectAsync(tblProject2);

                TblProject tblProject3 = new()
                {
                    ProjectName = "Sample Project 3",
                    StartDate = new System.DateTime(2019, 7, 15),
                    EndDate = new System.DateTime(2020, 1, 5)
                };

                _ = ProjectService.CreateProjectAsync(tblProject3);

                TblProject tblProject4 = new()
                {
                    ProjectName = "Sample Project 4",
                    StartDate = new System.DateTime(2021, 1, 25),
                    EndDate = new System.DateTime(2021, 6, 6)
                };

                _ = ProjectService.CreateProjectAsync(tblProject4);
            }
        }
    }
}
