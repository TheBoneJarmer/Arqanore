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

    for (auto& mesh : result.meshes)
    {
        file << "BEGIN_MESH " << mesh.name << "\n";
        file << "mat " << mesh.material.name << "\n";

        for (int i = 0; i < mesh.vertices.size(); i += 3)
        {
            file << "v " << mesh.vertices[i] << " " << mesh.vertices[i + 1] << " " << mesh.vertices[i + 2] << "\n";
            file << "n " << mesh.normals[i] << " " << mesh.normals[i + 1] << " " << mesh.normals[i + 2] << "\n";
            file << "g -1/0\n";
        }

        for (int i = 0; i < mesh.texcoords.size(); i++)
        {
            file << "tc " << mesh.texcoords[i] << " " << mesh.texcoords[i + 1] << "\n";
        }

        for (int i = 0; i < mesh.indices.size(); i += 3)
        {
            file << "f";
            file << " " << mesh.indices[i] << "/" << mesh.indices[i];
            file << " " << mesh.indices[i + 1] << "/" << mesh.indices[i + 1];
            file << " " << mesh.indices[i + 2] << "/" << mesh.indices[i + 2];
            file << "\n";
        }

        file << "END_MESH";
    }

    // Close fd
    file.close();
}
