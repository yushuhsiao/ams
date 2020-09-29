using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS
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
            services.AddHttpContextAccessor();
            services.AddConfigService();
            services.AddDbCache();
            services.AddSingleton<DataService>();

            var x = typeof(Dapper.SqlMapperExtensions);

            services
                .AddControllers()
                //.AddControllersWithViews()
                .AddNewtonsoftJson();
            //services.AddRazorPages();


            services.AddSwaggerGen(c =>
            {
                //c.OperationFilter<AuthorizationOperationFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CMS_API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "API Token",
                    Name = Microsoft.Net.Http.Headers.HeaderNames.Authorization,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });

                //try
                //{
                //    var asm_list = AppDomain.CurrentDomain.GetAssemblies();
                //    foreach (var asm in asm_list)
                //    {
                //        try { c.IncludeXmlComments(Path.ChangeExtension(asm.Location, "xml")); }
                //        catch { }
                //    }
                //}
                //catch { }

                //c.CustomSchemaIds(x =>
                //{
                //    return x.FullName;
                //});
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            //app.UseHttpsRedirection();
            //app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CMS API");
            });


            Task.Run(() =>
            {
                try
                {
                    app.ApplicationServices.DataService().Acl.Init();
                }
                catch (Exception ex)
                {
                    app.ApplicationServices.GetService<ILogger<Startup>>().LogError(ex, ex.Message);
                }
            });
        }
    }
}
