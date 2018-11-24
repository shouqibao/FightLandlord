using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BetGame.DDZ.WebHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

			Newtonsoft.Json.JsonConvert.DefaultSettings = () => {
				var st = new Newtonsoft.Json.JsonSerializerSettings();
				st.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
				st.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
				st.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.RoundtripKind;
				return st;
			};

			RedisHelper.Initialization(new CSRedis.CSRedisClient("127.0.0.1:6379,password=,defaultDatabase=11,poolsize=5"));
		}

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
			services.AddScoped<CustomExceptionFilter>();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			app.UseDeveloperExceptionPage();

			app.UseMvc();
			app.UseStaticFiles();
        }
    }
}
