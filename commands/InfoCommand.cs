using System;
using System.Linq;
using Oakton;
using Spectre.Console;

namespace blog.commands
{
    public class InfoCommand
        : OaktonCommand<InfoCommand.Options>
    {
        public class Options { }

        public override bool Execute(Options input)
        {
            var now = DateTime.Now;
            var latest = Settings.Blog.Latest;
            var nearest = Settings.Blog.Nearest;
            var recent = Settings.Blog.Posts.Skip(1).Take(5).ToList();
            var next = Settings.Blog.Next();
            var daysLeft = Math.Max(0, (int) (latest.Date - now).TotalDays);

            string recentFormat(Post post) =>
                post == null 
                    ? "[purple](n/a)[/]"
                    : $"[hotpink]‣[/] [purple]{post?.Name}[/] [fuchsia]({post?.Date:d})[/]";

            var grid = new Grid { Expand = false }
                .AddColumns(
                    new GridColumn().LeftAligned(),
                    new GridColumn().LeftAligned(),
                    new GridColumn(),
                    new GridColumn { NoWrap = true }.LeftAligned()
                )
                .AddRow("🌝", "[pink3]Today[/]", ":", $"[purple]{now:d}[/]")
                .AddRow("📝", "[pink3]Latest post[/]", ":", $"[purple]{latest.Name}[/] [fuchsia]({latest.Date:d})[/]")
                .AddRow("🔥", "[pink3]Nearest post[/]", ":", $"[purple]{nearest.Name}[/] [fuchsia]({nearest.Date:d})[/]")
                .AddRow("🚀", "[pink3]Next post date[/]", ":", $"[purple]{next:MM/dd/yyyy ddddd}[/]")
                .AddRow("🤔", "[pink3]# of days away[/]", ":", $"[purple]{daysLeft}[/]")
                .AddRow("🧮", "[pink3]# of posts[/]", ":", $"[purple]{Settings.Blog.Posts.Count}[/]")
                .AddRow("🦄", "[pink3]Latest posts[/]", ":", recentFormat(recent.FirstOrDefault()));
            
            foreach (var post in recent.Skip(1)) {
                grid.AddRow("", "", "", recentFormat(post));
            }

            var output = new Panel(grid)
                .SetHeader(
                    "  Blog Information  ", 
                    Style
                        .WithBackground(Color.MediumPurple4)
                        .WithForeground(Color.NavajoWhite1)
                        .WithDecoration(Decoration.Italic)
                    ,
                    Justify.Center
                )
                .SetBorderColor(Color.Pink3)
                .SetPadding(1, 1, 1, 1)
                .RoundedBorder();
                
            AnsiConsole.WriteLine();
            AnsiConsole.Render(output);

            return true;
        }
    }
}