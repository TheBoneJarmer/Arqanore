#include <stdexcept>
#include <fstream>
#include "arqanore/shader.h"
#include "glad/gl.h"
#include "arqanore/exceptions.h"

unsigned int arqanore::Shader::compile_program(const std::vector<unsigned int> &vertex_shaders, const std::vector<unsigned int> &fragment_shaders) {
    unsigned int program = glCreateProgram();
    int link_status = 0;

    for (unsigned int shader: vertex_shaders) {
        glAttachShader(program, shader);
    }

    for (unsigned int shader: fragment_shaders) {
        glAttachShader(program, shader);
    }

    glLinkProgram(program);
    glGetProgramiv(program, GL_LINK_STATUS, &link_status);

    if (link_status == 0) {
        char info_log[2048];
        glGetProgramInfoLog(program, 2048, nullptr, info_log);

        throw ArqanoreException("Shader program linking failed\n" + std::string(info_log));
    }

    for (unsigned int shader: vertex_shaders) {
        glDeleteShader(shader);
    }

    for (unsigned int shader: fragment_shaders) {
        glDeleteShader(shader);
    }

    return program;
}

unsigned int arqanore::Shader::compile_shader(const char *source, unsigned int type) {
    unsigned int shader = glCreateShader(type);
    int compile_status = 0;

    glShaderSource(shader, 1, &source, nullptr);
    glCompileShader(shader);

    glGetShaderiv(shader, GL_COMPILE_STATUS, &compile_status);

    if (compile_status == 0) {
        char info_log[1024];
        glGetShaderInfoLog(shader, 1024, nullptr, info_log);

        if (type == GL_VERTEX_SHADER) {
            throw ArqanoreException("Vertex shader compilation failed\n" + std::string(info_log));
        }

        if (type == GL_FRAGMENT_SHADER) {
            throw ArqanoreException("Fragment shader compilation failed\n" + std::string(info_log));
        }
    }

    return shader;
}

int arqanore::Shader::get_uniform_location(std::string &name) {
    const char *name_cstr = name.c_str();
    int location = glGetUniformLocation(id, name_cstr);

    return location;
}

arqanore::Shader::Shader() {
    this->id = 0;
}

arqanore::Shader::Shader(const Shader &shader) {
    this->id = shader.id;
}

void arqanore::Shader::add_vertex(const std::string &source, unsigned int source_type) {
    if (source_type == SHADER_SOURCE_TYPE_FILE) {
        std::ifstream file(source);

        if (file.is_open()) {
            std::string lines;
            std::string line;

            while (std::getline(file, line)) {
                lines += line + "\n";
            }

            file.close();
            this->vertex_sources.push_back(lines);
        } else {
            throw ArqanoreException("Failed to load vertex shader file " + source);
        }

        return;
    }

    if (source_type == SHADER_SOURCE_TYPE_RAW) {
        this->vertex_sources.push_back(source);
        return;
    }

    throw ArqanoreException("Unknown shader source type " + std::to_string(source_type));
}

void arqanore::Shader::add_fragment(const std::string &source, unsigned int source_type) {
    if (source_type == SHADER_SOURCE_TYPE_FILE) {
        std::ifstream file(source);

        if (file.is_open()) {
            std::string lines;
            std::string line;

            while (std::getline(file, line)) {
                lines += line + "\n";
            }

            file.close();
            this->fragment_sources.push_back(lines);
        } else {
            throw ArqanoreException("Failed to load fragment shader file " + source);
        }

        return;
    }

    if (source_type == SHADER_SOURCE_TYPE_RAW) {
        this->fragment_sources.push_back(source);
        return;
    }

    throw ArqanoreException("Unknown shader source type " + std::to_string(source_type));
}

void arqanore::Shader::compile() {
    std::vector<unsigned int> vertex_shaders;
    std::vector<unsigned int> fragment_shaders;

    for (auto &source: vertex_sources) {
        unsigned int shader = this->compile_shader(source.c_str(), GL_VERTEX_SHADER);
        vertex_shaders.push_back(shader);
    }

    for (auto &source: fragment_sources) {
        unsigned int shader = this->compile_shader(source.c_str(), GL_FRAGMENT_SHADER);
        fragment_shaders.push_back(shader);
    }

    this->id = this->compile_program(vertex_shaders, fragment_shaders);
}

void arqanore::Shader::set_uniform_1i(std::string name, int i) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform1i(uniform, i);
}

