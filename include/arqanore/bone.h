#pragma once
#include <string>
#include <vector>

#include "quaternion.h"
#include "vector3.h"

namespace arqanore
{
    struct BoneFrame
    {
        Vector3 location;
        Quaternion rotation;
        Vector3 scale;
    };

    class Bone
    {
    public:
        std::vector<BoneFrame> frames;
        std::string name;
        Bone* parent;
        Vector3 head;
        Vector3 tail;

        Bone();
        Bone(std::string name);
    };
}
