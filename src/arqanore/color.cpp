#include "arqanore/color.h"

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
}

arqanore::Color::Color(unsigned int r, unsigned int g, unsigned int b, unsigned int a)
{
	this->r = r;
	this->g = g;
	this->b = b;
	this->a = a;
}

arqanore::Color::Color(const Color &color)
{
	this->r = color.r;
	this->g = color.g;
	this->b = color.b;
	this->a = color.a;
}

bool arqanore::Color::operator==(const Color &color)
{
	return r == color.r && g == color.g && b == color.b && a == color.a;
}

bool arqanore::Color::operator!=(const Color &color)
{
	return r != color.r || g != color.g || b != color.b || a != color.a;
}

arqanore::Color &arqanore::Color::operator=(const Color &color)
{
	this->r = color.r;
	this->g = color.g;
	this->b = color.b;
	this->a = color.a;

	return *this;
}

arqanore::Color &arqanore::Color::operator=(const unsigned int value)
{
	this->r = value;
	this->g = value;
	this->b = value;
	this->a = value;

	return *this;
}

arqanore::Color arqanore::Color::operator+(const Color &color)
{
	Color result;
	result.r = r + color.r;
	result.g = g + color.g;
	result.b = b + color.b;
	result.a = a + color.a;

	return result;
}

arqanore::Color arqanore::Color::operator+(const unsigned int value)
{
	Color result;
	result.r = r + value;
	result.g = g + value;
	result.b = b + value;
	result.a = a + value;

	return result;
}

arqanore::Color arqanore::Color::operator-(const Color &color)
{
	Color result;
	result.r = r - color.r;
	result.g = g - color.g;
	result.b = b - color.b;
	result.a = a - color.a;

	return result;
}

arqanore::Color arqanore::Color::operator-(const unsigned int value)
{
	Color result;
	result.r = r - value;
	result.g = g - value;
	result.b = b - value;
	result.a = a - value;

	return result;
}

arqanore::Color arqanore::Color::operator/(const Color &color)
{
	Color result;
	result.r = r / color.r;
	result.g = g / color.g;
	result.b = b / color.b;
	result.a = a / color.a;

	return result;
}

arqanore::Color arqanore::Color::operator/(const unsigned int value)
{
	Color result;
	result.r = r / value;
	result.g = g / value;
	result.b = b / value;
	result.a = a / value;

	return result;
}

arqanore::Color arqanore::Color::operator*(const Color &color)
{
	Color result;
	result.r = r * color.r;
	result.g = g * color.g;
	result.b = b * color.b;
	result.a = a * color.a;

	return result;
}

arqanore::Color arqanore::Color::operator*(const unsigned int value)
{
	Color result;
	result.r = r * value;
	result.g = g * value;
	result.b = b * value;
	result.a = a * value;

	return result;
}
