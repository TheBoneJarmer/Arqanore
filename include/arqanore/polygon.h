#pragma once

#include <array>
#include <cstdlib>
#include "shader.h"
#include "window.h"

namespace arqanore {
    class Polygon {
        friend class Renderer;

    private:
        std::vector<float> vertices;
        std::vector<float> texcoords;
        unsigned int vbo_vertices;
        unsigned int vbo_texcoords;
        unsigned int vao;

        void generate_buffers();

    public:
        Polygon();

        Polygon(std::vector<float> vertices, std::vector<float> texcoords);
    };
}
