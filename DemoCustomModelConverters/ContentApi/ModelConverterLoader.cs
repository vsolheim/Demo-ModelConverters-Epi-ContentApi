using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using DemoCustomModelConverters.Models;
using EPiServer.Web.WebControls;

namespace DemoCustomModelConverters.ContentApi
{
    /// <summary>
    /// Class that organizes all the ContentModelConverters used in ContentAPI transformation.
    /// </summary>
    public static class ModelConverterLoader
    {
        /// <summary>
        /// List of all converters. The key is Type.FullName. For a regular page that means page.GetType().FullName.
        /// </summary>
        private static Dictionary<string, IContentModelConverter> Converters { get; set; } = new Dictionary<string, IContentModelConverter>();

        /// <summary>
        /// Get the correct converter for the <paramref name="fullname"/> pagetype. Returns null if there is no converter for it.
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static IContentModelConverter GetConverter(string fullname)
        {
            var foundConverter = Converters.TryGetValue(fullname, out var converter);

            return foundConverter ? converter : null;
        }


        /// <summary>
        /// Scan the assembly for classes inheriting IContentModelConverter.
        /// </summary>
        public static void ScanForConverters()
        {
            IList<IContentModelConverter> converters = new List<IContentModelConverter>();

            try
            {
                var executingAssembbly = Assembly.GetExecutingAssembly()?.GetName().CodeBase;
                // Get only the DLLs. Retrieving the pdb- and xml-files will cause bad mojo (crashes). Substring(6) to cut away the file:/ prefix.
                var dlls = Directory.EnumerateFiles(Path.GetDirectoryName(executingAssembbly).Substring(6), "*.dll");

                foreach (var dll in dlls)
                {
                    // Read .dll's from a bytestream from so they don't get locked by the IIS process. Else the solution can't be rebuilt without reseting the IIS process.
                    var file = Assembly.Load(File.ReadAllBytes(dll));

                    // Get only classes that implement the interface.
                    var filteredTypes = file.GetExportedTypes()
                        .Where(t => !t.IsInterface && !t.IsAbstract)
                        .Where(t => typeof(IContentModelConverter).IsAssignableFrom(t));

                    // Activate them. Necessary to later find out which pagetype they support. 
                    var activatedConverters = filteredTypes.Select(t => (IContentModelConverter)Activator.CreateInstance(t));
                    foreach (var converter in activatedConverters)
                    {
                        converters.Add(converter);
                    }
                }

                // Store the converters with the type.FullName of the type they handle as the key.
                // Can't use the type as this type won't match the type of the pages in ExtendedContentModelMapper because they belong to "different" versions of the assembly.
                foreach (var converter in converters)
                {
                    Converters.Add(converter.HandlesType.FullName, converter);
                }
            }
            catch (Exception e)
            {
                // TODO: Log this failure.
            }
        }
    }
}