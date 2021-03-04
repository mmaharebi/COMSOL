using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using System.Text.Json;

namespace COMSOL.com
{
    class Data
    {
        private string parametersPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../com/data/parameters.json");
        private string shapesNamesPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../com/data/shapes_names.json"); // readonly

        private List<Parameter> Parameters = new List<Parameter>();
        private List<string> ShapesNames = new List<string>();

        public Data()
        {
            readData();
            writeData();
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

        public List<string> GetShapesNames()
        {
            return ShapesNames;
        }

        private void readData()
        {
            Parameters = JsonSerializer.Deserialize<List<Parameter>>(File.ReadAllText(parametersPath));
            ShapesNames = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(shapesNamesPath));
        }
        private void writeData()
        {
            File.WriteAllText(parametersPath, JsonSerializer.Serialize(Parameters));
        }
    }
}
