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

    Color arq_color;
    arq_color.r = color.r * 255;
    arq_color.g = color.g * 255;
    arq_color.b = color.b * 255;
    arq_color.a = color.a * 255;

    return arq_color;
}

void arqanore::ModelImporter::parse_material(aiMaterial* material)
{
    Material arq_material;
    arq_material.name = get_material_string(material, "?mat.name");
    arq_material.diffuse = get_material_color(material, "$clr.diffuse");
    arq_material.ambient = get_material_color(material, "$clr.ambient");
    arq_material.specular = get_material_color(material, "$clr.specular");
    arq_material.shininess = get_material_float(material, "mat.shininess");

    // If the name is empty just assign one
    if (arq_material.name.empty())
    {
        arq_material.name = "Material_" + std::to_string(materials.size());
    }

    materials.push_back(arq_material);
}

void arqanore::ModelImporter::parse_mesh(aiMesh* mesh, const aiScene* scene)
{
    auto arq_mesh = Mesh();
    arq_mesh.name = mesh->mName.C_Str();
    arq_mesh.material = materials[mesh->mMaterialIndex];

    for (int i = 0; i < mesh->mNumVertices; i++)
    {
        auto vertex = mesh->mVertices[i];
        arq_mesh.vertices.push_back(vertex.x);
        arq_mesh.vertices.push_back(vertex.y);
        arq_mesh.vertices.push_back(vertex.z);

        auto normal = mesh->mNormals[i];
        arq_mesh.normals.push_back(normal.x);
        arq_mesh.normals.push_back(normal.y);
        arq_mesh.normals.push_back(normal.z);

        if (mesh->mTextureCoords[0] != nullptr)
        {
            auto texcoord = mesh->mTextureCoords[0][i];
            arq_mesh.texcoords.push_back(texcoord.x);
            arq_mesh.texcoords.push_back(texcoord.y);
        }
        else
        {
            arq_mesh.texcoords.push_back(0);
            arq_mesh.texcoords.push_back(0);
        }
    }

    for (int i = 0; i < mesh->mNumBones; i++)
    {
        auto bone = mesh->mBones[i];

    }

    for (int i = 0; i < mesh->mNumFaces; i++)
    {
        auto face = mesh->mFaces[i];

        for (int j = 0; j < face.mNumIndices; j++)
        {
            arq_mesh.indices.push_back(face.mIndices[j]);
        }
    }

    meshes.push_back(arq_mesh);
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

void arqanore::ModelImporter::parse_skeleton(aiSkeleton* skeleton, const aiScene* scene)
{
    for (int i=0; i<scene->mNumAnimations; i++)
    {
        auto animation = scene->mAnimations[i];

    }

    for (int i=0; i<skeleton->mNumBones; i++)
    {
        auto arq_bone = Bone();
        auto bone = skeleton->mBones[i];
        auto node = bone->mNode;

        arq_bone.name = node->mName.C_Str();
        bones.push_back(arq_bone);
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
    auto scene = importer.ReadFile(path,
        aiProcess_Triangulate |
        aiProcess_FlipUVs |
        aiProcess_PopulateArmatureData);

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
    for (int i=0; i<scene->mNumSkeletons; i++)
    {
        parse_skeleton(scene->mSkeletons[i], scene);
    }

    // And finish with all meshes
    parse_node(scene->mRootNode, scene);

    return {
        materials,
        meshes,
        bones
    };
}
