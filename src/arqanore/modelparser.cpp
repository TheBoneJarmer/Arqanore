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

void arqanore::ModelParser::parse_line(std::string& key, std::string& value, Mesh* & mesh, Material* & material, Bone* & bone, std::string& path)
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

    if (bone == nullptr && key == "BEGIN_BONE")
    {
        bone = new Bone();
        bone->name = value;
    }

    if (bone != nullptr && key == "END_BONE")
    {
        bones.push_back(*bone);
        bone = nullptr;
    }

    if (mesh != nullptr)
    {
        parse_mesh(key, value, mesh);
    }

    if (material != nullptr)
    {
        parse_material(key, value, material, path);
    }

    if (bone != nullptr)
    {
        parse_bone(key, value, bone);
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
    if (key == "g") parse_mesh_group(value, mesh);
}

void arqanore::ModelParser::parse_mesh_material(std::string& value, Mesh* mesh)
{
    for (Material& mat : materials)
    {
        if (mat.name == value)
        {
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

void arqanore::ModelParser::parse_mesh_group(std::string& value, Mesh* mesh)
{
    std::vector<std::string> values = string_split(value, ' ');
    int values_length = values.size();
    Vector4 group;
    Vector4 weight;

    for (int i = 0; i < 4; i++)
    {
        int group_value = -1;
        float weight_value = 0;

        if (i < values_length)
        {
            std::vector<std::string> boneweights = string_split(values[i], '/');

            group_value = std::stoi(boneweights[0]);
            weight_value = std::stof(boneweights[1]);
        }

        if (i == 0)
        {
            group.x = group_value;
            weight.x = weight_value;
        }

        if (i == 1)
        {
            group.y = group_value;
            weight.y = weight_value;
        }

        if (i == 2)
        {
            group.z = group_value;
            weight.z = weight_value;
        }

        if (i == 3)
        {
            group.w = group_value;
            weight.w = weight_value;
        }
    }

    groups.push_back(group);
    weights.push_back(weight);
}

void arqanore::ModelParser::parse_mesh_face(std::string& value, Mesh* mesh)
{
    std::vector<std::string> values = string_split(value, ' ');

    if (values.size() > 3)
    {
        throw ArqanoreException("Too many index_count definitions. Expected 3 but found " + std::to_string(values.size()));
    }

    for (int i = 0; i < 3; i++)
    {
        std::vector<std::string> indices = string_split(values[i], '/');
        int vertex_index = stoi(indices[0]);
        int texcoord_index = stoi(indices[1]);

        Vector3 vertex(0, 0, 0);
        Vector3 normal(0, 0, 0);
        Vector2 texcoord(0, 0);
        Vector4 group(-1, -1, -1, -1);
        Vector4 weight(0, 0, 0, 0);
        int index = mesh->indices.size();

        if (vertex_index != -1)
        {
            vertex = vertices[vertex_index];
            normal = normals[vertex_index];

            if (vertex_index < groups.size())
            {
                group = groups[vertex_index];
                weight = weights[vertex_index];
            }
        }

        if (texcoord_index != -1 && texcoord_index < texcoords.size())
        {
            texcoord = texcoords[texcoord_index];
        }

        mesh->texcoords.push_back(texcoord.x);
        mesh->texcoords.push_back(texcoord.y);
        mesh->vertices.push_back(vertex.x);
        mesh->vertices.push_back(vertex.y);
        mesh->vertices.push_back(vertex.z);
        mesh->normals.push_back(normal.x);
        mesh->normals.push_back(normal.y);
        mesh->normals.push_back(normal.z);
        mesh->groups.push_back(group.x);
        mesh->groups.push_back(group.y);
        mesh->groups.push_back(group.z);
        mesh->groups.push_back(group.w);
        mesh->weights.push_back(weight.x);
        mesh->weights.push_back(weight.y);
        mesh->weights.push_back(weight.z);
        mesh->weights.push_back(weight.w);
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

void arqanore::ModelParser::parse_bone(std::string& key, std::string& value, Bone* bone)
{
    if (key == "bf") parse_bone_frame(value, bone);
    if (key == "p") parse_bone_parent(value, bone);
}

void arqanore::ModelParser::parse_bone_parent(std::string& value, Bone* bone)
{
    for (Bone& child : bones)
    {
        if (child.name == value)
        {
            bone->parent = &child;
            return;
        }
    }

    throw ArqanoreException("No bone parent found with name '" + value + "'");
}

void arqanore::ModelParser::parse_bone_frame(std::string& value, Bone* bone)
{
    auto values = string_split(value, ' ');
    auto frame = BoneFrame();
    frame.location = Vector3(stof(values[0]), stof(values[1]), stof(values[2]));
    frame.rotation = Quaternion(stof(values[3]), stof(values[4]), stof(values[5]), stof(values[6]));
    frame.scale = Vector3(stof(values[7]), stof(values[8]), stof(values[9]));

    bone->frames.push_back(frame);
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
        Bone* bone = nullptr;

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

            parse_line(key, value, mesh, material, bone, path);
        }

        delete mesh;
        delete material;
        delete bone;
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
        this->bones,
        this->materials,
        this->meshes,
        this->version
    };
}
