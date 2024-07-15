using Contracts;
using Entities;
using Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.DTO.User;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Contracts.UserCrud;
using Repository.UserCrud;
using Entities.UserContext;
using Entities.AllContext.AnualyPlanContext;
using Contracts.Plan;
using Repository.Plan;
using Entities.AllContext.QuarterPlanContex;
using Entities.AllContext.MonthPlanContext;
using Entities.AllContext.WeeklyPlanContex;
using Entities.AllContext.DaylyPlanContex;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //JWT**************************************************
            IConfigurationSection appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            AppSettings appSettings = appSettingsSection.Get<AppSettings>();
            var secretKey = Encoding.ASCII.GetBytes(appSettings.SecretKey);
            
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });
            //*****************************************************

            services.AddDbContext<RepositoryContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")
               ));








            //User
            services.AddScoped<IRepository, SqlUserRepo>();
            services.AddScoped<UserDbContext>();


            //Anualy Plan
            services.AddScoped<IAnualyPlanRepository, AnualyPlanSqlRepo>();
            services.AddScoped<AnualyPlanContext>();


            //Quarter Plan
            services.AddScoped<IQuarterPlanRepository, QuarterPlanSqlRepo>();
            services.AddScoped<QuaretrPlanContext>();

            //Month Plan
            services.AddScoped<IMonthPlanRepository, MonthPlanSqlRepository>();
            services.AddScoped<MonthPlanContex>();


            //Weekly Plan
            services.AddScoped<IWeeklyPlanRepository, WeeklyPlanSqlRepository>();
            services.AddScoped<WeeklyPlanContext>();


            //Dayly Plan
            services.AddScoped<IDaylyPlanRepository, DaylyPlanSqlRepository>();
            services.AddScoped<DaylyPlanContext>();















            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin().AllowAnyHeader());
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Description = "Example enter like this => Bearer 'token'",
                    Name="Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme="Bearer",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                       new OpenApiSecurityScheme
                       {
                           Reference=new OpenApiReference
                           {
                               Type = ReferenceType.SecurityScheme,
                               Id = "Bearer",
                           },
                       },
                       Array.Empty<string>()
                    },
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "webapi/v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
