using System;
using System.Collections.Generic;
using SquishIt.Framework.Directories;

namespace SquishIt.Tests.Stubs
{
    public class StubDirectoryEnumerator: IDirectoryEnumerator
    {
        public IEnumerable<string> GetFiles(string path)
        {
            yield return @"C:\test\file1.js";
            yield return @"C:\test\file2.js";
            yield return @"C:\test\file3.js";
            yield return @"C:\test\file4.js";
            yield return @"C:\test\file5.js";
        }

        public IEnumerable<string> GetFiles(string path, string orderingFileName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFiles(string path, string[] filenamePatterns)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFiles(string path, string[] filenamePatterns, string[] fileNameExclusions)
        {
            throw new NotImplementedException();
        }
    }
}