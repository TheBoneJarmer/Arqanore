#include "arqanore/quaternion.h"
#include "glm/glm.hpp"
#include "glm/detail/type_quat.hpp"
#include "glm/gtc/quaternion.hpp"
#include "glm/ext/quaternion_transform.hpp"

arqanore::Quaternion::Quaternion() {
    this->x = 0;
    this->y = 0;
    this->z = 0;
    this->w = 1;
}

arqanore::Quaternion::Quaternion(float x, float y, float z, float w) {
    this->x = x;
    this->y = y;
    this->z = z;
    this->w = w;
}

arqanore::Vector3 arqanore::Quaternion::euler_angles(Quaternion q) {
    auto glm_quat = glm::quat(q.w, q.x, q.y, q.z);
    auto glm_vec = degrees(eulerAngles(glm_quat));

    return Vector3(glm_vec.x, glm_vec.y, glm_vec.z);
}

arqanore::Quaternion arqanore::Quaternion::normalize(Quaternion q) {
    auto glm_quat = glm::quat(q.w, q.x, q.y, q.z);
    glm_quat = glm::normalize(glm_quat);

    return Quaternion(glm_quat.x, glm_quat.y, glm_quat.z, glm_quat.w);
}

arqanore::Quaternion arqanore::Quaternion::rotate(Quaternion q, float angle, Vector3 axis) {
    auto glm_quat = glm::quat(q.w, q.x, q.y, q.z);
    auto glm_vec3 = glm::vec3(axis.x, axis.y, axis.z);
    glm_quat = glm::rotate(glm_quat, glm::radians(angle), glm_vec3);

    return Quaternion(glm_quat.x, glm_quat.y, glm_quat.z, glm_quat.w);
}

arqanore::Quaternion arqanore::Quaternion::rotate(Quaternion q, Vector3 euler) {
    q = rotate(q, euler.x, Vector3(1, 0, 0));
    q = rotate(q, euler.y, Vector3(0, 1, 0));
    q = rotate(q, euler.z, Vector3(0, 0, 1));

    return q;
}
