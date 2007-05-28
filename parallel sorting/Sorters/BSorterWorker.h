#ifndef BSORTERWORKER_H_
#define BSORTERWORKER_H_
#include<mpi.h>
#include<math.h>
#include<iostream>
#include "../Tools/DataLoader.h"
#include "../Tools/DataCollector.h"
#include "../Tools/Utils.h"
#include "LocalQSorter.h"

#define ASCENDING true
#define DESCENDING false

using namespace MPI;
using namespace std;
using namespace tools;

namespace sorting
{


class BSorterWorker
{
public:
	BSorterWorker(string inFile, string outFile, int argc, char** args);
	int sort();
	
private:
	int argc;
	char** args;
	string inFile;
	string outFile; 

	int getIdToCompSplit(int etap, int myId,  int coutProc,int* idProces);
	int getDirectionToCompSplit(int etap, int myId,  int coutProc,bool* direction, int idProces);
	int compareSplit(int idProces,int myId, bool direction,int* buffer, int bufSize); 
	void supervisorAction(int numprocs);
	void slaveAction(int numprocs, int myrank);
};
}
#endif /*BSORTERWORKER_H_*/
