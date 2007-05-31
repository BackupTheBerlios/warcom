#include "OemSorter.h"

namespace sorting
{

OemSorter::OemSorter()
{
}

/*
 * Sorts given array with odd-even mergeosort algorithm
 * a - array to sort
 * size - size of a array
 * return - sorted array
 */
int* OemSorter::sort(int a[], int size)
{
	int n = size;
	int temp;
	for(int p = 1; p < size; p += p)
		for(int k = p; k > 0; k /=2 )
			for(int j = k % p; ( j + k ) < n; j += ( k + k ))
				for(int i = 0; i < ( n - j - k ); i++)
					if(((j + i) / ( p + p )) == (( j + i + k) / ( p + p)))	
						if(!Utils::compare(a[j + i],a[j + i + k]))
						{
								temp = a[j + i];
								a[j + i] = a[j + i + k];
								a[j + i + k] = temp;
						}
					
	return a;
}
}
