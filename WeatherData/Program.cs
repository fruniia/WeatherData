namespace WeatherData
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<string> mainMenuText = new()
            {
                "Login",
                "Create User",
                "Exit"
            };

            Menu.MenuList("Test", 1, mainMenuText);
        }
    }
}