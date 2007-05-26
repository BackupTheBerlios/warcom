#include "BSorterWorker.h"

namespace sorting
{
	
BSorterWorker::BSorterWorker(string inFile, string outFile)
{
	this->inFile = inFile;
	this->outFile = outFile;
}

/* The most important procedure of this class. If process's myrank is 0 - process will load data, send parts od data to others and in the end will collect sorted data from other processes.
Otherwise process will communicate with rest and exchange data using function compareSplit. To get information about id partner-proces to compareSplit and about direction of sorting in particular stage, process call functions:  getIdToCompSplit and getDirectionToCompSplit 
*/	
int BSorterWorker::sort()
{
	Status status; 
   	MPI::Init();
   	int myrank = COMM_WORLD.Get_rank(); 
   	int numprocs = COMM_WORLD.Get_size(); 
   	if(myrank == 0)
   	{
   		DataLoader dl(inFile, numprocs);
		dl.loadAndSendData();
		DataCollector dc(outFile, numprocs,dl.getBufferSize());
		dc.collectData();		
   	}
   	else
   	{
   		MPI_Status status;
   		MPI_Request request;
   		int bufSize;
   		int* buffer;
		bool direction;
		int idProces;
		
   		MPI_Recv(&bufSize, 1, MPI_INT, 0, BUFFER_SIZE_TAG, MPI_COMM_WORLD, &status);
   		buffer = new int[bufSize];
		MPI_Recv(buffer, bufSize, MPI_INT, 0, WORK_TAG, MPI_COMM_WORLD, &status);
		
		cout<<"Process #"<<myrank<<" My buffer: ";		
		for(int i=0; i<bufSize; i++)
			cout<<buffer[i]<<" ";
		cout<<endl;		
		for(int i=0; i<log2(numprocs-1); ++i)
		{
			for(int j=i; j>=0; --j)
			{
				getIdToCompSplit(j,myrank,numprocs-1,&idProces);
				getDirectionToCompSplit(i,myrank,numprocs-1,
			&direction,idProces);
				compareSplit(idProces, myrank, direction, buffer, bufSize);
			}			
		}		
		cout<<"Sorted proces #: "<<myrank<<": ";
		for(int i = 0; i<bufSize; ++i)
			cout<< buffer[i]<<" ";
		MPI_Send( buffer, bufSize, MPI_INT, 0, END_TAG, MPI_COMM_WORLD);
		cout<<endl;	
		if(buffer != NULL)
			delete(buffer);
   	}
   	MPI::Finalize();
}

int BSorterWorker:: compareSplit(int idProces, int myId, bool direction, int* buffer, int bufSize)
{	
	int* buffer2 = new int[bufSize*2];
	MPI_Request request;
	MPI_Status status; 
	LocalQSorter* lqs = new LocalQSorter(buffer2,bufSize*2);
	
	MPI_Isend( buffer, bufSize, MPI_INT, idProces,
		WORK_TAG+50, MPI_COMM_WORLD, &request );
	MPI_Recv(buffer2, bufSize, MPI_INT, idProces,
		WORK_TAG+50, MPI_COMM_WORLD, &status);	
	
	for(int i = 0; i<bufSize; ++i)
		buffer2[i+bufSize] = buffer[i];//moze to po Isend a przed recv
	
	lqs->localSort(ASCENDING); 
	if(direction == ASCENDING)
	    for(int i = 0; i<bufSize; ++i)
		    buffer[i] = buffer2[i+bufSize];
	if(direction == DESCENDING)
		for(int i = 0; i<bufSize; ++i)
			buffer[i] = buffer2[i];
		
	if(buffer2 != NULL)
			delete(buffer2);
	return 0;
}

/*
function getDirectionToCompSplit 
*/
int BSorterWorker:: getDirectionToCompSplit(int etap, int myId,  int coutProc,bool* direction, int idProces)
{
	int et = (int)pow(2, etap);	
	myId-=1; //zeby byly od zera
	idProces -=1;	
	if(myId%(et*4) < et*2 )
	{
		if(myId<idProces)
			*direction = DESCENDING;			
		else
			*direction = ASCENDING;			
	}		
	else
	{
		if(myId<idProces)
			*direction = ASCENDING;		
		else			
			*direction = DESCENDING;		
		
	}	
	return 0;	
}

int BSorterWorker:: getIdToCompSplit(int depth, int myId,  int coutProc, int* idProces)
{
	myId-=1; //zeby byly od zera
	int et = (int)pow(2, depth);	
	int p;	
	int m = myId%(et*2);		
		
	if(m<et)p = myId+et;
	else p = myId-et;	
	*idProces = p+1; //		
	return 0;
}

}
