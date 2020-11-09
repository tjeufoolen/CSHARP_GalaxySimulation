using System.Collections.Generic;
using System.IO;
using System.Net;

namespace FlatGalaxy.FileHandling
{
    /// <summary>
    /// 
    /// ## Concern
    /// 
    /// Strategy pattern http file loader. 
    /// Starts when they used a link to an online file in the upload page.
    ///
    /// ## Resources
    /// 
    /// [Webrequest Class](https://docs.microsoft.com/en-us/dotnet/api/system.net.webrequest)
    /// 
    /// </summary>
    public class HttpFileLoaderStrategy
    {
        public List<string> Load(string filePath) => MakeRequest(filePath);

        private List<string> MakeRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            // Execute the request
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
                throw new FileLoadException();

            // Read data via the response stream
            Stream stream = response.GetResponseStream();

            using StreamReader streamReader = new StreamReader(stream);

            // Read the stream line by line and return list of lines
            List<string> lines = new List<string>();

            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                lines.Add(line);
            }

            return lines;
        }
    }
}
