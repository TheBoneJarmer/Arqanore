#include "arqanore/sound.h"
#include "arqanore/audio.h"
#include "arqanore/exceptions.h"

arqanore::Sound::Sound() {
    this->wav = nullptr;
}

arqanore::Sound::Sound(std::string path) {
    this->wav = new SoLoud::Wav();
    this->wav->load(path.c_str());
    this->handle = 0;
}

arqanore::Sound::~Sound() {
    delete this->wav;
}

bool arqanore::Sound::paused() {
    return Audio::engine->getPause(this->handle);
}

bool arqanore::Sound::looping() {
    return Audio::engine->getLooping(this->handle);
}

bool arqanore::Sound::playing() {
    return this->handle > 0 && !this->paused();
}

double arqanore::Sound::volume() {
    return Audio::engine->getVolume(this->handle);
}

double arqanore::Sound::pan() {
    return Audio::engine->getPan(this->handle);
}

void arqanore::Sound::looping(bool value) {
    if (!playing()) {
        throw arqanore::ArqanoreException("Cannot loop sound because the sound is not playing.");
    }

    Audio::engine->setLooping(this->handle, value);
}

void arqanore::Sound::paused(bool value) {
    Audio::engine->setPause(this->handle, value);
}

void arqanore::Sound::volume(double value) {
    if (value < 0) {
        value = 0;
    }

    if (value > 2) {
        value = 2;
    }

    Audio::engine->setVolume(this->handle, value);
}

void arqanore::Sound::pan(double value) {
    if (value < -1) {
        value = -1;
    }

    if (value > 1) {
        value = 1;
    }

    Audio::engine->setPan(this->handle, value);
}

void arqanore::Sound::play() {
    if (this->handle > 0) {
        Audio::engine->stop(this->handle);
    }

    this->handle = Audio::engine->play(*this->wav);
}

void arqanore::Sound::stop() {
    Audio::engine->stop(this->handle);
    this->handle = 0;
}
