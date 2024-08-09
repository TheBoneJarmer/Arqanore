#pragma once

#include <string>
#include "soloudw/soloud.h"
#include "soloudw/soloud_wav.h"

namespace arqanore {
    class Sound {
    private:
        SoLoud::Wav *wav;
        
        int handle;

    public:
        Sound();

        Sound(std::string path);

        ~Sound();
        
        bool paused();

        bool looping();
        
        bool playing();

        double volume();

        double pan();

        void looping(bool value);

        void paused(bool value);

        void volume(double value);

        void pan(double value);

        void play();

        void stop();
    };
}
