#pragma once

#include <array>
#include "mesh.h"

namespace arqanore
{
    struct Bone
    {
        std::string name;
        Bone* parent;
    };

    struct Armature
    {
        std::vector<Bone> bones;
    };

    class Model
    {
    private:
        void generate_meshes(std::string& path);

        void generate_buffers(Mesh& mesh);

    public:
        Armature* armature;

        std::vector<Mesh> meshes;

        std::array<int, 3> version;

        Model();

        Model(std::string path);

        ~Model();

        int total_vertices();

        int total_texcoords();

        int total_normals();

        int total_frames();

        void calculate_normals(bool flip);
    };
}
