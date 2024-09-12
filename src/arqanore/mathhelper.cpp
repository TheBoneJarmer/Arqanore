#include "arqanore/mathhelper.h"

#include <numbers>

/* DEFINITIONS */
float arqanore::MathHelper::radians(float deg)
{
    return deg * std::numbers::pi / 180;
}

float arqanore::MathHelper::degrees(float rad)
{
    return rad * 180 / std::numbers::pi;
}

float arqanore::MathHelper::lerp(float start, float end, float t)
{
    return (1 - t) * start + t * end;
}

float arqanore::MathHelper::clamp(float value, float min, float max)
{
    if (value < min) return min;
    if (value > max) return max;

    return value;
}
