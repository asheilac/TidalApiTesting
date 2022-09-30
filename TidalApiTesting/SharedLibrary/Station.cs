using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class Crs
    {
        public string Type { get; set; }
    }

    public class Geometry
    {
        public string Type { get; set; }
    }

    public class Properties
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public bool ContinuousHeightsAvailable { get; set; }
        public string Footnote { get; set; }
    }

    public class Root
    {
        public string type { get; set; }
        public string id { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
        public List<double> bbox { get; set; }
        public Crs crs { get; set; }
    }
}
