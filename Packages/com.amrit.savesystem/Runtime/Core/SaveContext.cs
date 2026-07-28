namespace Core.SaveSystem
{
    public sealed class SaveContext
    {
        public string Slot { get; set; } = "Default";

        public SaveOptions Options { get; set; } =
            SaveOptions.PrettyPrint;
    }
}