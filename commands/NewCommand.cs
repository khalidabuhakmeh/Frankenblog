using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oakton;
using SimpleExec;
using Spectre.Console;

namespace blog.commands
{
    public class NewCommand
        : OaktonAsyncCommand<NewCommand.Options>
    {
        public class Options
        {
            public const string DefaultEditor = "rider";

            [Description("Name of the post, will also be turned into slug for the url.")]
            public string Title { get; set; }

            [FlagAlias("now", 'n')]
            [Description("Create a post based on today's date", Name = "now")]
            public bool NowFlag { get; set; }

            [FlagAlias("tags", 't')]
            [Description("Tags to add to the newly created post.", Name = "tags")]
            public List<string> TagsFlag { get; set; }

            [FlagAlias("edit", 'e')]
            [Description("Launch the editor to start writing", Name = "edit")]
            public bool EditFlag { get; set; }

            [FlagAlias("editor", longAliasOnly: true)]
            [Description("The editor to launch. Rider by default.", Name = "edit")]
            public string EditorFlag { get; set; }
        }

        public override async Task<bool> Execute(Options input)
        {
            var date = input.NowFlag ? DateTime.Now : Settings.Blog.Next();
            date = new[] {DateTime.Now, date }.Max();

            input.EditorFlag ??= Options.DefaultEditor;

            AnsiConsole.MarkupLine($"‣ [purple]Creating post:[/] \"{input.Title}\"");
            var post =
                await Settings.Blog.CreateFile(input.Title, date, input.TagsFlag?.ToArray());

            AnsiConsole.MarkupLine($"‣ [purple]date:[/] {post.Date:MM/dd/yyyy dddd}");
            AnsiConsole.MarkupLine($"‣ [purple]post:[/] [link={post.FullPath}]{post.FullPath}[/]");

            if (input.EditFlag) {
                AnsiConsole.MarkupLine($"‣ [purple]starting editor:[/] ({input.EditorFlag})");
                await Command.RunAsync(input.EditorFlag, $"{post.FullPath}", noEcho:true);
            }

            return true;
        }
    }
}