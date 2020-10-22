using System.Reflection;
using System.Threading.Tasks;
using blog.commands;
using Oakton;

namespace blog
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var executor = CommandExecutor.For(_ =>
            {
                _.DefaultCommand = typeof(InfoCommand);
                _.RegisterCommands(typeof(Program).GetTypeInfo().Assembly);
            });

            return await executor.ExecuteAsync(args);
        }
    }
}
