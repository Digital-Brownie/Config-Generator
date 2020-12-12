namespace ConfigGenerator
{
    enum ServiceTypes
    {
        L2,
        PPPOE,
        DHCP 
    }

    static class CommandlineArgs
    {
        public const string OutputPath = "-o";
        public const string ConfigName = "-n";
        public const string Speeds = "-s";
        public const string InputPath = "-i";
    }
}