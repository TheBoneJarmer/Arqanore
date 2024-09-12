#pragma once

#include <vector>

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
