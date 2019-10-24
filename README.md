# Arqanore
Arqanore is an 2D OpenGL framework that focusses on multiplayer games

## Installation
This library has not yet a NuGet package, once I believe it is solid enough, I will start creating and pushing packages. For now, build it and add it as reference to your project.

## Requirements

### Dotnet core
Arqanore requires dotnet core 2.1 or higher to compile.

### Arqan
Arqanore is built on top of my other library, Arqan, which can be found at https://github.com/TheBoneJarmer/Arqan. In order to run applications using the Arqanore framework, 
you need to have a copy of Arqan.dll in your binaries folder. And of course the dependencies of Arqan, but please read the README in the Arqan repo for more info about that. 
To build Arqanore itself, you need to make a 'lib' folder in the root of the repository and copy Arqan.dll over there.

## Usage
Below code for showing a basic window. For more examples, please look in the 'examples' folder in the root of this repository.

```C#
using System;
using Arqanore.Framework;

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
