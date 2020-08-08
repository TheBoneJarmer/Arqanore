# Arqanore
Arqanore is an 2D OpenGL framework for creating desktop games for Windows and Linux

## Dependencies

### Dotnet core
Arqanore requires dotnet core 2.1 or higher to compile.

### Arqan
Arqanore is built on top of my other library, Arqan, which can be found at https://github.com/TheBoneJarmer/Arqan. **Please make sure you read the readme within the repo of Arqan as you will need to download and install several dependencies.**

### Linux 
Arqanore makes use of the System.Drawing package from Microsoft, this requires however several extra packages which are not required by Windows. Execute the following command to install them.

```
sudo apt-get install libc6-dev libgdiplus libx11-dev
```

## Building
Unlike with Arqan, you don't need to build a source per operating system. I configured the csproj file in such a way that dotnet will conditionally use the Arqan package per operating system.

## Installation
Because of Arqan I need to maintain two packages for Arqanore too. However, I won't need to maintain two sources which I need to with Arqan. Therefore in order to install Arqanore you need to run either one of these commands, depending on your operating system. You could also create a conditional section in your csproj, just like I did with Arqanore. Either ways are fine.

```
dotnet add package Arqanore.Windows
dotnet add package Arqanore.Linux
```

```
<Choose>
    <When Condition=" '$(OS)' == 'Windows_NT' ">
      <ItemGroup>
        <PackageReference Include="Arqanore.Windows" Version="0.1.1" />
      </ItemGroup>
    </When>
    <When Condition=" '$(OS)' == 'UNIX' ">
      <ItemGroup>
        <PackageReference Include="Arqanore.Linux" Version="0.1.1" />
      </ItemGroup>
    </When>
</Choose>
```

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
            var window = new Window(800, 600, "Basic Window");
            window.Open();
        }
    }
}
```

## Contribution
Feel free to send in pull requests at any time!

## License
[MIT](https://choosealicense.com/licenses/mit/)
