using System.Reflection;

namespace alive
{
    public class Locations
    {
        public static string DataRoot { get; }
        public static string Root { get; }
        static Locations()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new Uri(assemblyPath);
            Root = Path.GetDirectoryName(uri.AbsolutePath);
            DataRoot = Path.Join(Root, "rrd");
            if (!Directory.Exists(DataRoot))
                Directory.CreateDirectory(DataRoot);
        }
    }
}
