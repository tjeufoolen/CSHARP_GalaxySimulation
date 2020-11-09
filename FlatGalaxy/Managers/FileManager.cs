using FlatGalaxy.Exceptions;
using FlatGalaxy.FileHandling;
using FlatGalaxy.Models;
using FlatGalaxy.Parsers;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace FlatGalaxy.Managers
{
    /// <summary>
    /// 
    /// ## Concern
    /// 
    /// Strategy pattern Manager, will select the correct strategy.
    /// 
    /// ## Resources
    /// 
    /// [Strategy pattern](https://refactoring.guru/design-patterns/strategy)
    /// 
    /// </summary>
    public class FileManager
    {
        private Galaxy Parse(string fileName, List<string> fileLines)
        {
            string fileExtension = fileName.Split('.').Last().ToLower();
            IFileParserStrategy _fileParserStrategy = fileExtension switch
            {
                "csv" => new CSVParserStrategy(),
                "xml" => new XMLParserStrategy(),
                _ => throw new FileTypeNotSupportedException(),
            };
            return _fileParserStrategy.Parse(fileLines);
        }

        public Galaxy LoadOnline(string url)
        {
            string fileName = url.Split('/').Last().Split("?").First();
            return Parse(fileName, new HttpFileLoaderStrategy().Load(url));
        }

        public Galaxy LoadOffline(StorageFile file)
        {
            if (file == null) throw new FileNotSelectedException();
            return Parse(file.Name, new LocalFileLoaderStrategy().Load(file));
        }
    }
}
