#ifndef SHELLORTER_H_
#define SHELLSORTER_H_
#include<iostream>
#include<math.h>
#include<mpi.h>

#include "Sorter.h"
#include "LocalQSorter.h"
#include "../Tools/Utils.h"
using namespace std;
using namespace MPI;
using namespace tools;

#define COMPARE_SPLIT 2000
#define OE_RESULT 1900
#define OE_STOP_CONDITION 1901

#define OE_NO_CHANGES 1
#define OE_CHANGED 2

#define OE_SORTING_DONE 3
#define OE_SORTING_UNDONE 4

namespace sorting
{

class ShellSorter
{
public:
	//ShellSorter();
	ShellSorter(int myId, int allSortersCount);
	int* sort(int a[], int size);
	void display();
	static void test();
	
	//void setStages(int stages) { if(stages > 0) this.stages = stages; }
private:
	int* a;
	int size;
	
	int stages;
	int allSortersCount;
	int myId;
	
	void sendLocalSetChanged(int myId, bool localSetChanged);
	int receiveStopCondition(int myId);
	
	
	int getPIDForCompSplit(int myId, int sortPcsCount, int stage);
	//int getPIDForCompSplit(int stage);
	int compareSplit(int otherPId, int myId, int* buffer, int bufferSize);
	int compareSplit(int otherPId, int myId, int* buffer, int bufferSize, bool& changed);
	//int compareSplit(int otherPId);
};

}
#endif /*SHELLSORTER_H_*/
