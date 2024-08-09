#pragma once

#define SHADER_SOURCE_TYPE_RAW 0
#define SHADER_SOURCE_TYPE_FILE 1

#include <string>
#include <vector>

#include <cstdlib>
#include "texture.h"
#include "vector2.h"
#include "vector3.h"
#include "matrix3.h"
#include "color.h"
#include "sprite.h"

namespace arqanore {
    class Shader {
        friend class Font;

        friend class Sprite;

        friend class Polygon;

        friend class Renderer;

    private:
        unsigned int id;

        std::vector<std::string> vertex_sources;

        std::vector<std::string> fragment_sources;

        unsigned int compile_program(const std::vector<unsigned int> &vertex_shaders, const std::vector<unsigned int> &fragment_shaders);

        unsigned int compile_shader(const char *source, unsigned int type);

        int get_uniform_location(std::string &name);

    public:
        Shader();

        Shader(const Shader &shader);

        void add_vertex(const std::string &source, unsigned int source_type);

        void add_fragment(const std::string &source, unsigned int source_type);

        void compile();

        void set_uniform_1i(std::string name, int i);

        void set_uniform_2i(std::string name, int i1, int i2);

        void set_uniform_3i(std::string name, int i1, int i2, int i3);

        void set_uniform_4i(std::string name, int i1, int i2, int i3, int i4);

        void set_uniform_1f(std::string name, float f);

        void set_uniform_2f(std::string name, float f1, float f2);

        void set_uniform_3f(std::string name, float f1, float f2, float f3);

        void set_uniform_4f(std::string name, float f1, float f2, float f3, float f4);

        void set_uniform_1d(std::string name, double d);

        void set_uniform_2d(std::string name, double d1, double d2);

        void set_uniform_3d(std::string name, double d1, double d2, double d3);

        void set_uniform_4d(std::string name, double d1, double d2, double d3, double d4);

        void set_uniform_vec2(std::string name, Vector2 v);

        void set_uniform_vec3(std::string name, Vector3 v);

        void set_uniform_mat3(std::string name, Matrix3 m);

        void set_uniform_mat4(std::string name, Matrix4 m);

        void set_uniform_rgba(std::string name, Color c);

        void set_uniform_rgb(std::string name, Color c);

        void set_texture(unsigned int index, Texture &tex);

        void set_sprite(unsigned int index, Sprite &tex);

        void set_texture(unsigned int index, Texture *tex);

        void set_sprite(unsigned int index, Sprite *tex);

        bool operator==(const Shader &in);

        bool operator!=(const Shader &in);

        Shader &operator=(const Shader &in);
    };
}
