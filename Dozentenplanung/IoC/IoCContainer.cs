using System;
using Microsoft.Extensions.DependencyInjection;

namespace Dozentenplanung
{
    public static class IoC
    {
        public static  ApplicationDbContext applicationDbContext => IoCContainer.Provider.GetService<ApplicationDbContext>();
    }

    //Dependency injection container
    public static class IoCContainer
    {
        public static ServiceProvider Provider { get; set; }
    }
}
