#include "ShellSorterWorker.h"

namespace sorting
{

ShellSorterWorker::ShellSorterWorker(string inFile, string outFile)
{
	this->inFile = inFile;
	this->outFile = outFile;
}

void ShellSorterWorker::sort()
{
	Status status; 
   	MPI::Init();
   	int myrank = COMM_WORLD.Get_rank(); 
   	int numprocs = COMM_WORLD.Get_size();
   	
   	int* buffer;
   	int bufSize; 
   	
   	if(myrank == 0)
   	{
   		DataLoader dl(inFile, numprocs);
		dl.loadAndSendData();
		bufSize = dl.getBufferSize();
		buffer = dl.loadPrimeProcessData();
		
		cout<<"Process #"<<myrank<<": My buffer: ";
		for(int i=0; i<bufSize; i++)
			cout<<buffer[i]<<" ";
		cout<<endl;
		//TODO potem normalny udział w sortowaniu
   	}
   	else
   	{
   		ShellSorter* shells = new ShellSorter();
   		
   		MPI_Status mpi_status;
   		MPI_Request request;
   		
		buffer = Utils::recv_init_buffer(bufSize, myrank, mpi_status);
		
		cout<<"Process #"<<myrank<<": My buffer: ";
		for(int i=0; i<bufSize; i++)
			cout<<buffer[i]<<" ";
		cout<<endl;
		
		if(buffer != NULL)
			shells->sort(buffer, bufSize);
		
		if(buffer != NULL)
			delete(buffer);
   	}
   	
   	MPI::Finalize();
}
}
