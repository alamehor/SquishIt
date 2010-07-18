using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SquishIt.Framework.Css;
using SquishIt.Framework.FileResolvers;
using SquishIt.Framework.Files;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Tests.Mocks;
using SquishIt.Framework.Utilities;
using SquishIt.Tests.Stubs;

namespace SquishIt.Framework.Tests
{
    [TestFixture]
    public class PackagerTests
    {
        private string css = @" li {
                                    margin-bottom:0.1em;
                                    margin-left:0;
                                    margin-top:0.1em;
                                }";
        private string javaScript = @"
                                        function product(a, b)
                                        {
                                            return a * b;
                                        }

                                        function sum(a, b){
                                            return a + b;
                                        }";
        IDebugStatusReader mockDebugStatusReader;
        IFileWriterFactory mockFileWriterFactory ;
        StubFileReaderFactory cssMockFileReaderFactory;
        StubFileReaderFactory jsMockFileReaderFactory;
        private static FileResolver fileResolver = new FileResolver();
        private static AppRelativePathResolver appRelativePathResolver = new AppRelativePathResolver();

        [SetUp]
        public void SetUp()
        {
            mockDebugStatusReader = new StubDebugStatusReader(false);
            mockFileWriterFactory = new StubFileWriterFactory();
            cssMockFileReaderFactory = new StubFileReaderFactory();
            jsMockFileReaderFactory = new StubFileReaderFactory();
            cssMockFileReaderFactory.SetContents(css);            
            jsMockFileReaderFactory.SetContents(javaScript);            
        }

        [TearDown]
        public void TearDown()
        {
            Bundle.PackageJavaScript = false;
            Bundle.PackageCss = false;
        }

        [Test]
        public void CanConfigureBundlerToUsePackagedCss()
        {
            Bundle.PackageCss = true;
            Assert.IsTrue(Bundle.PackageCss);
        }
        
        [Test]
        public void CanConfigureBundlerToUsePackagedJavaScript()
        {
            Bundle.PackageJavaScript = true;
            Assert.IsTrue(Bundle.PackageJavaScript);
        }

        [Test]
        public void CanMarkCssBundleAsPackageable()
        {

            ICssBundle cssBundle = new CssBundle(mockDebugStatusReader,
                                                 mockFileWriterFactory,
                                                 cssMockFileReaderFactory);
            string tag = cssBundle
                .Add("/css/first.css")
                .Add("/css/second.css")
                .AsPackageable()
                .Render("/css/render_differently_if_packaged_#.css");
        }
        
        [Test]
        public void CanMarkJavaScriptBundleAsPackageable()
        {

            IJavaScriptBundle jsBundle = new JavaScriptBundle(mockDebugStatusReader,
                                                 mockFileWriterFactory,
                                                 jsMockFileReaderFactory);
            string tag = jsBundle
                .Add("/js/test.js")
                .AsPackageable()
                .Render("/js/render_differently_if_packaged_#.js");
        }

        public static void write_a_test_file_to_the_file_system(string testFile)
        {
            var f = resolve_single_file(testFile);

            if (!File.Exists(f))
            {
                var sw = new StreamWriter(f , false, System.Text.Encoding.UTF8);
                sw.Write("This is a test file created by unit tests for SquishIt. I'm sorry I forgot to delete myself. Please go ahead and delete me.");
                sw.Dispose();
            }
        }

        protected static string ExpandAppRelativePath(string file)
        {
            if (file.StartsWith("~/"))
            {
                string appRelativePath = Path.GetPathRoot("~/");
                if (appRelativePath != null && !appRelativePath.EndsWith("/"))
                    appRelativePath += "/";
                return file.Replace("~/", appRelativePath);
            }
            return file;
        }
        public static void delete_test_file_from_the_file_system_if_it_exists(string testFile)
        {
            if (File.Exists(resolve_single_file(testFile)))
                File.Delete(resolve_single_file(testFile));
        }

        public static void delete_all_the_test_files()
        {
            delete_all_the_test_files(@"c:\","myfilename_combined*");
            delete_all_the_test_files(Environment.CurrentDirectory,"myfilename_combined*");
        }
        
        public static void delete_all_the_test_files(string dir, string namepattern)
        {
            var testFiles = Directory.GetFiles(dir, namepattern);
            foreach (var f in testFiles)
                File.Delete(f);
        }

        private static string resolve_single_file(string file)
        {
            var appRelativePathResoledToFileSystem = appRelativePathResolver.TryResolve(file).FirstOrDefault();
            var fileResolverResolvedPath = fileResolver.TryResolve(file).FirstOrDefault().FirstOrDefault();
            var combined = fileResolver.TryResolve(appRelativePathResoledToFileSystem).FirstOrDefault();
            return combined;
            //return fileResolver.TryResolve(file).FirstOrDefault();
        }
        
    }
}