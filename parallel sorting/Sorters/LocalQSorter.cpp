#include "LocalQSorter.h"

namespace sorting
{
	/*
	* Constructor
	* a - buffer with data to sort
	* size - size of buffer
	*/
	LocalQSorter::LocalQSorter(int a[], int size)
	{
		this->a = a;
		this->size = size;
	}

	/*
	* Calling this procedure starts recursive calling function quicksort.
	*
	* dir - ascending or descending, direction in which data will be sorted
	*/
	void LocalQSorter::localSort(bool dir)
	{
		quicksort( 0 , size-1, dir );
	}

	/*
	* Its divide the task of sorting (last - first) elements into two smaller tasks,
	* by calling function pivot(). 
	* Then function calls recursive itself for smaller sequences.
	* 
	* first - index in buffer which is the begin of sequence which will be sorted 
	* last - index in buffer which is the end of sequence which will be sorted 
	* dir - ascending or descending, direction in which data will be sorted
	*/
	void LocalQSorter:: quicksort( int first, int last, bool dir ) 
	{
		int piv;

		if( Utils::compare(first, last)) {
			piv = pivot( first, last, dir );
			quicksort( first, piv-1, dir );
			quicksort( piv+1, last, dir );
		}
	}

	/*
	* sets element called pivot and by swaping data in buffer create two sequences: 
	* first with elements smaller or equal pivot and second with elements greater than pivot.
	* Return the index in buffer where is pivot.
	*
	* first - index in buffer which is the begin of sequence 
	* last - index in buffer which is the end of sequence 
	* dir - ascending or descending, direction in which data will be sorted
	*/
	int LocalQSorter:: pivot( int first, int last, bool dir ) 
	{
		int  p = first;
		int pivot = a[first];

		for( int i = first+1 ; i <= last ; i++ ) {
			if(!Utils::compare(pivot, a[i]) == dir) {
				p++;
				swap( a[i], a[p] );
			}
		}

		swap( a[p], a[first] );

		return p;
	}

	/*
	* swaps two integers
	v1 - the reference to first integer
	v2 - the reference to second integer 
	*/
	void  LocalQSorter::swap( int &v1, int &v2 )
	{
		int  tmpVal;

		tmpVal = v1;
		v1 = v2;
		v2 = tmpVal;
	}

	/*
    * displays buffer
	*/
	void  LocalQSorter::display()
	{
		cout << "[ ";

		for( int i = 0 ; i < size ; i++ )
		{
			cout << a[i] ;
			if( i < size-1 )
			   cout << ", ";
		}

		cout << " ] " << endl;
	}
}
