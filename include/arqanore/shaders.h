#pragma once

#include "shader.h"

namespace arqanore {
    class Shaders {
        friend class Window;

    private:
        static void init();

    public:
        static Shader polygon;
        static Shader font;
        static Shader sprite;
    };
}
