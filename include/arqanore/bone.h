#pragma once
#include <string>

namespace arqanore
{
    class Bone
    {
    private:
        std::string name;
        Bone* parent;

    public:
        Bone();
        Bone(std::string name);
    };
}
