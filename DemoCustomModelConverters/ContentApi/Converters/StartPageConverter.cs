using System;
using DemoCustomModelConverters.Models;
using DemoCustomModelConverters.Models.Pages.Start;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;

namespace DemoCustomModelConverters.ContentApi.Converters
{
    public class StartPageConverter : BasePageConverter, IContentModelConverter
    {
        public new Type HandlesType => typeof(StartPage);

        public new ContentApiModel TransformContent(IContentModelMapper defaultContentModelMapper, IContent content, bool excludePersonalizedContent = false, string expand = "")
        {
            // Because it calls the base.TransformContent(), all properties the baseclass adds will be available.
            var model = base.TransformContent(defaultContentModelMapper, content, excludePersonalizedContent, expand);

            // Add any additional properties here, or adjust what the baseclass has added.
            model.Properties.Add("Some key", "Som startpage-specific data");

            return model;
        }
    }
}