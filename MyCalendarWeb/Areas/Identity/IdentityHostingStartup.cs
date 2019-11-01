using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCalendarWeb.Areas.Identity.Data;
using MyCalendarWeb.Models;

[assembly: HostingStartup(typeof(MyCalendarWeb.Areas.Identity.IdentityHostingStartup))]
namespace MyCalendarWeb.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<MyCalendarWebContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("MyCalendarWebContextConnection")));

                services.AddDefaultIdentity<MyCalendarUser>()
                    .AddEntityFrameworkStores<MyCalendarWebContext>();
            });
        }
    }
}