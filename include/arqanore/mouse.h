#pragma once

#include <cstdlib>
#include "window.h"

namespace arqanore {
    class Mouse {
        friend class Window;

    private:
        static unsigned int states[10];
        static float x;
        static float y;
        static float prev_x;
        static float prev_y;
        static float move_x;
        static float move_y;
        static int scroll_x;
        static int scroll_y;

        static void update();

    public:
        static float get_x();
        static float get_y();
        static int get_scroll_x();
        static int get_scroll_y();
        static float get_move_x();
        static float get_move_y();

        static bool button_down(unsigned int button);
        static bool button_up(unsigned int button);
        static bool button_pressed(unsigned int button);
        static void hide(Window *win);
        static void disable(Window *win);
        static void show(Window *win);
    };
}
