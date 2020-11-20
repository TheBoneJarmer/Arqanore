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

## Tools
Along with the framework I introduced some command line tools for generating assets. I go more into detail about assets later.

### FontGenerator
The font generator toos allows you to generate Arqanore font files (.arqfnt) from TrueType font files (.ttf) using fontbm, a very very nice tool created by [Vladimir Gamalyan](https://github.com/vladimirgamalyan/). I managed to include fontbm within the NuGet packages so you don't need to install it seperately. In fact, I prefer if you don't. The fontgenerator just starts a C# Process and picks which "fontbm" he can find on the command line. Although I am not doing crazy stuff with fontbm, it would be better if the font generator uses the fontbm that is bundled along with it.

#### Installation
```
dotnet tool install Arqanore.FontGenerator.Windows
dotnet tool install Arqanore.FontGenerator.Linux
```

### TexGenerator
The tex generator tool allows you to generate Arqanore texture images from bitmaps, png images, jpeg and what not. In this case whatever the System.Drawing.Bitmap supports actually. If you look at the source you may wonder why the hell I introduced this tool as I only change the file extension. And to answer that question, quite simple: Just in case. I have some futuristic plans about introducing animated sprites and having a format and a tool already present only makes it easier to do so.

## Assets
Arqanore does not directly read png, jpg, bmp and font files. Instead it only supports two custom formats. One for fonts and one for images. The motivation behind this decision has to do with the way font files are used to render text. Arqanore uses the tool [fontbm](https://github.com/vladimirgamalyan/fontbm), created by [Vladimir Gamalyan](https://github.com/vladimirgamalyan), to generate bitmap fonts and some data files required to render glyphs correctly. However, to keep things clean and user-friendly I figured it'd be better if I had just one file to load. So hence the font generator was introduced and the media type **Arqanore Font** was invented. And the font generator does nothing more and nothing less than merging the output of fontbm into a single file using a custom standard. On its turn, Arqanore parses that file and extracts the image data along with the font data which are than being used to generate glyphs and values for the Font class.

As for the texture generator, there is actually nothing special about it. Truth is, while I had a good motiviation to invent a custom format for fonts, I did not had one for images. However, in the future I might want to pack images or support spritesheets in a different way. And having a tool already makes that easier. But right now it simply renames files. Also, I did not want to end up with a mix of custom formats and standard formats. I rather have either all assets using a custom format, or none.

#### Installation
```
dotnet tool install Arqanore.TexGenerator.Windows
dotnet tool install Arqanore.TexGenerator.Linux
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

## Credits
I already mentioned him but I would like to take this opportunity to thank [Vladimir Gamalyan](https://github.com/vladimirgamalyan/) again for his outstanding work with fontbm. That small tool made it possible for me not only to introduce fonts but create a generator tool for it as well. I mean for real, fonts are difficult. I tried all sorts of stuff to render text the right way and I could not figure it out. But he did, so thanks and well done!

## License
[MIT](https://choosealicense.com/licenses/mit/)
