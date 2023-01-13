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
            var fileName = $"{path}todo1.json";

            // Create a list to store our todo items
            List<Todo> todoList;

            // Load the todo list from the JSON file
            try
            {
                todoList = JsonConvert.DeserializeObject<List<Todo>>(File.ReadAllText(fileName)) ?? new List<Todo>();
            }
            catch (FileNotFoundException)
            {
                todoList = new List<Todo>();
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
                    Console.WriteLine("  edit, e   : Edit text of an item. Parameter is index of item to be edited. Parameter2 is new text.");
                    Console.WriteLine("  pri, p    : Set priority of an item. Parameter is index of item. Parameter2 is ne priority.");
                    Console.WriteLine("  list, l   : Lists all items. No parameter.");
                    Console.WriteLine("  find, d   : Lists all items that contain the given text. Parameter is text to search for.");
                    Console.WriteLine("  clear     : Clears list of all items.");
                    Console.WriteLine("If no argument is given, the list action will be executed.");
                    Console.WriteLine();
                    break;
                case "a":
                case "add":
                    if (args.Length < 3)
                    {
                        Console.WriteLine("Missing argument. Expected text for new item as argument.");
                        return;
                    }

                    if (!int.TryParse(args[1], out var pri_a))
                    {
                        Console.WriteLine("Invalid parameter: Priority");
                    }
                    var text_a = args[2];
                    todoList.Add(new Todo(pri_a, text_a));
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
                        Console.WriteLine("Missing argument. There should be 3 arguments.");
                        return;
                    }
                    var itemToEdit = args[1];
                    var itemNumberToEdit = int.Parse(itemToEdit);
                    string newText = args[2];
                    todoList[itemNumberToEdit].Text = newText;
                    ListItems(todoList);
                    break;
                case "p":
                case "pri":
                    if (args.Length < 3)
                    {
                        Console.WriteLine("Missing argument. The pri/p command requires 3 arguments.");
                        return;
                    }
                    if (!int.TryParse(args[1], out var itemToPri))
                    {
                        Console.WriteLine("Invalid parameter: Item");
                        return;
                    }
                    if (!int.TryParse(args[2], out var newPri))
                    {
                        Console.WriteLine("Invalid parameter: Priority");
                        return;
                    }
                    todoList[itemToPri].Priority = newPri;
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

            File.WriteAllText(fileName, JsonConvert.SerializeObject(todoList.OrderBy(i => i.Priority).ThenBy(i => i.Text)));
        }

        private static void ListItems(List<Todo> todoList, string? textToSearchFor = null)
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
            foreach (var itm in todoList?.OrderBy(e => e.Priority)?.ThenBy(e => e.Text)?.ToList() ?? new List<Todo>())
            {
                if (!string.IsNullOrEmpty(textToSearchFor) 
                    && !itm.Text.Contains(textToSearchFor, StringComparison.InvariantCultureIgnoreCase))
                {
                    i++;
                    continue;
                }
                Console.WriteLine($"{i:00}: {itm.Priority:00}:{itm.Text}");
                i++;
            }
        }
    }
}

