#include "arqanore/sprite.h"
#include "glad/gl.h"
#include "stb/stb_image.h"
#include "arqanore/exceptions.h"

void arqanore::Sprite::generate_buffers() {
    auto texcoord_x = 1.0f / static_cast<float>(get_frames_hor());
    auto texcoord_y = 1.0f / static_cast<float>(get_frames_vert());
    auto vertices = std::vector<float>({0, 0, static_cast<float>(frame_width), 0, 0, static_cast<float>(frame_height), static_cast<float>(frame_width), 0, 0, static_cast<float>(frame_height), static_cast<float>(frame_width), static_cast<float>(frame_height)});
    auto texcoords = std::vector<float>({0, 0, texcoord_x, 0, 0, texcoord_y, texcoord_x, 0, 0, texcoord_y, texcoord_x, texcoord_y});

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

void arqanore::Sprite::generate_texture() {
    int channels, width, height;
    unsigned char *data = stbi_load(path.c_str(), &width, &height, &channels, STBI_rgb_alpha);

    if (!data) {
        std::string reason = stbi_failure_reason();
        std::string error = "Failed to load texture '" + path + "'. Reason is '" + reason + "'";

        stbi_image_free(data);
        throw ArqanoreException(error);
    }

    glGenTextures(1, &this->id);
    glBindTexture(GL_TEXTURE_2D, this->id);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);

    // Correct the framesize when the user requests it
    if (frame_width == 0) {
        frame_width = width;
    }

    if (frame_height == 0) {
        frame_height = height;
    }

    stbi_image_free(data);
    this->width = width;
    this->height = height;
}

int arqanore::Sprite::get_width() {
    return width;
}

int arqanore::Sprite::get_height() {
    return height;
}

int arqanore::Sprite::get_frame_width() {
    return frame_width;
}

int arqanore::Sprite::get_frame_height() {
    return frame_height;
}

int arqanore::Sprite::get_frames_hor() {
    return width / frame_width;
}

int arqanore::Sprite::get_frames_vert() {
    return height / frame_height;
}

arqanore::Sprite::Sprite() {
    path = "";

    id = 0;
    vbo_vertices = 0;
    vbo_texcoords = 0;
    vao = 0;

    width = 0;
    height = 0;
    frame_width = 0;
    frame_height = 0;
}

arqanore::Sprite::Sprite(std::string path, int frame_width, int frame_height) : Sprite() {
    this->path = path;
    this->frame_width = frame_width;
    this->frame_height = frame_height;

    this->generate_texture();
    this->generate_buffers();
}
