#pragma once

// In the old days of VSC++ near and far were a researved keyword. It only exists because of compatibility reasons.
// Hence we undef it because we actually need to use it as variable names
#undef far
#undef near
#include "vector3.h"
#include "quaternion.h"

namespace arqanore {
    class Camera {
    public:
        Vector3 position;
        Quaternion rotation;
        float fov;
        float near;
        float far;

        Camera();
    };
}