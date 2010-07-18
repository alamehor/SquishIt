using System.Collections.Generic;

namespace SquishIt.Framework.Directories
{
    public interface IDirectoryEnumerator
    {
        IEnumerable<string> GetFiles(string path);
        IEnumerable<string> GetFiles(string path, string orderingFileName);
        IEnumerable<string> GetFiles(string path, string[] filenamePatterns);
        IEnumerable<string> GetFiles(string path, string[] filenamePatterns, string[] fileNameExclusions);
    }
}