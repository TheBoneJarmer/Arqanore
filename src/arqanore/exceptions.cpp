#include "arqanore/exceptions.h"

int arqanore::GlfwException::get_code() {
    return this->code;
}

arqanore::GlfwException::GlfwException() {
    this->code = 0;
}

arqanore::GlfwException::GlfwException(int code, const char *message) {
    this->code = code;
    this->message = message;
}

const char *arqanore::GlfwException::what() {
    return (const char *) message.c_str();
}

arqanore::ArqanoreException::ArqanoreException() {

}

arqanore::ArqanoreException::ArqanoreException(std::string message) {
    this->message = message;
}

const char *arqanore::ArqanoreException::what() {
    return (const char *) message.c_str();
}
