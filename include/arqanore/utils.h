#pragma once

#include <string>
#include <filesystem>
#include <vector>

namespace arqanore {
    std::vector<std::string> string_split(const std::string &s, char delim);

    std::string string_replace(const std::string &str, const std::string &what, const std::string &with);

    std::string string_replace_all(const std::string &str, const std::string &what, const std::string &with);

    std::filesystem::path real_path(const std::filesystem::path& src);

    std::filesystem::path get_parent_path(const std::filesystem::path &src);

    bool read_file(const std::string &filename, std::vector<std::string> &out);

    bool read_file(const std::string &filename, std::string &out);

    bool write_file(const std::string &filename, const std::string &content);
}