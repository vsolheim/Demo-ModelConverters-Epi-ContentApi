using System;
using DemoCustomModelConverters.Infrastructure;
using DemoCustomModelConverters.Models;
using DemoCustomModelConverters.Models.Baseclasses;
using DemoCustomModelConverters.Models.Pages.Start;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;
using EPiServer.Web.Routing;

namespace DemoCustomModelConverters.ContentApi.Converters
{
    public class StartPageConverter : BasePageConverter, IContentModelConverter
    {
        private IDummyInterface dummyClass;

        public StartPageConverter(IDummyInterface dummyInterface)
        {
            dummyClass = dummyInterface;
        } 

        public new Type HandlesType => typeof(StartPage);

        public new ContentApiModel TransformContent(IContentModelMapper defaultContentModelMapper, IContent content, bool excludePersonalizedContent = false, string expand = "")
        {
            // Because it calls the base.TransformContent(), all properties the baseclass adds will be available.
            var model = base.TransformContent(defaultContentModelMapper, content, excludePersonalizedContent, expand);

            // If you don't want the properties from BasePageConverter, just remove the line above and do the default transformation here.
            //var model = defaultContentModelMapper.TransformContent(content, excludePersonalizedContent, expand);

            // Add any additional properties here, or adjust what the baseclass has added.
            model.Properties.Add("Some key", "Som startpage-specific data");

            // To show that the dummyclass dependency has been injected.
            model.Properties.Add("Dummyclass", dummyClass.SomeText);

            return model;
        }
    }
}