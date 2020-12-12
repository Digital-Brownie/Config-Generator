using System.Xml.Serialization;

namespace ConfigGenerator
{
    [XmlRoot(ElementName="root")]
    public class Config
    {
         public Config()
         {

         }

        private string profileName;
        private string serviceProfile;
        private string downSpeed;
        private string upSpeed;

        private string _serviceProfile
        {
            get => serviceProfile is null ? "LP" : serviceProfile;
        }

        public string ProfileName
        {
            set => profileName = value;
        }

        [XmlElement(ElementName = "LINEPROF")]
        public string LineProfile
        {
            get => $"LP-{profileName}-{_serviceProfile}-{downSpeed}-{upSpeed}";
            set => _ = value;
        }

        [XmlElement(ElementName = "SRVPROF")]
        public string ServiceProfile
        {
            get => $"SP-{profileName}-{_serviceProfile}";
            set => serviceProfile = value;
        }

        [XmlElement(ElementName = "VAPROFILE")]
        public string VasType
        {
            get => serviceProfile == "L2" ? null : $"VAS-{profileName}-{_serviceProfile}";
            set => _ = value;
        }
        
        [XmlElement(ElementName = "RX")]
        public string DownSpeed
        {
            get => $"{profileName}-{downSpeed}-{upSpeed}-DOWN";
            set => downSpeed = value;
        }
        
        [XmlElement(ElementName = "TX")]
        public string UpSpeed
        {
            get => $"{profileName}-{downSpeed}-{upSpeed}-UP";
            set => upSpeed = value;
        }
        
        [XmlIgnore]
        public string FileName => $"{_serviceProfile}.xml";
        [XmlIgnore]
        public string Directory => $"{downSpeed}-{upSpeed}";
        [XmlIgnore]
        public string FilePath => @$"{Directory}\{FileName}";


        public Config ShallowCopy()
        {
            return (Config) this.MemberwiseClone();
        }
    }
}