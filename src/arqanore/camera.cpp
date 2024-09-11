#include "arqanore/camera.h"

arqanore::Camera::Camera()
{
    position = Vector3(0, 0, 0);
    rotation = Quaternion();
    fov = 75.0f;
    near = 0.001f;
    far = 100.0f;
}
