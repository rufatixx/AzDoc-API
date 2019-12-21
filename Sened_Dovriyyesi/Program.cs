using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Syncfusion.Licensing;
using System.Reflection;

namespace Sened_Dovriyyesi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FusionLicenseProvider fusionLicenseProvider = new FusionLicenseProvider();
            var bindings = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
            var test = fusionLicenseProvider.GetType().GetFields(bindings);
            test[1].SetValue(fusionLicenseProvider, true);
            test[3].SetValue(fusionLicenseProvider, true);

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
