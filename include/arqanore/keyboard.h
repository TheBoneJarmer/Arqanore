#pragma once

#include <cstdlib>

namespace arqanore {
    class Keyboard {
        friend class Window;

    private:
        static unsigned int scancode;
        static unsigned int states[512];

        static void update();

    public:
        static unsigned int get_scancode();
        static bool key_down(unsigned int key);
        static bool key_up(unsigned int key);
        static bool key_pressed(unsigned int key);
    };
    
    class Keys {
    public:
        static const unsigned int UNKNOWN;
        static const unsigned int SPACE;
        static const unsigned int APOSTROPHE;
        static const unsigned int COMMA;
        static const unsigned int MINUS;
        static const unsigned int PERIOD;
        static const unsigned int SLASH;
        static const unsigned int ALPHA_0;
        static const unsigned int ALPHA_1;
        static const unsigned int ALPHA_2;
        static const unsigned int ALPHA_3;
        static const unsigned int ALPHA_4;
        static const unsigned int ALPHA_5;
        static const unsigned int ALPHA_6;
        static const unsigned int ALPHA_7;
        static const unsigned int ALPHA_8;
        static const unsigned int ALPHA_9;
        static const unsigned int SEMICOLON;
        static const unsigned int EQUAL;
        static const unsigned int A;
        static const unsigned int B;
        static const unsigned int C;
        static const unsigned int D;
        static const unsigned int E;
        static const unsigned int F;
        static const unsigned int G;
        static const unsigned int H;
        static const unsigned int I;
        static const unsigned int J;
        static const unsigned int K;
        static const unsigned int L;
        static const unsigned int M;
        static const unsigned int N;
        static const unsigned int O;
        static const unsigned int P;
        static const unsigned int Q;
        static const unsigned int R;
        static const unsigned int S;
        static const unsigned int T;
        static const unsigned int U;
        static const unsigned int V;
        static const unsigned int W;
        static const unsigned int X;
        static const unsigned int Y;
        static const unsigned int Z;
        static const unsigned int LEFT_BRACKET;
        static const unsigned int BACKSLASH;
        static const unsigned int RIGHT_BRACKET;
        static const unsigned int GRAVE_ACCENT;
        static const unsigned int WORLD_1;
        static const unsigned int WORLD_2;
        static const unsigned int ESCAPE;
        static const unsigned int ENTER;
        static const unsigned int TAB;
        static const unsigned int BACKSPACE;
        static const unsigned int INSERT;
        static const unsigned int DELETE;
        static const unsigned int RIGHT;
        static const unsigned int LEFT;
        static const unsigned int DOWN;
        static const unsigned int UP;
        static const unsigned int PAGE_UP;
        static const unsigned int PAGE_DOWN;
        static const unsigned int HOME;
        static const unsigned int END;
        static const unsigned int CAPS_LOCK;
        static const unsigned int SCROLL_LOCK;
        static const unsigned int NUM_LOCK;
        static const unsigned int PRINT_SCREEN;
        static const unsigned int PAUSE;
        static const unsigned int F1;
        static const unsigned int F2;
        static const unsigned int F3;
        static const unsigned int F4;
        static const unsigned int F5;
        static const unsigned int F6;
        static const unsigned int F7;
        static const unsigned int F8;
        static const unsigned int F9;
        static const unsigned int F10;
        static const unsigned int F11;
        static const unsigned int F12;
        static const unsigned int F13;
        static const unsigned int F14;
        static const unsigned int F15;
        static const unsigned int F16;
        static const unsigned int F17;
        static const unsigned int F18;
        static const unsigned int F19;
        static const unsigned int F20;
        static const unsigned int F21;
        static const unsigned int F22;
        static const unsigned int F23;
        static const unsigned int F24;
        static const unsigned int F25;
        static const unsigned int KP_0;
        static const unsigned int KP_1;
        static const unsigned int KP_2;
        static const unsigned int KP_3;
        static const unsigned int KP_4;
        static const unsigned int KP_5;
        static const unsigned int KP_6;
        static const unsigned int KP_7;
        static const unsigned int KP_8;
        static const unsigned int KP_9;
        static const unsigned int KP_DECIMAL;
        static const unsigned int KP_DIVIDE;
        static const unsigned int KP_MULTIPLY;
        static const unsigned int KP_SUBTRACT;
        static const unsigned int KP_ADD;
        static const unsigned int KP_ENTER;
        static const unsigned int KP_EQUAL;
        static const unsigned int LEFT_SHIFT;
        static const unsigned int LEFT_CONTROL;
        static const unsigned int LEFT_ALT;
        static const unsigned int LEFT_SUPER;
        static const unsigned int RIGHT_SHIFT;
        static const unsigned int RIGHT_CONTROL;
        static const unsigned int RIGHT_ALT;
        static const unsigned int RIGHT_SUPER;
        static const unsigned int MENU;
    };
}
