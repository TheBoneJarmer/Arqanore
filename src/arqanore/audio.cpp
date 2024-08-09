#include "arqanore/audio.h"

SoLoud::Soloud* arqanore::Audio::engine;

void arqanore::Audio::init()
{
    engine = new SoLoud::Soloud();
    engine->init();
}

void arqanore::Audio::destroy()
{
    delete engine;
}

void arqanore::Audio::stop()
{
    engine->stopAll();
}
