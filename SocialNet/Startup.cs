using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNet.Data;
using SocialNet.Models;
using SocialNet.Services;
using SocialNet.Data.Interfaces;

namespace SocialNet
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.

            //services.AddTransient<DbContext, ApplicationDbContext>(); //TODO: Workaround?
            services.AddTransient<IEmailSender, EmailSender>();

            //services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddTransient<IRepository<Post>, Repository<Post>>();
            services.AddScoped<DbContext, ApplicationDbContext>(provider=>provider.GetService<ApplicationDbContext>());

            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            var simplePasswordPolicy = new PasswordOptions()
            {
                RequireDigit = false,
                RequiredLength = 5,
                RequireLowercase = false,
                RequiredUniqueChars = 0,
                RequireNonAlphanumeric = false,
                RequireUppercase = false
            };

            services.Configure<IdentityOptions>(options =>
                    options.Password = simplePasswordPolicy);


            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
