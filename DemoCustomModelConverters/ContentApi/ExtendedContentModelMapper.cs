using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace DemoCustomModelConverters.ContentApi
{
    /// <summary>
    /// A decorator for the DefaultContentModelMapper. Need this to extend the returned models with custom properties.
    /// </summary>
    public class ExtendedContentModelMapper : IContentModelMapper
    {
        #region Fields and constructor

        private readonly IContentModelMapper _defaultContentModelMapper;
        private readonly IUrlResolver _urlResolver;
        private readonly ServiceAccessor<HttpContextBase> _httpContextAccessor;

        public ExtendedContentModelMapper(IContentModelMapper defaultContentModelMapper, IUrlResolver urlResolver, ServiceAccessor<HttpContextBase> httpContextAccessor)
        {
            _defaultContentModelMapper = defaultContentModelMapper;
            _urlResolver = urlResolver;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<IPropertyModelConverter> PropertyModelConverters { get; }
        #endregion

        /// <summary>
        /// The entry point where EPiServer ContentAPI lets us do something with the data before and after it is transformed.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="excludePersonalizedContent"></param>
        /// <param name="expand"></param>
        /// <returns></returns>
        public ContentApiModel TransformContent(IContent content, bool excludePersonalizedContent = false, string expand = "")
        {
            ContentApiModel contentModel;
            
            // It's GetType().BaseType.FullName because the content comes here as proxies.
            // Can't compare the types because the types ModelConverterLoader has and the types from here, while technically being the same type, belong to different "versions" of the assembly and thus won't hit.
            // Therefore comparing the namespace is more reliable.
            var converter = ModelConverterLoader.GetConverter(content.GetType().BaseType.FullName);

            // If it has a converter, use that converter. Else use the default.
            if (converter != null)
            {
                contentModel = converter.TransformContent(_defaultContentModelMapper, content, excludePersonalizedContent, expand);
            }
            else
            {
                contentModel = _defaultContentModelMapper.TransformContent(content, excludePersonalizedContent, expand);
            }

            // Flatten the properties to make it more convenient to use. Meaning remove the propety types to remove an unnecessary layer in the JSON.
            contentModel.Properties = contentModel.Properties.Select(p => FlattenProperty(p)).ToDictionary(x => x.Key, x => x.Value);


            return contentModel;
        }

        
        #region Private flattening methods.
        /// <summary>
        /// Flattens the property, i.e. mostly just removing the object layer and returning only the value.
        /// Example: instead of "title": { "value": "Startpage title", "propertyDataType": "PropertyLongString" },
        /// it just makes it "title": "Startpage Title"
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private KeyValuePair<string, object> FlattenProperty(KeyValuePair<string, object> property)
        {
            return new KeyValuePair<string, object>(property.Key, FlattenPropertyValue(property.Value));
        }

        /// <summary>
        /// Extract the actual value from the property model.
        /// </summary>
        private static object FlattenPropertyValue(object propertyValue)
        {
            switch (propertyValue)
            {
                case ContentAreaPropertyModel contentAreaModel:
                    return contentAreaModel?.ExpandedValue?.Select(p => ConvertContentAreaItem(p, contentAreaModel));
                case PropertyModel<string, PropertyString> stringModel:
                    return stringModel.Value;
                case PropertyModel<string, PropertyUrl> urlModel:
                    return urlModel.Value;
                case PropertyModel<DateTime?, PropertyDate> dateTimeModel:
                    return dateTimeModel.Value;
                case PropertyModel<bool?, PropertyBoolean> booleanModel:
                    return booleanModel.Value;
                case PropertyModel<string, PropertyLongString> longStringModel:
                    return longStringModel.Value;
                case PropertyModel<string, PropertyXhtmlString> xhtmlStringModel:
                    return xhtmlStringModel.Value;
                case CategoryPropertyModel propertyModel:
                    return propertyModel.Value;
                default:
                    return propertyValue;
            }
        }

        /// <summary>
        /// NOTE: This is from Music Festival sample. No idea whether we have any use for it.
        /// We need to extend the model for content areas with available display options so our component will get a correct css class.
        /// </summary>
        private static object ConvertContentAreaItem(ContentApiModel contentApiModel, ContentAreaPropertyModel contentArea)
        {
            var contentModelDisplayOption = contentArea.Value.FirstOrDefault(i => i.ContentLink.Id == contentApiModel.ContentLink.Id)?.DisplayOption;

            contentApiModel.Properties.Add("displayOption", contentModelDisplayOption);

            return contentApiModel;
        }

        #endregion
    }
}