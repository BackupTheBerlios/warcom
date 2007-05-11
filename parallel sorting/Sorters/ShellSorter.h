#ifndef SHELLORTER_H_
#define SHELLSORTER_H_
#include<iostream>

#include "Sorter.h"
using namespace std;


namespace sorting
{

class ShellSorter
{
public:
	ShellSorter();
	int* sort(int a[], int size);
	void display();
	static void test();
private:
	int* a;
	int size;
	
	int getValue(int value);
};

}
#endif /*SHELLSORTER_H_*/
