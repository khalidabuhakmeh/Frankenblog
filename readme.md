# Frankenblog

![frankenblog post header](https://khalidabuhakmeh.com/assets/images/posts/frankenblog/frankenblog-header.jpg)

This is a mix of .NET Core and Jekyll. It's mostly setup to be Jekyll, but with commands written using .NET Core and Oakton.

## Getting Started

## Prerequsites

- [.NET 5](https://dot.net)
- [Ruby](https://www.ruby-lang.org/en/)

After cloning this repo

```console
> cd frankenblog
> gem install jekyll bundler
> bundle install
> bundle update
> dotnet run info
> dotnet run server
```

### Info Command

To see how your blog is doing run the `dotnet run info` command.

```console
dotnet run info
```

### Server Command

This command starts the jekyll server with future drafts being possible to see.

```console
dotnet run server
```

### New Post

This command will create a new blog post. Run the command to see the possible options.

```console
dotnet run new "this is my awesome new blog post"
```

## License

This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org/>