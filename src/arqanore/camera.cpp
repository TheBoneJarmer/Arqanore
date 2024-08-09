#include "arqanore/camera.h"

arqanore::Camera::Camera() {
    position = Vector3(0,0,0);
    rotation = Quaternion();
    fov = 60.0f;
    near = 0.001f;
    far = 1000.0f;
}
