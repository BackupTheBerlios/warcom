#ifndef DATACOLLECTOR_H_
#define DATACOLLECTOR_H_
#include<iostream>
#include<mpi.h>
#include"myio.h"
#include "TaskTimer.h"
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
	void collectData(int *buffer);
	void commonAction(int numProcs, int *buffer);
private:
	string fileName;
	int pcsCount;
	int bufferSize;
	int fd;
	int removeGuards(int* buffer, int size);
	
};

}

#endif /*DATACOLLECTOR_H_*/
