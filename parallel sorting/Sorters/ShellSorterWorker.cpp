#include "ShellSorterWorker.h"

namespace sorting
{

ShellSorterWorker::ShellSorterWorker(string inFile, string outFile, int argc, char** args)
{
	this->inFile = inFile;
	this->outFile = outFile;
	this->argc = argc;
	this->args = args;
}

void ShellSorterWorker::sort()
{
	Status status; 
   	MPI::Init(argc, args);
   	int myrank = COMM_WORLD.Get_rank(); 
   	int numprocs = COMM_WORLD.Get_size();
   	
   	MPI_Status mpi_status;
   	MPI_Request request;
   	
   	int* buffer;
   	int bufSize;
   	
   	if(myrank == 0)
   	{
   		TaskTimer* tt = new TaskTimer();
   		tt->startTask("whole");
		tt->startTask("load");
		
   		DataLoader dl(inFile, numprocs, false);
		dl.loadAndSendData();
		tt->endTask("load",1);
		
		//in sorter - before entering 2nd phase
		//cout<<"Process #0 before msc barrier"<<endl;
		MPI_Barrier(MPI_COMM_WORLD);
		//cout<<"Process #0 after msc barrier"<<endl;
		manageStopCondition(numprocs);
		
		DataCollector dc(outFile, numprocs,dl.getBufferSize());
		dc.collectData();
		tt->endTask("whole",1);
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
		//MPI_Send( buffer, bufSize, MPI_INT, 0, END_TAG, MPI_COMM_WORLD);
		if(Utils::mpi_send(buffer, bufSize, 0, END_TAG)!= 0)
				Utils::exitWithError();
		
		if(buffer != NULL)
			delete(buffer);
   	}
   	
   	MPI::Finalize();
}

void ShellSorterWorker::manageStopCondition(int numprocs)
{	
	   	MPI_Status mpi_status;
   		MPI_Request request;	
	
		bool done = false;
		//while(!done)
		for(int i=1; i<numprocs; i++)
		{
			int noChangesRes;
			bool sortingDone = true;
			
			
			
			for(int i=1; i<numprocs; i++)
			{
				//MPI_Recv(&noChangesRes, 1, MPI_INT, i, OE_RESULT, MPI_COMM_WORLD, &mpi_status);
				if(Utils::mpi_recv(&noChangesRes, 1, i, OE_RESULT, &mpi_status)!= 0)
						Utils::exitWithError();
				//cout<<"Changes report received from #"<<i<<endl;
				if(noChangesRes == OE_CHANGED)
					sortingDone = false;
			}
			
			int res = OE_SORTING_UNDONE;
			if(sortingDone)
			{
				done = true;
				res = OE_SORTING_DONE;
			}
				
			for(int i=1; i<numprocs; i++)
			{
				//cout<<"Sending stop condition to #"<<i<<endl;			
				MPI_Isend(&res, 1, MPI_INT, i, OE_STOP_CONDITION, MPI_COMM_WORLD, &request );
			}
			
			if(done)
				return;
			
			MPI_Barrier(MPI_COMM_WORLD);
		}
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
