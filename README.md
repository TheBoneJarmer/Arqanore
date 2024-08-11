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
> For local development I installed the dll and header files in `C:\Lib` and `C:\Include` respectively. These are the defaults. To change this you need to set the `WIN_LINK_DIR` and `WIN_INCLUDE_DIR` respectively.

```powershell
mkdir build
cd build

cmake -DBUILD_SHARED_LIBS=ON -S .. -G "MinGW Makefiles"
cmake --build .
```

## Examples
* [Window](./src/examples/window/main.cpp)
* [Sprites](./src/examples/sprites/main.cpp)
* [Fonts](./src/examples/fonts/main.cpp)
* [Models](./src/examples/models/main.cpp)
* [Audio](./src/examples/audio/main.cpp)
* [Shaders](./src/examples/shaders/main.cpp)

## Contributing
At this moment I do not accept pull requests from anyone but approved contributors. You can however open up issues or ask questions in the discussions forum.

## License
[MIT](./LICENSE)
