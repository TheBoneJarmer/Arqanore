#include "arqanore/vector3.h"
#include "glm/gtc/type_ptr.hpp"
#include "glm/gtx/vector_angle.hpp"
#include "glm/gtx/compatibility.hpp"

const arqanore::Vector3 arqanore::Vector3::ZERO = arqanore::Vector3(0, 0, 0);
const arqanore::Vector3 arqanore::Vector3::ONE = arqanore::Vector3(1, 1, 1);
const arqanore::Vector3 arqanore::Vector3::UP = arqanore::Vector3(0, 1, 0);
const arqanore::Vector3 arqanore::Vector3::DOWN = arqanore::Vector3(0, -1, 0);
const arqanore::Vector3 arqanore::Vector3::LEFT = arqanore::Vector3(-1, 0, 0);
const arqanore::Vector3 arqanore::Vector3::RIGHT = arqanore::Vector3(1, 0, 0);
const arqanore::Vector3 arqanore::Vector3::FORWARD = arqanore::Vector3(0, 0, 1);
const arqanore::Vector3 arqanore::Vector3::BACKWARD = arqanore::Vector3(0, 0, -1);

arqanore::Vector3::Vector3() {
    this->x = 0;
    this->y = 0;
    this->z = 0;
}

arqanore::Vector3::Vector3(float x, float y, float z) {
    this->x = x;
    this->y = y;
    this->z = z;
}

float arqanore::Vector3::distance(arqanore::Vector3 v1, arqanore::Vector3 v2) {
    auto glm_vec1 = glm::vec3(v1.x, v1.y, v1.z);
    auto glm_vec2 = glm::vec3(v2.x, v2.y, v2.z);

    return glm::distance(glm_vec1, glm_vec2);
}

float arqanore::Vector3::angle(arqanore::Vector3 v1, arqanore::Vector3 v2) {
    auto glm_vec1 = glm::vec3(v1.x, v1.y, v1.z);
    auto glm_vec2 = glm::vec3(v2.x, v2.y, v2.z);

    return glm::angle(glm_vec1, glm_vec2);
}

float arqanore::Vector3::dot(arqanore::Vector3 v1, arqanore::Vector3 v2) {
    auto glm_vec1 = glm::vec3(v1.x, v1.y, v1.z);
    auto glm_vec2 = glm::vec3(v2.x, v2.y, v2.z);

    return glm::dot(glm_vec1, glm_vec2);
}

arqanore::Vector3 arqanore::Vector3::cross(arqanore::Vector3 v1, arqanore::Vector3 v2) {
    auto glm_vec1 = glm::vec3(v1.x, v1.y, v1.z);
    auto glm_vec2 = glm::vec3(v2.x, v2.y, v2.z);
    auto cross = glm::cross(glm_vec1, glm_vec2);

    return arqanore::Vector3(cross.x, cross.y, cross.z);
}

arqanore::Vector3 arqanore::Vector3::normalized(arqanore::Vector3 v) {
    auto glm_vec = glm::vec3(v.x, v.y, v.z);
    auto glm_vec_normalized = glm::normalize(glm_vec);

    return arqanore::Vector3(glm_vec_normalized.x, glm_vec_normalized.y, glm_vec_normalized.z);
}

arqanore::Vector3 arqanore::Vector3::lerp(arqanore::Vector3 v1, arqanore::Vector3 v2, float by) {
    auto glm_vec1 = glm::vec3(v1.x, v1.y, v1.z);
    auto glm_vec2 = glm::vec3(v2.x, v2.y, v2.z);
    auto lerp = glm::lerp(glm_vec1, glm_vec2, by);

    return arqanore::Vector3(lerp.x, lerp.y, lerp.z);
}

arqanore::Vector3& arqanore::Vector3::operator+(arqanore::Vector3 &v) {
    x += v.x;
    y += v.y;
    z += v.z;
    
    return *this;
}

arqanore::Vector3& arqanore::Vector3::operator+(float f) {
    x += f;
    y += f;
    z += f;
    
    return *this;
}

arqanore::Vector3& arqanore::Vector3::operator-(arqanore::Vector3 &v) {
    x -= v.x;
    y -= v.y;
    z -= v.z;
    
    return *this;
}

arqanore::Vector3& arqanore::Vector3::operator-(float f) {
    x -= f;
    y -= f;
    z -= f;
    
    return *this;
}

arqanore::Vector3& arqanore::Vector3::operator*(arqanore::Vector3 &v) {
    x *= v.x;
    y *= v.y;
    z *= v.z;
    
    return *this;
}

arqanore::Vector3& arqanore::Vector3::operator*(float f) {
    x *= f;
    y *= f;
    z *= f;
    
    return *this;
}

arqanore::Vector3& arqanore::Vector3::operator/(arqanore::Vector3 &v) {
    x /= v.x;
    y /= v.y;
    z /= v.z;
    
    return *this;
}

arqanore::Vector3& arqanore::Vector3::operator/(float f) {
    x /= f;
    y /= f;
    z /= f;
    
    return *this;
}

arqanore::Vector3& arqanore::Vector3::operator+=(Vector3 &v) {
    x += v.x;
    y += v.y;
    z += v.z;
    
    return *this;
}

arqanore::Vector3& arqanore::Vector3::operator+=(float f) {
    x += f;
    y += f;
    z += f;
    
    return *this;
}

arqanore::Vector3& arqanore::Vector3::operator-=(Vector3 &v) {
    x -= v.x;
    y -= v.y;
    z -= v.z;
    
    return *this;
}

arqanore::Vector3& arqanore::Vector3::operator-=(float f) {
    x -= f;
    y -= f;
    z -= f;
    
    return *this;
}

arqanore::Vector3 &arqanore::Vector3::operator*=(arqanore::Vector3 &v) {
    x *= v.x;
    y *= v.y;
    z *= v.z;

    return *this;
}

arqanore::Vector3 &arqanore::Vector3::operator*=(float f) {
    x *= f;
    y *= f;
    z *= f;

    return *this;
}

arqanore::Vector3 &arqanore::Vector3::operator/=(arqanore::Vector3 &v) {
    x /= v.x;
    y /= v.y;
    z /= v.z;

    return *this;
}

arqanore::Vector3 &arqanore::Vector3::operator/=(float f) {
    x /= f;
    y /= f;
    z /= f;

    return *this;
}

arqanore::Vector3& arqanore::Vector3::operator++() {
    x++;
    y++;
    z++;
    
    return *this;
}

arqanore::Vector3& arqanore::Vector3::operator++(int i) {
    x++;
    y++;
    z++;
        
    return *this;
}

arqanore::Vector3& arqanore::Vector3::operator--() {
    x--;
    y--;
    z--;
    
    return *this;
}

arqanore::Vector3& arqanore::Vector3::operator--(int i) {
    x--;
    y--;
    z--;
    
    return *this;
}

bool arqanore::Vector3::operator==(arqanore::Vector3 &v) {
    return x == v.x && y == v.y && z == v.z;
}

bool arqanore::Vector3::operator!=(arqanore::Vector3 &v) {
    return !(v == *this);
}
