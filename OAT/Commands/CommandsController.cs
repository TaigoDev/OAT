using System.Data;


public class CommandsController
{
    public static List<ICommand> commands = new List<ICommand>();
    public static void init() => new Task(() =>
    {
        try
        {
            foreach (var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(t => typeof(ICommand).IsAssignableFrom(t)).ToList())
                if (type.Name.ToString() != "ICommand")
                    commands.Add((ICommand)Activator.CreateInstance(type));

            while (true)
            {
                try
                {
                    var ReadLine = Console.ReadLine() ?? "error";

                    var UserCommand = !string.IsNullOrEmpty(ReadLine) && !string.IsNullOrWhiteSpace(ReadLine) ? ReadLine : "/error";
                    bool notFound = true;

                    foreach (var command in commands)
                        if (command.pattern.Matches(UserCommand)[0].Value == $"/{command?.name}")
                        {
                            command?.Execute(UserCommand.Replace($"/{command?.name}", "").Split(" "));
                            notFound = false;
                        }
                    if (notFound == true)
                        Console.WriteLine("Command not found, use /help to show all commands");
                }
                catch { }
            }
        }

        catch(Exception ex) {
            Logger.Error(ex.ToString());

        }
    }).Start();

}



