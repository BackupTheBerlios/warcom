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
/*
* called in case of error, which determine the need to end program.
* Calls MPI: Finalize() and exit with code of error.
*/
void Utils::exitWithError()
{
	MPI::Finalize();
	exit(1);
}
/*
* encapsulate operation MPI_Send and provide proper action in case of error. 
*
* buf - buffer with data to send
* count - the size of buffer buf
* dest - rank of process to which data will be sended
* tag - the tag of operation 
*/
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
/*
* encapsulate operation MPI_Recv and provide proper action in case of error. 
* buf - buffer where receive data will be saven
* count - the size of expected data and buffer
* source - rank of process which will send data
* tag - the tag of operation 
* status - structure MPI_Status for operation MPI_Recv
*/
int Utils:: mpi_recv(int* buf, int count, int source, int tag, MPI_Status *status)
{
	int ret = MPI_Recv((void*) buf, count, MPI_INT, source, tag, MPI_COMM_WORLD, status);
	if(ret == MPI_SUCCESS) 
		return 0;
	else
	{
		cout<<"mpi_recv error: ";
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

/*
 * Receives buffer with data to sort from prime process.
 * First it calls mpi_recv to receive the size of data, then create buffer and
 * calls mpi_recv to receive block of data.
 * Returns buffer with data.
 * bufSize - size of buffer
 * myrank - id process which call function
 * mpi_status - structure MPI_Status for operation MPI_Recv
 */
int* Utils::recv_init_buffer(int& bufSize, int myrank, MPI_Status& mpi_status)
{
	//MPI_Recv(&bufSize, 1, MPI_INT, 0, BUFFER_SIZE_TAG, MPI_COMM_WORLD, &mpi_status);
 	if(Utils::mpi_recv(&bufSize, 1, 0, BUFFER_SIZE_TAG, &mpi_status) != 0)
	    Utils::exitWithError();
    
    //cout<<"Process #"<<myrank<<": I received bufferSize="<<bufSize<<endl;
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

/*
 * Compares two integers.
 * Return 1 if first element is smaller than second and 0 otherwise.
 * To slow down this operation and simulate  more expensive computations its provide 
 * the busy waiting computation - 10000 itrations. 
 * a - first element to compare
 * b - second element to compare
 */
int Utils::compare(int a, int b)
{
	//struct timespec ts;
	//ts.tv_sec = 0;
    //ts.tv_nsec = COMPARE_SLEEP_NS;
   	// nanosleep (&ts, NULL);
	int temp;	
	for(int i=0;i<10000;i++)
	{
		temp+=2;
		temp-=2;
	}	
	if(a < b)
		return 1;
	else
		return 0;
}

}
    
