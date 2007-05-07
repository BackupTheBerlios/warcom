#ifndef OEMSORTERWORKER_H_
#define OEMSORTERWORKER_H_
#include<mpi.h>
#include<iostream>
#include "../Tools/DataLoader.h"
using namespace MPI;
using namespace std;
using namespace tools;

namespace sorting
{

class OemSorterWorker
{
public:
	OemSorterWorker(string inFile, string outFile);
	void sort();
	
private:
	string inFile;
	string outFile; 
};
}
#endif /*OEMSORTERWORKER_H_*/
