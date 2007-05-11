#ifndef LOCALQSORTER_H_
#define LOCALQSORTER_H_
#include<iostream>

//#define ASCENDING true
//#define DESCENDING false

using namespace std;


namespace sorting
{

class LocalQSorter 
{
public:	
	LocalQSorter(int a[], int size);
	void localSort( bool dir); //true: ASCENDING, false: DESCENDING
	void display();
private:
	int* a;
	int size;
	void  swap(int &v1, int &v2 );
	int pivot(int first, int last, bool dir ) ;
	void quicksort( int first, int last, bool dir );

};

}
#endif /*LOCALQSORTER_H_*/