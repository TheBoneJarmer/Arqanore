#include "arqanore/bone.h"

arqanore::Bone::Bone()
{
    parent = nullptr;
    name = "Bone";
}

arqanore::Bone::Bone(std::string name) : Bone()
{
    this->name = name;
}