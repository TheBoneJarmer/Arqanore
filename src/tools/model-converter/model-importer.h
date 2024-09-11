#pragma once
#include <assimp/Importer.hpp>

#include "arqanore/model.h"
#include "arqanore/mesh.h"

class ModelImporter
{
private:
    Assimp::Importer importer;

public:
    std::vector<arqanore::Mesh> meshes;
    std::vector<arqanore::Material> materials;
    arqanore::Armature* armature;

    ModelImporter();

    ~ModelImporter();

    bool load(std::string path);
};
