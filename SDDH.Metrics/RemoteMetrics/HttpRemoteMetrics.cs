﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SDDH.Metrics.Json;

namespace SDDH.Metrics.RemoteMetrics
{
    public static class HttpRemoteMetrics
    {
        private class CustomClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                return request;
            }
        }

        public static JsonMetricsContext FetchRemoteMetrics(Uri remoteUri, Func<string, JsonMetricsContext> deserializer, CancellationToken token)
        {
            using (CustomClient client = new CustomClient())
            {
                client.Headers.Add("Accept-Encoding", "gizp");
                var json = Metrics.Visualization.TaskHelper.RunAsync<string>(() => { return client.DownloadString(remoteUri); }, null).Result;
                return deserializer(json);
            }
        }
    }
}
