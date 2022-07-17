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

            // ���JWT��֤����
            services.AddAuthentication("Bearer")
                     .AddJwtBearer("Bearer", option => {
                         // OIDC�����ַ
                         option.Authority = "http://localhost:5001";
                         // ��ʹ��Https
                         option.RequireHttpsMetadata = false;
                         // ����JWT����֤����
                         option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                         { 
                            // ��Ϊʹ�õ���api��Χ���ʣ��ò���������false
                            ValidateAudience=false
                         };

                     });
            // ���api��Ȩ����
            services.AddAuthorization(options => {
                // "ApiScope"Ϊ��������
                options.AddPolicy("ApiScope", builder =>
                {
                    builder.RequireAuthenticatedUser();
                    // ����claim�Ƿ����
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

            // ��֤
            app.UseAuthentication();
            // ��Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllers();
                // ����ȫ�ֲ��ԣ�Ӧ��������api
                //endpoints.MapControllers().RequireAuthorization("ApiScope");
            });
        }
    }
}
