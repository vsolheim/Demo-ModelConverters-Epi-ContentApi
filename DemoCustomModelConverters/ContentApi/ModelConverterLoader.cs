using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
        public static Dictionary<string, IContentModelConverter> Converters { get; set; } = new Dictionary<string, IContentModelConverter>();

        /// <summary>
        /// Get the correct converter for the <paramref name="fullname"/> pagetype. Returns null if there is no converter for it.
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static IContentModelConverter GetConverter(string fullname)
        {
            var foundConverter = Converters.TryGetValue(fullname, out var converter);

            if (foundConverter)
            {
                return converter;
            }

            return null;
        }


        /// <summary>
        /// Scan the assembly for classes inheriting IContentModelConverter.
        /// Uses code to load the assembly borrowed from here: https://www.codeproject.com/Tips/836907/Loading-Assembly-to-Leave-Assembly-File-Unlocked
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
                    // Code borrowed from alternative 1 here: https://www.codeproject.com/Tips/836907/Loading-Assembly-to-Leave-Assembly-File-Unlocked
                    var file = Assembly.Load(File.ReadAllBytes(dll));

                    // Get only classes.
                    var types = file.GetExportedTypes().Where(t => !t.IsInterface && !t.IsAbstract);

                    //// Then filter down to only classes implementing the interface.
                    var filteredTypes = types.Where(t => typeof(IContentModelConverter).IsAssignableFrom(t));

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