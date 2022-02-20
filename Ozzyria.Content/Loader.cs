using System.IO;

namespace Ozzyria.Content
{
    public static class Loader
    {
        public static string Root()
        {
#if (DEBUG)
            // find root of repo then go into Content project
            return Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Ozzyria.Content");
#else
            return Directory.GetCurrentDirectory();
#endif
        }
    }
}
