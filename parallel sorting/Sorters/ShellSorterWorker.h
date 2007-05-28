#ifndef SHELLSORTERWORKER_H_
#define SHELLSORTERWORKER_H_
#include<mpi.h>
#include<iostream>
#include "../Tools/DataLoader.h"
#include "../Tools/DataCollector.h"
#include "ShellSorter.h"
using namespace MPI;
using namespace std;
using namespace tools;

namespace sorting
{

class ShellSorterWorker
{
public:
	ShellSorterWorker(string inFile, string outFile, int argc, char** args);
	void sort();
	
private:
	string inFile;
	int argc;
	char** args;
	string outFile; 
	
	void manageStopCondition(int numprocs);
	void displayBuffer(int* buffer, int bufferSize, string comment);
};
}
#endif /*SHELLSORTERWORKER_H_*/
