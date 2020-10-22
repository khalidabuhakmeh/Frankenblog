using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace blog.commands
{
    public static class Settings
    {
        private static Lazy<string> BlogDirectory => new Lazy<string>(() => {
            var current = typeof(Program).Assembly.Location;
            var index = current.IndexOf("/bin", StringComparison.Ordinal);
            return current.Substring(0, index);
        });

        public static string GetDirectory(string folder)
            => Path.Combine(CurrentDirectory, folder);

        public static string CurrentDirectory => BlogDirectory.Value; 

        public static class Blog
        {
            private static readonly IDictionary<string, string> Keywords =
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "c#", "csharp" },
                    { ".net", "dotnet" },
                    { "asp.net", "aspnet" }
                };
            
            private static readonly string[] markdownExtensions = new [] { ".markdown", ".md" };
            
            private static Lazy<IReadOnlyList<Post>> posts =
                new Lazy<IReadOnlyList<Post>>(() =>
                {
                    var directory = GetDirectory("_posts");
                    var posts = Directory
                        .GetFiles(directory)
                        .Where(x => 
                            markdownExtensions.Any(f => f.Equals(Path.GetExtension(x), StringComparison.OrdinalIgnoreCase)) 
                        )
                        .OrderByDescending(x => x)
                        .Select(x => new Post(x))
                        .ToList()
                        .AsReadOnly();

                    return posts;
                });

            public static IReadOnlyList<Post> Posts => posts.Value;
            public static Post Latest => 
                Posts.FirstOrDefault() ?? new Post("");
            public static Post Nearest =>
                Posts.Select(p => new {
                        ticks = Math.Abs((p.Date - DateTime.Now).Ticks),
                        post = p
                    })
                    .OrderBy(p => p.ticks)
                    .Select(p => p.post)
                    .FirstOrDefault() ?? new Post("");

            private static DateTime Next(DateTime from, DayOfWeek dayOfTheWeek)
            {
                var date = from.AddDays(1);
                var days = ((int) dayOfTheWeek - (int) date.DayOfWeek + 7) % 7;
                return date.AddDays(days);
            }

            public static DateTime Next()
            {
                // We want the day after the latest post
                // to exclude it from the process
                var date = Latest?.Date ?? DateTime.Now;
                // get next Tuesday and next Thursday
                var dates = new []
                {
                    Next(date, DayOfWeek.Tuesday),
                    Next(date, DayOfWeek.Thursday)
                };

                return dates.Min();
            }

            public static async Task<Post> CreateFile(string title, DateTime date, string[] tags = null)
            {
                var contents = new StringBuilder();
                contents.Append("---");
                contents.AppendLine("layout: post");
                contents.AppendLine($"title: \"{title}\"");
                contents.AppendLine($"tags: {string.Join(",", tags ?? new string[0])}");
                contents.AppendLine("image:");
                contents.AppendLine("image_credit_name:");
                contents.AppendLine("image_credit_url:");
                contents.AppendLine("image_alt");
                contents.AppendLine("---");
                
                // slug clean up for pesky words
                var slug = title;
                foreach (var keyword in Keywords) {
                    slug = slug.Replace(keyword.Key, keyword.Value);
                }
                slug = slug.ToUrlSlug();
                
                var filename = $"{date:yyyy-MM-dd}-{slug}.md";
                var path = Path.Combine(CurrentDirectory, "_posts", filename);
                
                await File.WriteAllTextAsync(path, contents.ToString());
                return new Post(path);
            }
        }
    }

    public class Post
    {
        public Post(string fullPath)
        {
            FullPath = fullPath;
            if (!string.IsNullOrWhiteSpace(fullPath))
            { 
              Filename = Path.GetFileName(FullPath);
              Name = Path.GetFileNameWithoutExtension(Filename[11..]);
              Date = DateTime.Parse(Filename[0..10]);
            }
        }
        
        public string FullPath { get; }
        public string Filename { get; }
        public string Name { get; } 
        public DateTime Date { get; }
    }
    
    public static class UrlSlugger
    {
        // white space, em-dash, en-dash, underscore
        static readonly Regex WordDelimiters = new Regex(@"[\s—–_]", RegexOptions.Compiled);

        // characters that are not valid
        static readonly Regex InvalidChars = new Regex(@"[^a-z0-9\-]", RegexOptions.Compiled);

        // multiple hyphens
        static readonly Regex MultipleHyphens = new Regex(@"-{2,}", RegexOptions.Compiled);

        public static string ToUrlSlug(this string value)
        {
            // convert to lower case
            value = value.ToLowerInvariant();

            // remove diacritics (accents)
            value = RemoveDiacritics(value);

            // ensure all word delimiters are hyphens
            value = WordDelimiters.Replace(value, "-");

            // strip out invalid characters
            value = InvalidChars.Replace(value, "");

            // replace multiple hyphens (-) with a single hyphen
            value = MultipleHyphens.Replace(value, "-");

            // trim hyphens (-) from ends
            return value.Trim('-');
        }

        /// See: http://www.siao2.com/2007/05/14/2629747.aspx
        private static string RemoveDiacritics(string stIn)
        {
            string stFormD = stIn.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}