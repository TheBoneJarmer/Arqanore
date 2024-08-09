#pragma once

#include <vector>
#include <array>
#include "scene.h"
#include "camera.h"
#include "light.h"

namespace arqanore {
    class Scene {
        friend class Renderer;

    public:
        static std::vector<Camera> cameras;
        static std::vector<Light> lights;
        static unsigned int active_camera;

        static void reset();
    };
}