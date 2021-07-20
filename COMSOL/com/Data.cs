using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace COMSOL.com
{
    class Data
    {
        private string parametersPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../com/data/parameters.json");
        
        private string shapesNamesPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../com/data/shapes_names.json"); // readonly
        
        private string unitsPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../com/data/units.json"); // readonly

        private string regionTypesPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../com/data/region_types.json"); // readonly

        private List<Parameter> Parameters = new List<Parameter>();
        private List<Unit> Units = new List<Unit>();
        private List<Region> Regions = new List<Region>();

        private List<string> ShapesNames = new List<string>();
        private List<string> RegionTypes = new List<string>();

        public Polygon polygon = new Polygon();

        public Data()
        {
            readData();
            writeData();
        }

        public void sampler(Canvas canvas)
        {
            var shapes = canvas.Children;
            foreach (Shape shape in shapes)
            {
                //here
            }
        }

        public List<Parameter> GetParameters()
        {
            return Parameters;
        }
        public void addParameter(Parameter parameter)
        {
            Parameters.Add(parameter);
            writeData();
        }
        public void removeParameter(Parameter parameter)
        {
            Parameters.Remove(parameter);
            writeData();
        }

        public void addRegion(Region region)
        {
            Regions.Add(region);
        }

        public List<string> GetShapesNames()
        {
            return ShapesNames;
        }

        public List<string> GetReegionTypes()
        {
            return RegionTypes;
        }

        public List<Unit> GetUnits()
        {
            return Units;
        }

        private void readData()
        {
            Parameters = JsonSerializer.Deserialize<List<Parameter>>(File.ReadAllText(parametersPath));
            ShapesNames = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(shapesNamesPath));
            Units = JsonSerializer.Deserialize<List<Unit>>(File.ReadAllText(unitsPath));
            RegionTypes = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(regionTypesPath));
        }
        private void writeData()
        {
            File.WriteAllText(parametersPath, JsonSerializer.Serialize(Parameters));
        }
    }
}
