using DmcSocial.Interfaces;
using DmcSocial.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DmcSocial
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }
    const string _corsPolicy = "MyPolicy";
    public static readonly ILoggerFactory factory
        = LoggerFactory.Create(builder => { builder.AddConsole(); });
    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(o => o.AddPolicy(_corsPolicy, builder =>
      {
        builder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
      }));
      services.AddDbContext<AppDbContext>(options =>
      {
        options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        options.UseLoggerFactory(factory);
      });
      services.AddControllers();
      services.AddScoped<IPostRepository, PostRepository>();
      services.AddScoped<ICommentRepository, CommentRepository>();
      services.AddScoped<ITagRepository, TagRepository>();
      services.AddScoped<Authenticate, Authenticate>();
      services.AddSwaggerGen();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      using (var scope = app.ApplicationServices.CreateScope())
      {
        var db = scope.ServiceProvider.GetService<AppDbContext>();
        db.Database.Migrate();
        var seeder = new DataSeeder(db);
        seeder.Seed();
      }
      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
      // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
      });
      app.UseCors(_corsPolicy);
      app.UseRouting();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
