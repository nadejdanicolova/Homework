﻿using System;
using System.Collections.Generic;
using System.Linq;

using JSONProcessingHW.Logic.Models.Contracts;
using JSONProcessingHW.Logic.Parsers.Contracts;

using Newtonsoft.Json.Linq;

namespace JSONProcessingHW.Logic.Parsers
{
    public class JsonParser : IJsonParser
    {
        public IEnumerable<ModelType> ParseJson<ModelType>(
            string json,
            string rootName,
            string elementName,
           IJTokenValueExtractor titleCallback,
           IJTokenValueExtractor urlCallback)
            where ModelType : IModel, new()
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (rootName == null)
            {
                throw new ArgumentNullException(nameof(rootName));
            }

            if (elementName == null)
            {
                throw new ArgumentNullException(nameof(elementName));
            }

            var jsonObject = JObject.Parse(json);
            var rootElement = jsonObject.SelectToken(rootName);
            var modelElements = rootElement.SelectTokens(elementName);

            var modelElementsChildren = modelElements.Children();
            var result = modelElementsChildren.Select(element => new ModelType
            {
                Title = titleCallback.ExtractJTokenValue(element),
                Url = urlCallback.ExtractJTokenValue(element)
            })
                .ToList();

            return result;
        }
    }
}
