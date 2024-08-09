#include "arqanore/keyboard.h"

#include <string>

#include "arqanore/exceptions.h"

/* KEYS */
const unsigned int arqanore::Keys::UNKNOWN = -1;
const unsigned int arqanore::Keys::SPACE = 32;
const unsigned int arqanore::Keys::APOSTROPHE = 39;
const unsigned int arqanore::Keys::COMMA = 44;
const unsigned int arqanore::Keys::MINUS = 45;
const unsigned int arqanore::Keys::PERIOD = 46;
const unsigned int arqanore::Keys::SLASH = 47;
const unsigned int arqanore::Keys::ALPHA_0 = 48;
const unsigned int arqanore::Keys::ALPHA_1 = 49;
const unsigned int arqanore::Keys::ALPHA_2 = 50;
const unsigned int arqanore::Keys::ALPHA_3 = 51;
const unsigned int arqanore::Keys::ALPHA_4 = 52;
const unsigned int arqanore::Keys::ALPHA_5 = 53;
const unsigned int arqanore::Keys::ALPHA_6 = 54;
const unsigned int arqanore::Keys::ALPHA_7 = 55;
const unsigned int arqanore::Keys::ALPHA_8 = 56;
const unsigned int arqanore::Keys::ALPHA_9 = 57;
const unsigned int arqanore::Keys::SEMICOLON = 59;
const unsigned int arqanore::Keys::EQUAL = 61;
const unsigned int arqanore::Keys::A = 65;
const unsigned int arqanore::Keys::B = 66;
const unsigned int arqanore::Keys::C = 67;
const unsigned int arqanore::Keys::D = 68;
const unsigned int arqanore::Keys::E = 69;
const unsigned int arqanore::Keys::F = 70;
const unsigned int arqanore::Keys::G = 71;
const unsigned int arqanore::Keys::H = 72;
const unsigned int arqanore::Keys::I = 73;
const unsigned int arqanore::Keys::J = 74;
const unsigned int arqanore::Keys::K = 75;
const unsigned int arqanore::Keys::L = 76;
const unsigned int arqanore::Keys::M = 77;
const unsigned int arqanore::Keys::N = 78;
const unsigned int arqanore::Keys::O = 79;
const unsigned int arqanore::Keys::P = 80;
const unsigned int arqanore::Keys::Q = 81;
const unsigned int arqanore::Keys::R = 82;
const unsigned int arqanore::Keys::S = 83;
const unsigned int arqanore::Keys::T = 84;
const unsigned int arqanore::Keys::U = 85;
const unsigned int arqanore::Keys::V = 86;
const unsigned int arqanore::Keys::W = 87;
const unsigned int arqanore::Keys::X = 88;
const unsigned int arqanore::Keys::Y = 89;
const unsigned int arqanore::Keys::Z = 90;
const unsigned int arqanore::Keys::LEFT_BRACKET = 91;
const unsigned int arqanore::Keys::BACKSLASH = 92;
const unsigned int arqanore::Keys::RIGHT_BRACKET = 93;
const unsigned int arqanore::Keys::GRAVE_ACCENT = 96;
const unsigned int arqanore::Keys::WORLD_1 = 161;
const unsigned int arqanore::Keys::WORLD_2 = 162;
const unsigned int arqanore::Keys::ESCAPE = 256;
const unsigned int arqanore::Keys::ENTER = 257;
const unsigned int arqanore::Keys::TAB = 258;
const unsigned int arqanore::Keys::BACKSPACE = 259;
const unsigned int arqanore::Keys::INSERT = 260;
const unsigned int arqanore::Keys::DELETE = 261;
const unsigned int arqanore::Keys::RIGHT = 262;
const unsigned int arqanore::Keys::LEFT = 263;
const unsigned int arqanore::Keys::DOWN = 264;
const unsigned int arqanore::Keys::UP = 265;
const unsigned int arqanore::Keys::PAGE_UP = 266;
const unsigned int arqanore::Keys::PAGE_DOWN = 267;
const unsigned int arqanore::Keys::HOME = 268;
const unsigned int arqanore::Keys::END = 269;
const unsigned int arqanore::Keys::CAPS_LOCK = 280;
const unsigned int arqanore::Keys::SCROLL_LOCK = 281;
const unsigned int arqanore::Keys::NUM_LOCK = 282;
const unsigned int arqanore::Keys::PRINT_SCREEN = 283;
const unsigned int arqanore::Keys::PAUSE = 284;
const unsigned int arqanore::Keys::F1 = 290;
const unsigned int arqanore::Keys::F2 = 291;
const unsigned int arqanore::Keys::F3 = 292;
const unsigned int arqanore::Keys::F4 = 293;
const unsigned int arqanore::Keys::F5 = 294;
const unsigned int arqanore::Keys::F6 = 295;
const unsigned int arqanore::Keys::F7 = 296;
const unsigned int arqanore::Keys::F8 = 297;
const unsigned int arqanore::Keys::F9 = 298;
const unsigned int arqanore::Keys::F10 = 299;
const unsigned int arqanore::Keys::F11 = 300;
const unsigned int arqanore::Keys::F12 = 301;
const unsigned int arqanore::Keys::F13 = 302;
const unsigned int arqanore::Keys::F14 = 303;
const unsigned int arqanore::Keys::F15 = 304;
const unsigned int arqanore::Keys::F16 = 305;
const unsigned int arqanore::Keys::F17 = 306;
const unsigned int arqanore::Keys::F18 = 307;
const unsigned int arqanore::Keys::F19 = 308;
const unsigned int arqanore::Keys::F20 = 309;
const unsigned int arqanore::Keys::F21 = 310;
const unsigned int arqanore::Keys::F22 = 311;
const unsigned int arqanore::Keys::F23 = 312;
const unsigned int arqanore::Keys::F24 = 313;
const unsigned int arqanore::Keys::F25 = 314;
const unsigned int arqanore::Keys::KP_0 = 320;
const unsigned int arqanore::Keys::KP_1 = 321;
const unsigned int arqanore::Keys::KP_2 = 322;
const unsigned int arqanore::Keys::KP_3 = 323;
const unsigned int arqanore::Keys::KP_4 = 324;
const unsigned int arqanore::Keys::KP_5 = 325;
const unsigned int arqanore::Keys::KP_6 = 326;
const unsigned int arqanore::Keys::KP_7 = 327;
const unsigned int arqanore::Keys::KP_8 = 328;
const unsigned int arqanore::Keys::KP_9 = 329;
const unsigned int arqanore::Keys::KP_DECIMAL = 330;
const unsigned int arqanore::Keys::KP_DIVIDE = 331;
const unsigned int arqanore::Keys::KP_MULTIPLY = 332;
const unsigned int arqanore::Keys::KP_SUBTRACT = 333;
const unsigned int arqanore::Keys::KP_ADD = 334;
const unsigned int arqanore::Keys::KP_ENTER = 335;
const unsigned int arqanore::Keys::KP_EQUAL = 336;
const unsigned int arqanore::Keys::LEFT_SHIFT = 340;
const unsigned int arqanore::Keys::LEFT_CONTROL = 341;
const unsigned int arqanore::Keys::LEFT_ALT = 342;
const unsigned int arqanore::Keys::LEFT_SUPER = 343;
const unsigned int arqanore::Keys::RIGHT_SHIFT = 344;
const unsigned int arqanore::Keys::RIGHT_CONTROL = 345;
const unsigned int arqanore::Keys::RIGHT_ALT = 346;
const unsigned int arqanore::Keys::RIGHT_SUPER = 347;
const unsigned int arqanore::Keys::MENU = 348;

/* KEYBOARD */
unsigned int arqanore::Keyboard::states[512];
unsigned int arqanore::Keyboard::scancode;

void arqanore::Keyboard::update()
{
    for (unsigned int& i : states)
    {
        unsigned int* state = &i;

        if (*state == 1)
        {
            *state = 2;
        }
        if (*state == 4)
        {
            *state = 0;
        }
    }

    scancode = 0;
}

unsigned int arqanore::Keyboard::get_scancode()
{
    return scancode;
}

bool arqanore::Keyboard::key_down(unsigned int key)
{
    if (key >= 512)
    {
        throw ArqanoreException("Invalid key code " + std::to_string(key));
    }

    return states[key] > 0;
}

bool arqanore::Keyboard::key_up(unsigned int key)
{
    if (key >= 512)
    {
        throw ArqanoreException("Invalid key code " + std::to_string(key));
    }

    return states[key] == 4;
}

bool arqanore::Keyboard::key_pressed(unsigned int key)
{
    if (key >= 512)
    {
        throw ArqanoreException("Invalid key code " + std::to_string(key));
    }

    return states[key] == 1 || states[key] == 3;
}
