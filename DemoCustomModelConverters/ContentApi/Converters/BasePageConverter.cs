using System;
using DemoCustomModelConverters.Models;
using DemoCustomModelConverters.Models.Baseclasses;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;

namespace DemoCustomModelConverters.ContentApi.Converters
{
    public class BasePageConverter : IContentModelConverter
    {
        public Type HandlesType => typeof(BasePage);

        public ContentApiModel TransformContent(IContentModelMapper defaultContentModelMapper, IContent content, bool excludePersonalizedContent = false, string expand = "")
        {
            var model = defaultContentModelMapper.TransformContent(content, excludePersonalizedContent, expand);

            // Add any additional properties here.
            // This could for example be a footer.
            model.Properties.Add("Footer", "Some footer data");
            return model;
        }
    }
}