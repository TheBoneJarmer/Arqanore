#pragma once

namespace arqanore {
    class Vector3 {
    public:
        static const Vector3 ZERO;
        static const Vector3 ONE;
        static const Vector3 UP;
        static const Vector3 DOWN;
        static const Vector3 FORWARD;
        static const Vector3 BACKWARD;
        static const Vector3 LEFT;
        static const Vector3 RIGHT;

        float x;
        float y;
        float z;

        Vector3();

        Vector3(float x, float y, float z);

        static float distance(Vector3 v1, Vector3 v2);

        static float angle(Vector3 v1, Vector3 v2);

        static float dot(Vector3 v1, Vector3 v2);

        static Vector3 cross(Vector3 v1, Vector3 v2);

        static Vector3 normalized(arqanore::Vector3 v);

        static Vector3 lerp(Vector3 v1, Vector3 v2, float by);

        Vector3 &operator+(Vector3 &v);

        Vector3 &operator+(float f);

        Vector3 &operator-(Vector3 &v);

        Vector3 &operator-(float f);

        Vector3 &operator*(Vector3 &v);

        Vector3 &operator*(float f);

        Vector3 &operator/(Vector3 &v);

        Vector3 &operator/(float f);

        Vector3 &operator+=(Vector3 &v);

        Vector3 &operator+=(float f);

        Vector3 &operator-=(Vector3 &v);

        Vector3 &operator-=(float f);

        Vector3 &operator*=(Vector3 &v);

        Vector3 &operator*=(float f);

        Vector3 &operator/=(Vector3 &v);

        Vector3 &operator/=(float f);

        Vector3 &operator++();

        Vector3 &operator++(int i);

        Vector3 &operator--();

        Vector3 &operator--(int i);

        bool operator==(Vector3 &v);

        bool operator!=(Vector3 &v);
    };
}
