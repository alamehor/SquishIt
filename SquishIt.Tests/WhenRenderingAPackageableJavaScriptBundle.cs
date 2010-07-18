using NUnit.Framework;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Tests.Mocks;
using SquishIt.Tests.Stubs;

namespace SquishIt.Framework.Tests
{
    [TestFixture]
    public class WhenRenderingAPackageableJavaScriptBundle
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
        private IJavaScriptBundle debugJavaScriptBundle;
        private IJavaScriptBundle debugJavaScriptBundle2;
        private StubFileWriterFactory fileWriterFactory;
        private StubFileReaderFactory fileReaderFactory;

        [SetUp]
        public void SetUp()
        {
            var nonDebugStatusReader = new StubDebugStatusReader(false);
            var debugStatusReader = new StubDebugStatusReader(true);

            fileWriterFactory = new StubFileWriterFactory();
            fileReaderFactory = new StubFileReaderFactory();
            fileReaderFactory.SetContents(javaScript);
            javaScriptBundle = new JavaScriptBundle(nonDebugStatusReader,
                                                    fileWriterFactory,
                                                    fileReaderFactory);

            javaScriptBundle2 = new JavaScriptBundle(nonDebugStatusReader,
                                                     fileWriterFactory,
                                                     fileReaderFactory);

            debugJavaScriptBundle = new JavaScriptBundle(debugStatusReader,
                                                         fileWriterFactory,
                                                         fileReaderFactory);

            debugJavaScriptBundle2 = new JavaScriptBundle(debugStatusReader,
                                                          fileWriterFactory,
                                                          fileReaderFactory);
     
        }
        
        [TearDown]
        public void TearDown()
        {
            Bundle.PackageJavaScript = false;
            Bundle.PackageCss = false;
        }

        [Test]
        public void IfPackageFlagIsNotSetThenRenderAsUsual()
        {
            Bundle.PackageJavaScript = false;

            var tag = javaScriptBundle
                .Add("~/js/test.js")
                .AsPackageable()
                .Render("~/js/output_1.js");

            Assert.AreEqual("<script type=\"text/javascript\" src=\"js/output_1.js?r=8E8C548F4F6300695269DE689B903BA3\"></script>", tag);
            Assert.AreEqual("function product(d,c){return d*c}function sum(d,c){return d+c};", fileWriterFactory.Files[@"C:\js\output_1.js"]);
        }
        
        [Test]
        public void IfPackageFlagIsSetButDebugsIsTrueThenRenderAsUsual()
        {
            Bundle.PackageJavaScript = true;
            debugJavaScriptBundle
                .Add("~/js/test1.js")
                .Add("~/js/test2.js")
                .AsPackageable()
                .AsNamed("TestWithDebug", "~/js/output_3.js");

            var tag = debugJavaScriptBundle.RenderNamed("TestWithDebug");

            Assert.AreEqual("<script type=\"text/javascript\" src=\"js/test1.js\"></script><script type=\"text/javascript\" src=\"js/test2.js\"></script>", tag);
        }

    
    }
}