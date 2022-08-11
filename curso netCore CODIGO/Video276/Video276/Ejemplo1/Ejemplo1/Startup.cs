using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Ejemplo1.Models;
using Ejemplo1.Seguridad;
using Ejemplo1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ejemplo1
{
    public class Startup
    {

        private IConfiguration _configuration;
        

        public object Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("ConexionSQL")));
            services.AddMvc(options =>
               {
                   var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                   options.Filters.Add(new AuthorizeFilter(policy));

               }).AddXmlSerializerFormatters();
            services.AddScoped<IAmigoAlmacen, SQLAmigoRepositorio>();
            services.AddIdentity<UsuarioAplicacion, IdentityRole>(
                options => {
                   options.SignIn.RequireConfirmedEmail = true;
                }).AddErrorDescriber<ErroresCastellano>().
                AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => 
            {
                options.LoginPath = "/Cuentas/Login";
                options.AccessDeniedPath = "/Cuentas/AccesoDenegado";
            });
            services.AddAuthentication()
                 .AddGoogle(opciones =>
                 {
                     opciones.ClientId = "Pon tu id cliente";
                     opciones.ClientSecret = "Pon tu clave secreta";
                 })
                 .AddFacebook(options =>
                 {
                     options.AppId = "Pon tu id cliente";
                     options.AppSecret = "Pon tu clave secreta";
                 });

            services.Configure<IdentityOptions>(opciones =>
            {
                opciones.Password.RequiredLength = 8;
                opciones.Password.RequiredUniqueChars = 3;
                opciones.Password.RequireNonAlphanumeric = false;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("BorrarRolPolicy",
                    policy => policy.RequireClaim("Borrar Rol"));

                //options.AddPolicy("EditarRolPolicy", policy =>
                //policy.RequireClaim("Editar Role", "true")
                //.RequireRole("Administrador")
                //.RequireRole("Dios")
                //);

                //options.AddPolicy("EditarRolPolicy", policy => policy.RequireAssertion(context =>
                //    context.User.IsInRole("Administrador") &&
                //    context.User.HasClaim(claim => claim.Type == "Editar Role" && claim.Value == "true") ||
                //    context.User.IsInRole("Dios")
                //));

                options.AddPolicy("EditarRolPolicy", policy =>policy.AddRequirements(new GestionarAdminRolesyClaims()));

                services.Configure<DataProtectionTokenProviderOptions>(o =>o.TokenLifespan = TimeSpan.FromHours(1));
            });


            services.AddSingleton<IAuthorizationHandler, PoderEditarSoloOtrosClaimsRoles>();

            services.AddSingleton<IAuthorizationHandler, SoySuperAdmin>();

            services.AddSingleton<ProteccionStrings>();

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env )
        {

            if (env.IsDevelopment())
            {
                DeveloperExceptionPageOptions d = new DeveloperExceptionPageOptions
                {
                    SourceCodeLineCount = 2
                };
                app.UseDeveloperExceptionPage(d);
            }
            else if (env.IsProduction() || env.IsStaging())
            {

                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithRedirects("/Error/{0}");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
          
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id}");
            });


        }
    }
}
