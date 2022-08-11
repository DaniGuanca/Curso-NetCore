using Ejemplo1.Models;
using Ejemplo1.Seguridad;
using Ejemplo1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1
{
    public class Startup
    {
        //Esta lineas las agrego por si necesito acceder a la configracion que esta en appsettings.json
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        ////
        

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Inicializacion del motor BD
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("ConexionSQL")));



            //Indico que va a usar una arquitectura MVC
            //services.AddMvc();
            //PERO ESO YA NO SIRVE A PARTIR DEL NET 3.1, ASI QUE AGREGO LO SIGUIENTE
            services.AddMvc(options => options.EnableEndpointRouting = false);


            //USAMOS LA BD PARA LA INTERFAZ 
            //ESTO ES SOLO POR LA FORMA QUE EL MANEJA EN EL CONTROLLER LA INTERFAZ, EL LLAMA A LA INTERFAZ
            //LO HACE AL REVES DIGAMOS, EN REALIAD YO ESTOY ACOSTUMBRADO A LLAMAR A LA CLASE QUE IMPLEMENTA LA INTERFAZ
            //ENTONCES EN MIS CASOS NO HARIA FALTA ASOCIARLE UNA CLASE POR DEFECTO A LA INTERFAZ
            services.AddScoped<IAmigoAlmacencs, SQLAmigoRepositorio>();

            //CONFIRMACION CORREO ELECTRONICO
            services.AddIdentity<UsuarioAplicacion, IdentityRole>(
                options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                }).AddErrorDescriber<ErroresCastellano>().
                AddEntityFrameworkStores<AppDbContext>(). 
                AddDefaultTokenProviders();

            //PARA EL IDENTITY CORE
            //Lo que hace es agregar las implementaciones de configuraciones por default de identity a mis objetos de mi contexto
            //services.AddIdentity<UsuarioAplicacion, IdentityRole>().AddErrorDescriber<ErroresCastellano>().
            //    AddEntityFrameworkStores<AppDbContext>();


            //Cambio la direccion default Account/Login que hace el authorization por la que ya tengo creada yo.
            services.ConfigureApplicationCookie(options => 
            {
                options.LoginPath = "/Cuentas/Login";
                options.AccessDeniedPath = "/Cuentas/AccesoDenegado";
             
            });


            //Autenticacion google
            services.AddAuthentication()
                .AddGoogle(opciones =>
            {
                opciones.ClientId = "144495585413-npt47lshdjlddsunbekteb07mjnagv1d.apps.googleusercontent.com";
                opciones.ClientSecret = "08VYAvl4g4tOvAYaWkTid2PA";
            })
                .AddFacebook(opciones =>
                {
                    opciones.AppId = "753946655482790";
                    opciones.AppSecret = "35b697f38cdcfe4eb27b174ff7c552d0";
                });



            //Configuro las validaciones para personalizarlas a mi gusto
            services.Configure<IdentityOptions>(opciones =>
            {
                opciones.Password.RequiredLength = 4;
                opciones.Password.RequireNonAlphanumeric = false;
            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("BorrarRolPolicy",
                    policy => policy.RequireClaim("Borrar Rol"));

                //options.AddPolicy("EditarRolPolicy",
                //    policy => policy.RequireClaim("Editar Rol", "true")
                //    .RequireRole("Administrador")
                //    .RequireRole("Dios"));

                //options.AddPolicy("EditarRolPolicy", policy => policy.RequireAssertion(context =>
                //    context.User.IsInRole("Administrador") &&
                //    context.User.HasClaim(claim => claim.Type == "Editar Rol" && claim.Value == "true") ||
                //    context.User.IsInRole("Dios")
                //));

                options.AddPolicy("EditarRolPolicy", policy => policy.AddRequirements(new GestionarAdminRolesyClaims()));

                //Caducidad del token
                services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(1));
            });


            services.AddSingleton<IAuthorizationHandler, PoderEditarSoloOtrosClaimsRoles>();

            services.AddSingleton<ProteccionStrings>();

            //Ahora muestro el mvc core
            //services.AddMvcCore(options => options.EnableEndpointRouting = false);
            //MVC VS MVC Core: En teoria el MVC Core no puede mostrar Json.El addMvc tiene mas funcionalidades y librerias.
            //AddMvc parece que ya llama dentro al MvcCore

            //En el HomeControler en vez de hacer un objeto MockAmigoRepositorio que ya implementa la clase
            //lo que hace Jap (el profe) es hacer un objeto IAmigoAlamacencs osea que hace un objeto interfaz
            //por lo que al runear no sabe donde ir porque no sabe que objeto de todos los que implementan la interfaz usar
            //para crear esa unica instancia uso el addSingleton
            //services.AddSingleton<interfaz, clase que la implementa a elegir>();
            //Lo que hace es crear la instancia de la interfaz entonces cada vez que se use un objeto interfaz solo automaticamente
            //va a usar la clase que elegimos.
            //Por lo que lei en documentacion no es muy recomendable usar
            //services.AddSingleton<IAmigoAlmacencs, MockAmigoRepositorio>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if(env.IsProduction() || env.IsStaging())
            {
                //Si esta en produccion o staging hago la redireccion de errores http
                //no lo hago en desarrollo porque generalmente en desarrollo uno quiere saber los detalles del error
                //en cambio en staging o en produccion el usuario no tiene que saber los detalles

                //El entorno lo cambio en launchSettings.json


                //PARA CONTROLAR LOS ERRORES HTTP
                //Entre parentesis pongo a la ruta que va a ir y el {0} va a ser reemplazado dinamicamente con el error http que se produzca
                app.UseStatusCodePagesWithRedirects("/Error/{0}");

                //Para mandar cualquier excepcion sin importar cual al controlador error
                app.UseExceptionHandler("/Error");
            }


            



            //Si quiero agregar otra pagina html que no sea default o index para que cargue de forma predeterminada
            //tengo que crear una instancia de DefaultFilesOptions y con DefaultFilesName y su metodo Clear() borro todas las instancias que tenia(index, default)
            //y con Add() agrego las instancia que yo quiera
            //DefaultFilesOptions d = new DefaultFilesOptions();
            //d.DefaultFileNames.Clear();
            //d.DefaultFileNames.Add("nodefault.html");

            //Y por ultimo a useDefaultFiles le mando la variable de DefaultFilesOPtions
            //app.UseDefaultFiles(d);


            //Para que abra el archivo por defecto de los html
            //app.UseDefaultFiles();

            //Para que este disponible wwwroot
            app.UseStaticFiles();



            //PARA USAR EL IDENTITY
            app.UseAuthentication();


            //Debajo del useStaticFiles() llamo al ruteo usando mvc
            //app.UseMvcWithDefaultRoute();

            //Rutas personalizadas
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            //});


            //Ruta personalizada por atributo
            app.UseMvc(); 
            //Luego arriba de cada ViewResult se indica la ruta con [Route ("ruta")]



            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hola");
            //    });
            //});
        }
    }
}
