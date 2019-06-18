﻿using System.Collections.Generic;
using System.Linq;
using Coderr.Client.Contracts;
using Coderr.Client.Reporters;

namespace Coderr.Client.ContextCollections
{
    /// <summary>
    /// Extensions to get the Coderr collection that we store meta data in
    /// </summary>
    public static class CoderrContextCollectionExtensions
    {
        /// <summary>
        /// Get or create our collection
        /// </summary>
        /// <param name="context">context to find the collection in</param>
        /// <returns>collection</returns>
        public static ContextCollectionDTO GetCoderrCollection(this IErrorReporterContext context)
        {
            var collection = context.ContextCollections.FirstOrDefault(x => x.Name == "CoderrData");
            if (collection != null)
                return collection;

            collection = new ContextCollectionDTO("CoderrData");
            context.ContextCollections.Add(collection);
            return collection;
        }

        /// <summary>
        /// Get or create our collection
        /// </summary>
        /// <param name="collections">Collections array</param>
        /// <returns>collection</returns>
        public static ContextCollectionDTO GetCoderrCollection(this IList<ContextCollectionDTO> collections)
        {
            var collection = collections.FirstOrDefault(x => x.Name == "CoderrData");
            if (collection != null)
                return collection;

            collection = new ContextCollectionDTO("CoderrData");
            collections.Add(collection);
            return collection;
        }


        public static void AddTag(this IList<ContextCollectionDTO> collections, string tagName)
        {
            var coderrCollection = GetCoderrCollection(collections);
            if (!coderrCollection.Properties.TryGetValue(CoderrCollectionProperties.Tags, out var tags))
            {
                coderrCollection.Properties[CoderrCollectionProperties.Tags] = tagName;
                return;
            }

            if (!tags.Contains(tagName))
                coderrCollection.Properties[CoderrCollectionProperties.Tags] = $"{tags},{tagName}";
        }

        public static void AddQuickFact(this IList<ContextCollectionDTO> collections, string propertyName, string propertyValue)
        {
            var coderrCollection = GetCoderrCollection(collections);

            var name = CoderrCollectionProperties.QuickFact.Replace("{Name}", propertyName);
            var value = propertyValue;
            coderrCollection.Properties[name] = value;
        }

        public static void AddHighlightedProperty(this IList<ContextCollectionDTO> collections, string contextCollectionName, string propertyName)
        {
            var coderrCollection = GetCoderrCollection(collections);

            var value = $"{contextCollectionName}.{propertyName}";
            if (!coderrCollection.Properties.TryGetValue(CoderrCollectionProperties.HighlightProperties, out var values))
            {
                coderrCollection.Properties[CoderrCollectionProperties.HighlightProperties] = value;
                return;
            }

            if (!values.Contains(value))
                coderrCollection.Properties[CoderrCollectionProperties.HighlightProperties] = $"{values},{value}";
        }


        public static void AddHighlightedCollection(this IList<ContextCollectionDTO> collections, string contextCollectionName)
        {
            var coderrCollection = GetCoderrCollection(collections);

            var value = contextCollectionName;
            if (!coderrCollection.Properties.TryGetValue(CoderrCollectionProperties.HighlightCollection, out var values))
            {
                coderrCollection.Properties[CoderrCollectionProperties.HighlightCollection] = value;
                return;
            }

            if (!values.Contains(value))
                coderrCollection.Properties[CoderrCollectionProperties.HighlightCollection] = $"{values},{value}";
        }


        public static void AddTag(this IErrorReporterContext context, string tagName)
        {
            AddTag(context.ContextCollections, tagName);
        }

        public static void AddHighlightedProperty(this IErrorReporterContext context, string contextCollectionName, string propertyName)
        {
            AddHighlightedProperty(context.ContextCollections, contextCollectionName, propertyName);
        }

        public static void AddHighlightedCollection(this IErrorReporterContext context, string contextCollectionName)
        {
            AddHighlightedCollection(context.ContextCollections, contextCollectionName);
        }

        public static void AddQuickFact(this IErrorReporterContext context, string propertyName, string propertyValue)
        {
            AddQuickFact(context.ContextCollections, propertyName, propertyValue);
        }

    }
}
