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
		
		//bufSize = dl.getBufferSize();
		//buffer = dl.loadPrimeProcessData();
		
		//buffer = Utils::recv_init_buffer(bufSize, myrank, mpi_status);
		
//		if(buffer != NULL)
//		{
//			cout<<"Process #"<<myrank<<": Buffer received"<<endl;
//			displayBuffer("Buffer received");
//			
//			//TODO potem normalny udział w sortowaniu
//			
//			ShellSorter* shells = new ShellSorter();
//			if(buffer != NULL)
//				shells->sort(buffer, bufSize);
//			
//			if(buffer != NULL)
//				delete(buffer);
//		}
//		else
//			cout << "!!! Process #"<<myrank<<": Buffer receiving error!"<<endl;

		//MPI_Barrier(MPI_COMM_WORLD);
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
