using Microsoft.Extensions.DependencyInjection;
using WorstGamesStudios.DependencyInjection.Library.Container;
using WorstGamesStudios.DependencyInjection.Library.Core;
using WorstGamesStudios.DependencyInjection.Library.ServiceResolver;
using WorstGamesStudios.DependencyInjection.Library.Container.Extensions;

namespace AILibrary
{
    public class HostEnvironment : IHostEnvironment
    {
        public ApplicationBundle ApplicationBundle { get; }
        public string StreamingAssetsDataPath { get; }
        public string PersistentDataPath { get; }
    }

    public abstract class StartupBase : IStartup
    {
        private IServiceCollection serviceCollection;
        private IContainerCollection containerCollection;

        protected StartupBase()
        {
            serviceCollection = new ServiceCollection();
            containerCollection = new ContainerCollection();

            var env = new HostEnvironment();
            var app = new ApplicationBuilder();

            serviceCollection.AddSingleton(app);
            serviceCollection.AddSingleton(containerCollection);
            serviceCollection.AddSingleton<IHostEnvironment>(env);

            containerCollection.RegisterSubContainer<ServiceResolverContainer>();

            ConfigureSubContainers(containerCollection);

            BaseConfigureServices(serviceCollection);
            ConfigureServices(serviceCollection);
            containerCollection.ConfigureServices(serviceCollection);

            app.ApplicationServices = serviceCollection.BuildServiceProvider();

            BaseConfigure(app, env);
            Configure(app, env);
            containerCollection.Configure(app, env);
        }

        private void BaseConfigureServices(IServiceCollection serviceCollection)
        {

        }

        private void BaseConfigure(IApplicationBuilder app, IHostEnvironment env)
        {

        }

        protected abstract void ConfigureSubContainers(IContainerCollection containerCollection);


        public abstract void ConfigureServices(IServiceCollection serviceCollection);

        public abstract void Configure(IApplicationBuilder app, IHostEnvironment env);

        public virtual void Dispose()
        {
            containerCollection.Dispose();
        }
    }
}
