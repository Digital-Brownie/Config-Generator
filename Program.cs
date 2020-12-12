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
            // var configs = GenerateConfigs(args);

            // configs = configs.SelectMany(s => {
            //     return Enum.GetValues(typeof(ServiceTypes)).Cast<ServiceTypes>().Select(t =>{
            //         s.ServiceProfile = t.ToString();
            //         return s;
            //     });
            // });

            // var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            // // MemoryStream strm = new MemoryStream();
            // XmlWriterSettings settings = new XmlWriterSettings
            // {
            //     Indent = true,
            //     OmitXmlDeclaration = true
            // };

            // XmlSerializer xmlSerializer = new XmlSerializer(typeof(Config));

            // foreach (var config in configs)
            // {
            //     if(!Directory.Exists(config.Directory))
            //         Directory.CreateDirectory(config.Directory);

            //     XmlOutputter writer = XmlOutputter.Create(config.FilePath, settings);
                
            //     try
            //     {
            //         xmlSerializer.Serialize(writer, config, emptyNs);
            //     }
            //     catch (System.Exception e)
            //     {
            //         System.Console.WriteLine(config.FilePath);
            //         System.Console.WriteLine(e.Message);
            //     }
            // }

            var settings = ParseArgs(args);
            var generator = new Generator(settings);

            generator.Generate();

            // var settings  = new ConfigSettings
            // {
            //     OutputPath = "./",
            //     ConfigName = "Lu is a cutie",
            //     InputPath = "blah",
            //     Speeds = "10-10"
            // };
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

        private static IEnumerable<Config> GenerateConfigs(string[] args)
        {
            Action<Config> doStuff = default;

            for (int i = 0; i < args.Length; i++)
            {
                switch (i)
                {
                    case 0: doStuff += c => c.ProfileName = args[0];
                    break;
                    case 1: doStuff += c => c.DownSpeed = args[1];
                    break;
                    case 2: doStuff += c => c.UpSpeed = args[2];
                    break;
                    default:
                    throw new Exception("JISIS");
                }
            }

            var config = new Config();

            doStuff(config);

            if (args.Length == 1)
            {
                return HandleSpeeds(config);
            }
            
            return new [] { config };
        }

        private static IEnumerable<Config> HandleSpeeds(Config config)
        {
            string text = System.IO.File.ReadAllText("list");
            var speedsList = text.Replace("\r\n","").Split(',').Where(s => !string .IsNullOrEmpty(s));

            var newConf2 = config.ShallowCopy();

            Config ParseSpeeds(string speed)
            {
                System.Console.WriteLine("TEST!!!!");
                var newConf = config.ShallowCopy();
                var speeds = speed.Split('-');
                System.Console.WriteLine(speeds);
                newConf.DownSpeed = speeds[0];
                newConf.UpSpeed = speeds[1];
                return newConf;
            }

            return speedsList.Select(ParseSpeeds);
        }
    }
}
