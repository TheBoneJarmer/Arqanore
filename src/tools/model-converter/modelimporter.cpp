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

void arqanore::ModelImporter::parse_material(aiMaterial* material)
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

void arqanore::ModelImporter::parse_mesh(aiMesh* mesh, const aiScene* scene)
{
    auto result = Mesh();
    result.name = mesh->mName.C_Str();
    result.material = materials[mesh->mMaterialIndex];

    for (int i = 0; i < mesh->mNumVertices; i++)
    {
        auto vertex = mesh->mVertices[i];
        result.vertices.push_back(vertex.x);
        result.vertices.push_back(vertex.y);
        result.vertices.push_back(vertex.z);

        auto normal = mesh->mNormals[i];
        result.normals.push_back(normal.x);
        result.normals.push_back(normal.y);
        result.normals.push_back(normal.z);

        if (mesh->mTextureCoords[0] != nullptr)
        {
            auto texcoord = mesh->mTextureCoords[0][i];
            result.texcoords.push_back(texcoord.x);
            result.texcoords.push_back(texcoord.y);
        }
        else
        {
            result.texcoords.push_back(0);
            result.texcoords.push_back(0);
        }
    }

    for (int i=0; i<mesh->mNumFaces; i++)
    {
        auto face = mesh->mFaces[i];

        for (int j=0; j<face.mNumIndices; j++)
        {
            result.indices.push_back(face.mIndices[j]);
        }
    }

    meshes.push_back(result);
}

void arqanore::ModelImporter::parse_node(aiNode* node, const aiScene* scene)
{
    for (int i = 0; i < node->mNumMeshes; i++)
    {
        aiMesh* mesh = scene->mMeshes[node->mMeshes[i]];
        parse_mesh(mesh, scene);
    }

    for (int i = 0; i < node->mNumChildren; i++)
    {
        parse_node(node->mChildren[i], scene);
    }
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

    // Start with parsing all materials first
    for (int i = 0; i < scene->mNumMaterials; i++)
    {
        parse_material(scene->mMaterials[i]);
    }

    // Do armatures second

    // And finish with all meshes
    parse_node(scene->mRootNode, scene);

    return {
        materials,
        meshes,
        bones
    };
}
