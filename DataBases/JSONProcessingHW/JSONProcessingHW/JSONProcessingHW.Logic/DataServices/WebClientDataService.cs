﻿using System;
using System.IO;
using System.Net;

using JSONProcessingHW.Logic.DataServices.Contracts;

namespace JSONProcessingHW.Logic.DataServices
{
    public class WebClientDataService : IDataService
    {
        public void GetData(string url, string fileName)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var fileExists = File.Exists(fileName);
            if (!fileExists)
            {
                var client = new WebClient();
                client.DownloadFile(url, fileName);
            }
        }
    }
}
