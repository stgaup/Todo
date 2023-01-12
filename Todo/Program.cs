using Newtonsoft.Json;

namespace Todo
{
    class Program
    {
        static void Main(string?[] args)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).TrimEnd('\\') + "\\Todo\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var fileName = $"{path}todo.json";

            // Create a list to store our todo items
            List<string?> todoList = new List<string?>();

            // Load the todo list from the JSON file
            try
            {
                todoList = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(fileName)) ?? new List<string?>();
            }
            catch (FileNotFoundException)
            {
                todoList = new List<string?>();
            }

            var arg = args.Length > 0 ? args[0] : string.Empty;

            switch (arg)
            {
                case "?":
                case "h":
                case "help":
                    Console.WriteLine("Todo:");
                    Console.WriteLine("-----------------------------------------------------------------------");
                    Console.WriteLine("Usage: TODO {arg} {parameter} [parameter2]");
                    Console.WriteLine("arg:");
                    Console.WriteLine("  help, h   : Writes this help text. No parameter.");
                    Console.WriteLine("  add, a    : Adds todo item. Parameter is item; must be quoted.");
                    Console.WriteLine("  remove, r : Removes an item. Parameter is index of item to be removed.");
                    Console.WriteLine("  edit, e   : Edit an item. Parameter is index of item to be edited. Parameter2 is new text.");
                    Console.WriteLine("  list, l   : Lists all items. No parameter.");
                    Console.WriteLine("  find, d   : Lists all items that contain the given text. Parameter is text to search for.");
                    Console.WriteLine("  clear     : Clears list of all items.");
                    Console.WriteLine("If no argument is given, the list action will be executed.");
                    Console.WriteLine();
                    break;
                case "a":
                case "add":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Missing argument. Expected text for new item as argument.");
                        return;
                    }
                    var item = args[1];
                    todoList.Add(item);
                    ListItems(todoList);
                    break;
                case "r":
                case "remove":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Missing argument. Expected number of item to be removed as argument.");
                        return;
                    }
                    var itemToDelete = args[1];
                    var itemNumber = int.Parse(itemToDelete);
                    if (itemNumber >= todoList.Count)
                    {
                        Console.WriteLine("Invalid item number. Cannot remove.");
                        return;
                    }
                    todoList.RemoveAt(itemNumber);
                    ListItems(todoList);
                    break;
                case "e":
                case "edit":
                    if (args.Length < 3)
                    {
                        Console.WriteLine("Missing argument. Expected number of item to delete as argument.");
                        return;
                    }
                    var itemToEdit = args[1];
                    var itemNumberToEdit = int.Parse(itemToEdit);
                    var newText = args[2];
                    todoList[itemNumberToEdit] = newText;
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
                    ListItems(todoList);
                    break;
                case "f":
                case "find":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Missing argument. Expected text to search for as argument.");
                        return;
                    }
                    var textToSearchFor = args[1];
                    ListItems(todoList, textToSearchFor);
                    break;
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

            File.WriteAllText(fileName, JsonConvert.SerializeObject(todoList.OrderBy(i => i)));
        }

        private static void ListItems(List<string?>? todoList, string? textToSearchFor = null)
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
            foreach (var itm in todoList?.OrderBy(e => e)?.ToList() ?? new List<string?>())
            {
                if (!string.IsNullOrEmpty(textToSearchFor) 
                    && !itm.Contains(textToSearchFor, StringComparison.InvariantCultureIgnoreCase))
                {
                    i++;
                    continue;
                }
                Console.WriteLine($"{i++}: {itm}");
            }
        }
    }
}

