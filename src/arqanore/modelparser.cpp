#include <string>
#include "arqanore/modelparser.h"
#include "arqanore/utils.h"
#include "arqanore/exceptions.h"
#include "arqanore/quaternion.h"

arqanore::Vector3 arqanore::ModelParser::parse_vector3(std::string& value)
{
    auto vec = Vector3();
    auto values = string_split(value, ' ');

    vec.x = stof(values[0]);
    vec.y = stof(values[1]);
    vec.z = stof(values[2]);

    return vec;
}

arqanore::Vector2 arqanore::ModelParser::parse_vector2(std::string& value)
{
    auto vec = Vector2();
    auto values = string_split(value, ' ');

    vec.x = stof(values[0]);
    vec.y = stof(values[1]);

    return vec;
}

arqanore::Color arqanore::ModelParser::parse_color(std::string& value)
{
    auto color = Color();
    auto values = string_split(value, ' ');

    color.r = stoi(values[0]);
    color.g = stoi(values[1]);
    color.b = stoi(values[2]);
    color.a = stoi(values[3]);

    return color;
}

arqanore::Quaternion arqanore::ModelParser::parse_quaternion(std::string& value)
{
    auto quat = Quaternion();
    auto values = string_split(value, ' ');

    quat.x = stof(values[0]);
    quat.y = stof(values[1]);
    quat.z = stof(values[2]);
    quat.w = stof(values[3]);

    return quat;
}

void arqanore::ModelParser::parse_line(std::string& key, std::string& value, Mesh*& mesh, Material*& material, std::string& path)
{
    if (key == "VERSION")
    {
        parse_version(value);
    }

    if (mesh == nullptr && key == "BEGIN_MESH")
    {
        mesh = new Mesh(value);
    }

    if (mesh != nullptr && key == "END_MESH")
    {
        vertices.clear();
        normals.clear();
        texcoords.clear();

        meshes.push_back(*mesh);
        mesh = nullptr;
    }

    if (material == nullptr && key == "BEGIN_MAT")
    {
        material = new Material(value);
    }

    if (material != nullptr && key == "END_MAT")
    {
        materials.push_back(*material);
        material = nullptr;
    }

    if (mesh != nullptr)
    {
        parse_mesh(key, value, mesh);
    }

    if (material != nullptr)
    {
        parse_material(key, value, material, path);
    }
}

void arqanore::ModelParser::parse_version(std::string& value)
{
    std::vector<std::string> values = string_split(value, '.');

    this->version[0] = std::stoi(values[0]);
    this->version[1] = std::stoi(values[1]);
    this->version[2] = std::stoi(values[2]);
}

void arqanore::ModelParser::parse_mesh(std::string& key, std::string& value, Mesh* mesh)
{
    if (key == "mat") parse_mesh_material(value, mesh);
    if (key == "v") parse_mesh_vertex(value, mesh);
    if (key == "n") parse_mesh_normal(value, mesh);
    if (key == "tc") parse_mesh_texcoord(value, mesh);
    if (key == "f") parse_mesh_face(value, mesh);
    if (key == "loc") parse_mesh_location(value, mesh);
    if (key == "rot") parse_mesh_rotation(value, mesh);
    if (key == "scl") parse_mesh_scale(value, mesh);
}

void arqanore::ModelParser::parse_mesh_material(std::string &value, Mesh *mesh) {
    for (Material &mat: materials) {
        if (mat.name == value) {
            mesh->material = mat;
        }
    }
}

void arqanore::ModelParser::parse_mesh_location(std::string& value, Mesh* mesh)
{
    mesh->location = parse_vector3(value);
}

void arqanore::ModelParser::parse_mesh_rotation(std::string& value, Mesh* mesh)
{
    mesh->rotation = parse_quaternion(value);
}

void arqanore::ModelParser::parse_mesh_scale(std::string& value, Mesh* mesh)
{
    mesh->scale = parse_vector3(value);
}

void arqanore::ModelParser::parse_mesh_vertex(std::string& value, Mesh* mesh)
{
    auto vector = parse_vector3(value);
    vertices.push_back(vector);
}

void arqanore::ModelParser::parse_mesh_normal(std::string& value, Mesh* mesh)
{
    auto vector = parse_vector3(value);
    normals.push_back(vector);
}

void arqanore::ModelParser::parse_mesh_texcoord(std::string& value, Mesh* mesh)
{
    auto vector = parse_vector2(value);
    vector.y *= -1;

    texcoords.push_back(vector);
}

