#pragma once

#include <vector>
#include <string>
#include <cstdlib>

namespace arqanore {
    class Sprite {
        friend class Renderer;
        friend class Shader;

    private:
        std::string path;

        unsigned int id;
        unsigned int vbo_vertices;
        unsigned int vbo_texcoords;
        unsigned int vao;

        int width;
        int height;
        int frame_width;
        int frame_height;

        void generate_buffers();

        void generate_texture();

    public:
        int get_width();

        int get_height();

        int get_frame_width();

        int get_frame_height();

        int get_frames_hor();

        int get_frames_vert();

        Sprite();

        Sprite(std::string path, int frame_width, int frame_height);
    };
}