# Seanuts
Seanuts is an 2D OpenGL framework that focusses on multiplayer games

## Installation
This library has not yet a NuGet package, once I believe it is solid enough, I will start creating and pushing packages. For now, build it and add it as reference to your project.

## Building

### Requirements

#### Dotnet core
Seanuts requires dotnet core 2.1 or higher to compile.

#### TilarGL
Seanuts is built on top of my other library, TilarGL, which can be found at https://github.com/TheBoneJarmer/TilarGL.

### Step-By-Step

* Clone the TilarGL repository
* Compile TilarGL according the instructions in the README
* Make a folder called 'lib' in the root of your local Seanuts repository
* Copy the resulting dll, TilarGL.dll, to the 'lib' folder
* Compile Seanuts

## Usage
Below code is the source of the example 'basic_window'. For more examples, please look in the 'examples' folder in the root of this repository.

```C#
using System;
using Seanuts.Framework;

namespace example1
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = new GameWindow(800, 600, "Basic Window");
            window.Open(false);
        }
    }
}
```

## Contribution
Feel free to send in pull requests at any time!

## License
[MIT](https://choosealicense.com/licenses/mit/)
