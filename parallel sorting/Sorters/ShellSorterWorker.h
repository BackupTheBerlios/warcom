#ifndef SHELLSORTERWORKER_H_
#define SHELLSORTERWORKER_H_
#include<mpi.h>
#include<iostream>
#include "../Tools/DataLoader.h"
#include "ShellSorter.h"
using namespace MPI;
using namespace std;
using namespace tools;

namespace sorting
{

class ShellSorterWorker
{
public:
	ShellSorterWorker(string inFile, string outFile);
	void sort();
	
private:
	string inFile;
	string outFile; 
};
}
#endif /*SHELLSORTERWORKER_H_*/
