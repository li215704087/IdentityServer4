using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
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
            services.AddControllers();

            // 添加JWT认证方案
            services.AddAuthentication("Bearer")
                     .AddJwtBearer("Bearer", option => {
                         // OIDC服务地址
                         option.Authority = "http://localhost:5001";
                         // 不使用Https
                         option.RequireHttpsMetadata = false;
                         // 设置JWT的验证参数
                         option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                         { 
                            // 因为使用的是api范围访问，该参数需设置false
                            ValidateAudience=false
                         };

                     });
            // 添加api授权策略
            services.AddAuthorization(options => {
                // "ApiScope"为策略名称
                options.AddPolicy("ApiScope", builder =>
                {
                    builder.RequireAuthenticatedUser();
                    // 鉴定claim是否存在
                    builder.RequireClaim("scope", "sample_api");
                });
            
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // 认证
            app.UseAuthentication();
            // 授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllers();
                // 设置全局策略，应用于所有api
                //endpoints.MapControllers().RequireAuthorization("ApiScope");
            });
        }
    }
}
