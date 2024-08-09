# Arqanore
## About
Arqanore is a 2D and 3D OpenGL game library written in C++ for Windows and Linux desktop.

## Requirements
### 3rd party libs
Arqanore uses GLFW for window and input handling, FreeType for fonts and SoLoud for audio. I created a fork of every dependency which I highly recommend you use since Arqanore is being built and tested with those. For soloud I created a wrapper with only the stuff I needed as that was easier to build and maintain. You can find the urls to my forks below.

* [GLFW](https://github.com/thebonejarmer/glfw)
* [FreeType](https://github.com/thebonejarmer/freetype)
* [SoloudW](https://github.com/thebonejarmer/soloudw)

#### Installation
On my Linux Mint machine I installed them like any other library. I placed the include folders in `/usr/local/include` and the .so files in `/usr/local/lib`. If you wish to build Arqanore manually, you could do the same if you are on Linux.

## Building
### Linux
```bash
mkdir build
cd build
cmake -S .. -DBUILD_SHARED_LIBS=ON
cmake --build .
```

### Windows
> I use MinGW as compiler on Windows. You can pull the latest version from [winlibs](https://winlibs.com/).
> For local development I installed the dll and header files in `C:\Lib` and `C:\Include` respectively. The CMakeLists.txt file is expecting these folders to be present. You might want to modify that part if your setup is different.

```powershell
New-Item -Path . -Name "build" -ItemType "directory" > $null
Set-Location build

cmake -DBUILD_SHARED_LIBS=ON -S .. -G "MinGW Makefiles"
cmake --build .
```

## Examples
* [Basic Window](./src/examples/window/main.cpp)
* [Sprites](./src/examples/sprites/main.cpp)
* [Fonts](./src/examples/fonts/main.cpp)

## Scripts
In this repo you will find some shell scripts. I use these for local development on my Linux Mint machine. They are by no means required to build. The exception _could_ be `generate-shaders.sh`. This script converts the contents of the GLSL shader files to c++ macros. But this is only neccessary when you modify the shader files. You do not need to run this script prior to building.

## Contributing
At this moment I do not accept pull requests from anyone but approved contributors. You can however open up issues or ask questions in the discussions forum.

## License
[MIT](./LICENSE)