void arqanore::ModelParser::parse_mesh_face(std::string& value, Mesh* mesh)
{
    auto values = string_split(value, ' ');

    if (values.size() > 3)
    {
        throw ArqanoreException("Too many index_count definitions. Expected 3 but found " + std::to_string(values.size()));
    }

    for (int i = 0; i < 3; i++)
    {
        auto indices = string_split(values[i], '/');
        auto vertex_index = stoi(indices[0]);
        auto texcoord_index = stoi(indices[1]);

        if (vertex_index != -1)
        {
            auto vertex = vertices[vertex_index];
            mesh->vertices.push_back(vertex.x);
            mesh->vertices.push_back(vertex.y);
            mesh->vertices.push_back(vertex.z);

            auto normal = normals[vertex_index];
            mesh->normals.push_back(normal.x);
            mesh->normals.push_back(normal.y);
            mesh->normals.push_back(normal.z);
        }
        else
        {
            mesh->vertices.push_back(0);
            mesh->vertices.push_back(0);
            mesh->vertices.push_back(0);

            mesh->normals.push_back(0);
            mesh->normals.push_back(0);
            mesh->normals.push_back(0);
        }

        if (texcoord_index != -1 && texcoord_index < texcoords.size())
        {
            auto texcoord = texcoords[texcoord_index];
            mesh->texcoords.push_back(texcoord.x);
            mesh->texcoords.push_back(texcoord.y);
        }
        else
        {
            mesh->texcoords.push_back(0);
            mesh->texcoords.push_back(0);
        }

        auto index = mesh->indices.size();
        mesh->indices.push_back(index);
    }
}

void arqanore::ModelParser::parse_material(std::string& key, std::string& value, Material* material, std::string& path)
{
    if (key == "clr") parse_material_color(value, material);
    if (key == "dif") parse_material_diffuse(value, material);
    if (key == "amb") parse_material_ambient(value, material);
    if (key == "spc") parse_material_specular(value, material);
    if (key == "shn") parse_material_shininess(value, material);
    if (key == "dif_map") parse_material_diffuse_map(value, material, path);
    if (key == "amb_map") parse_material_ambient_map(value, material, path);
    if (key == "spc_map") parse_material_specular_map(value, material, path);
}

void arqanore::ModelParser::parse_material_color(std::string& value, Material* material)
{
    material->color = parse_color(value);
}

void arqanore::ModelParser::parse_material_diffuse(std::string& value, Material* material)
{
    material->diffuse = parse_color(value);
}

void arqanore::ModelParser::parse_material_ambient(std::string& value, Material* material)
{
    material->ambient = parse_color(value);
}

void arqanore::ModelParser::parse_material_specular(std::string& value, Material* material)
{
    material->specular = parse_color(value);
}

void arqanore::ModelParser::parse_material_shininess(std::string& value, Material* material)
{
    material->shininess = std::stof(value);
}

void arqanore::ModelParser::parse_material_diffuse_map(std::string& value, Material* material, std::string& path)
{
    auto parent_path = get_parent_path(path);
    auto full_path = parent_path.string() + "/" + value;

    material->diffuse_map = new Texture(full_path);
}

void arqanore::ModelParser::parse_material_ambient_map(std::string& value, Material* material, std::string& path)
{
    auto parent_path = get_parent_path(path);
    auto full_path = parent_path.string() + "/" + value;

    material->ambient_map = new Texture(full_path);
}

void arqanore::ModelParser::parse_material_specular_map(std::string& value, Material* material, std::string& path)
{
    auto parent_path = get_parent_path(path);
    auto full_path = parent_path.string() + "/" + value;

    material->specular_map = new Texture(full_path);
}

arqanore::ModelParser::ModelParser()
{
}

arqanore::ModelParserResult arqanore::ModelParser::parse(std::string& path)
{
    int line_number = 0;
    std::vector<std::string> lines;

    if (!path.ends_with(".arqm"))
    {
        throw ArqanoreException("Unsupported model type");
    }

    read_file(path, lines);

    try
    {
        Mesh* mesh = nullptr;
        Material* material = nullptr;

        for (std::string& line : lines)
        {
            line_number++;

            // Prevent whitespace at the beginning of a line as the parser depends on it as separator
            if (line.starts_with("\t") || line.starts_with(" "))
            {
                throw ArqanoreException("Illegal whitespace detected at begin of line.");
            }

            // Skip empty lines and comments
            if (line.empty() || line.starts_with("#"))
            {
                continue;
            }

            std::string key = string_split(line, ' ')[0];
            std::string value = string_replace(line, key + " ", "");

            parse_line(key, value, mesh, material, path);
        }

        delete mesh;
        delete material;
    }
    catch (ArqanoreException& ex)
    {
        throw ArqanoreException(static_cast<std::string>(ex.what()) + " at line " + std::to_string(line_number));
    } catch (std::exception& ex)
    {
        throw ArqanoreException(static_cast<std::string>(ex.what()) + " at line " + std::to_string(line_number));
    } catch (...)
    {
        throw ArqanoreException("An unknown error occurred while parsing model file at line " + std::to_string(line_number));
    }

    return {
        this->materials,
        this->meshes,
        this->version
    };
}
