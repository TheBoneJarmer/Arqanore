#include <string>
#include <vector>
#include "glad/gl.h"
#include "arqanore/arqanore.h"
#include "arqanore/utils.h"

std::string arqanore::Arqanore::get_opengl_version() {
    const unsigned char* value = glGetString(GL_VERSION);
    std::string result = std::string((const char *) value);

    return result;
}

std::array<int, 3> arqanore::Arqanore::get_version() {
    std::vector<std::string> values = string_split(VERSION, '.');
    std::array<int, 3> result;
    result[0] = std::stoi(values[0]);
    result[1] = std::stoi(values[1]);
    result[2] = std::stoi(values[2]);
    
    return result;
}
