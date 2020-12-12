using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ConfigGenerator
{
    public abstract class XmlOutputter
    {

        private XmlSerializerNamespaces emptyNameSpaces;
        private XmlWriterSettings settings;
        private XmlSerializer _xmlSerializer;

        protected XmlOutputter()
        {
            emptyNameSpaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            settings = new XmlWriterSettings
                {
                    Indent = true,
                    OmitXmlDeclaration = true
                };
            _xmlSerializer = new XmlSerializer(typeof(Config));
        }

        public static XmlOutputter Create(ConfigSettings settings)
        {
            if (settings.OutputPath != null)
                return new XmlFileWriter(settings.OutputPath);

            return new XmlConsoleWriter();
        }

        public abstract void Write(Config config);

        class XmlFileWriter : XmlOutputter
        {
            private string filePath;
            public XmlFileWriter(string path)
            {
                filePath = path;
            }

            public override void Write(Config config)
            {
                var outputPath = @$"{filePath}\{config.Directory}";

                if(!Directory.Exists(outputPath))
                    Directory.CreateDirectory(outputPath);

                XmlWriter writer = XmlWriter.Create(@$"{outputPath}\{config.FileName}", settings);

                try
                {
                    _xmlSerializer.Serialize(writer, config, emptyNameSpaces);
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(config.FilePath);
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        class XmlConsoleWriter : XmlOutputter
        {
            public override void Write(Config config)
            {
                XmlWriter writer = XmlWriter.Create(System.Console.Out, settings);

                try
                {
                    _xmlSerializer.Serialize(writer, config, emptyNameSpaces);
                    System.Console.WriteLine();
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(config.FilePath);
                    System.Console.WriteLine(e.Message);
                }
            }
        }
    }
}