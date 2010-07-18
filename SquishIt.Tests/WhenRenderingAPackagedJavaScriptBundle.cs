using NUnit.Framework;
using SquishIt.Framework.Css;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Tests.Mocks;
using SquishIt.Tests.Stubs;

namespace SquishIt.Framework.Tests
{
    [TestFixture]
    public class WhenRenderingAPackagedBundle
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

        private IJavaScriptBundle javaScriptBundle;
        private IJavaScriptBundle javaScriptBundle2;
        private ICssBundle cssBundle;
        private StubFileWriterFactory jsFileWriterFactory;
        private StubFileReaderFactory jsFileReaderFactory;
        private StubFileWriterFactory cssFileWriterFactory;
        private StubFileReaderFactory cssFileReaderFactory;

        [SetUp]
        public void SetUp()
        {
            var nonDebugStatusReader = new StubDebugStatusReader(false);

            jsFileWriterFactory = new StubFileWriterFactory();
            jsFileReaderFactory = new StubFileReaderFactory();
            cssFileWriterFactory = new StubFileWriterFactory();
            cssFileReaderFactory = new StubFileReaderFactory();

            jsFileReaderFactory.SetContents(javaScript);
            jsFileReaderFactory.SetPackagedFileNameExists(true);
            jsFileReaderFactory.SetPackagedFileName("/myfilename_combined_3_234448i484884844848848484484844848.js");

            cssFileReaderFactory.SetContents(css);
            cssFileReaderFactory.SetPackagedFileNameExists(true);
            cssFileReaderFactory.SetPackagedFileName("/myfilename_combined_3_234448i484884844848848484484844848.css");

            javaScriptBundle = new JavaScriptBundle(nonDebugStatusReader,
                                                    jsFileWriterFactory,
                                                    jsFileReaderFactory);

            javaScriptBundle2 = new JavaScriptBundle(nonDebugStatusReader,
                                                     jsFileWriterFactory,
                                                     jsFileReaderFactory);

            cssBundle = new CssBundle(nonDebugStatusReader, cssFileWriterFactory, cssFileReaderFactory);

            Bundle.PackageJavaScript = true;
            Bundle.PackageCss = true;

        }

        [TearDown]
        public void TearDown()
        {
            Bundle.PackageJavaScript = false;
            Bundle.PackageCss = false;
        }

        [Test]
        public void ShouldRenderTheExistingPackagedJavaScriptBundleInsteadOfCreatingANewOne()
        {
            var testFile = "/myfilename_combined_3_234448i484884844848848484484844848.js";
            PackagerTests.delete_all_the_test_files();
            PackagerTests.write_a_test_file_to_the_file_system(testFile);

            //make sure the file reader does not try to load the added files

            var tag = javaScriptBundle
                .Add("~/js/test.js")
                .AsPackageable()
                .Render("~/myfilename_combined_#.js");

            Assert.AreEqual("<script type=\"text/javascript\" src=\"/myfilename_combined_3_234448i484884844848848484484844848.js\"></script>", tag);
        }
        
        [Test]
        public void ShouldRenderTheExistingPackagedCssBundleInsteadOfCreatingANewOne()
        {
            var testFile = "/myfilename_combined_3_234448i484884844848848484484844848.css";
            PackagerTests.delete_all_the_test_files();
            PackagerTests.write_a_test_file_to_the_file_system(testFile);

            //make sure the file reader does not try to load the added files

            var tag = cssBundle
                .Add("~/css/test.css")
                .AsPackageable()
                .Render("~/myfilename_combined_#.css");

            Assert.AreEqual("<link rel=\"stylesheet\" type=\"text/css\"  href=\"/myfilename_combined_3_234448i484884844848848484484844848.css\" />", tag);
        }
    }
}