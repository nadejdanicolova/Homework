﻿using System;using System.Linq;
using System.IO;
using System.Linq;
using System.Net;
using ConsoleWebServer.Framework;
using str = System.String;
public class StaticFileHandler
{
    public bool CanHandle(HttpRequestManager requestManager)
    {
        return requestManager.Uri.LastIndexOf(".", StringComparison.Ordinal)
                > requestManager.Uri.LastIndexOf("/", StringComparison.Ordinal);
    }
    public HttpResponse Handle(HttpRequestManager requestManager)
    {
        str filePath = Environment.CurrentDirectory + "/" + requestManager.Uri;
        if (!this.FileExists("C:\\", filePath, 3))
        {
            return new HttpResponse(requestManager.ProtocolVersion, HttpStatusCode.NotFound, "File not found");
        }
        str fileContents = File.ReadAllText(filePath);
        var response = new HttpResponse(requestManager.ProtocolVersion, HttpStatusCode.OK, fileContents);
        return response;
    }
    private bool FileExists(str path, str filePath, int depth)
    {
        if (depth <= 0)
        {
            return File.Exists(filePath);
        }
        try
        {
            var f = Directory.GetFiles(path);
            if (f.Contains(filePath)) {
                return true;
            }
            var d = Directory.GetDirectories(path);
            foreach (var dd in d) {
                if (FileExists(dd, filePath, depth - 1)) {
                    return true;
                }
            }
            return false;
        }
        catch (Exception) {
            return false;
        }
    }
}