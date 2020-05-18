using System;
using DemoCustomModelConverters.Models;
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
            var model = defaultContentModelMapper.TransformContent(content, excludePersonalizedContent, expand);

            // Add any additional properties here.
            model.Properties.Add("Some key", "Some data that all blocks should have");
            return model;
        }
    }
}