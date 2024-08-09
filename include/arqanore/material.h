#pragma once

#include "color.h"
#include "texture.h"

namespace arqanore {
    class Material {
    public:
        std::string name;
        Color color;
        Color diffuse;
        Color ambient;
        Color specular;
        float shininess;
        Texture* diffuse_map;
        Texture* ambient_map;
        Texture* specular_map;

        Material();

        Material(const std::string& name);

        Material(const Material& material);

        Material& operator=(const Material& material);

        ~Material();
    };
}