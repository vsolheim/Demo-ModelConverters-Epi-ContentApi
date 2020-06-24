using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DemoCustomModelConverters.Infrastructure;
using DemoCustomModelConverters.Infrastructure.Initialization;
using DemoCustomModelConverters.Models;
using EPiServer.Framework.Internal;
using StructureMap;

namespace DemoCustomModelConverters.ContentApi
{
    /// <summary>
    /// Class that organizes all the ContentModelConverters used in ContentAPI transformation.
    /// </summary>
    public static class ModelConverterLoader
    {
        // This is really really ugly, but it's the only way I've found to reliably be able to inject EPiServer's own dependencies into converters. If it's done during configuration, those dependencies aren't set. 
        private static IContainer Container;
        private static bool HaveScanned;
        public static void SetContainer(IContainer container)
        {
            Container = container;
        }


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
            if (HaveScanned == false)
            {
                ScanForConverters(Container);
                HaveScanned = true;
            }

            var foundConverter = Converters.TryGetValue(fullname, out var converter);

            if (foundConverter)
            {
                return converter;
            }

            return null;
        }


        /// <summary>
        /// Scan the assembly for classes inheriting IContentModelConverter.
        /// </summary>
        private static void ScanForConverters(IContainer container)
        {
            try
            {
                container.Configure(c => c.Scan(s =>
                {
                    s.AssembliesFromApplicationBaseDirectory();
                    s.AddAllTypesOf<IContentModelConverter>();
                }));

                var converters = container.GetAllInstances<IContentModelConverter>();


                foreach (var converter in converters)
                {
                    Converters.Add(converter.HandlesType.FullName, converter);
                }
            }
            catch (Exception)
            {
                // Log this
            }
        }
    }
}