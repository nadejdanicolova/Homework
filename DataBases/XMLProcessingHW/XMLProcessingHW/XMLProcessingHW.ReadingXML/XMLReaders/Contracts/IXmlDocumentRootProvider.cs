﻿using System.Xml;

namespace XMLProcessingHW.ReadingXML.XMLReaders.Contracts
{
    public interface IXmlDocumentProvider
    {
        XmlDocument GetXmlDocument(string fileName);
    }
}
