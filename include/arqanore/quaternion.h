#pragma once

#include "vector3.h"

namespace arqanore {
    class Quaternion {
    public:
        float x;
        float y;
        float z;
        float w;

        Quaternion();

        Quaternion(float x, float y, float z, float w);

        static Vector3 euler_angles(Quaternion q);

        static Quaternion normalize(Quaternion q);

        static Quaternion rotate(Quaternion q, float angle, Vector3 axis);

        static Quaternion rotate(Quaternion q, Vector3 euler);
    };
}