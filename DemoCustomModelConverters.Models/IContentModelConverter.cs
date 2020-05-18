using System;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;

namespace DemoCustomModelConverters.Models
{
    // As mentioned, this interface must be in another project in the solution than the converters.
    // I think this is because the module.FullyQualifiedAssemblyName on the interface set in the converter classes' GetInterfaces() is set to unknown according to Visual Studio debugging and therefore comparing and casting the classes to the interface is not possible.
    // If anyone have a better explanation, please do tell.


    /// <summary>
    /// Classes with this can convert IContent to a ContentApiModel. This works for all types of IContent, both pages, blocks and images.
    /// <remarks>All converters must have a parameterless constructor. There's no support yet for constructor injection.</remarks>
    /// <remarks>This interface MUST be in another project in the solution than the converters</remarks>
    /// </summary>
    public interface IContentModelConverter
    {
        /// <summary>
        /// Necessary for the ModelConverterLoader to know which converter to select.
        /// </summary>
        Type HandlesType { get; }

        /// <summary>
        /// Convert the page <paramref name="content"/> to ContentApiModel.
        /// See the converter examples for how to use it.
        /// </summary>
        /// <param name="defaultContentModelMapper"></param>
        /// <param name="content"></param>
        /// <param name="excludePersonalizedContent"></param>
        /// <param name="expand"></param>
        /// <returns></returns>
        ContentApiModel TransformContent(IContentModelMapper defaultContentModelMapper, IContent content, bool excludePersonalizedContent = false, string expand = "");
    }
}
