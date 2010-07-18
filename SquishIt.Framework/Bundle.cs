using SquishIt.Framework.Css;
using SquishIt.Framework.JavaScript;

namespace SquishIt.Framework
{
    public class Bundle
    {
        public static bool PackageCss { get; set; }
        public static bool PackageJavaScript { get; set; }

        public static IJavaScriptBundle JavaScript()
        {
            return new JavaScriptBundle();
        }
       
        public static ICssBundle Css()
        {
            return new CssBundle();
        }
    }
}