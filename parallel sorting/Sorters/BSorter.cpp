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
	bitonicSort(0, size, ASCENDING);
	return a;
}

int BSorter::greatestPowerOfTwoLessThan(int n)
{
	int k=1;
    while (k<n)
        k=k<<1;
    return k>>1;
}

void BSorter::bitonicMerge(int lo, int n, bool dir)
{
	if (n>1)
	{
		int m = greatestPowerOfTwoLessThan(n);
		for (int i = lo; i< lo +n-m; ++i)
			compare(i, i+m, dir);
		bitonicMerge(lo, m, dir);
		bitonicMerge(lo+m, n-m, dir); 
	}
}

void BSorter::bitonicSort(int lo, int n, bool dir)
{
	if (n > 1)
	{
		int m = n / 2;
		bitonicSort(lo, m,!dir);			
		bitonicSort(lo + m, n-m, dir);
		bitonicMerge(lo, n, dir);
	}
}

void BSorter::compare(int i, int j, bool dir)
{
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
