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
