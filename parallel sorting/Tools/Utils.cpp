#include "Utils.h"
using namespace std;

namespace tools
{

Utils::Utils()
{
	
}

void Utils::example()
{
	cout<<"TEST";
}

void Utils::exitWithError()
{
	MPI::Finalize();
	exit(1);
}

int Utils::mpi_send(int* buf, int count, int dest, int tag)
{
	int ret = MPI_Send((void*) buf, count, MPI_INT,
		 dest, tag, MPI_COMM_WORLD);
	if(ret == MPI_SUCCESS) 
		return 0;
	else
	{
		cout<<"mpi_send error: ";
		if(ret == MPI_ERR_COMM)
			cout<<"MPI_ERR_COMM";
		if(ret == MPI_ERR_COUNT)
			cout<<"MPI_ERR_COUNT";			
		if(ret == MPI_ERR_TYPE)
			cout<<"MPI_ERR_TYPE";
		if(ret == MPI_ERR_TAG)
			cout<<"MPI_ERR_TAG";
		if(ret == MPI_ERR_RANK)
			cout<<"MPI_ERR_RANK";	
		return 1;
	}		
}
int Utils:: mpi_recv(int* buf, int count, int source, int tag, MPI_Status *status)
{
	int ret = MPI_Recv((void*) buf, count, MPI_INT, source, tag, MPI_COMM_WORLD, status);
	if(ret == MPI_SUCCESS) 
		return 0;
	else
	{
		cout<<"mpi_recv error: ";
		return 1;
	}	
}

int* Utils::recv_init_buffer(int& bufSize, int myrank, MPI_Status& mpi_status)
{
	//MPI_Recv(&bufSize, 1, MPI_INT, 0, BUFFER_SIZE_TAG, MPI_COMM_WORLD, &mpi_status);
 	if(Utils::mpi_recv(&bufSize, 1, 0, BUFFER_SIZE_TAG, &mpi_status) != 0)
	    Utils::exitWithError();
    
    cout<<"Process #"<<myrank<<": I received bufferSize="<<bufSize<<endl;
    if(bufSize <= 0)
    	return NULL;
    	
   	int* buffer = new int[bufSize];
   	
   	if(buffer == NULL)
   		return NULL;
	
	for(int i=0; i< bufSize; i++)
   		buffer[i] = 0;	
	
	//MPI_Recv(buffer, bufSize, MPI_INT, 0, WORK_TAG, MPI_COMM_WORLD, &mpi_status);
	if(Utils::mpi_recv(buffer, bufSize, 0, WORK_TAG, &mpi_status) != 0)
		Utils::exitWithError();
	
	return buffer;
}

int Utils::compare(int a, int b)
{
	struct timespec ts;
	ts.tv_sec = 0;
    ts.tv_nsec = COMPARE_SLEEP_NS;
    nanosleep (&ts, NULL);
	
	if(a < b)
		return 1;
	else
		return 0;
}

}
    
