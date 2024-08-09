#pragma once

#include "matrix3.h"
#include "matrix4.h"
#include "vector3.h"

namespace arqanore {
    class MathHelper {
    public:
        static float radians(float deg);

        static float degrees(float rad);

        static float lerp(float start, float end, float t);

        static float clamp(float value, float min, float max);
    };
}
