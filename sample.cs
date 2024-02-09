using System;
using System.Collections.Generic;
using System.IO;

namespace AdvancedBoilerplateApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advanced C# Application");

            Logger logger = new Logger("app_log.txt");
            UserDataBase userDataBase = new UserDataBase(logger);
            Menu menu = new Menu(userDataBase, logger);

            try
            {
                menu.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                logger.Log($"Error: {ex.Message}");
            }

            Console.WriteLine("Application ended. Press any key to exit.");
            Console.ReadKey();
        }
    }

    class Menu
    {
        private UserDataBase _userDataBase;
        private Logger _logger;

        public Menu(UserDataBase userDataBase, Logger logger)
        {
            _userDataBase = userDataBase;
            _logger = logger;
        }

        public void Show()
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\nMenu");
                Console.WriteLine("1. Add User");
                Console.WriteLine("2. List Users");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddUser();
                        break;
                    case "2":
                        ListUsers();
                        break;
                    case "3":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void AddUser()
        {
            Console.Write("Enter user name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Invalid name.");
                _logger.Log("Attempted to add user with invalid name.");
                return;
            }

            _userDataBase.AddUser(new User(name));
            Console.WriteLine($"User {name} added.");
            _logger.Log($"User added: {name}");
        }

        private void ListUsers()
        {
            var users = _userDataBase.GetUsers();
            Console.WriteLine("Users:");
            foreach (var user in users)
            {
                Console.WriteLine(user.Name);
            }
            _logger.Log("Listed users.");
        }
    }

    class UserDataBase
    {
        private List<User> _users = new List<User>();
        private Logger _logger;

        public UserDataBase(Logger logger)
        {
            _logger = logger;
        }

        public void AddUser(User user)
        {
            _users.Add(user);
            _logger.Log($"User added to database: {user.Name}");
        }

        public List<User> GetUsers()
        {
            return _users;
        }
    }

    class User
    {
        public string Name { get; private set; }

        public User(string name)
        {
            Name = name;
        }
    }

    class Logger
    {
        private string _filePath;

        public Logger(string filePath)
        {
            _filePath = filePath;
        }

        public void Log(string message)
        {
            string logMessage = $"{DateTime.Now}: {message}\n";
            File.AppendAllText(_filePath, logMessage);
        }
    }
}
