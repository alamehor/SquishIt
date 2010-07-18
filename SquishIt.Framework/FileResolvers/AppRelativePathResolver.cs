using System.Collections.Generic;
using System.Web;

namespace SquishIt.Framework.FileResolvers
{
    public class AppRelativePathResolver: IFileResolver
    {
        public static string Type
        {
            get { return "~"; }
        }        

        public IEnumerable<string> TryResolve(string file)
        {
            if (HttpContext.Current == null)
            {
                file = file.Replace("/", "\\").TrimStart('~').TrimStart('\\');
                yield return @"C:\" + file.Replace("/", "\\");
            }            
            yield return HttpContext.Current.Server.MapPath(file);
        }
    }
}