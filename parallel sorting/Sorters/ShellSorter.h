#ifndef SHELLORTER_H_
#define SHELLSORTER_H_
#include<iostream>
#include<math.h>

#include "Sorter.h"
#include "LocalQSorter.h"
using namespace std;

#define COMPARE_SPLIT 2000

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
	
	
	int getPIDForCompSplit(int myId, int sortPcsCount, int stage);
	//int getPIDForCompSplit(int stage);
	int compareSplit(int otherPId, int myId, int* buffer, int bufferSize);
	//int compareSplit(int otherPId);
};

}
#endif /*SHELLSORTER_H_*/
