using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SquishIt.Framework.Directories
{
    public class DirectoryEnumerator: IDirectoryEnumerator
    {
        private IDirectory directory;

        public DirectoryEnumerator()
        {
            this.directory = new Directory();
        }

        public DirectoryEnumerator(IDirectory directory)
        {
            this.directory = directory;
        }

        public IEnumerable<string> GetFiles(string path)
        { 
            var dir = Path.GetDirectoryName(path);
            var filenamePattern = Path.GetFileName(path);
            var files = directory.GetFiles(dir, filenamePattern).ToArray();
            return files;
        }

        public IEnumerable<string> GetFiles(string path, string orderingFileName)
        {
            var files = GetFiles(path);
            var ordering = GetOrdering(path);

            if (ordering.Count() <= 0)
            {
                return files;
            }

            if (ordering.Count() != files.Count())
            {
                Console.Error.WriteLine("Number of entries in 'ordering.txt' does not match number of javascript files in folder.");
            }

            return OrderFiles(ordering, files.ToList());

        }

        public IEnumerable<string> GetFiles(string path, string[] filenamePatterns)
        {
            IEnumerable<string> files = new string[0];
            foreach(string pattern in filenamePatterns)
            {
                files = files.Union(directory.GetFiles(path, pattern));
            }
            return files;
        }

        public IEnumerable<string> GetFiles(string path, string[] filenamePatterns, string[] fileNameExclusions)
        {
            IEnumerable<string> files = new string[0];
            foreach(string pattern in filenamePatterns)
            {
                files = files.Union(directory.GetFiles(path, pattern));
            }
            foreach(string exclusion in fileNameExclusions)
            {
                files = files.Except(directory.GetFiles(path, exclusion));
            }
            return files;
        }

        private IEnumerable<string> OrderFiles(IEnumerable<string> ordering, IEnumerable<string> files)
        {
            var result = new List<string>();
            foreach (string order in ordering)
            {
                foreach (string file in files)
                {
                    if (Path.GetFileName(file).ToLower() == order)
                    {
                        result.Add(file);
                    }
                }
            }
            return result;
        }

        private IEnumerable<string> GetOrdering(string path)
        {
            var ordering = new List<string>();
            string orderingFile = path + "ordering.txt";
            if (File.Exists(path + "ordering.txt"))
            {
                using (var sr = new StreamReader(orderingFile))
                {
                    while(!sr.EndOfStream)
                    {
                        yield return sr.ReadLine();
                    }
                }
            }
        }
    }
}