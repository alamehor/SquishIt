using System.Collections.Generic;

namespace SquishIt.Framework.Directories
{
    public class Directory: IDirectory
    {
        public IEnumerable<string> GetFiles(string path, string filePattern)
        {
            if(string.IsNullOrEmpty(filePattern))
                return System.IO.Directory.GetFiles(path);
            else
                return System.IO.Directory.GetFiles(path, filePattern);
        }
    }
}