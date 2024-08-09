#include "arqanore/scene.h"
#include "arqanore/exceptions.h"

std::vector<arqanore::Camera> arqanore::Scene::cameras;

std::vector<arqanore::Light> arqanore::Scene::lights;

unsigned int arqanore::Scene::active_camera;

void arqanore::Scene::reset() {
    cameras.clear();
    cameras.emplace_back();

    lights.clear();
    lights.emplace_back();

    active_camera = 0;
}