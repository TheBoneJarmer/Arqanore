#include <iostream>
#include "arqanore/mesh.h"

arqanore::Mesh::Mesh() {
    this->name = "mesh";
    this->material = Material();
    this->vao = 0;
    this->vbo_normals = 0;
    this->vbo_texcoords = 0;
    this->vbo_vertices = 0;
    this->ebo = 0;
}

arqanore::Mesh::Mesh(std::string name) : Mesh() {
    this->name = name;
}

void arqanore::Mesh::calculate_normals(bool flip) {
    std::vector<Vector3> v_vertices;
    std::vector<Vector3> v_normals;

    for (int i = 0; i < vertices.size(); i += 3) {
        Vector3 v;
        v.x = vertices[i];
        v.y = vertices[i + 1];
        v.z = vertices[i + 2];

        v_vertices.push_back(v);
    }

    for (int i = 0; i < v_vertices.size(); i += 3) {
        Vector3& v1  = v_vertices[i];
        Vector3& v2 = v_vertices[i + 1];
        Vector3& v3 = v_vertices[i + 2];
        Vector3 n = Vector3::cross(v2 - v1, v3 - v1);

        if (flip) {
            n = Vector3::cross(v3 - v1, v2 - v1);
        }

        n = Vector3::normalized(n);

        v_normals.push_back(n);
        v_normals.push_back(n);
        v_normals.push_back(n);
    }

    normals.clear();

    for (Vector3& n : v_normals) {
        normals.push_back(n.x);
        normals.push_back(n.y);
        normals.push_back(n.z);
    }
}
