#define _USE_MATH_DEFINES
#include <cmath>
#include "arqanore/vector2.h"

const arqanore::Vector2 arqanore::Vector2::ZERO = Vector2(0, 0);
const arqanore::Vector2 arqanore::Vector2::ONE = Vector2(1, 1);

arqanore::Vector2::Vector2() {
    this->x = 0;
    this->y = 0;
}

arqanore::Vector2::Vector2(float x, float y) {
    this->x = x;
    this->y = y;
}

float arqanore::Vector2::distance(Vector2 v1, Vector2 v2) {
    auto x = v1.x - v2.x;
    auto y = v1.y - v2.y;

    return sqrt((x * x) + (y * y));
}

float arqanore::Vector2::angle(Vector2 v1, Vector2 v2) {
    auto theta = atan2((v2.y - v1.y), (v2.x - v1.x));

    if (theta < 0) {
        theta += 2 * M_PI;
    }

    return theta;
}

arqanore::Vector2 &arqanore::Vector2::operator+(Vector2 &v) {
    x += v.x;
    y += v.y;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator+(float f) {
    x += f;
    y += f;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator-(Vector2 &v) {
    x -= v.x;
    y -= v.y;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator-(float f) {
    x -= f;
    y -= f;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator*(Vector2 &v) {
    x *= v.x;
    y *= v.y;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator*(float f) {
    x *= f;
    y *= f;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator/(Vector2 &v) {
    x /= v.x;
    y /= v.y;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator/(float f) {
    x /= f;
    y /= f;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator+=(Vector2 &v) {
    x += v.x;
    y += v.y;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator+=(float f) {
    x += f;
    y += f;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator-=(Vector2 &v) {
    x -= v.x;
    y -= v.y;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator-=(float f) {
    x -= f;
    y -= f;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator*=(Vector2 &v) {
    x *= v.x;
    y *= v.y;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator*=(float f) {
    x *= f;
    y *= f;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator/=(Vector2 &v) {
    x /= v.x;
    y /= v.y;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator/=(float f) {
    x /= f;
    y /= f;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator++() {
    x++;
    y++;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator++(int i) {
    x++;
    y++;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator--() {
    x--;
    y--;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator--(int i) {
    x--;
    y--;

    return *this;
}

bool arqanore::Vector2::operator==(Vector2 &v) {
    return x == v.x && y == v.y;
}

bool arqanore::Vector2::operator!=(Vector2 &v) {
    return !(v == *this);
}
