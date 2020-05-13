using System;
using DemoCustomModelConverters.Models.Pages.Article;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;

namespace DemoCustomModelConverters.ContentApi.Converters
{
    public class ArticlePageConverter : BasePageConverter, IContentModelConverter
    {
        public new Type HandlesType => typeof(ArticlePage);

        public new ContentApiModel TransformContent(IContentModelMapper defaultContentModelMapper, IContent content, bool excludePersonalizedContent = false, string expand = "")
        {
            return base.TransformContent(defaultContentModelMapper, content, excludePersonalizedContent, expand);
        }
    }
}