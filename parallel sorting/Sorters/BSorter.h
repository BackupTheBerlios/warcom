#ifndef BSORTER_H_
#define BSORTER_H_
#include<iostream>
#include "../Tools/Utils.h"

#define ASCENDING true
#define DESCENDING false

using namespace std;
using namespace tools;


namespace sorting
{

class BSorter
{
public:
	BSorter();
	int* sort(int a[], int size);
	void display();
	static void test();
private:
	int* a;
	int size;
	void bitonicSort(int lo, int n, bool dir);
	void bitonicMerge(int lo, int n, bool dir);
	void compare(int i, int j, bool dir);
	void exchange(int i, int j);
	int greatestPowerOfTwoLessThan(int n);
};

}
#endif /*BSORTER_H_*/
