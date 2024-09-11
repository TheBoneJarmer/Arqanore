#include <iostream>

#include "modelexporter.h"
#include "modelimporter.h"
#include "arqanore/exceptions.h"

struct ConverterOptions
{
    std::string src;
    std::string dest;
};

void print_help()
{
    std::cout << "arq-mc -s <path_to_model_file> -d <destination_path>" << std::endl;
    std::cout << std::endl;
    std::cout << "ABOUT" << std::endl;
    std::cout << "Arqanore Model Converter is a tool that converts your model file to the model format used by Arqanore" << std::endl;
    std::cout << std::endl;
    std::cout << "ARGUMENTS" << std::endl;
    std::cout << "  -s    Path to the model file you wish to convert. This argument is required." << std::endl;
    std::cout << "  --source" << std::endl;
    std::cout << "  -d    Path to the destination model file. Make sure to add the extension '.arqm'." << std::endl;
    std::cout << "  --dest" << std::endl;
    std::cout << std::endl;
}

bool parse_options(int argc, char** argv, ConverterOptions* options)
{
    if (argc == 2 && (argv[1] == "-h" || argv[1] == "--help"))
    {
        print_help();
        return false;
    }

    for (int i = 1; i < argc; i++)
    {
        std::string value = argv[i];

        if (i == argc)
        {
            std::cerr << "Not enough arguments provided" << std::endl;
            return false;
        }

        if (value == "-s" || value == "--source")
        {
            options->src = argv[i + 1];
            i++;
            continue;
        }

        if (value == "-d" || value == "--dest")
        {
            options->dest = argv[i + 1];
            i++;
            continue;
        }

        std::cerr << "Unknown option " << value << std::endl;
        return false;
    }

    return true;
}

bool validate_options(ConverterOptions* options)
{
    if (options->src.empty())
    {
        std::cerr << "No source file was provided" << std::endl;
        return false;
    }

    if (options->dest.empty())
    {
        std::cerr << "No destination file was provided" << std::endl;
        return false;
    }

    if (!options->dest.ends_with(".arqm"))
    {
        std::cerr << "Destination file is missing .arqm extension" << std::endl;
        return false;
    }

    return true;
}

bool run(ConverterOptions* options)
{
    try
    {
        arqanore::ModelImporter importer;
        arqanore::ModelExporter exporter;

        auto result = importer.load(options->src);
        exporter.save(result, options->dest);
    }
    catch (arqanore::AssimpException& ex)
    {
        std::cerr << ex.what() << std::endl;
        return false;
    }
    catch (arqanore::ArqanoreException& ex)
    {
        std::cerr << ex.what() << std::endl;
        return false;
    }

    return true;
}

int main(int argc, char** argv)
{
    ConverterOptions options;

    if (!parse_options(argc, argv, &options))
    {
        return 1;
    }

    if (!validate_options(&options))
    {
        return 2;
    }

    if (!run(&options))
    {
        return 3;
    }

    return 0;
}
