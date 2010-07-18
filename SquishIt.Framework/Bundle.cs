using SquishIt.Framework.Css;
using SquishIt.Framework.Files;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Utilities;

namespace SquishIt.Framework
{
    public class Bundle
    {
        static bool _packageCss;
        public static bool PackageCss
        {
            get { return _packageCss; }
            set { _packageCss = value; }
        }

        static bool _packageJavaScript;
        public static bool PackageJavaScript
        {
            get { return _packageJavaScript; }
            set { _packageJavaScript = value; }
        }

        public static IJavaScriptBundle JavaScript()
        {
            return new JavaScriptBundle(new DebugStatusReader(), new FileWriterFactory(), new PackagedFileReaderFactory());
        }
       
        public static ICssBundle Css()
        {
            return new CssBundle(new DebugStatusReader(), new FileWriterFactory(), new PackagedFileReaderFactory());
        }
    }
}