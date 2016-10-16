﻿using System.Collections.Generic;

using JSONProcessingHW.Logic.HtmlGenerator.Contracts;
using JSONProcessingHW.Logic.Models.Contracts;
using JSONProcessingHW.Logic.Parsers.Contracts;

namespace JSONProcessingHW.Logic
{
    public class DataParser : IDataParser
    {
        private readonly IXmlDocumentProvider xmlDocumentProvider;
        private readonly IXmlToJsonConverter xmlToJsonConverter;
        private readonly IJsonParser jsonParser;
        private readonly IHtmlGenerator htmlGenerator;
        private readonly IHtmlFileCreator htmlCreator;

        public DataParser(
            IXmlDocumentProvider xmlDocumentProvider,
            IXmlToJsonConverter xmlToJsonConverter,
            IJsonParser jsonParser,
            IHtmlGenerator htmlGenerator,
            IHtmlFileCreator htmlCreator)
        {
            this.xmlDocumentProvider = xmlDocumentProvider;
            this.xmlToJsonConverter = xmlToJsonConverter;
            this.jsonParser = jsonParser;
            this.htmlGenerator = htmlGenerator;
            this.htmlCreator = htmlCreator;
        }

        public void CreateHtml<ModelType>(string inputXmlFile, string outputHtmlFile)
            where ModelType : IModel, new()
        {
            var xmlDocument = this.xmlDocumentProvider.GetXmlDocument(inputXmlFile);
            var json = this.xmlToJsonConverter.ConvertXmlToJson(xmlDocument);
            var data = this.jsonParser.ParseJson<ModelType>(json, "feed", "entry");
            var html = this.htmlGenerator.GenerateHtml((IEnumerable<IModel>)data);
            this.htmlCreator.CreateHtmlFile(outputHtmlFile, "YouTube RSS", html);
        }
    }
}
