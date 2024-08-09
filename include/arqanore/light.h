#pragma once
#define DIRECTIONAL_LIGHT 0
#define POINT_LIGHT 1

#include "vector3.h"
#include "color.h"

namespace arqanore {
    class Light {
    public:
        unsigned int type;

        Vector3 source;

        Color color;

        bool enabled;

        float strength;

        float range;

        Light();

        Light(unsigned int type);
    };
}