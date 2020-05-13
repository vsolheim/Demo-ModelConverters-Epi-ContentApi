using System;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;

namespace DemoCustomModelConverters.ContentApi
{
    /// <summary>
    /// Classes with this can convert IContent to an ContentApiModel.
    /// </summary>
    public interface IContentModelConverter
    {
        Type HandlesType { get; }

        /// <summary>
        /// Convert the page <paramref name="content"/> to ContentApiModel.
        /// See the BasePageConverter or BlockTypeConverter on how to use it.
        /// </summary>
        /// <param name="defaultContentModelMapper"></param>
        /// <param name="content"></param>
        /// <param name="excludePersonalizedContent"></param>
        /// <param name="expand"></param>
        /// <returns></returns>
        ContentApiModel TransformContent(IContentModelMapper defaultContentModelMapper, IContent content, bool excludePersonalizedContent = false, string expand = "");
    }
}
