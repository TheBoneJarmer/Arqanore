#include "arqanore/texture.h"
#include "stb/stb_image.h"
#include "glad/gl.h"
#include "arqanore/exceptions.h"

arqanore::Texture::Texture() {
    id = 0;
    width = 0;
    height = 0;
};

arqanore::Texture::Texture(std::string filename) {
    int channels, width, height;
    unsigned char *data = stbi_load(filename.c_str(), &width, &height, &channels, STBI_rgb_alpha);

    if (!data) {
        std::string reason = stbi_failure_reason();
        std::string error = "Failed to load texture '" + (std::string) filename + "'. Reason is '" + reason + "'";

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

    stbi_image_free(data);
    this->width = width;
    this->height = height;
}
