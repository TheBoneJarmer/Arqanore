#pragma once
#include <string>

namespace arqanore
{
    class Image
    {
        friend class Window;

    private:
        unsigned char* data;
        int width;
        int height;
        int channels;

    public:
        int get_channels();
        int get_width();
        int get_height();
        unsigned char* get_data();

        Image();

        Image(std::string path);

        Image(const Image& src);

        ~Image();
    };
}
