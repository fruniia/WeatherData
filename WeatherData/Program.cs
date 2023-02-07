using WeatherData.Data;

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
            mainMenuText = ReadData.GetFileDate();
          
            Menu.MenuList("Test", 1, mainMenuText);


        }
    }
}