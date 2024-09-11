#pragma once

#include <vector>
#include "material.h"
#include "quaternion.h"
#include "vector3.h"

namespace arqanore {
    class Mesh {
        friend class Model;

        friend class ModelLoader;

        friend class Renderer;

    private:
        unsigned int vao;
        unsigned int vbo_vertices;
        unsigned int vbo_normals;
        unsigned int vbo_texcoords;
        unsigned int vbo_groups;
        unsigned int vbo_weights;
        unsigned int ebo;

    public:
        std::vector<float> vertices;
        std::vector<float> normals;
        std::vector<float> texcoords;
        std::vector<float> groups;
        std::vector<float> weights;
        std::vector<int> indices;
        std::string name;

        Material material;
        Vector3 location;
        Quaternion rotation;
        Vector3 scale;

        Mesh();

        Mesh(std::string name);

        void calculate_normals(bool flip);
    };
}
