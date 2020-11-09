using FlatGalaxy.Models;
using System.Collections.Generic;

namespace FlatGalaxy.Parsers
{
    public interface IFileParserStrategy
    {
        Galaxy Parse(List<string> fileLines);
    }
}
