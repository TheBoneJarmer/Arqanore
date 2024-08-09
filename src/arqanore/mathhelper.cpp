#include "arqanore/mathhelper.h"
#include "glm/gtc/type_ptr.hpp"

/* DEFINITIONS */
float arqanore::MathHelper::radians(float deg) {
    return glm::radians(deg);
}

float arqanore::MathHelper::degrees(float rad) {
    return glm::degrees(rad);
}

float arqanore::MathHelper::lerp(float start, float end, float t) {
    return (1 - t) * start + t * end;
}

float arqanore::MathHelper::clamp(float value, float min, float max) {
    if (value < min) return min;
    if (value > max) return max;

    return value;
}