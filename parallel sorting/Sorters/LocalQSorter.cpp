#include "LocalQSorter.h"

namespace sorting
{
	LocalQSorter::LocalQSorter(int a[], int size)
	{
		this->a = a;
		this->size = size;
	}
	void LocalQSorter::localSort(bool dir)
	{
		quicksort( 0 , size-1, dir );
	}

	void LocalQSorter:: quicksort( int first, int last, bool dir ) 
	{
		int piv;

		if( Utils::compare(first, last)) {
			piv = pivot( first, last, dir );
			quicksort( first, piv-1, dir );
			quicksort( piv+1, last, dir );
		}
	}
	int LocalQSorter:: pivot( int first, int last, bool dir ) 
	{
		int  p = first;
		int pivot = a[first];

		for( int i = first+1 ; i <= last ; i++ ) {
			if( (a[i] <= pivot) == dir  ) {
				p++;
				swap( a[i], a[p] );
			}
		}

		swap( a[p], a[first] );

		return p;
	}
	
	void  LocalQSorter::swap( int &v1, int &v2 )
	{
		int  tmpVal;

		tmpVal = v1;
		v1 = v2;
		v2 = tmpVal;
	}

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
