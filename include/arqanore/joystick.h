#pragma once

#include <vector>
#include <string>

namespace arqanore {
    class Joystick {
    public:
        static bool is_connected(int jid);

        static std::string get_name(int jid);

        static std::string get_guid(int jid);

        static std::vector<float> get_axes(int jid);

        static std::vector<int> get_buttons(int jid);
    };
}
