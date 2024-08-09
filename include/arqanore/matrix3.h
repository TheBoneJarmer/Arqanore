#pragma once

#include <array>
#include "matrix4.h"

namespace arqanore {
    class Matrix3 {
    public:
        std::array<float, 9> values;

        Matrix3();

        Matrix3(float* values);

        Matrix3(std::array<float, 9> values);

        Matrix3(Matrix4 &mat);

        static float* array(std::array<float, 9> arr);

        static std::array<float, 9> array(float* arr);

        static Matrix3 inverse(Matrix3 m);

        static Matrix3 transpose(Matrix3 m);

        Matrix3 operator+(const Matrix3 &m) const;

        Matrix3 operator+(const float f) const;

        Matrix3 operator-(const Matrix3 &m) const;

        Matrix3 operator-(const float f) const;

        Matrix3 operator*(const Matrix3 &m) const;

        Matrix3 operator*(const float f) const;

        Matrix3 operator/(const Matrix3 &m) const;

        Matrix3 operator/(const float f) const;

        bool operator==(const Matrix3 &m) const;

        bool operator!=(const Matrix3 &m) const;
    };
}