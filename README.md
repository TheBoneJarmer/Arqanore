# Seanuts
Seanuts is an 2D OpenGL framework that focusses on multiplayer games

## Installation
This library has not yet a NuGet package, once I believe it is solid enough, I will start creating and pushing packages. For now, build it and add it as reference to your project.

## Requirements

### Dotnet core
Seanuts requires dotnet core 2.1 or higher to compile.

### TilarGL
Seanuts is built on top of my other library, TilarGL, which can be found at https://github.com/TheBoneJarmer/TilarGL. In order to run applications using the Seanuts framework, 
you need to have a copy of TilarGL.dll in your binaries folder. And of course the dependencies of TilarGL, but please read the README in the TilarGL repo for more info about that.

## Usage
Below code for showing a basic window. For more examples, please look in the 'examples' folder in the root of this repository.

```C#
using System;
using Seanuts.Framework;

namespace example
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = new GameWindow(800, 600, "Basic Window");
            window.Open();
        }
    }
}
```

## Contribution
Feel free to send in pull requests at any time!

## License
[MIT](https://choosealicense.com/licenses/mit/)
