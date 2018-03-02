namespace ProvisioningTool
{
    public class Context 
    {
        public string ServerHost { get; set; }
        public string LoginUserName { get; set; }
        public string LoginPassword { get; set; }

        public string ParameterCsvFileName => "Parameters.csv";
        public string LocationCsvFileName => "Locations.csv";

        public string InputFolder => "Input";
    }
}
