#pragma once
#define STB_IMAGE_IMPLEMENTATION

#include <string>
#include <cstdlib>

namespace arqanore {
    class Texture {
        friend class Renderer;
        friend class Shader;

    private:
        unsigned int id;
    public:
        int width;
        int height;

        Texture();

        Texture(std::string filename);
    };
}
