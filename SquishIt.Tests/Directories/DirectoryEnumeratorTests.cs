using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SquishIt.Framework.Directories;
using Directory = System.IO.Directory;

namespace SquishIt.Framework.Tests
{
    [TestFixture]
    public class DirectoryEnumeratorTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            directoryEnumerator = new DirectoryEnumerator();
            temporaryDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + "\\");
            Directory.CreateDirectory(temporaryDirectory);

            f1 = Path.GetRandomFileName() + ".js";
            f2 = Path.GetRandomFileName() + ".js";
            f3 = Path.GetRandomFileName() + ".css";
            f4 = Path.GetRandomFileName() + ".css";
            f5 = Path.GetRandomFileName() + "-vsdoc.js";


            File.CreateText(temporaryDirectory + f1).Dispose();
            File.CreateText(temporaryDirectory + f2).Dispose();
            File.CreateText(temporaryDirectory + f3).Dispose();
            File.CreateText(temporaryDirectory + f4).Dispose();
            File.CreateText(temporaryDirectory + f5).Dispose();
            
            var sw = new StreamWriter(temporaryDirectory + "ordering.txt", false, System.Text.Encoding.UTF8);
            sw.WriteLine(f1);
            sw.WriteLine(f2);
            sw.WriteLine(f3);
            sw.WriteLine(f4);
            sw.WriteLine(f5);
            sw.WriteLine("ordering.txt");
            sw.Dispose();    
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(temporaryDirectory, true);
        }

        #endregion

        string temporaryDirectory;
        DirectoryEnumerator directoryEnumerator;
        string f1;
        string f2;
        string f3;
        string f4;
        string f5;

        [Test]
        public void CanEnumerateDirectory()
        {
            List<string> temporaryFiles = directoryEnumerator.GetFiles(temporaryDirectory).ToList();
            Assert.AreEqual(6, temporaryFiles.Count);
        }
        
        [Test]
        public void CanEnumerateDirectoryWithWildcardEmbeddedInThePath()
        {
            List<string> temporaryFiles = directoryEnumerator.GetFiles(temporaryDirectory + "/*.js").ToList();
            Assert.AreEqual(3, temporaryFiles.Count);
        }
        
        [Test]
        public void CanEnumerateDirectoryUsingWildCardCollection()
        {
            var patterns = new string[] {"*-vsdoc.js", "*.css"};
            List<string> temporaryFiles = directoryEnumerator.GetFiles(temporaryDirectory,patterns).ToList();
            Assert.AreEqual(3, temporaryFiles.Count);
        }
        
        [Test]
        public void CanEnumerateDirectoryUsingWildCardCollectionAndExclusions()
        {
            var patterns = new string[] {"*.js"};
            var exclusions = new string[] {"*-vsdoc.js"};
            List<string> temporaryFiles = directoryEnumerator.GetFiles(temporaryDirectory,patterns, exclusions).ToList();
            Assert.AreEqual(2, temporaryFiles.Count);
        }
        
        [Test]
        public void CanEnumerateDirectoryAndSpecifyOrderingFile()
        {
            List<string> temporaryFiles = directoryEnumerator.GetFiles(temporaryDirectory,"ordering.txt").ToList();
            Assert.AreEqual(temporaryDirectory + f1,temporaryFiles[0]);
            Assert.AreEqual(temporaryDirectory + f2,temporaryFiles[1]);
            Assert.AreEqual(temporaryDirectory + f3,temporaryFiles[2]);
            Assert.AreEqual(temporaryDirectory + f4,temporaryFiles[3]);
            Assert.AreEqual(temporaryDirectory + f5,temporaryFiles[4]);
            Assert.AreEqual(temporaryDirectory + "ordering.txt",temporaryFiles[5]);
        }
    }
}