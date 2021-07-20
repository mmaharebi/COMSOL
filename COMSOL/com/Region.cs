using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Shapes;

namespace COMSOL.com
{
    class Region
    {
        public Shape shape { get; set; }
        public int id { get; set; }
        public Dielectric dielectric { get; set; }
    }
}
