#pragma once

#include <stdexcept>

namespace arqanore {
    class GlfwException : public std::exception {
    private:
        int code;
        std::string message;

    public:
        int get_code();

        GlfwException();

        GlfwException(int code, const char *message);

        const char *what();
    };

    class ArqanoreException : public std::exception {
    private:
        std::string message;

    public:
        ArqanoreException();

        ArqanoreException(std::string message);

        const char *what();
    };
}