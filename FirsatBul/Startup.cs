using FirsatBul.Business.Abstract;
using FirsatBul.DataAccess;
using FirsatBul.Business.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirsatBul.DataAccess.Abstract;
using FirsatBul.DataAccess.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using FirsatBul.Entities;

namespace FirsatBul
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

            services.AddControllers().AddNewtonsoftJson(options =>
                           options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                       ); //Fixing JSON Self Referencing Loop i�in kullan�ld�. yani Circular Referance Loop i�in kullan�ld�.
           
            services.AddSingleton<IFirsatlarService, FirsatlarMenager>();
            services.AddSingleton<IFirsatlarRepository, FirsatlarRepostory>();
            services.AddSingleton<FirsatBulDbContext>();
            services.AddSingleton<FirsatBulIdentityDbContext>();
            services.AddAuthentication();
            services.ConfigureApplicationCookie(_ =>
            {
                _.LoginPath = new PathString("/Account/Login");
                _.Cookie = new CookieBuilder
                {
                    Name = "AspNetCoreIdentityExampleCookie", //Olu�turulacak Cookie'yi isimlendiriyoruz.
                    HttpOnly = false, //K�t� niyetli insanlar�n client-side taraf�ndan Cookie'ye eri�mesini engelliyoruz.
                   // Expiration = TimeSpan.FromMinutes(30), //Olu�turulacak Cookie'nin vadesini belirliyoruz.
                    SameSite = SameSiteMode.Lax, //Top level navigasyonlara sebep olmayan requestlere Cookie'nin g�nderilmemesini belirtiyoruz.
                    SecurePolicy = CookieSecurePolicy.SameAsRequest //HTTPS ve HTTP �zerinden eri�ilebilir yap�yoruz.
                };
                _.SlidingExpiration = true; //Expiration s�resinin yar�s� kadar s�re zarf�nda istekte bulunulursa e�er geri kalan yar�s�n� tekrar s�f�rlayarak ilk ayarlanan s�reyi tazeleyecektir.
                _.ExpireTimeSpan = TimeSpan.FromMinutes(30); //CookieBuilder nesnesinde tan�mlanan Expiration de�erinin varsay�lan de�erlerle ezilme ihtimaline kar��n tekrardan Cookie vadesi burada da belirtiliyor.
            });
            services.AddMvc();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Login/";
        });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();// sayfa yenilenince htmldeki de�i�iklerin yans�mas�n� sa�lar
            services.Configure<IdentityOptions>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@/";
                opts.Password.RequiredLength = 8;
                opts.Password.RequireNonAlphanumeric = true;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireDigit = true;

                //opts.SignIn.RequireConfirmedEmail = true;

                opts.Lockout.AllowedForNewUsers = true;
                opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                opts.Lockout.MaxFailedAccessAttempts = 3;
            });
            services.AddAuthentication()
                .AddGoogle(opts =>
                {
                    opts.ClientId = "717469225962-3vk00r8tglnbts1cgc4j1afqb358o8nj.apps.googleusercontent.com";
                    opts.ClientSecret = "babQzWPLGwfOQVi0EYR-7Fbb";
                    opts.SignInScheme = IdentityConstants.ExternalScheme;
                });

            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<FirsatBulIdentityDbContext>().AddDefaultTokenProviders();
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
            
        }
    }
}
