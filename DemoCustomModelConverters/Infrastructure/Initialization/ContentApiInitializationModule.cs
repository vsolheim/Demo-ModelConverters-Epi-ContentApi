using System.Web;
using DemoCustomModelConverters.ContentApi;
using EPiServer.ContentApi.Cms;
using EPiServer.ContentApi.Core.Configuration;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

namespace DemoCustomModelConverters.Infrastructure.Initialization
{
    /// <summary>
    /// Initializationmodule for ONLY things related to ContentApi. If it's not ContentApi, use the other initializationmodule(s).
    /// </summary>
    [InitializableModule]
    [ModuleDependency(typeof(ServiceContainerInitialization), typeof(ContentApiCmsInitialization))]
    public class ContentApiInitializationModule : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            // Register the extended contentmodelmapper to be able to provide custom models from the content APi.
            context.Services.Intercept<IContentModelMapper>((locator, defaultModelMapper) =>
                new ExtendedContentModelMapper(
                    defaultModelMapper,
                    locator.GetInstance<IUrlResolver>(),
                    locator.GetInstance<ServiceAccessor<HttpContextBase>>())
            );

            // Set minimumRoles to empty to allow anonymous calls (for visitors to view site in view mode)
            context.Services.Configure<ContentApiConfiguration>(config =>
            {
                config.Default().SetMinimumRoles(string.Empty);
            });

            // Scan for all classes implementing IContentModelConverter. I.e. being able to convert IContent for ContentAPI.
            ModelConverterLoader.ScanForConverters();
        }


        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}