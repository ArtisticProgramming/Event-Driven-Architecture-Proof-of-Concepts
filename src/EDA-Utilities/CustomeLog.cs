namespace EDA_Utilities
{
    public class CustomeLog 
    {
        public static void WaitForEnter()
        {
            LogInfo("Press Enter!");
            Console.ReadLine();
        }

        protected static void Log(string message)
        {
            Console.WriteLine($"{message}");
        }

        protected static void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan; // Set text color to Cyan
            Console.WriteLine($"[INFO] {message}");
            Console.ResetColor(); // Reset to default color
        }

        protected static void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow; // Set text color to Yellow
            Console.WriteLine($"[WARNING] {message}");
            Console.ResetColor(); // Reset to default color
        }

        protected static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red; // Set text color to Red
            Console.WriteLine($"[ERROR] {message}");
            Console.ResetColor(); // Reset to default color
        }

        protected static void LogSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green; // Set text color to Green
            Console.WriteLine($"[SUCCESS] {message}");
            Console.ResetColor(); // Reset to default color
        }
    }
}