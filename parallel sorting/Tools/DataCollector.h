#ifndef DATACOLLECTOR_H_
#define DATACOLLECTOR_H_
#include<iostream>
#include"myio.h"
using namespace std;
using namespace tools;

namespace tools
{

class DataCollector
{
public:
	DataCollector(string fileName, int pcsCount, int bufferSize, int elemSize);
	~DataCollector();
private:
	string fileName;
	int pcsCount;
	int bufferSize;
	int elemSize;
	int fd;
	string* buffer;
};

}

#endif /*DATACOLLECTOR_H_*/
