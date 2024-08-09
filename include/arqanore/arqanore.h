#pragma once

#include <array>

namespace arqanore {
    class Arqanore {
    public:
        static std::string get_opengl_version();
        
        static std::array<int, 3> get_version();
    };
}
