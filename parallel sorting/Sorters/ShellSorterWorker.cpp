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
   	
   	MPI_Status mpi_status;
   	MPI_Request request;
   	
   	int* buffer;
   	int bufSize; 
   	
   	if(myrank == 0)
   	{
   		DataLoader dl(inFile, numprocs);
		dl.loadAndSendData();
		DataCollector dc(outFile, numprocs,dl.getBufferSize());
		dc.collectData();
   	}
   	else
   	{
   		//cout<<"allSortersCount = "<< numprocs-1<<endl;
   		ShellSorter* shells = new ShellSorter(myrank, numprocs-1);
   		
		buffer = Utils::recv_init_buffer(bufSize, myrank, mpi_status);
		//cout<<"Process #"<<myrank<<": Buffer received"<<endl;
		//displayBuffer("Buffer received");
		
		
		
		if(buffer != NULL)
			buffer = shells->sort(buffer, bufSize);
		
		//wysłać procesowi głownemu
		MPI_Send( buffer, bufSize, MPI_INT, 0, END_TAG, MPI_COMM_WORLD);
		
		if(buffer != NULL)
			delete(buffer);
   	}
   	
   	MPI::Finalize();
}

void ShellSorterWorker::displayBuffer(int* buffer, int bufSize, string comment)
{
		cout<<comment<<endl;
			
		for(int i=0; i<bufSize; i++)
			cout<<buffer[i]<<" ";
		cout<<endl;
		cout<<"-------------------------------------------------------------------------"<<endl;
}
}
