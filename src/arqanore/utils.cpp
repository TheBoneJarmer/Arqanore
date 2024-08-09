#include <fstream>
#include "arqanore/utils.h"
#include "arqanore/exceptions.h"

std::vector<std::string> arqanore::string_split(const std::string &s, char delim) {
    std::stringstream stream(s);
    std::string item;
    std::vector<std::string> result;

    while (getline(stream, item, delim)) {
        result.push_back(item);
    }

    return result;
}

std::string arqanore::string_replace(const std::string &str, const std::string &what, const std::string &with) {
    auto src = str;
    auto pos = src.find(what);

    if (pos == std::string::npos) {
        return str;
    }

    src.replace(pos, what.length(), with);
    return src;
}

std::string arqanore::string_replace_all(const std::string &str, const std::string &what, const std::string &with) {
    auto src = str;
    auto pos = 0;

    if (what.empty()) {
        return src;
    }

    while (true) {
        pos = src.find(what, pos);

        if (pos == std::string::npos) {
            break;
        }

        src.replace(pos, what.length(), with);
        pos += with.length();
    }

    return src;
}

std::filesystem::path arqanore::get_parent_path(const std::filesystem::path &src) {
    return src.parent_path();
}

std::filesystem::path arqanore::real_path(const std::filesystem::path& src) {
    std::filesystem::path clean_path = src.string();
    clean_path = string_replace_all(clean_path.string(), "..", "$");
    clean_path = string_replace_all(clean_path.string(), "./", "");
    clean_path = string_replace_all(clean_path.string(), "$", "..");

    auto directory = canonical(absolute(clean_path.parent_path()));
    auto filename = clean_path.filename();

    return directory.string() + "/" + filename.string();
}

bool arqanore::read_file(const std::string &filename, std::vector<std::string> &out) {
    std::string line;
    std::ifstream file(filename);

    if (file.is_open()) {
        while (getline(file, line)) {
            out.push_back(line);
        }

        file.close();
        return true;
    }

    throw ArqanoreException("Failed to read file " + filename);
}

bool arqanore::read_file(const std::string &filename, std::string &out) {
    std::ifstream file(filename);
    std::stringstream buffer;

    if (file.is_open()) {
        buffer << file.rdbuf();
        file.close();
        out = buffer.str();
        return true;
    }

    throw ArqanoreException("Failed to read file " + filename);
}

bool arqanore::write_file(const std::string &filename, const std::string &content) {
    std::ofstream file(filename);

    if (file.is_open()) {
        file << content;
        file.close();
        return true;
    }

    throw ArqanoreException("Failed to write to file " + filename);
}