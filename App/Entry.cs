

namespace ATM_App.App
{
    public class Entry
    {
        public static void Main(string[] args)
        {
            ATMApp atmApp = new();
            atmApp.InitializeData();
            atmApp.Run();
        }
    }
}
