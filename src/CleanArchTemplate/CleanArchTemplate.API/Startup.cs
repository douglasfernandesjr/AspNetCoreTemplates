using System.Security.Principal;
using CleanArchTemplate.API.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace CleanArchTemplate.API
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
			ConfigCurrentUser(services);
			ConfigDataBase(services);
			ConfigInfraDI(services);
			ConfigCoreDI(services);


			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new Info { Title = "CleanArchTemplate", Version = "v1" });
			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArchTemplate V1");
			});
		}

		public virtual void ConfigCurrentUser(IServiceCollection services)
		{
			//Disponibilizar o usuário logado através de DI
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddTransient<IPrincipal>
				(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
		}

		public virtual void ConfigDataBase(IServiceCollection services)
		{
			services.AddDbContext<DbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("Default"));
			});
		}

		public virtual void ConfigInfraDI(IServiceCollection services)
		{
			InfraDI.Config(services);
		}

		public virtual void ConfigCoreDI(IServiceCollection services)
		{
			CoreDI.Config(services);
		}
	}
}
