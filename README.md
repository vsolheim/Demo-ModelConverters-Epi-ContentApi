## Demo of custom conversion of IContent for EPiServer ContentDeliveryAPI

This project shows how the EPiServer ContentDeliveryAPI can be customized to easily add new properties to ContentApiModels before they are sent to the client. It is a minimum demo with only the required classes to make it work, plus some converters to show how to use it.

I've tested this version briefly and it should work and be good to go as is, but I naturally can't guarantee it is without issues. If you discover any, please tell me and I'll update the code.

This code uses the regular ContentDeliveryApi's `IContentModelMapper` to do the initial transformation from IContent to the ContentApiModel, therefore requiring essentially zero maintenance, then adds the possibility to easily add additional properties to the model before it is sent to the client. The code scans the assemblies for any file implementing the necessary interface, eliminating the need to register them. 

## Requirements
There are five important pieces. I'll describe each file in turn. The code itself also contain quite a bit of comments and some code is more expanded than it need be to make it more readable.

### IContentModelConverter
This identifies the converters so that they are discovered during startup. This contains the two methods any converter must implement in other to work. What they do is explained later. 

**It is very important that this interface is in another project in the solution than the converters, else the assembly scanning won't work.** I'm not entirely sure of the reason, but it seems to be due to different Type.Module.FullyQualifiedAssemblyName. If you have a better explanation, please do tell.
``` C#
Type HandlesType { get; }

ContentApiModel TransformContent(IContentModelMapper defaultContentModelMapper, IContent content, bool excludePersonalizedContent = false, string expand = "");
```

### ExtendedContentModelMapper.cs 
This is much the same as in EPiServer's own Music Festival demo, but contains some code to get and use the converters. If there is no converter for a given IContent it falls back to the default ContentDeliveryApi transformation. It uses `content.GetType().BaseType.FullName` because content is sent in as a proxy class for the IContent, and even the .BaseType won't hit the IContent-type that the converter say it handles. FullName works consistently.
```C#
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
    return contentModel;
}
```

### BasePageConverter
This handles the actual transformation. The `HandlesType()` method tells which type of IContent the converter supports. `ModelConverterLoader.cs` uses this to decide which converter to use. `TransformContent()` naturally handles the transformation. It first uses EPiServer's own mapper, then leaves you free to add additional properties. In this base-converter you could add properties all pages should have.
``` C#
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
```

The converters support an optional hierarchy to use the properties of the ancestor converters. This is from the `StartPageConverter.cs` which inherits the `BasePageConverter.cs`. Here it calls base.TransformContent to handle the initial transformation and adding properties, then `StartPageConverter `adds its own properties. 
``` C#
public new ContentApiModel TransformContent(IContentModelMapper defaultContentModelMapper, IContent content, bool excludePersonalizedContent = false, string expand = "")
        {
            // Because it calls the base.TransformContent(), all properties the baseclass adds will be available.
            var model = base.TransformContent(defaultContentModelMapper, content, excludePersonalizedContent, expand);

            // Add any additional properties here, or adjust what the baseclass has added.
            model.Properties.Add("Some key", "Som startpage-specific data");

            return model;
        }
```
If the properties from `BasePageConverter.cs` aren't needed, skip inheriting it and do the default transformation here instead of in the base converterter. 
``` C# 
// var model = base.TransformContent(defaultContentModelMapper, content, excludePersonalizedContent, expand);
var model = defaultContentModelMapper.TransformContent(content, excludePersonalizedContent, expand);
```

### ContentApiInitializationModule.cs 
This registers the ExtendedContentModelMapper and starts the scan for ContentModelConverters, 
``` C#
public class ContentApiInitializationModule : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            // Scan for all classes implementing IContentModelConverter. I.e. being able to convert IContent for ContentAPI.
            ModelConverterLoader.ScanForConverters();
        }
```

### ModelConverterLoader.cs
This is where the magic of discovering the converters happen. At startup - since `ScanForConverters()` is called in the InitializationModule - it scans all .dll-files for any classes implementing `IContentModelConverter`. The code is a bit longer than is convenient in the README, so just check it out in  `/ContentApi/ModelConverterLoader.cs`.

In short. `GetConverter()` returns the correct converter for a given IContent. It returns only exact hits; giving it `StartPage.GetType().FullName` will not return the `BasePageConverter`. `ScanForConverters()` scans the assembly for any converters. 
