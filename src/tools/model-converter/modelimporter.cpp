#include "modelimporter.h"
#include <fstream>

arqanore::ModelImporter::ModelImporter()
{
}

arqanore::ModelImporter::~ModelImporter()
{
}

arqanore::ModelImporterResult arqanore::ModelImporter::load(std::string path)
{


    return {
        materials,
        meshes,
        bones
    };
}
