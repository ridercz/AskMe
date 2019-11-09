using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Altairis.AskMe.Web.Mvc {
    public class Program {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseUrls("http://0.0.0.0:5001")
                });
    }
}
