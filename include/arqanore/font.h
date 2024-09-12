#pragma once

#include "shader.h"

namespace arqanore {
    struct Glyph {
        unsigned int id;
        unsigned int width;
        unsigned int height;
        int left;
        int top;
        long advance;
    };

    class Font {
        friend class Renderer;

    private:
        Glyph glyphs[512];
        const unsigned int glyphs_length = 512;

        unsigned int vbo_vertices;
        unsigned int vbo_texcoords;
        unsigned int vao;
        unsigned int pixel_width;
        unsigned int pixel_height;

        void generate_glyphs(std::string &path);

        void generate_buffers();

    public:
        Glyph* glyph(unsigned int code);

        Font();

        Font(std::string path, unsigned int width, unsigned int height);

        float measure(const std::u16string& text, float scale);
    };
}
