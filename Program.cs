using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace ConfigGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = ParseArgs(args);
            var generator = new Generator(settings);

            generator.Generate();
        }

        private static ConfigSettings ParseArgs(string[] args)
        {
            var configNameIndex = Array.IndexOf(args, CommandlineArgs.ConfigName);
            var speedsIndex = Array.IndexOf(args, CommandlineArgs.Speeds);
            var inputPathIndex = Array.IndexOf(args, CommandlineArgs.InputPath);
            var outPathIndex = Array.IndexOf(args, CommandlineArgs.OutputPath);

            var settings = new ConfigSettings
            {
                ConfigName = configNameIndex.IsPositive() ? args[++configNameIndex] : throw new Exception("Missing Config Name!"),
                Speeds = speedsIndex.IsPositive() ? args[++speedsIndex].Split('-') : null,
                OutputPath = outPathIndex.IsPositive() ? args[++outPathIndex] : null,
                InputPath = inputPathIndex.IsPositive() ? args[++inputPathIndex] : null
            };

            if (settings.Speeds is null && settings.InputPath is null)
                throw new Exception("Provide speeds!");

            return settings;
        }

        }
}
