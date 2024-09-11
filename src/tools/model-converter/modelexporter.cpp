#include "modelexporter.h"
#include <fstream>

arqanore::ModelExporter::ModelExporter()
{
}

void arqanore::ModelExporter::save(ModelImporterResult result, std::string path)
{
    std::ofstream file(path);

    // Write version first
    file << "VERSION 0.1.0\n";

    // Write down all materials
    for (auto& mat : result.materials)
    {
        file << "BEGIN_MAT " << mat.name << "\n";
        file << "dif " << mat.diffuse.r << " " << mat.diffuse.g << " " << mat.diffuse.b << "\n";
        file << "amb " << mat.ambient.r << " " << mat.ambient.g << " " << mat.ambient.b << "\n";
        file << "spc " << mat.specular.r << " " << mat.specular.g << " " << mat.specular.b << "\n";
        file << "shn " << mat.shininess << "\n";
        file << "END_MAT\n";
    }

    // Close fd
    file.close();
}
