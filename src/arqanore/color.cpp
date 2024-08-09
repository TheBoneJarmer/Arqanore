#include "arqanore/color.h"

void arqanore::Color::fix_values(Color* color)
{
    if (color->r > 255) color->r = 255;
    if (color->g > 255) color->g = 255;
    if (color->b > 255) color->b = 255;
    if (color->a > 255) color->a = 255;
    if (color->r < 0) color->r = 0;
    if (color->g < 0) color->g = 0;
    if (color->b < 0) color->b = 0;
    if (color->a < 0) color->a = 0;
}

const arqanore::Color arqanore::Color::RED = Color(255, 0, 0);
const arqanore::Color arqanore::Color::GREEN = Color(0, 255, 0);
const arqanore::Color arqanore::Color::BLUE = Color(0, 0, 255);
const arqanore::Color arqanore::Color::CYAN = Color(0, 255, 255);
const arqanore::Color arqanore::Color::YELLOW = Color(255, 255, 0);
const arqanore::Color arqanore::Color::PURPLE = Color(255, 0, 255);
const arqanore::Color arqanore::Color::WHITE = Color(255, 255, 255);
const arqanore::Color arqanore::Color::BLACK = Color(0, 0, 9);

arqanore::Color::Color()
{
    this->r = 0;
    this->g = 0;
    this->b = 0;
    this->a = 255;
}

arqanore::Color::Color(unsigned int r, unsigned int g, unsigned int b)
{
    this->r = r;
    this->g = g;
    this->b = b;
    this->a = 255;

    fix_values(this);
}

arqanore::Color::Color(unsigned int r, unsigned int g, unsigned int b, unsigned int a)
{
    this->r = r;
    this->g = g;
    this->b = b;
    this->a = a;

    fix_values(this);
}

arqanore::Color::Color(const Color& color)
{
    this->r = color.r;
    this->g = color.g;
    this->b = color.b;
    this->a = color.a;

    fix_values(this);
}

bool arqanore::Color::operator==(const Color& color)
{
    return r == color.r && g == color.g && b == color.b && a == color.a;
}

bool arqanore::Color::operator!=(const Color& color)
{
    return r != color.r || g != color.g || b != color.b || a != color.a;
}

arqanore::Color& arqanore::Color::operator=(const Color& color)
{
    this->r = color.r;
    this->g = color.g;
    this->b = color.b;
    this->a = color.a;

    fix_values(this);
    return *this;
}

arqanore::Color& arqanore::Color::operator=(const unsigned int value)
{
    this->r = value;
    this->g = value;
    this->b = value;
    this->a = value;

    fix_values(this);
    return *this;
}

arqanore::Color arqanore::Color::operator+(const Color& color)
{
    Color result;
    result.r = r + color.r;
    result.g = g + color.g;
    result.b = b + color.b;
    result.a = a + color.a;

    fix_values(&result);
    return result;
}

arqanore::Color arqanore::Color::operator+(const unsigned int value)
{
    Color result;
    result.r = r + value;
    result.g = g + value;
    result.b = b + value;
    result.a = a + value;

    fix_values(&result);
    return result;
}

arqanore::Color arqanore::Color::operator-(const Color& color)
{
    Color result;
    result.r = r - color.r;
    result.g = g - color.g;
    result.b = b - color.b;
    result.a = a - color.a;

    fix_values(&result);
    return result;
}

arqanore::Color arqanore::Color::operator-(const unsigned int value)
{
    Color result;
    result.r = r - value;
    result.g = g - value;
    result.b = b - value;
    result.a = a - value;

    fix_values(&result);
    return result;
}

arqanore::Color arqanore::Color::operator/(const Color& color)
{
    Color result;
    result.r = r / color.r;
    result.g = g / color.g;
    result.b = b / color.b;
    result.a = a / color.a;

    fix_values(&result);
    return result;
}

arqanore::Color arqanore::Color::operator/(const unsigned int value)
{
    Color result;
    result.r = r / value;
    result.g = g / value;
    result.b = b / value;
    result.a = a / value;

    fix_values(&result);
    return result;
}

arqanore::Color arqanore::Color::operator*(const Color& color)
{
    Color result;
    result.r = r * color.r;
    result.g = g * color.g;
    result.b = b * color.b;
    result.a = a * color.a;

    fix_values(&result);
    return result;
}

arqanore::Color arqanore::Color::operator*(const unsigned int value)
{
    Color result;
    result.r = r * value;
    result.g = g * value;
    result.b = b * value;
    result.a = a * value;

    fix_values(&result);
    return result;
}
