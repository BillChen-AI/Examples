namespace ProvisioningTool.Commands
{
    public interface IProgramCommand
    {
        string Id { get; set; }
        string HelpText { get; set; }
        void Run();
    }
}
