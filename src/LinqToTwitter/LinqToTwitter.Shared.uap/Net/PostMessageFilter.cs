﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace LinqToTwitter.Net
{
    class PostMessageFilter : IHttpFilter
    {
        readonly IHttpFilter innerFilter;
        readonly TwitterExecute exe;
        readonly IDictionary<string, string> parameters;
        readonly string url;

        public PostMessageFilter(TwitterExecute exe, IDictionary<string, string> parameters, string url, IHttpFilter innerFilter)
        {
            this.exe = exe;
            this.parameters = parameters;
            this.url = url;

            if (innerFilter == null)
            {
                throw new ArgumentException("innerFilter cannot be null.");
            }
            this.innerFilter = innerFilter;
        }

        public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(HttpRequestMessage request)
        {
            return AsyncInfo.Run<HttpResponseMessage, HttpProgress>(async (cancellationToken, progress) =>
            {
                exe.SetAuthorizationHeader(HttpMethod.Post, url, parameters, request);
                request.Headers.Add("User-Agent", exe.UserAgent);
                request.Headers.Add("Expect", "100-continue");
                request.Headers.Add("Accept-Encoding", "gzip");
                request.Headers.Add("Cache-Control", "no-cache");

                return await innerFilter.SendRequestAsync(request).AsTask(cancellationToken, progress);
            });
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
