using System;
using System.Collections.Generic;
using System.IO;
using SquishIt.Framework.Files;

namespace SquishIt.Framework.Tests.Mocks
{
    public class StubFileReaderFactory: IFileReaderFactory, IPackagedFileReaderFactory
    {
        private string contents;
        private bool fileExists;
        private Dictionary<string, string> contentsForFiles = new Dictionary<string, string>();
        private bool packagedFileNameExists;
        private string packagedFileName;

        public void SetContents(string contents)
        {
            this.contents = contents;
        }

        public void SetContentsForFile(string file, string contents)
        {
            contentsForFiles.Add(file, contents);
        }

        public void SetFileExists(bool fileExists)
        {
            this.fileExists = fileExists;
        }
        
        public void SetPackagedFileNameExists(bool fileExists)
        {
            this.packagedFileNameExists = fileExists;
        }
        public void SetPackagedFileName(string fileName)
        {
            this.packagedFileName = fileName;
        }
        
        public IFileReader GetFileReader(string file)
        {
            if (contentsForFiles.ContainsKey(file))
            {
                return new StubFileReader(file, contentsForFiles[file]);
            }
            return new StubFileReader(file, contents);
        }

        public bool FileExists(string file)
        {
            return fileExists;
        }

        public string PackagedFileName(string file)
        {
            return convert_back_to_original_path_format(packagedFileName,"/sssss.s");
        }

        public bool PackagedFileNameExists(string file)
        {
            return packagedFileNameExists;
        }

        string convert_back_to_original_path_format(string foundFile, string file)
        {
            string originalDir = Path.GetDirectoryName(file);
            string foundFileName = Path.GetFileName(foundFile);
            return Path.Combine(originalDir, foundFileName).Replace("\\", "/");
        }
    }
}