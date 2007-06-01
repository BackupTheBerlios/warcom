#include "ShellSorterWorker.h"

namespace sorting
{

/*
 * Constructor
 * inFile - file with data to save
 * outFile - file where to save data
 * args - entry of the program
 * argc - number of arguments
 */
ShellSorterWorker::ShellSorterWorker(string inFile, string outFile, int argc, char** args)
{
	this->inFile = inFile;
	this->outFile = outFile;
	this->argc = argc;
	this->args = args;
}

/*
 * Main ShellSorterWOrker function.
 * For prime process it creates DataLoader and sends data to sorting processes,
 * then manages distributed stop condition, and finally creates DataCollector,
 * receives sorted data from sorting processes and save it.
 */
void ShellSorterWorker::sort()
{
   	MPI::Init(argc, args);
   	int myrank = COMM_WORLD.Get_rank(); 
   	int numprocs = COMM_WORLD.Get_size();
   	MPI_Status mpi_status;
   	MPI_Request request;
   	int *buffer, bufSize;
   	
   	if(myrank == 0)
   	{
   		TaskTimer* tt = new TaskTimer();
   		tt->startTask("whole");
		tt->startTask("load");		
   		DataLoader dl(inFile, numprocs, false);
		dl.loadAndSendData();
		tt->endTask("load",1);
		
		//in sorter - before entering 2nd phase
		MPI_Barrier(MPI_COMM_WORLD);
		manageStopCondition(numprocs);
		
		DataCollector dc(outFile, numprocs,dl.getBufferSize());
		dc.collectData();
		tt->endTask("whole",1);
   	}
   	else
   	{
   		ShellSorter* shells = new ShellSorter(myrank, numprocs-1);	
		buffer = Utils::recv_init_buffer(bufSize, myrank, mpi_status);
		
		if(buffer != NULL)
			buffer = shells->sort(buffer, bufSize);
		//sending sorted buffer to prime process
		if(Utils::mpi_send(buffer, bufSize, 0, END_TAG)!= 0)
				Utils::exitWithError();
		if(buffer != NULL)
			delete(buffer);
   	}
   	MPI::Finalize();
}

/*
 * Manages distributed stop condition.
 * Receives change report (whether local buffer has changed after last odd-even phase) 
 * from each of sorting process (OE_CHANGED or OE_UNCHANGED) and sends them command 
 * OE_SORTING_DONE (if there were no changes in all buffers) or OE_SORTING_DONE (if at least one process submitted changes). 
 * 
 * numprocs - number of all processes
 */
void ShellSorterWorker::manageStopCondition(int numprocs)
{	
	   	MPI_Status mpi_status;
   		MPI_Request request;	
	
		bool done = false;
		for(int i=1; i<numprocs; i++)
		{
			int noChangesRes;
			bool sortingDone = true;
			
			for(int i=1; i<numprocs; i++)
			{
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
				MPI_Isend(&res, 1, MPI_INT, i, OE_STOP_CONDITION, MPI_COMM_WORLD, &request);
			
			if(done)
				return;
			MPI_Barrier(MPI_COMM_WORLD);
		}
}

/*
 * Displays buffer and prints comment on stdout.
 * idProcess - id of process with which compare split operation is process
 * buffer - contains data to display
 * bufSize - size of a buffer to display
 */
void ShellSorterWorker::displayBuffer(int* buffer, int bufSize, string comment)
{
		cout<<comment<<endl;
			
		for(int i=0; i<bufSize; i++)
			cout<<buffer[i]<<" ";
		cout<<endl;
		cout<<"-------------------------------------------------------------------------"<<endl;
}
}
