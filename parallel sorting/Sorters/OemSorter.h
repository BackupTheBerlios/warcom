#ifndef OEMSORTER_H_
#define OEMSORTER_H_
#include<iostream>
#include<stack>

#include "Sorter.h"
#include "../Tools/Utils.h"
using namespace std;
using namespace tools;


namespace sorting
{

class OemSorter
{
public:
	OemSorter();
	int* sort(int a[], int size);
};

}
#endif /*OEMSORTER_H_*/
