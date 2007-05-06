#include "OemSorter.h"

namespace sorting
{

OemSorter::OemSorter()
{
}

int* OemSorter::sort(int a[], int size)
{
	this->a = a;
	this->size = size;
	oddEvenMergeSort(0, getValue(size));
	return a;
}

int OemSorter::getValue(int value)
{
	int i=1;
	while(i < value)
		i *= 2;
	return i;
}

void OemSorter::oddEvenMerge(int lo, int n, int r)
{
	int m=r * 2;
	if (m < n)
	{
		oddEvenMerge(lo, n, m);      // even subsequence
		oddEvenMerge(lo + r, n, m);    // odd subsequence
		for (int i = lo + r; i + r < lo + n; i+=m)
			compare(i, i + r);
	}
	else 
		compare(lo, lo + r);
}

void OemSorter::oddEvenMergeSort(int lo, int n)
{
	if (n > 1)
	{
		int m = n / 2;
		oddEvenMergeSort(lo, m);
		oddEvenMergeSort(lo + m, m);
		oddEvenMerge(lo, n, 1);
	}
}

void OemSorter::compare(int i, int j)
{
	if(j < size && i < size)
		if(a[i] > a[j])
			exchange(i, j);
}

void OemSorter::exchange(int i, int j)
{
	int t = a[i];
	a[i] = a[j];
	a[j] = t;
}

void OemSorter::display()
{
	for(int i = 0; i < size; ++i)
	{
		std::cout<<a[i];
		std::cout<<" ";
	}
	std::cout<<"\n";
}


}
