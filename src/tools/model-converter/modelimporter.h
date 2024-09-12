#pragma once
#include <assimp/Importer.hpp>
#include <assimp/material.h>
#include <assimp/scene.h>

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

    class AssimpException : public std::exception
    {
    private:
        std::string message;

    public:
        AssimpException();

        AssimpException(const std::string& message);

        const char *what();
    };

    class ModelImporter
    {
    private:
        Assimp::Importer importer;
        std::vector<Material> materials;
        std::vector<Mesh> meshes;
        std::vector<Bone> bones;

        std::string get_material_string(aiMaterial* material, const char* key);
        float get_material_float(aiMaterial* material, const char* key);
        Color get_material_color(aiMaterial* material, const char* key);

        void parse_material(aiMaterial* material);
        void parse_mesh(aiMesh* mesh, const aiScene* scene);
        void parse_node(aiNode* node, const aiScene* scene);
        void parse_skeleton(aiSkeleton* skeleton, const aiScene* scene);

    public:
        ModelImporter();

        ~ModelImporter();

        ModelImporterResult load(std::string path);
    };
}