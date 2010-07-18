using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SquishIt.Framework.FileResolvers;

namespace SquishIt.Framework.Files
{
    public class PackagedFileReaderFactory : FileReaderFactory, IPackagedFileReaderFactory
    {
        #region IPackagedFileReaderFactory Members

        public string PackagedFileName(string file)
        {
            return get_the_best_match_for_the_packaged_file_name(file.Replace("#", "*"));
        }


        public bool PackagedFileNameExists(string file)
        {
            var directoryResolver = new DirectoryResolver();

            bool exists = false;
            string directory = Path.GetDirectoryName(file);
            string filename = Path.GetFileName(file).Replace("#", "*");
            var matchingFiles = directoryResolver.TryResolve(Path.Combine(directory,filename)); 

            //string[] matchingFiles = Directory.GetFiles(directory, filename, SearchOption.TopDirectoryOnly);

            if (matchingFiles != null && matchingFiles.Count() > 0)
            {
                exists = true;
            }

            return exists;
        }

        #endregion

        string convert_back_to_original_path_format(string foundFile, string file)
        {
            string originalDir = Path.GetDirectoryName(file);  
            string foundFileName = Path.GetFileName(foundFile);
            return Path.Combine(originalDir, foundFileName).Replace("\\", "/");
        }

        string get_the_best_match_for_the_packaged_file_name(string filename)
        {
            var directoryResolver = new DirectoryResolver();
            var matchingFiles = directoryResolver.TryResolve(filename);
            //string[] matchingFiles = Directory.GetFiles(directory, filename, SearchOption.TopDirectoryOnly);)

            if (matchingFiles == null || matchingFiles.Count() == 0)
            {
                throw new FileNotFoundException("Could not find Packaged bundle matching filename: \"" + filename + "\"");
            }

            return select_the_most_recent_matching_file(matchingFiles);
        }

        string select_the_most_recent_matching_file(IEnumerable<string> files)
        {
            DateTime mostRecent = DateTime.MinValue;
            string mostRecentFileName = files.FirstOrDefault();

            foreach (string file in files)
            {
                DateTime lastWrite = File.GetLastWriteTime(file);

                if (lastWrite > mostRecent)
                {
                    mostRecentFileName = file;
                }
            }
            return mostRecentFileName;
        }
    }
}