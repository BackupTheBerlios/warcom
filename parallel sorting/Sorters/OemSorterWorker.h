#ifndef OEMSORTERWORKER_H_
#define OEMSORTERWORKER_H_
#include<mpi.h>
#include<math.h>
#include<iostream>
#include "../Tools/DataLoader.h"
#include "OemSorter.h"
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
	int canTransferInThisStep(int,int,int);
	int findPartner(int, int,int);
	int compareSplit(int idProces,int myId, int* buffer, int bufSize);
	
};
}
#endif /*OEMSORTERWORKER_H_*/
