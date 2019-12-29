using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Exam.Areas.Identity.IdentityHostingStartup))]
namespace Exam.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}