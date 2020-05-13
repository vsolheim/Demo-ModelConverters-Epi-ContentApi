using System;
using DemoCustomModelConverters.Models.Pages.Start;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;

namespace DemoCustomModelConverters.ContentApi.Converters
{
    public class StartPageConverter : BasePageConverter, IContentModelConverter
    {
        public StartPageConverter()
        {
        }

        public new Type HandlesType => typeof(StartPage);

        public new ContentApiModel TransformContent(IContentModelMapper defaultContentModelMapper, IContent content, bool excludePersonalizedContent = false, string expand = "")
        {
            var temp = base.TransformContent(defaultContentModelMapper, content, excludePersonalizedContent, expand);

            temp.Properties.Add("this is key", "this is value");
            return temp;
        }
    }
}