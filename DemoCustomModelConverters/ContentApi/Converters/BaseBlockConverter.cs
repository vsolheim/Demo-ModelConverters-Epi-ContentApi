using System;
using DemoCustomModelConverters.Models.Baseclasses;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;

namespace DemoCustomModelConverters.ContentApi.Converters
{
    public class BaseBlockConverter : IContentModelConverter
    {
        public Type HandlesType => typeof(BaseBlock);

        public ContentApiModel TransformContent(IContentModelMapper defaultContentModelMapper, IContent content, bool excludePersonalizedContent = false, string expand = "")
        {
            return defaultContentModelMapper.TransformContent(content, excludePersonalizedContent, expand);
        }
    }
}