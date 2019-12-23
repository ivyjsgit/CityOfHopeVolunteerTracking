using 
Microsoft.AspNetCore.Hosting;
using 
Microsoft.Extensions.Hosting;

namespace CoHO
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                   webBuilder.UseStartup<Startup>().UseKestrel().UseUrls("http://+:5000;https://+:5001");
                //    webBuilder.UseStartup<Startup>().UseKestrel().UseUrls("http://+:80;https://+:443");
                	var DomainToUse=""; //Your domain
			//webBuilder.UseKestrel(kestrelOptions => kestrelOptions.ConfigureHttpsDefaults(httpsOptions => httpsOptions.ServerCertificateSelector = (c, s) => LetsEncryptRenewalService.Certificate)).UseUrls("http://" + DomainToUse, "https://" + DomainToUse).UseStartup<Startup>();			





});
    }
}
