## Demo of custom transformation of IContent for EPiServer ContentDeliveryAPI

This project shows how the EPiServer ContentDeliveryAPI can be customized to easily add new properties to ContentApiModels before they are sent to the client. It is a minimum demo with only the required classes to make it work, plus a few converters to show how to use it.

This demo should work and be good to go as is, but I naturally can't guarantee it is without issues. If you discover any, please tell me and I'll update the code.

It uses the regular ContentDeliveryApi's `IContentModelMapper` to do the initial transformation from IContent to the ContentApiModel, therefore requiring essentially zero maintenance, then adds some code to add additional properties. It scans the assemblies for any file implementing the required interface, eliminating the need to register them. Since pages, blocks and media files all implement IContent, you can add properties to all of them.

### What it does is:
* During startup it scans the assembly for any classes implementing a specific interface.
* When a page is requested through ContentDeliveryApi it enters the ExtendedContentModelMapper, which finds the correct converter for that IContent.
* The converter uses the default IContentModelMapper for initial transformation, which makes this pattern essentially maintenance-free, then you can add any additional data to the ContentApiModel.
* Afterwards the resulting ContentApiModel is sent to the client as usual.

---
The project supports dependency injection with StructureMap as the IoC tool.
If you wish to use another tool, or no injection, the basic code is on the branch `Basic_without_dependency_injection`.

---

## Notable files

* [IContentModelConverter](/DemoCustomModelConverters.Models/IContentModelConverter.cs) : identifies the converters to be discovered during startup. 
If you decide to use the basic version without dependency injection, it is very important that this interface is in another project in the solution than the converters, else the assembly scanning won't work. I'm not entirely sure of the reason, but it seems to be due to different Type.Module.FullyQualifiedAssemblyName. If you have a better explanation, please do tell.

* [ExtendedContentModelMapper.cs](/DemoCustomModelConverters/ContentApi/ExtendedContentModelMapper.cs) : Intercepts the IContent transformation. File is mostly as in EPiServer's own Music Festival demo.

* [BasePageConverter](/DemoCustomModelConverters/ContentApi/Converters/BasePageConverter.cs) (and the other converters) : Handles the transformation. It first uses EPiServer's own mapper, then leaves you free to add additional properties.

* [ContentApiInitializationModule.cs](/DemoCustomModelConverters/Infrastructure/Initialization/ContentApiInitializationModule.cs) : Registers the `ExtendedContentModelMapper.cs` and starts the scan for converters.

* [ModelConverterLoader.cs](/DemoCustomModelConverters/ContentApi/ModelConverterLoader.cs) : Scans the assembly for converters.

---

This project is licensed under the terms of the MIT license.
