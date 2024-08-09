#include "arqanore/matrix3.h"
#include "arqanore/matrix4.h"
#include "glm/gtc/type_ptr.hpp"

glm::mat3 to_glm_mat3(arqanore::Matrix3 m) {
    float *m_values = arqanore::Matrix3::array(m.values);
    glm::mat3 glm_mat = glm::make_mat3(m_values);

    delete m_values;
    return glm_mat;
}

arqanore::Matrix3 from_glm_mat3(glm::mat3 m) {
    float m_values[9] = {
            m[0].x, m[0].y, m[0].z,
            m[1].x, m[1].y, m[1].z,
            m[2].x, m[2].y, m[2].z
    };

    return arqanore::Matrix3(m_values);
}

arqanore::Matrix3::Matrix3() {
    this->values.fill(0);
}

arqanore::Matrix3::Matrix3(float *values) {
    for (int i = 0; i < 9; i++) {
        this->values[i] = values[i];
    }
}

arqanore::Matrix3::Matrix3(std::array<float, 9> values) {
    this->values = values;
}

arqanore::Matrix3::Matrix3(arqanore::Matrix4 &mat) : Matrix3() {
    float *mat_values = Matrix4::array(mat.values);
    glm::mat4 glm_mat4 = glm::make_mat4(mat_values);
    glm::mat3 glm_mat3 = glm::mat3(glm_mat4);

    this->values[0] = glm_mat3[0].x;
    this->values[1] = glm_mat3[0].y;
    this->values[2] = glm_mat3[0].z;
    this->values[3] = glm_mat3[1].x;
    this->values[4] = glm_mat3[1].y;
    this->values[5] = glm_mat3[1].z;
    this->values[6] = glm_mat3[2].x;
    this->values[7] = glm_mat3[2].y;
    this->values[8] = glm_mat3[2].z;

    delete mat_values;
}

float *arqanore::Matrix3::array(std::array<float, 9> arr) {
    float *result = new float[9];

    for (int i = 0; i < 9; i++) {
        result[i] = arr[i];
    }

    return result;
}

std::array<float, 9> arqanore::Matrix3::array(float *arr) {
    std::array<float, 9> result;

    for (int i = 0; i < 9; i++) {
        result[i] = arr[i];
    }

    return result;
}

arqanore::Matrix3 arqanore::Matrix3::inverse(arqanore::Matrix3 m) {
    glm::mat3 glm_mat = to_glm_mat3(m);
    glm_mat = glm::inverse(glm_mat);

    return from_glm_mat3(glm_mat);
}

arqanore::Matrix3 arqanore::Matrix3::transpose(arqanore::Matrix3 m) {
    glm::mat3 glm_mat = to_glm_mat3(m);
    glm_mat = glm::transpose(glm_mat);

    return from_glm_mat3(glm_mat);
}

arqanore::Matrix3 arqanore::Matrix3::operator+(const arqanore::Matrix3 &m) const {
    glm::mat3 glm_mat1 = to_glm_mat3(*this);
    glm::mat3 glm_mat2 = to_glm_mat3(m);
    glm::mat3 glm_mat = glm_mat1 + glm_mat2;

    return from_glm_mat3(glm_mat);
}

arqanore::Matrix3 arqanore::Matrix3::operator+(const float f) const {
    glm::mat3 glm_mat = to_glm_mat3(*this);
    glm_mat += f;

    return from_glm_mat3(glm_mat);
}

arqanore::Matrix3 arqanore::Matrix3::operator-(const arqanore::Matrix3 &m) const {
    glm::mat3 glm_mat1 = to_glm_mat3(*this);
    glm::mat3 glm_mat2 = to_glm_mat3(m);
    glm::mat3 glm_mat = glm_mat1 - glm_mat2;

    return from_glm_mat3(glm_mat);
}

arqanore::Matrix3 arqanore::Matrix3::operator-(const float f) const {
    glm::mat3 glm_mat = to_glm_mat3(*this);
    glm_mat -= f;

    return from_glm_mat3(glm_mat);
}

arqanore::Matrix3 arqanore::Matrix3::operator*(const arqanore::Matrix3 &m) const {
    glm::mat3 glm_mat1 = to_glm_mat3(*this);
    glm::mat3 glm_mat2 = to_glm_mat3(m);
    glm::mat3 glm_mat = glm_mat1 * glm_mat2;

    return from_glm_mat3(glm_mat);
}

arqanore::Matrix3 arqanore::Matrix3::operator*(const float f) const {
    glm::mat3 glm_mat = to_glm_mat3(*this);
    glm_mat *= f;

    return from_glm_mat3(glm_mat);
}

arqanore::Matrix3 arqanore::Matrix3::operator/(const arqanore::Matrix3 &m) const {
    glm::mat3 glm_mat1 = to_glm_mat3(*this);
    glm::mat3 glm_mat2 = to_glm_mat3(m);
    glm::mat3 glm_mat = glm_mat1 / glm_mat2;

    return from_glm_mat3(glm_mat);
}

arqanore::Matrix3 arqanore::Matrix3::operator/(const float f) const {
    glm::mat3 glm_mat = to_glm_mat3(*this);
    glm_mat /= f;

    return from_glm_mat3(glm_mat);
}

bool arqanore::Matrix3::operator==(const arqanore::Matrix3 &m) const {
    for (int i = 0; i < 9; i++) {
        float f1 = values[i];
        float f2 = m.values[i];

        if (f1 != f2) {
            return false;
        }
    }

    return true;
}

bool arqanore::Matrix3::operator!=(const arqanore::Matrix3 &m) const {
    return !(m == *this);
}
