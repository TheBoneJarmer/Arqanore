#include "arqanore/material.h"

arqanore::Material::Material() {
    this->shininess = 512.0f;
    this->color = Color(255, 255, 255);
    this->ambient = Color(35, 35, 35);
    this->diffuse = Color(255, 255, 255);
    this->specular = Color(128, 128, 128);
    this->name = "mat";
    this->diffuse_map = nullptr;
    this->ambient_map = nullptr;
    this->specular_map = nullptr;
}

arqanore::Material::Material(std::string name) : Material() {
    this->name = name;
}

arqanore::Material::Material(const arqanore::Material &material) {
    this->name = material.name;
    this->color = material.color;
    this->shininess = material.shininess;
    this->ambient = material.ambient;
    this->diffuse = material.diffuse;
    this->specular = material.specular;
    this->diffuse_map = nullptr;
    this->ambient_map = nullptr;
    this->specular_map = nullptr;

    if (material.diffuse_map != nullptr) {
        this->diffuse_map = new Texture(*material.diffuse_map);
    }

    if (material.ambient_map != nullptr) {
        this->ambient_map = new Texture(*material.ambient_map);
    }

    if (material.specular_map != nullptr) {
        this->specular_map = new Texture(*material.specular_map);
    }
}

arqanore::Material &arqanore::Material::operator=(const arqanore::Material &material) {
    this->name = material.name;
    this->shininess = material.shininess;
    this->color = material.color;
    this->ambient = material.ambient;
    this->diffuse = material.diffuse;
    this->specular = material.specular;
    this->diffuse_map = nullptr;
    this->ambient_map = nullptr;
    this->specular_map = nullptr;

    if (material.diffuse_map != nullptr) {
        this->diffuse_map = new Texture(*material.diffuse_map);
    }

    if (material.ambient_map != nullptr) {
        this->ambient_map = new Texture(*material.ambient_map);
    }

    if (material.specular_map != nullptr) {
        this->specular_map = new Texture(*material.specular_map);
    }

    return *this;
}

arqanore::Material::~Material() {
    delete this->diffuse_map;
    delete this->ambient_map;
    delete this->specular_map;
}
