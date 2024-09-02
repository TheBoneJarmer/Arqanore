#pragma once

#include <array>

#include "bone.h"
#include "mesh.h"

namespace arqanore
{
    class Model
    {
    private:
        void generate_meshes(std::string& path);

        void generate_buffers(Mesh& mesh);

    public:
        std::vector<Bone> bones;

        std::vector<Mesh> meshes;

        std::array<int, 3> version;

        Model();

        Model(std::string path);

        int total_vertices();

        int total_texcoords();

        int total_normals();

        void calculate_normals(bool flip);
    };
}
