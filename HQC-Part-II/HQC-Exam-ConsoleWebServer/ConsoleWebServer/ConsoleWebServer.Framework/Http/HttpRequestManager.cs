﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ConsoleWebServer.Framework.Http.Contracts;
using ConsoleWebServer.Framework.Http.Exceptions;

namespace ConsoleWebServer.Framework.Http
{
    // TODO: Split Parse and object.
    public class HttpRequestManager : IHttpRequestManager, IHttpRequest
    {
        public HttpRequestManager()
        {
        }

        private HttpRequestManager(string method, string uri, string httpVersion)
        {
            this.ProtocolVersion = Version.Parse(httpVersion.ToLower().Replace("HTTP/".ToLower(), string.Empty));
            this.Headers = new SortedDictionary<string, ICollection<string>>();
            this.Uri = uri;
            this.Method = method;
            this.Action = new HttpRequestWords(uri);
        }

        public HttpRequestWords Action { get; private set; }

        public string Method { get; private set; }

        public Version ProtocolVersion { get; protected set; }

        public IDictionary<string, ICollection<string>> Headers { get; protected set; }

        public string Uri { get; private set; }

        public IHttpRequest Parse(string reqAsStr)
        {
            var textReader = new StringReader(reqAsStr);
            var firstLine = textReader.ReadLine();
            var requestObject = this.CreateRequest(firstLine);

            string line;
            while ((line = textReader.ReadLine()) != null)
            {
                this.AddHeaderToRequest(requestObject, line);
            }

            return requestObject;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(
                string.Format(
                    "{0} {1} {2}{3}",
                    this.Method,
                    this.Action,
                    "HTTP/",
                    this.ProtocolVersion));

            var headerStringBuilder = new StringBuilder();
            foreach (var key in this.Headers.Keys)
            {
                headerStringBuilder.AppendLine(string.Format("{0}: {1}", key, string.Join("; ", this.Headers[key])));
            }

            sb.AppendLine(headerStringBuilder.ToString());
            return sb.ToString();
        }

        private void AddHeader(string name, string valueValueValue)
        {
            if (!this.Headers.ContainsKey(name))
            {
                this.Headers.Add(name, new HashSet<string>(new List<string>()));
            }

            this.Headers[name].Add(valueValueValue);
        }

        private HttpRequestManager CreateRequest(string firstRequestLine)
        {
            var firstRequestLineParts = firstRequestLine.Split(' ');
            if (firstRequestLineParts.Length != 3)
            {
                throw new HttpNotFoundException.ParserException(
                    "Invalid format for the first requestManager line. Expected format: [Method] [Uri] HTTP/[Version]");
            }
            var requestObject = new HttpRequestManager(
                firstRequestLineParts[0],
                firstRequestLineParts[1],
                firstRequestLineParts[2]);

            return requestObject;
        }

        private void AddHeaderToRequest(HttpRequestManager r, string headerLine)
        {
            var hp = headerLine.Split(new[] { ':' }, 2);
            var hn = hp[0].Trim();
            var hv = hp.Length == 2 ? hp[1].Trim() : string.Empty;
            r.AddHeader(hn, hv);
        }
    }
}