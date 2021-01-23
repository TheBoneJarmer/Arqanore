# Arqanore
Arqanore is an 2D OpenGL framework for creating desktop games for Windows and Linux

## Dependencies

### Arqan
Arqanore is built on top of my other library, [Arqan](https://github.com/TheBoneJarmer/Arqan). Please make sure you have all dependencies installed required for Arqan, otherwise your application will crash during runtime.

### Linux 
Arqanore makes use of the System.Drawing package from Microsoft, this requires however several extra packages which are not required by Windows. Execute the following command to install them.

```
sudo apt-get install libc6-dev libgdiplus libx11-dev
```

## Installation
Just like with Arqan I need to maintain several packages for Arqanore too. And the Windows package is also divided into two packages. One for the x64 architecture and one for the x86. Therefore you can decide to either install Arqanore using the dotnet cli or you could modify your csproj and include a conditional packagereference.

```
dotnet add package Arqanore.Windows.x64
dotnet add package Arqanore.Windows.x86
dotnet add package Arqanore.Linux
```

```
<PropertyGroup Condition="'$(OS)' == 'Windows_NT'">
    <PackageReference Include="Arqanore.Windows.x64" Version="0.6.2" Condition="'$(Platform)' == 'x64' />
    <PackageReference Include="Arqanore.Windows.x86" Version="0.6.2" Condition="'$(Platform)' == 'x86' />
</PropertyGroup>

<PropertyGroup Condition="'$(OS)' == 'UNIX'">
    <PackageReference Include="Arqanore.Linux" Version="0.6.2" />
</PropertyGroup>
```

## Usage
Below code shows how to display a custom window using some properties.

```C#
using System;
using Arqanore;
using Arqanore.Input;
using Arqanore.Graphics;

namespace example
{
    class Program
    {
        Sprite sprite;
        float x;
    
        static void Main(string[] args)
        {
            var window = new Window(800, 600, "Basic Window");
            window.OnOpen += Window_OnOpen;
            window.OnUpdate += Window_OnUpdate;
            window.OnTick += Window_OnTick;
            window.OnRender += Window_OnRender;
            window.Open();
        }
        
        static void Window_OnOpen()
        {
            // Load assets, fonts, init data and what not
            x = 0;   
            sprite = new Sprite("assets/sprite.arqtex"); // Just a path I made up
        }
        
        static void Window_OnUpdate()
        {
            // Perform basic updates, handle keyboard input or what not
            if (KeyBoard.KeyPressed(KeyCode.ESCAPE))
            {
                Close();
            }
        }
        
        static void Window_OnTick(double delta)
        {
            // Use this method for frame-sensitive motions like movements, rotations and physics.
            x += (float)delta;
        }
        
        static void Window_OnRender()
        {
            // Use this method to render your stuff
            sprite.Render(x, 32);
        }
    }
}
```

## Assets
Arqanore does not directly read png, jpg, bmp and font files. Instead it only supports two custom formats. One for fonts and one for images. The motivation behind this decision has to do with the way font files are used to render text. Arqanore uses the tool [fontbm](https://github.com/vladimirgamalyan/fontbm), created by [Vladimir Gamalyan](https://github.com/vladimirgamalyan), to generate bitmap fonts and some data files required to render glyphs correctly. However, to keep things clean and user-friendly I figured it'd be better if I had just one file to load. So hence the font generator was introduced and the media type **Arqanore Font** was invented. And the font generator does nothing more and nothing less than merging the output of fontbm into a single file using a custom standard. On its turn, Arqanore parses that file and extracts the image data along with the font data which are than being used to generate glyphs and values for the Font class.

As for the texture generator, there is actually nothing special about it. Truth is, while I had a good motiviation to invent a custom format for fonts, I did not had one for images. However, in the future I might want to pack images or support spritesheets in a different way. And having a tool already makes that easier. But right now it simply renames files. Also, I did not want to end up with a mix of custom formats and standard formats. I rather have either all assets using a custom format, or none.

## Tools
I already mentioned them in the previous section, but this one will be dedicated to the tools entirely. I created two different command line applications for generating assets. The first being the font generator and the second being the texture generator. I already explained what they were invented for. But now I will explain how to use them.

1. Download the latest release from this repo
2. Extract the zip file somewhere on your system. If possible, add that folder to the PATH environment variable.
3. Open up a command prompt
4. Run either *arqanore-fontgenerator* or *arqanore-texgenerator*. From here on the tools are rather self-explanatory.

### Example usage
```
arqanore-fontgenerator -f /home/ruben/Documents/assets/arial.ttf -s 16 -o .
arqanore-texgenerator -f /home/ruben/Documents/assets/logo.png -o .
```

## Building
Arqanore uses my other project, [Arqan](https://github.com/TheBoneJarmer/Arqan), for access to the OpenGL and GLFW methods in C#. Therefore you need to make sure you have all dependencies installed required by Arqan, otherwise **debugging** will fail. And I purposely focus on debugging here since you can actually build Arqanore, but it would crash during runtime without the required dependencies. Other than that you need _dotnet sdk 5_.

## Contribution
Feel free to send in pull requests at any time!

## Credits
I already mentioned him but I would like to take this opportunity to thank [Vladimir Gamalyan](https://github.com/vladimirgamalyan/) again for his outstanding work with fontbm. That small tool made it possible for me not only to introduce fonts but create a generator tool for it as well. I mean for real, fonts are difficult. I tried all sorts of stuff to render text the right way and I could not figure it out. But he did, so thanks and well done!

## License
[MIT](https://choosealicense.com/licenses/mit/)
