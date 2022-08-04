

namespace ATM_App.App
{
    class Entry
    {
        static void Main(string[] args)
        {
            ATMApp atmApp = new();
            atmApp.InitializeData();
            atmApp.Run();
        }
    }
}
