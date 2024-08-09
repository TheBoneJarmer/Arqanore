#include "arqanore/light.h"

arqanore::Light::Light() {
    this->source = Vector3(0.25f,-0.5f,-0.5f);
    this->color = Color(255, 255, 255, 255);
    this->type = 0;
    this->enabled = true;
    this->strength = 1.0f;
    this->range = 10.f;
}

arqanore::Light::Light(unsigned int type) : Light() {
    this->type = type;
}
