using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace ConfigGenerator
{
    public class Generator
    {
        private XmlSerializer xmlSerializer;
        private ConfigSettings _settings;

        public Generator(ConfigSettings settings)
        {
            xmlSerializer = new XmlSerializer(typeof(Config));
            _settings = settings;
        }

        public void Generate()
        {
            var configs = GenerateConfigs();

            configs = configs.SelectMany(s => {
                return Enum.GetValues(typeof(ServiceTypes)).Cast<ServiceTypes>().Select(t =>{
                    s.ServiceProfile = t.ToString();
                    return s;
                });
            });

            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Config));

            foreach (var config in configs)
            {
                XmlOutputter writer = XmlOutputter.Create(_settings);

                try
                {
                    writer.Write(config);
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(config.FilePath);
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        private IEnumerable<Config> GenerateConfigs(string[] args)
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

        private IEnumerable<Config> HandleSpeeds(Config config)
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
    
        private IEnumerable<Config> GenerateConfigs()
        {

            var parentConfig = new Config
            {
                ProfileName = _settings.ConfigName
            };

            if (_settings.Speeds.Any())
            {
                parentConfig.DownSpeed = _settings.Speeds[0];
                parentConfig.UpSpeed = _settings.Speeds[1];

                return new[] { parentConfig };
            }
            else
            {
                if (string.IsNullOrEmpty(_settings.InputPath))
                {
                    throw new ArgumentNullException(nameof(_settings.InputPath));
                }

                string text = System.IO.File.ReadAllText(_settings.InputPath);
                var speedsList = text.Replace("\r\n","")
                .Split(',')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.Split('-'));

                return speedsList.Select(s => {
                    var temp = parentConfig.ShallowCopy();
                    temp.DownSpeed = s[0];
                    temp.UpSpeed = s[1];
                    return temp;
                });
            }

        }
    
    }
}