using System.Threading.Tasks;
using Oakton;
using SimpleExec;

namespace blog.commands
{
    public class ServerCommand
        : OaktonAsyncCommand<ServerCommand.Options>
    {
        public class Options
        {
        }

        public override async Task<bool> Execute(Options input)
        {
            // allow to see future posts
            await Command.RunAsync(
                "bundle",
                "exec jekyll serve --host=localhost --drafts --future --watch --livereload",
                Settings.CurrentDirectory,
                configureEnvironment: env => {
                        env.Add("JEKYLL_ENV", "development");
                    }
                );

            return true;
        }
    }
}