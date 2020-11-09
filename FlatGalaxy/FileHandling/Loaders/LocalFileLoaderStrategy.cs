using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace FlatGalaxy.FileHandling
{
    /// <summary>
    /// 
    /// ## Concern
    /// 
    /// Strategy pattern local file loader.
    /// User used a standard UWP filepicker because of the sandbox like environment. 
    /// Therefore it was not possible to directly open a file. 
    /// 
    /// ## Resources
    ///   [File access permissions](https://docs.microsoft.com/en-us/windows/uwp/files/file-access-permissions)
    /// 
    /// </summary>
    public class LocalFileLoaderStrategy
    {
        private StorageFile _file;

        public List<string> Load(StorageFile file)
        {
            _file = file;
            return AsyncContext.Run(ReadLines).ToList();
        }

        private async Task<IEnumerable<string>> ReadLines() => await FileIO.ReadLinesAsync(_file);
    }
}
