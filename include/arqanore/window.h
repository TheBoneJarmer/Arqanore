#pragma once

#define GLFW_INCLUDE_NONE

#include <GLFW/glfw3.h>
#include <string>

#include "color.h"
#include "image.h"

namespace arqanore {
    class Window {
        friend class Mouse;

        friend class Sound;

    private:
        GLFWwindow *handle;
        int x;
        int y;
        int width;
        int height;
        bool vsync;
        std::string title;
        Color clear_color;
        double fps;

        void init(bool fullscreen, bool maximized, bool resizable);

        void loop();

        void (*window_open_cb)(Window *window);

        void (*window_close_cb)(Window *window);

        void (*window_update_cb)(Window *window, double);

        void (*window_render_cb)(Window *window);

        void (*window_resize_cb)(Window *window, int, int);

        void (*window_pos_cb)(Window *window, int, int);

        void (*window_char_cb)(Window *window, unsigned int);

        static void error_callback(int error_code, const char *error_description);

        static void window_close_callback(GLFWwindow *handle);

        static void window_resize_callback(GLFWwindow *handle, int width, int height);

        static void window_pos_callback(GLFWwindow *handle, int x, int y);

        static void key_callback(GLFWwindow *handle, int key, int scancode, int action, int mods);

        static void mouse_button_callback(GLFWwindow *handle, int button, int action, int mods);

        static void cursor_position_callback(GLFWwindow *handle, double xpos, double ypos);

        static void character_callback(GLFWwindow *handle, unsigned int codepoint);

        static void scroll_callback(GLFWwindow *handle, double xoffset, double yoffset);

    public:
        int get_width();

        int get_height();

        int get_x();

        int get_y();

        std::string get_title();

        bool get_vsync();

        Color get_clear_color();

        double get_fps();

        bool is_closed();

        bool is_minimized();

        bool is_maximized();

        bool is_visible();

        void set_width(int value);

        void set_height(int value);

        void set_x(int value);

        void set_y(int value);

        void set_title(std::string value);

        void set_vsync(bool value);

        void set_clear_color(Color value);

        void set_closed(bool value);

        void set_icon(Image images[], int count);

        void set_icon(Image& image);

        Window();

        Window(int width, int height, std::string title);

        void open(bool fullscreen, bool maximized, bool resizable);

        void close();

        void minimize();

        void maximize();

        void restore();

        void show();

        void hide();

        void on_open(void (*cb)(Window *));

        void on_update(void (*cb)(Window *, double));

        void on_render(void (*cb)(Window *));

        void on_close(void (*cb)(Window *));

        void on_resize(void (*cb)(Window *, int, int));

        void on_position(void (*cb)(Window *, int, int));

        void on_char(void (*cb)(Window *, unsigned int));

        //void on_opengl(void (*cb)(Window *, std::string, std::string, std::string));
    };
}
