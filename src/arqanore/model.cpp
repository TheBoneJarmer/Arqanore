#include "glad/gl.h"
#include "arqanore/model.h"
#include "arqanore/modelloader.h"
#include "arqanore/exceptions.h"

void arqanore::Model::generate_meshes(std::string& path)
{
    ModelLoader parser;
    ModelLoaderResult result = parser.load(path);

    this->bones = result.bones;
    this->meshes = result.meshes;
    this->version = result.version;

    for (Mesh& mesh : this->meshes)
    {
        generate_buffers(mesh);
    }
}

void arqanore::Model::generate_buffers(Mesh& mesh)
{
    int vertex_attrib_location = 0;
    int normal_attrib_location = 1;
    int texcoord_attrib_location = 2;
    int bone_attrib_location = 3;
    int weight_attrib_location = 4;

    glGenVertexArrays(1, &mesh.vao);
    glBindVertexArray(mesh.vao);

    glGenBuffers(1, &mesh.vbo_vertices);
    glBindBuffer(GL_ARRAY_BUFFER, mesh.vbo_vertices);
    glBufferData(GL_ARRAY_BUFFER, mesh.vertices.size() * sizeof(float), mesh.vertices.data(), GL_STATIC_DRAW);
    glVertexAttribPointer(vertex_attrib_location, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), nullptr);
    glEnableVertexAttribArray(vertex_attrib_location);

    glGenBuffers(1, &mesh.vbo_normals);
    glBindBuffer(GL_ARRAY_BUFFER, mesh.vbo_normals);
    glBufferData(GL_ARRAY_BUFFER, mesh.normals.size() * sizeof(float), mesh.normals.data(), GL_STATIC_DRAW);
    glVertexAttribPointer(normal_attrib_location, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), nullptr);
    glEnableVertexAttribArray(normal_attrib_location);

    glGenBuffers(1, &mesh.vbo_texcoords);
    glBindBuffer(GL_ARRAY_BUFFER, mesh.vbo_texcoords);
    glBufferData(GL_ARRAY_BUFFER, mesh.texcoords.size() * sizeof(float), mesh.texcoords.data(), GL_STATIC_DRAW);
    glVertexAttribPointer(texcoord_attrib_location, 2, GL_FLOAT, GL_FALSE, 2 * sizeof(float), nullptr);
    glEnableVertexAttribArray(texcoord_attrib_location);

    glGenBuffers(1, &mesh.vbo_groups);
    glBindBuffer(GL_ARRAY_BUFFER, mesh.vbo_groups);
    glBufferData(GL_ARRAY_BUFFER, mesh.groups.size() * sizeof(float), mesh.groups.data(), GL_STATIC_DRAW);
    glVertexAttribPointer(bone_attrib_location, 4, GL_FLOAT, GL_FALSE, 4 * sizeof(float), nullptr);
    glEnableVertexAttribArray(bone_attrib_location);

    glGenBuffers(1, &mesh.vbo_weights);
    glBindBuffer(GL_ARRAY_BUFFER, mesh.vbo_weights);
    glBufferData(GL_ARRAY_BUFFER, mesh.weights.size() * sizeof(float), mesh.weights.data(), GL_STATIC_DRAW);
    glVertexAttribPointer(weight_attrib_location, 4, GL_FLOAT, GL_FALSE, 4 * sizeof(float), nullptr);
    glEnableVertexAttribArray(weight_attrib_location);

    glGenBuffers(1, &mesh.ebo);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, mesh.ebo);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, mesh.indices.size() * sizeof(unsigned int), mesh.indices.data(), GL_STATIC_DRAW);

    glBindVertexArray(0);
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
}

arqanore::Model::Model()
{
    version = std::array<int, 3>();
}

arqanore::Model::Model(std::string path) : Model()
{
    if (!path.ends_with(".arqm"))
    {
        throw ArqanoreException("Invalid model format");
    }

    this->generate_meshes(path);
}

int arqanore::Model::total_vertices()
{
    auto result = 0;

    for (auto& mesh : meshes)
    {
        result += static_cast<int>(mesh.vertices.size());
    }

    return result;
}

int arqanore::Model::total_texcoords()
{
    auto result = 0;

    for (auto& mesh : meshes)
    {
        result += static_cast<int>(mesh.texcoords.size());
    }

    return result;
}

int arqanore::Model::total_normals()
{
    auto result = 0;

    for (auto& mesh : meshes)
    {
        result += static_cast<int>(mesh.normals.size());
    }

    return result;
}

void arqanore::Model::calculate_normals(bool flip)
{
    for (Mesh& mesh : meshes)
    {
        mesh.calculate_normals(flip);

        // Update the VBO for normals because otherwise the change wont be visible
        glBindVertexArray(mesh.vao);
        glBindBuffer(GL_ARRAY_BUFFER, mesh.vbo_normals);
        glBufferData(GL_ARRAY_BUFFER, mesh.normals.size() * sizeof(float), mesh.normals.data(), GL_STATIC_DRAW);
        glBindBuffer(GL_ARRAY_BUFFER, 0);
        glBindVertexArray(0);
    }
}
