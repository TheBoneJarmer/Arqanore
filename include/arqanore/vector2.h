#pragma once

namespace arqanore {
    class Vector2 {
    public:
        static const Vector2 ZERO;
        static const Vector2 ONE;

        float x;
        float y;

        Vector2();

        Vector2(float x, float y);

        static float distance(Vector2 v1, Vector2 v2);

        static float angle(Vector2 v1, Vector2 v2);

        static float cross(Vector2 v1, Vector2 v2);

        static float dot(Vector2 v1, Vector2 v2);

        static Vector2 normalized(arqanore::Vector2 v);

        static Vector2 lerp(arqanore::Vector2 v1, arqanore::Vector2 v2, float by);

        Vector2 &operator+(Vector2 &v);

        Vector2 &operator+(float f);

        Vector2 &operator-(Vector2 &v);

        Vector2 &operator-(float f);

        Vector2 &operator*(Vector2 &v);

        Vector2 &operator*(float f);

        Vector2 &operator/(Vector2 &v);

        Vector2 &operator/(float f);

        Vector2 &operator+=(Vector2 &v);

        Vector2 &operator+=(float f);

        Vector2 &operator-=(Vector2 &v);

        Vector2 &operator-=(float f);

        Vector2 &operator*=(Vector2 &v);

        Vector2 &operator*=(float f);

        Vector2 &operator/=(Vector2 &v);

        Vector2 &operator/=(float f);

        Vector2 &operator++();

        Vector2 &operator++(int i);

        Vector2 &operator--();

        Vector2 &operator--(int i);

        bool operator==(Vector2 &v);

        bool operator!=(Vector2 &v);
    };
}
