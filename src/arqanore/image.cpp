#include "arqanore/image.h"
#include "stb/stb_image.h"
#include "arqanore/exceptions.h"

int arqanore::Image::get_channels()
{
    return channels;
}

int arqanore::Image::get_width()
{
    return width;
}

int arqanore::Image::get_height()
{
    return height;
}

unsigned char* arqanore::Image::get_data()
{
    return data;
}

arqanore::Image::Image()
{
    data = nullptr;
    width = 0;
    height = 0;
    channels = 0;
}

arqanore::Image::Image(std::string path) : Image()
{
    data = stbi_load(path.c_str(), &width, &height, &channels, STBI_rgb_alpha);

    if (!data)
    {
        std::string reason = stbi_failure_reason();
        std::string error = "Failed to load image '" + path + "'. Reason is '" + reason + "'";

        stbi_image_free(data);
        throw ArqanoreException(error);
    }
}

arqanore::Image::Image(const Image& src) : Image()
{
    if (src.data != nullptr)
    {
        data = new unsigned char;
        *data = *src.data;
    }

    width = src.width;
    height = src.height;
    channels = src.channels;
}

arqanore::Image::~Image()
{
    delete data;
}
