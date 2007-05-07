#ifndef SORTER_H_
#define SORTER_H_
#include "mpi.h"

namespace sorting
{

class Sorter
{
public:
	Sorter();
	~Sorter();
	int* sort(int*, int size);
	
private:
	int numprocs;
	int myid;
	
};

}

#endif /*SORTER_H_*/
