#include "model-importer.h"

#include <iostream>
#include <assimp/postprocess.h>

ModelImporter::ModelImporter()
{
    this->armature = nullptr;
}

ModelImporter::~ModelImporter()
{
    delete this->armature;
}

bool ModelImporter::load(std::string path)
{
    auto scene = importer.ReadFile(path, aiProcess_Triangulate);

    if (scene == nullptr)
    {
        std::cerr << importer.GetErrorString() << std::endl;
        return false;
    }

    // Process the scene

    return true;
}
