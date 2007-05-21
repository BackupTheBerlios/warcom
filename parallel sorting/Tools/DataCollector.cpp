#include "DataCollector.h"

namespace tools
{

DataCollector::DataCollector(string fileName, int pcsCount, int bufferSize, int maxElemSize)
{
	this->fileName = fileName;
	this->pcsCount = pcsCount;
	this->bufferSize = bufferSize;
	this->elemSize = elemSize;
	this->buffer = new string[bufferSize];
	fd = MyIO::my_open(fileName,O_RDWR);
	
	
}

DataCollector::~DataCollector()
{
	MyIO::my_close(fd);
}

}
