#include "BSorter.h"

namespace sorting
{

BSorter::BSorter()
{
}

int* BSorter::sort(int a[], int size)
{	
	this->a = a;
	this->size = size;
	bitonicSort(0, getValue(size),ASCENDING);
	return a;
}

int BSorter::getValue(int value)
{
	int i=1;
	while(i < value)
		i *= 2;
	return i;
}

void BSorter::bitonicMerge(int lo, int n, bool dir)
{
	if (n>1)
	{
		int m = n/2;
		for (int i = lo; i< lo + m; ++i)
			compare(i, i +m, dir);
		bitonicMerge(lo, m, dir);
		bitonicMerge(lo+m, m, dir); 
	}
}

void BSorter::bitonicSort(int lo, int n, bool dir)
{
	if (n > 1)
	{
		int m = n / 2;
		bitonicSort(lo, m, ASCENDING);
		bitonicSort(lo + m, m, DESCENDING);
		bitonicMerge(lo, n, dir);
	}
}

void BSorter::compare(int i, int j, bool dir)
{
	if(j < size && i < size)
		if(dir == (a[i] > a[j]))
			exchange(i, j);
}

void BSorter::exchange(int i, int j)
{
	int t = a[i];
	a[i] = a[j];
	a[j] = t;
}

void BSorter::display()
{
	for(int i = 0; i < size; ++i)
	{
		std::cout<<a[i];
		std::cout<<" ";
	}
	std::cout<<"\n";
}


}
