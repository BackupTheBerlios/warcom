#include "DataLoader.h"

namespace tools
{

DataLoader::DataLoader(string fileName)
{
	this->fileName = fileName;
}

int* DataLoader::loadData()
{
	return NULL;
}

void DataLoader::saveData(int* array, int size, string outputFile)
{
}

}
