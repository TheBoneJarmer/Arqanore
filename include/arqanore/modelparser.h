#pragma once

#include <vector>
#include "vector2.h"
#include "vector3.h"
#include "mesh.h"
#include "material.h"
#include "model.h"

namespace arqanore {
    struct ModelParserResult {
        std::vector<Material> materials;
        std::vector<Mesh> meshes;
        std::array<int, 3> version;
    };

    class ModelParser {
    private:
        std::vector<Vector3> vertices;
        std::vector<Vector3> normals;
        std::vector<Vector2> texcoords;
        std::vector<Material> materials;
        std::vector<Mesh> meshes;
        std::array<int, 3> version;

        Vector3 parse_vector3(std::string &value);

        Vector2 parse_vector2(std::string &value);

        Color parse_color(std::string &value);

        void parse_line(std::string &key, std::string &value, Mesh *&mesh, Material *&material, std::string &path);
        
        void parse_version(std::string& value);

        void parse_mesh(std::string &key, std::string &value, Mesh *mesh);

        void parse_mesh_frame(std::string &value, Mesh *mesh);

        void parse_mesh_vertex(std::string &value, Mesh *mesh);

        void parse_mesh_normal(std::string &value, Mesh *mesh);

        void parse_mesh_texcoord(std::string &value, Mesh *mesh);

        void parse_mesh_material(std::string &value, Mesh *mesh);

        void parse_mesh_face(std::string &value, Mesh *mesh);

        void parse_material(std::string &key, std::string &value, Material *material, std::string &path);

        void parse_material_color(std::string &value, Material *material);

        void parse_material_diffuse(std::string &value, Material *material);

        void parse_material_ambient(std::string &value, Material *material);

        void parse_material_specular(std::string &value, Material *material);

        void parse_material_shininess(std::string &value, Material *material);

        void parse_material_diffuse_map(std::string &value, Material *material, std::string &path);

        void parse_material_ambient_map(std::string &value, Material *material, std::string &path);

        void parse_material_specular_map(std::string &value, Material *material, std::string &path);

    public:
        ModelParser();

        ModelParserResult parse(std::string &path);
    };
}
