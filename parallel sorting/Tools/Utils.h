#ifndef UTILS_H_
#define UTILS_H_
#include<iostream>
#include<mpi.h>
#include "DataLoader.h"
using namespace std;

namespace tools
{

class Utils
{
public:
	Utils();
	void example();
	int mpi_send(int* buf, int count, int dest,int tag);
	int mpi_recv(int* buf, int count, int source, int tag, MPI_Status *status);
	
	static int* recv_init_buffer(int& bufSize, int myrank, MPI_Status& status);
};

}

#endif /*UTILS_H_*/
