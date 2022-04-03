using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Lotlab.PluginCommon.Updater
{
    public class HttpWrapper
    {
        public static HttpWebResponse GetResponse(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            return (HttpWebResponse)request.GetResponse();
        }

        public static async Task<HttpWebResponse> GetResponseAsync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            return (HttpWebResponse)await request.GetResponseAsync();
        }

        public static string Get(string uri)
        {
            using (HttpWebResponse response = GetResponse(uri))
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static async Task<string> GetAsync(string uri)
        {
            using (HttpWebResponse response = await GetResponseAsync(uri))
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public static void GetFile(string uri, string path)
        {
            using (var fs = File.OpenWrite(path))
            {
                using (HttpWebResponse response = GetResponse(uri))
                using (Stream stream = response.GetResponseStream())
                    stream.CopyTo(fs);
            }
        }

        public static async Task GetFileAsync(string uri, string path)
        {
            using (var fs = File.OpenWrite(path))
            {
                using (HttpWebResponse response = await GetResponseAsync(uri))
                using (Stream stream = response.GetResponseStream())
                {
                    await stream.CopyToAsync(fs);
                }
            }
        }
    }
}
