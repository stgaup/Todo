using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Todo
{
    class Program
    {
        static void Main(string[] args)
        {
            const string fileName = "todo.json";

            // Create a list to store our todo items
            List<string>? todoList = new List<string>();

            // Load the todo list from the JSON file
            try
            {
                todoList = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(fileName)) ?? new List<string>();
            }
            catch (System.IO.FileNotFoundException)
            {
                todoList = new List<string>();
            }

            var arg = args.Length > 0 ? args[0] : string.Empty;

            switch (arg)
            {
                case "h":
                case "help":
                    Console.WriteLine("Todo:");
                    Console.WriteLine("-----------------------------------------------------------------------");
                    Console.WriteLine("Usage: TODO {arg} {parameter}");
                    Console.WriteLine("arg:");
                    Console.WriteLine("  help, h  : Writes this help text. No parameter.");
                    Console.WriteLine("  add, a   : Adds todo item. Parameter is item; must be quoted.");
                    Console.WriteLine("  remove, r: Removes an item. Parameter is index of item to be removed.");
                    Console.WriteLine("  list, l  : Lists all items. No parameter.");
                    Console.WriteLine("If no argument is given, the list action will be executed.");
                    Console.WriteLine();
                    break;
                case "a":
                case "add":
                    var item = args[1];
                    todoList.Add(item);
                    ListItems(todoList);
                    break;
                case "r":
                case "remove":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Missing argument. Expected number of item to delete.");
                        return;
                    }
                    var itemToDelete = args[1];
                    var itemNumber = int.Parse(itemToDelete);
                    if (itemNumber >= todoList.Count)
                    {
                        Console.WriteLine("Invalid item number. Cannot remove.");
                    }
                    todoList.RemoveAt(itemNumber);
                    ListItems(todoList);
                    break;
                case "clear":
                    Console.WriteLine("Delete all items. Are you sure? ");
                    if (Console.ReadLine() != "yes")
                    {
                        Console.WriteLine("Aborted.");
                    }
                    todoList.Clear();
                    break;
                case "l":
                case "list":
                default:
                    if (!string.IsNullOrWhiteSpace(arg))
                    {
                        Console.WriteLine("Invalid option specified.");
                    }
                    else
                    {
                        ListItems(todoList);
                    }
                    break;
            }

            File.WriteAllText(fileName, JsonConvert.SerializeObject(todoList));
        }

        private static void ListItems(List<string>? todoList)
        {
            if (todoList == null)
            {
                Console.WriteLine("Todo list is NULL.");
                return;
            }

            if (todoList?.Count == 0)
            {
                Console.WriteLine("List is empty.");
                return;
            }

            var i = 0;
            foreach (var itm in todoList ?? new List<string>())
            {
                Console.WriteLine($"{i++}: {itm}");
            }
        }
    }
}

