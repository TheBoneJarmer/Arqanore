#include "arqanore/vector4.h"

arqanore::Vector4::Vector4()
{
    x = 0;
    y = 0;
    z = 0;
    w = 0;
}

arqanore::Vector4::Vector4(float x, float y, float z, float w)
{
    this->x = x;
    this->y = y;
    this->z = z;
    this->w = w;
}

// TODO: Expend functionality with operators