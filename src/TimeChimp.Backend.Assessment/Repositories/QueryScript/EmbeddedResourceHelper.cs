using System.IO;
using System.Reflection;

namespace TimeChimp.Backend.Assessment.Repositories.QueryScript
{
    public static class EmbeddedResourceHelper
    {
        public static string GetQuery(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = new StreamReader(assembly.GetManifestResourceStream($"TimeChimp.Backend.Assessment.Repositories.QueryScript.{fileName}")))
            {
                return stream.ReadToEnd();
            }
        }
    }
}