void arqanore::Shader::set_uniform_2i(std::string name, int i1, int i2) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform2i(uniform, i1, i2);
}

void arqanore::Shader::set_uniform_3i(std::string name, int i1, int i2, int i3) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform3i(uniform, i1, i2, i3);
}

void arqanore::Shader::set_uniform_4i(std::string name, int i1, int i2, int i3, int i4) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform4i(uniform, i1, i2, i3, i4);
}

void arqanore::Shader::set_uniform_1f(std::string name, float f) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform1f(uniform, f);
}

void arqanore::Shader::set_uniform_2f(std::string name, float f1, float f2) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform2f(uniform, f1, f2);
}

void arqanore::Shader::set_uniform_3f(std::string name, float f1, float f2, float f3) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform3f(uniform, f1, f2, f3);
}

void arqanore::Shader::set_uniform_4f(std::string name, float f1, float f2, float f3, float f4) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform4f(uniform, f1, f2, f3, f4);
}

void arqanore::Shader::set_uniform_1d(std::string name, double d) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform1d(uniform, d);
}

void arqanore::Shader::set_uniform_2d(std::string name, double d1, double d2) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform2d(uniform, d1, d2);
}

void arqanore::Shader::set_uniform_3d(std::string name, double d1, double d2, double d3) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform3d(uniform, d1, d2, d3);
}

void arqanore::Shader::set_uniform_4d(std::string name, double d1, double d2, double d3, double d4) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform4d(uniform, d1, d2, d3, d4);
}

void arqanore::Shader::set_uniform_vec2(std::string name, Vector2 v) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform2f(uniform, v.x, v.y);
}

void arqanore::Shader::set_uniform_vec3(std::string name, Vector3 v) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform3f(uniform, v.x, v.y, v.z);
}

void arqanore::Shader::set_uniform_vec4(std::string name, Vector4 v)
{
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform4f(uniform, v.x, v.y, v.z, v.w);
}

void arqanore::Shader::set_uniform_mat3(std::string name, Matrix3 m) {
    int uniform = get_uniform_location(name);
    float *values = Matrix3::array(m.values);

    if (uniform != -1) {
        glUniformMatrix3fv(uniform, 1, GL_FALSE, values);
    }

    delete values;
}

void arqanore::Shader::set_uniform_mat4(std::string name, Matrix4 m) {
    int uniform = get_uniform_location(name);
    float *values = Matrix4::array(m.values);

    if (uniform != -1) {
        glUniformMatrix4fv(uniform, 1, GL_FALSE, values);
    }

    delete values;
}

void arqanore::Shader::set_uniform_rgba(std::string name, Color c) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform4f(uniform, c.r / 255.f, c.g / 255.f, c.b / 255.f, c.a / 255.f);
}

void arqanore::Shader::set_uniform_rgb(std::string name, Color c) {
    int uniform = get_uniform_location(name);

    if (uniform == -1) {
        return;
    }

    glUniform3f(uniform, c.r / 255.f, c.g / 255.f, c.b / 255.f);
}

void arqanore::Shader::set_texture(unsigned int index, Texture &tex) {
    glActiveTexture(GL_TEXTURE0 + index);
    glBindTexture(GL_TEXTURE_2D, tex.id);
}

void arqanore::Shader::set_sprite(unsigned int index, Sprite &tex) {
    glActiveTexture(GL_TEXTURE0 + index);
    glBindTexture(GL_TEXTURE_2D, tex.id);
}

void arqanore::Shader::set_texture(unsigned int index, Texture *tex) {
    if (tex == nullptr) {
        throw ArqanoreException("Texture cannot be null");
    }

    glActiveTexture(GL_TEXTURE0 + index);
    glBindTexture(GL_TEXTURE_2D, tex->id);
}

void arqanore::Shader::set_sprite(unsigned int index, Sprite *tex) {
    if (tex == nullptr) {
        throw ArqanoreException("Sprite cannot be null");
    }

    glActiveTexture(GL_TEXTURE0 + index);
    glBindTexture(GL_TEXTURE_2D, tex->id);
}

bool arqanore::Shader::operator==(const Shader &in) {
    return id == in.id;
}

bool arqanore::Shader::operator!=(const Shader &in) {
    return id != in.id;
}

arqanore::Shader &arqanore::Shader::operator=(const Shader &in) {
    this->id = in.id;

    return *this;
}
