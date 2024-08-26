#pragma once

#include <vector>
#include "vector3.h"
#include "material.h"
#include "quaternion.h"

namespace arqanore {
    struct MeshFrame {
        int index;
        Vector3 position;
        Quaternion rotation;
        Vector3 scale;
    };

    struct MeshAnimation {
        std::vector<MeshFrame> frames;
    };

    class Mesh {
        friend class Model;

        friend class ModelParser;

        friend class Renderer;

    private:
        unsigned int vao;
        unsigned int vbo_vertices;
        unsigned int vbo_normals;
        unsigned int vbo_texcoords;
        unsigned int ebo;

    public:
        std::vector<float> vertices;
        std::vector<float> normals;
        std::vector<float> texcoords;
        std::vector<int> indices;
        std::string name;

        MeshAnimation animation;
        Material material;

        Mesh();

        Mesh(std::string name);

        void calculate_normals(bool flip);
    };
}