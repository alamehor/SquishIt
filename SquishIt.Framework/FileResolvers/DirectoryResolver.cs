using System.Collections.Generic;
using SquishIt.Framework.Directories;

namespace SquishIt.Framework.FileResolvers
{
    public class DirectoryResolver: IFileResolver, IDirectoryResolver
    {
        public static string Type
        {
            get { return "dir"; }
        }
        
        private readonly IDirectoryEnumerator directoryEnumerator;

        public DirectoryResolver()
        {
            this.directoryEnumerator = new DirectoryEnumerator();
        }

        public DirectoryResolver(IDirectoryEnumerator directoryEnumerator)
        {
            this.directoryEnumerator = directoryEnumerator;
        }        

        public IEnumerable<string> TryResolve(string directory)
        {
            return directoryEnumerator.GetFiles(directory);            
        }        
        
        public IEnumerable<string> TryResolve(string directory,string[] filenamePatterns)
        {
            return directoryEnumerator.GetFiles(directory, filenamePatterns);            
        }        
        
        public IEnumerable<string> TryResolve(string directory,string[] filenamePatterns, string[] fileNameExclusions)
        {
            return directoryEnumerator.GetFiles(directory, filenamePatterns, fileNameExclusions);            
        }        
    }

    public interface IDirectoryResolver 
    {
        IEnumerable<string> TryResolve(string directory,string[] filenamePatterns);
        IEnumerable<string> TryResolve(string directory,string[] filenamePatterns, string[] fileNameExclusions);


    }
}