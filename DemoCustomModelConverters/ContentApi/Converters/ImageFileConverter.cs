using System;
using DemoCustomModelConverters.Models;
using DemoCustomModelConverters.Models.Media;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;

namespace DemoCustomModelConverters.ContentApi.Converters
{
    public class ImageFileConverter : IContentModelConverter
    {
        public Type HandlesType => typeof(ImageFile);

        public ContentApiModel TransformContent(IContentModelMapper defaultContentModelMapper, IContent content, bool excludePersonalizedContent = false, string expand = "")
        {
            // Because it calls the base.TransformContent(), all properties the baseclass adds will be available.
            return defaultContentModelMapper.TransformContent(content, excludePersonalizedContent, expand);

            // Add any additional properties here.
        }
    }
}