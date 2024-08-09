#pragma once

namespace arqanore
{
    class Color
    {
        static void fix_values(Color* color);

    public:
        static const Color RED;
        static const Color GREEN;
        static const Color BLUE;
        static const Color CYAN;
        static const Color YELLOW;
        static const Color PURPLE;
        static const Color WHITE;
        static const Color BLACK;

        unsigned int r;
        unsigned int g;
        unsigned int b;
        unsigned int a;

        Color();

        Color(unsigned int r, unsigned int g, unsigned int b);

        Color(unsigned int r, unsigned int g, unsigned int b, unsigned int a);

        Color(const Color& color);

        bool operator==(const Color& color);

        bool operator!=(const Color& color);

        Color& operator=(const Color& color);

        Color& operator=(const unsigned int value);

        Color operator+(const Color& color);

        Color operator+(const unsigned int value);

        Color operator-(const Color& color);

        Color operator-(const unsigned int value);

        Color operator/(const Color& color);

        Color operator/(const unsigned int value);

        Color operator*(const Color& color);

        Color operator*(const unsigned int value);
    };
}
