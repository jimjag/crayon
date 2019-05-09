﻿using System;
using System.Collections.Generic;

namespace AssemblyResolver
{
    internal class HttpRequestSender
    {
        private string method;
        private string url;
        private byte[] content;
        private string contentType;
        private Dictionary<string, string> requestHeaders = new Dictionary<string, string>();

        public HttpRequestSender(string method, string url)
        {
            this.method = method;
            this.url = url;
        }

        public HttpRequestSender SetContentBinary(byte[] value, string contentType)
        {
            this.content = value;
            this.contentType = contentType;
            return this;
        }

        public HttpRequestSender SetContentUtf8(string value, string contentType) { return this.SetContentBinary(System.Text.Encoding.UTF8.GetBytes(value), contentType); }
        public HttpRequestSender SetContentJson(string value) { return this.SetContentUtf8(value, "application/json"); }
        public HttpRequestSender SetContentBinaryOctetStream(byte[] value) { return this.SetContentBinary(value, "application/octet-stream"); }

        public HttpRequestSender SetHeader(string name, string value)
        {
            this.requestHeaders[name] = (value ?? "").Trim();
            return this;
        }

        private static readonly byte[] readBuffer = new byte[1000];

        public HttpResponse Send()
        {
            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(this.url);
            req.UserAgent = Common.VersionInfo.UserAgent;

            if (this.content != null)
            {
                req.ContentLength = this.content.Length;
                req.ContentType = this.contentType;
                using (System.IO.Stream stream = req.GetRequestStream())
                {
                    stream.Write(this.content, 0, this.content.Length);
                }
            }
            else
            {
                req.ContentLength = 0;
            }
            req.Method = this.method;

            foreach (string headerName in this.requestHeaders.Keys)
            {
                req.Headers.Add(headerName + ": " + this.requestHeaders[headerName]);
            }

            System.Net.HttpWebResponse response = null;
            try
            {
                response = (System.Net.HttpWebResponse)req.GetResponse();
            }
            catch (System.Net.WebException we)
            {
                response = (System.Net.HttpWebResponse)we.Response;
            }
            catch (Exception e)
            {
                throw new NotImplementedException("Cannot handle exception: " + e.GetType().ToString());
            }

            System.IO.Stream responseStream = response.GetResponseStream();
            System.IO.BinaryReader br = new System.IO.BinaryReader(responseStream);
            int bytesRead = 0;
            List<byte> responseContent = new List<byte>();
            do
            {
                bytesRead = br.Read(readBuffer, 0, readBuffer.Length);
                if (bytesRead == readBuffer.Length)
                {
                    responseContent.AddRange(readBuffer);
                }
                else
                {
                    for (int i = 0; i < bytesRead; ++i)
                    {
                        responseContent.Add(readBuffer[i]);
                    }
                }
            } while (bytesRead > 0);

            byte[] responseBytes = responseContent.ToArray();
            string responseContentType = response.ContentType;
            int statusCode = (int)response.StatusCode;

            List<string> headersRawPairs = new List<string>();
            int length = response.Headers.Count;
            for (int i = 0; i < length; ++i)
            {
                string key = response.Headers.GetKey(i);
                foreach (string value in response.Headers.GetValues(i))
                {
                    headersRawPairs.Add(key);
                    headersRawPairs.Add(value);
                }
            }

            return new HttpResponse(statusCode, responseContentType, responseBytes, headersRawPairs);
        }
    }
}
