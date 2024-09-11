#include "modelimporter.h"
#include <fstream>
#include <assimp/postprocess.h>
#include <assimp/scene.h>
#include <assimp/material.h>

arqanore::AssimpException::AssimpException()
{
    this->message = "";
}

arqanore::AssimpException::AssimpException(const std::string& message)
{
    this->message = message;
}

const char* arqanore::AssimpException::what()
{
    return this->message.c_str();
}

std::string arqanore::ModelImporter::get_material_string(aiMaterial* material, const char* key)
{
    aiString str;
    aiGetMaterialString(material, key, 0, 0, &str);

    return std::string(str.C_Str());
}

float arqanore::ModelImporter::get_material_float(aiMaterial* material, const char* key)
{
    float result = 0;
    aiGetMaterialFloat(material, key, 0, 0, &result);

    return result;
}

arqanore::Color arqanore::ModelImporter::get_material_color(aiMaterial* material, const char* key)
{
    aiColor4D color(0, 0, 0, 0);
    aiGetMaterialColor(material, key, 0, 0, &color);

    Color result;
    result.r = color.r * 255;
    result.g = color.g * 255;
    result.b = color.b * 255;
    result.a = color.a * 255;

    return result;
}

void arqanore::ModelImporter::load_material(aiMaterial* material)
{
    Material result;
    result.name = get_material_string(material, "?mat.name");
    result.diffuse = get_material_color(material, "$clr.diffuse");
    result.ambient = get_material_color(material, "$clr.ambient");
    result.specular = get_material_color(material, "$clr.specular");
    result.shininess = get_material_float(material, "mat.shininess");

    // If the name is empty just assign one
    if (result.name.empty())
    {
        result.name = "Material";
    }

    materials.push_back(result);
}

arqanore::ModelImporter::ModelImporter()
{
}

arqanore::ModelImporter::~ModelImporter()
{
}

arqanore::ModelImporterResult arqanore::ModelImporter::load(std::string path)
{
    auto scene = importer.ReadFile(path, aiProcess_Triangulate | aiProcess_FlipUVs);

    if (scene == nullptr)
    {
        throw AssimpException(importer.GetErrorString());
    }

    for (int i = 0; i < scene->mNumMaterials; i++)
    {
        load_material(scene->mMaterials[i]);
    }

    return {
        materials,
        meshes,
        bones
    };
}
