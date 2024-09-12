#pragma once

#include "arqanore/model.h"
#include "arqanore/mesh.h"

namespace arqanore
{
    struct ModelImporterResult
    {
        std::vector<Material> materials;
        std::vector<Mesh> meshes;
        std::vector<Bone> bones;
    };

    class ModelImporter
    {
    private:
        std::vector<Material> materials;
        std::vector<Mesh> meshes;
        std::vector<Bone> bones;

    public:
        ModelImporter();

        ~ModelImporter();

        ModelImporterResult load(std::string path);
    };
}