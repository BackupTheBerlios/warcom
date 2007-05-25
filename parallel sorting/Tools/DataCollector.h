#ifndef DATACOLLECTOR_H_
#define DATACOLLECTOR_H_
#include<iostream>
#include<mpi.h>
#include"myio.h"
using namespace std;
using namespace tools;

#define END_TAG  2121

namespace tools
{

class DataCollector
{
public:
	DataCollector(string fileName, int pcsCount, int bufferSize);
	~DataCollector();
	void collectData();
private:
	string fileName;
	int pcsCount;
	int bufferSize;
	int fd;
};

}

#endif /*DATACOLLECTOR_H_*/
