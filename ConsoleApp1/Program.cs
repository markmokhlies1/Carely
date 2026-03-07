using BCrypt.Net;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Abdelrahman123@");
            Console.WriteLine(hashedPassword);
        }
    }
}
