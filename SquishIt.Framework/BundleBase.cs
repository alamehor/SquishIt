using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using SquishIt.Framework.FileResolvers;
using SquishIt.Framework.Files;
using SquishIt.Framework.Utilities;

namespace SquishIt.Framework
{
    public abstract class BundleBase
    {
        protected IFileWriterFactory fileWriterFactory;
        protected IFileReaderFactory fileReaderFactory;
        protected IDebugStatusReader debugStatusReader;

        protected BundleBase(IFileWriterFactory fileWriterFactory, IFileReaderFactory fileReaderFactory, IDebugStatusReader debugStatusReader)
        {
            this.fileWriterFactory = fileWriterFactory;
            this.fileReaderFactory = fileReaderFactory;
            this.debugStatusReader = debugStatusReader;
        }

        protected string RenderFiles(string template, IEnumerable<string> files)
        {
            var sb = new StringBuilder();
            foreach (string file in files)
            {
                string processedFile = ExpandAppRelativePath(file);
                sb.Append(String.Format(template, processedFile));
            }
            return sb.ToString();
        }

        protected List<string> GetFiles(List<InputFile> fileArguments)
        {
            var files = new List<string>();
            var fileResolverCollection = new FileResolverCollection();
            foreach (InputFile file in fileArguments)
            {
                files.AddRange(fileResolverCollection.Resolve(file.FilePath, file.FileType));
            }
            return files;
        }

        protected void WriteFiles(string output, string outputFile)
        {
            if (outputFile != null)
            {
                using (var fileWriter = fileWriterFactory.GetFileWriter(outputFile))
                {
                    fileWriter.Write(output); 
                }
            }
            else
            {
                Console.WriteLine(output);
            }
        }

        protected void WriteGZippedFile(string outputJavaScript, string gzippedOutputFile)
        {
            if (gzippedOutputFile != null)
            {
                var gzipper = new FileGZipper();
                gzipper.Zip(gzippedOutputFile, outputJavaScript);
            }
        }

        protected List<InputFile> GetFilePaths(List<string> list)
        {
            var result = new List<InputFile>();
            foreach (string file in list)
            {
                string mappedPath = ResolveAppRelativePathToFileSystem(file);
                result.Add(new InputFile(mappedPath, FileResolver.Type));
            }
            return result;
        }

        protected string ResolveAppRelativePathToFileSystem(string file)
        {
            var fileResolverCollection = new FileResolverCollection();
            return fileResolverCollection.Resolve(file, AppRelativePathResolver.Type).FirstOrDefault();
        }

        protected string ExpandAppRelativePath(string file)
        {            
            if (file.StartsWith("~/"))
            {
                string appRelativePath = HttpRuntime.AppDomainAppVirtualPath;
                if (appRelativePath != null && !appRelativePath.EndsWith("/"))
                    appRelativePath += "/";
                return file.Replace("~/", appRelativePath);    
            }
            return file;
        }

        protected string ReadFile(string file)
        {
            using (var sr = fileReaderFactory.GetFileReader(file))
            {
                return sr.ReadToEnd();
            }
        }

        protected bool FileExists(string file)
        {
            return fileReaderFactory.FileExists(file);
        }   
        
        protected string FindAnExistingPackagedBundle(string file)
        {
            string packagedFile = string.Empty;
            string expandedRelativePathToFile = ResolveAppRelativePathToFileSystem(ExpandAppRelativePath(file));

            try
            {
                if(((IPackagedFileReaderFactory)fileReaderFactory).PackagedFileNameExists(expandedRelativePathToFile))
                {
                    packagedFile = ((IPackagedFileReaderFactory)fileReaderFactory).PackagedFileName(expandedRelativePathToFile);
                }
            }
            catch (Exception e)
            {
                packagedFile = string.Empty;
            }
            
            return convert_back_to_original_path_format(packagedFile,file);
        }

        string convert_back_to_original_path_format(string foundFile, string file)
        {
            string originalDir = Path.GetDirectoryName(file.Replace("~/","/"));  

            string foundFileName = Path.GetFileName(foundFile);
            return Path.Combine(originalDir, foundFileName).Replace("\\", "/");
        }
    }
}