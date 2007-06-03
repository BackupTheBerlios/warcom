#include "BSorter.h"

namespace sorting
{

BSorter::BSorter()
{
}
	/*
	* Constructor
	* a - buffer with data to sort
	* size - size of buffer
	*/
int* BSorter::sort(int a[], int size)
{	
	this->a = a;
	this->size = size;
	bitonicSort(0, size, ASCENDING);
	return a;
}

	/*
	* Gives the greatest power of two less than n
	* n - integer 
	*/
int BSorter::greatestPowerOfTwoLessThan(int n)
{
	int k=1;
    while (k<n)
        k=k<<1;
    return k>>1;
}

	/*
	* recursively sorts a bitonic sequence in ascending order, if dir = ASCENDING, 
	* and in descending order otherwise. The sequence to be sorted starts at index position lo, 
	* the number of elements is n.
	* lo - index in buffer a[] which is begining of sequence
	* n - size of sequence
	* dir - ascending or descending, direction in which data will be sorted
	*/
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

	/*
	* Divide the task of sorting n elements into two smaller tasks.
	* Call recursive itself for sorting the first half of sequence in direction opposite
	* to direction dir and for sorting second half of sequence in deirection dir.
	* After calling bitonicMerge on two sequence sorted in opposite directions sequence is bitonic.
	*
	* lo - index in buffer a[] which is begining of sequence
	* n - size of sequence
	* dir - ascending or descending, direction in which data will be sorted
	*/
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

	/*
	* swaps elements of buffer a[] with indexes i, j, if they aren't sorted in direction 
	* determine by parameter dir.
	* calls function compare from class Utils and eventually call exchange
	* i - index in buffer of first integer
	* j - index in buffer of second integer
	* dir - ascending or descending, direction in which inegers i,j will be sorted
	*/
void BSorter::compare(int i, int j, bool dir)
{
	if(dir == (!Utils::compare(a[j],a[i])))
		exchange(i, j);
}

	/*
	* swaps two integers in buffer a
	* i - index in buffer of first integer
	* j - index in buffer of second integer
	*/
void BSorter::exchange(int i, int j)
{
	int t = a[i];
	a[i] = a[j];
	a[j] = t;
}


	/*
	* displays buffer
	*/
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
