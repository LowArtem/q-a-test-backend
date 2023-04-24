using AspTemplate.Api.Extensions.Application;
using AspTemplate.Api.Mappers;
using AspTemplate.Data.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

namespace AspTemplate.Api;

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
        services.AddBaseModuleDi("DefaultConnection", Configuration);
        
        // Services can be added here
        services.AddTransient(typeof(UserService), typeof(UserService));
        services.AddTransient(typeof(AnswerService), typeof(AnswerService));
        services.AddTransient(typeof(QuestionService), typeof(QuestionService));
        services.AddTransient(typeof(RoomService), typeof(RoomService));
        
        // Auto Mapper Configurations
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app,
        IApiVersionDescriptionProvider provider,
        IWebHostEnvironment env,
        ILogger<Startup> logger)
    {
        app.MigrateDatabase(logger);

        app.UseBaseServices(env, provider);

        app.UseSerilogRequestLogging();
    }
}