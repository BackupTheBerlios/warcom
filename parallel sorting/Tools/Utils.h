#ifndef UTILS_H_
#define UTILS_H_
#include<iostream>
#include<mpi.h>
#include<time.h>
#include "DataLoader.h"
using namespace std;

#define COMPARE_SLEEP_NS 5000

namespace tools
{

class Utils
{
public:
	Utils();
	void example();
	static int mpi_send(int* buf, int count, int dest,int tag);
	static int mpi_recv(int* buf, int count, int source, int tag, MPI_Status *status);
	
	
	static int compare(int a, int b);
	static int* recv_init_buffer(int& bufSize, int myrank, MPI_Status& status);
};

}

#endif /*UTILS_H_*/
