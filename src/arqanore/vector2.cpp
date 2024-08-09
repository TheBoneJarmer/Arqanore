#define _USE_MATH_DEFINES
#include <cmath>
#include "arqanore/vector2.h"
#include "glm/vec2.hpp"
#include "glm/gtx/exterior_product.hpp"
#include "glm/geometric.hpp"
#include "glm/gtx/compatibility.hpp"

const arqanore::Vector2 arqanore::Vector2::ZERO = arqanore::Vector2(0, 0);
const arqanore::Vector2 arqanore::Vector2::ONE = arqanore::Vector2(1, 1);

arqanore::Vector2::Vector2() {
    this->x = 0;
    this->y = 0;
}

arqanore::Vector2::Vector2(float x, float y) {
    this->x = x;
    this->y = y;
}

float arqanore::Vector2::distance(arqanore::Vector2 v1, arqanore::Vector2 v2) {
    auto x = v1.x - v2.x;
    auto y = v1.y - v2.y;

    return sqrt((x * x) + (y * y));
}

float arqanore::Vector2::angle(arqanore::Vector2 v1, arqanore::Vector2 v2) {
    auto theta = atan2((v2.y - v1.y), (v2.x - v1.x));

    if (theta < 0) {
        theta += 2 * M_PI;
    }

    return theta;
}

float arqanore::Vector2::dot(arqanore::Vector2 v1, arqanore::Vector2 v2) {
    auto glm_vec1 = glm::vec2(v1.x, v1.y);
    auto glm_vec2 = glm::vec2(v1.x, v2.y);

    return glm::dot(glm_vec1, glm_vec2);
}

float arqanore::Vector2::cross(arqanore::Vector2 v1, arqanore::Vector2 v2) {
    auto glm_vec1 = glm::vec2(v1.x, v1.y);
    auto glm_vec2 = glm::vec2(v1.x, v2.y);

    return glm::cross(glm_vec1, glm_vec2);
}

arqanore::Vector2 arqanore::Vector2::normalized(arqanore::Vector2 v) {
    auto glm_vec = glm::vec2(v.x, v.y);
    auto glm_vec_norm = glm::normalize(glm_vec);

    return arqanore::Vector2(glm_vec_norm.x, glm_vec_norm.y);
}

arqanore::Vector2 arqanore::Vector2::lerp(arqanore::Vector2 v1, arqanore::Vector2 v2, float by) {
    auto glm_vec1 = glm::vec2(v1.x, v1.y);
    auto glm_vec2 = glm::vec2(v1.x, v2.y);
    auto lerp = glm::lerp(glm_vec1, glm_vec2, by);

    return arqanore::Vector2(lerp.x, lerp.y);
}

arqanore::Vector2 &arqanore::Vector2::operator+(arqanore::Vector2 &v) {
    x += v.x;
    y += v.y;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator+(float f) {
    x += f;
    y += f;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator-(arqanore::Vector2 &v) {
    x -= v.x;
    y -= v.y;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator-(float f) {
    x -= f;
    y -= f;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator*(arqanore::Vector2 &v) {
    x *= v.x;
    y *= v.y;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator*(float f) {
    x *= f;
    y *= f;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator/(arqanore::Vector2 &v) {
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

arqanore::Vector2 &arqanore::Vector2::operator*=(arqanore::Vector2 &v) {
    x *= v.x;
    y *= v.y;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator*=(float f) {
    x *= f;
    y *= f;

    return *this;
}

arqanore::Vector2 &arqanore::Vector2::operator/=(arqanore::Vector2 &v) {
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

bool arqanore::Vector2::operator==(arqanore::Vector2 &v) {
    return x == v.x && y == v.y;
}

bool arqanore::Vector2::operator!=(arqanore::Vector2 &v) {
    return !(v == *this);
}
