#pragma once

#include <array>
#include "vector3.h"
#include "quaternion.h"

namespace arqanore {
    class Matrix4 {
    public:
        std::array<float, 16> values;

        Matrix4();

        Matrix4(float *values);

        Matrix4(std::array<float, 16> values);

        static float* array(std::array<float, 16> arr);

        static std::array<float, 16> array(float* arr);

        static Matrix4 identity();

        static Matrix4 scale(arqanore::Matrix4 m, Vector3 vec);

        static Matrix4 translate(arqanore::Matrix4 m, Vector3 vec);

        static Matrix4 rotate(arqanore::Matrix4 m, Quaternion quat);

        static Matrix4 perspective(arqanore::Matrix4 m, float fovy, float ratio, float near, float far);

        Matrix4 operator+(const Matrix4 &m) const;

        Matrix4 operator+(const float f) const;

        Matrix4 operator-(const Matrix4 &m) const;

        Matrix4 operator-(const float f) const;

        Matrix4 operator*(const Matrix4 &m) const;

        Matrix4 operator*(const float f) const;

        Matrix4 operator/(const Matrix4 &m) const;

        Matrix4 operator/(const float f) const;

        bool operator==(const Matrix4 &m) const;

        bool operator!=(const Matrix4 &m) const;
    };
}