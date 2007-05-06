#include "Sorter.h"

namespace sorting
{

Sorter::Sorter()
{
	MPI_Init(NULL,NULL);
}

Sorter::~Sorter()
{
	MPI_Finalize();
}

int* Sorter::sort(int* array, int size)
{
	return array;
}

}
