#include "arqanore/polygon.h"
#include "arqanore/shaders.h"
#include "arqanore/exceptions.h"
#include "glad/gl.h"

void arqanore::Polygon::generate_buffers() {
    glGenBuffers(1, &this->vbo_vertices);
    glGenBuffers(1, &this->vbo_texcoords);

    glGenVertexArrays(1, &this->vao);
    glBindVertexArray(this->vao);

    glBindBuffer(GL_ARRAY_BUFFER, this->vbo_vertices);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(float), vertices.data(), GL_STATIC_DRAW);
    glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 2 * sizeof(float), nullptr);
    glEnableVertexAttribArray(0);

    glBindBuffer(GL_ARRAY_BUFFER, this->vbo_texcoords);
    glBufferData(GL_ARRAY_BUFFER, texcoords.size() * sizeof(float), texcoords.data(), GL_STATIC_DRAW);
    glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 2 * sizeof(float), nullptr);
    glEnableVertexAttribArray(1);

    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindVertexArray(0);
}

arqanore::Polygon::Polygon() {
    this->vbo_texcoords = 0;
    this->vbo_vertices = 0;
    this->vao = 0;
}

arqanore::Polygon::Polygon(std::vector<float> vertices, std::vector<float> texcoords) : Polygon() {
    if (vertices.size() < 4) {
        throw ArqanoreException("Polygon requires a minimum of 3 vertices");
    }

    if (texcoords.size() == 0) {
        throw ArqanoreException("Texcoords cannot be empty");
    }

    if (texcoords.size() != vertices.size()) {
        throw ArqanoreException("Texcoords and vertices must have the same size");
    }

    this->vertices = vertices;
    this->texcoords = texcoords;
    this->generate_buffers();
}
