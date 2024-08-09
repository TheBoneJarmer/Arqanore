#include "arqanore/matrix4.h"
#include "glm/gtc/type_ptr.hpp"
#include "glm/gtc/quaternion.hpp"

glm::mat4 to_glm_mat4(arqanore::Matrix4 m) {
    float *m_values = arqanore::Matrix4::array(m.values);
    glm::mat4 glm_mat = glm::make_mat4(m_values);

    delete m_values;
    return glm_mat;
}

arqanore::Matrix4 from_glm_mat4(glm::mat4 m) {
    float m_values[16] = {
            m[0].x, m[0].y, m[0].z, m[0].w,
            m[1].x, m[1].y, m[1].z, m[1].w,
            m[2].x, m[2].y, m[2].z, m[2].w,
            m[3].x, m[3].y, m[3].z, m[3].w,
    };

    return arqanore::Matrix4(m_values);
}

arqanore::Matrix4::Matrix4() {
    for (int i = 0; i < 16; i++) {
        this->values[i] = 0;
    }
}

arqanore::Matrix4::Matrix4(float *values) {
    for (int i = 0; i < 16; i++) {
        this->values[i] = values[i];
    }
}

arqanore::Matrix4::Matrix4(std::array<float, 16> values) {
    this->values = values;
}

float *arqanore::Matrix4::array(std::array<float, 16> arr) {
    float *result = new float[16];

    for (int i = 0; i < 16; i++) {
        result[i] = arr[i];
    }

    return result;
}

std::array<float, 16> arqanore::Matrix4::array(float *arr) {
    std::array<float, 16> result;

    for (int i = 0; i < 16; i++) {
        result[i] = arr[i];
    }

    return result;
}

arqanore::Matrix4 arqanore::Matrix4::identity() {
    glm::mat4 glm_mat = glm::mat4(1.0f);
    return from_glm_mat4(glm_mat);
}

arqanore::Matrix4 arqanore::Matrix4::scale(Matrix4 m, Vector3 vec) {
    glm::mat4 glm_mat = to_glm_mat4(m);
    glm_mat = glm::scale(glm_mat, glm::vec3(vec.x, vec.y, vec.z));

    return from_glm_mat4(glm_mat);
}

arqanore::Matrix4 arqanore::Matrix4::translate(Matrix4 m, Vector3 vec) {
    glm::mat4 glm_mat = to_glm_mat4(m);
    glm_mat = glm::translate(glm_mat, glm::vec3(vec.x, vec.y, vec.z));

    return from_glm_mat4(glm_mat);
}

arqanore::Matrix4 arqanore::Matrix4::rotate(Matrix4 m, Quaternion quat) {
    glm::mat4 glm_mat = to_glm_mat4(m);
    glm::quat glm_quat = glm::quat(quat.w, quat.x, quat.y, quat.z);
    glm_mat *= mat4_cast(glm_quat);

    return from_glm_mat4(glm_mat);
}

arqanore::Matrix4 arqanore::Matrix4::perspective(Matrix4 m, float fovy, float ratio, float near, float far) {
    glm::mat4 glm_mat = to_glm_mat4(m);
    glm_mat = glm::perspective(glm::radians(fovy), ratio, near, far);

    return from_glm_mat4(glm_mat);
}

arqanore::Matrix4 arqanore::Matrix4::operator+(const Matrix4 &m) const {
    glm::mat4 glm_mat1 = to_glm_mat4(*this);
    glm::mat4 glm_mat2 = to_glm_mat4(m);
    glm::mat4 glm_mat = glm_mat1 + glm_mat2;

    return from_glm_mat4(glm_mat);
}

arqanore::Matrix4 arqanore::Matrix4::operator+(const float f) const {
    glm::mat4 glm_mat = to_glm_mat4(*this);
    glm_mat = glm_mat + f;

    return from_glm_mat4(glm_mat);
}

arqanore::Matrix4 arqanore::Matrix4::operator-(const Matrix4 &m) const {
    glm::mat4 glm_mat1 = to_glm_mat4(*this);
    glm::mat4 glm_mat2 = to_glm_mat4(m);
    glm::mat4 glm_mat = glm_mat1 - glm_mat2;

    return from_glm_mat4(glm_mat);
}

arqanore::Matrix4 arqanore::Matrix4::operator-(const float f) const {
    glm::mat4 glm_mat = to_glm_mat4(*this);
    glm_mat = glm_mat - f;

    return from_glm_mat4(glm_mat);
}

arqanore::Matrix4 arqanore::Matrix4::operator*(const Matrix4 &m) const {
    glm::mat4 glm_mat1 = to_glm_mat4(*this);
    glm::mat4 glm_mat2 = to_glm_mat4(m);
    glm::mat4 glm_mat = glm_mat1 * glm_mat2;

    return from_glm_mat4(glm_mat);
}

arqanore::Matrix4 arqanore::Matrix4::operator*(const float f) const {
    glm::mat4 glm_mat = to_glm_mat4(*this);
    glm_mat = glm_mat * f;

    return from_glm_mat4(glm_mat);
}

arqanore::Matrix4 arqanore::Matrix4::operator/(const Matrix4 &m) const {
    glm::mat4 glm_mat1 = to_glm_mat4(*this);
    glm::mat4 glm_mat2 = to_glm_mat4(m);
    glm::mat4 glm_mat = glm_mat1 / glm_mat2;

    return from_glm_mat4(glm_mat);
}

arqanore::Matrix4 arqanore::Matrix4::operator/(const float f) const {
    glm::mat4 glm_mat = to_glm_mat4(*this);
    glm_mat = glm_mat / f;

    return from_glm_mat4(glm_mat);
}

bool arqanore::Matrix4::operator==(const Matrix4 &m) const {
    for (int i = 0; i < 16; i++) {
        float f1 = values[i];
        float f2 = m.values[i];

        if (f1 != f2) {
            return false;
        }
    }

    return true;
}

bool arqanore::Matrix4::operator!=(const Matrix4 &m) const {
    return !(m == *this);
}
