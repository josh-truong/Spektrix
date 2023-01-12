using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Spektrix.Code
{
    internal class ApplicationResources
    {
        private readonly List<string> resources;

        public ApplicationResources() { resources = GetResourcesLocation(); }

        public List<string> GetResourcesLocation(string resource_name = "Spektrix.g.resources")
        {
            // executingAssembly - Gets the resource name in this case we need "Spektrix.g.resources".
            //var executingAssembly = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            Stream fs = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource_name);
            ResourceReader rr = new ResourceReader(fs);
            IDictionaryEnumerator dict = rr.GetEnumerator();

            List<string> resources = new List<string>();
            while (dict.MoveNext())
                resources.Add(dict.Key.ToString());
            rr.Close();
            return resources;
        }

        public List<string> GetResourcesName(string path = "resources/")
        {
            /**
             * Icons Directory: resources/icons/
             * Backgrounds Directory: resources/backgrounds/
             */
            List<string> ret = new List<string>();
            foreach (string resource in resources)
                if (resource.Contains(path)) ret.Add(resource.Split('/').Last());
            return ret;
        }
    }
}
