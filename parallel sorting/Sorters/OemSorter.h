#ifndef OEMSORTER_H_
#define OEMSORTER_H_
#include<iostream>

#include "Sorter.h"
using namespace std;


namespace sorting
{

class OemSorter
{
public:
	OemSorter();
	int* sort(int a[], int size);
	void display();
	static void test();
private:
	int* a;
	int size;
	void oddEvenMergeSort(int lo, int n);
	void oddEvenMerge(int lo, int n, int r);
	void compare(int i, int j);
	void exchange(int i, int j);
	int getValue(int value);
};

}
#endif /*OEMSORTER_H_*/
