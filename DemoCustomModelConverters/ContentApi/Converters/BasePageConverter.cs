using System;
using DemoCustomModelConverters.Models.Baseclasses;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;

namespace DemoCustomModelConverters.ContentApi.Converters
{
    public class BasePageConverter : IContentModelConverter
    {
        public BasePageConverter()
        {
        }

        public Type HandlesType => typeof(BasePage);

        public ContentApiModel TransformContent(IContentModelMapper defaultContentModelMapper, IContent content, bool excludePersonalizedContent = false, string expand = "")
        {
            return defaultContentModelMapper.TransformContent(content, excludePersonalizedContent, expand);
        }
    }
}