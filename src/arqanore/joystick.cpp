#include <stdexcept>
#include <GLFW/glfw3.h>
#include "arqanore/joystick.h"
#include "arqanore/exceptions.h"

bool arqanore::Joystick::is_connected(int jid) {
    if (jid < 0 || jid > 15) {
        throw ArqanoreException("Illegal joystick id. Expected a number between or equal to 0 and 15.");
    }

    return glfwJoystickPresent(jid);
}


std::string arqanore::Joystick::get_name(int jid) {
    auto input = glfwGetJoystickName(jid);

    if (input == nullptr) {
        throw ArqanoreException("Joystick " + std::to_string(jid) + " is not connected.");
    }

    return std::string(input);
}

std::string arqanore::Joystick::get_guid(int jid) {
    auto input = glfwGetJoystickGUID(jid);

    if (input == nullptr) {
        throw ArqanoreException("Joystick " + std::to_string(jid) + " is not connected.");
    }

    return std::string(input);
}

std::vector<float> arqanore::Joystick::get_axes(int jid) {
    int *arr_length = new int;
    auto arr = glfwGetJoystickAxes(jid, arr_length);
    std::vector<float> result;

    if (arr == nullptr) {
        throw ArqanoreException("Joystick " + std::to_string(jid) + " is not connected.");
    }

    for (int i = 0; i < *arr_length; i++) {
        result.push_back(arr[i]);
    }

    delete arr_length;
    return result;
}

std::vector<int> arqanore::Joystick::get_buttons(int jid) {
    int *arr_length = new int;
    auto arr = glfwGetJoystickButtons(jid, arr_length);
    std::vector<int> result;

    if (arr == nullptr) {
        throw ArqanoreException("Joystick " + std::to_string(jid) + " is not connected.");
    }

    for (int i = 0; i < *arr_length; i++) {
        result.push_back(arr[i]);
    }

    delete arr_length;
    return result;
}
