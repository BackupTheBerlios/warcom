#include "BSorterWorker.h"

namespace sorting
{
/*
 * Constructor
 * inFile - file with data to sort
 * outFile - file where to save sorted data
 * args - entry of the program
 * argc - number of arguments
 */
BSorterWorker::BSorterWorker(string inFile, string outFile, int argc, char** args)
{
	this->inFile = inFile;
	this->outFile = outFile;
	this->argc = argc;
	this->args = args;
}
/* 
 * Main procedure of this class. 
 * Prime process call function supervisorAction(numprocs),
 * the rest of processes call slaveAction(numprocs,myrank).
 */	
int BSorterWorker::sort()
{
	Status status; 
   	MPI::Init(argc, args);
   	int myrank = COMM_WORLD.Get_rank(); 
   	int numprocs = COMM_WORLD.Get_size(); 
   	if(myrank == 0)
		supervisorAction(numprocs);
   	else
		slaveAction(numprocs,myrank);
   	MPI::Finalize();
	return 0;
}

/* 
* Function, which is called by prime process.
* It create DataLoader, which load file and sends blocks of data to other processes.
* Then it takes the last block of data - prime process also sorts data by proper calling 
* function compareSplit(). 
* To get information about id partner-proces to compareSplit and about direction of sorting 
* in particular phase, process call functions:  getIdToCompSplit and getDirectionToCompSplit 
* Finally creates DataCollector, which receives sorted data from sorting processes and 
* save it (it saves also block of data, which was sorted by itself).
*
* numprocs - number of all processes
*/
void BSorterWorker:: supervisorAction(int numprocs)
{
	int bufSize,idProces,*buffer;
	bool direction;
	
	TaskTimer* tt = new TaskTimer();
   	tt->startTask("whole");
	tt->startTask("load");
	DataLoader dl(inFile, numprocs, true);
   	dl.loadAndSendData();	
	buffer = dl.loadPrimeProcessData();
    bufSize = dl.getBufferSize() ;
	tt->endTask("load",1);
	
	for(int i=0; i<log2(numprocs/*-1*/); ++i)
	{
		for(int j=i; j>=0; --j)
		{
			getIdToCompSplit(j,0,numprocs/*-1*/,&idProces);
			getDirectionToCompSplit(i,0,numprocs/*-1*/,
			&direction,idProces);
			if(compareSplit(idProces, 0, direction, buffer, bufSize))
				Utils::exitWithError(); 
		}			
	}
	//	cout<<"Sorted proces #: 0: ";
	//for(int i = 0; i<bufSize; ++i)
	//	cout<< buffer[i]<<" ";
	//cout<<endl;	
	
	DataCollector dc(outFile, numprocs,dl.getBufferSize());
	tt->startTask("collect");
	dc.collectData(buffer);
	tt->endTask("collect",1);	
	tt->endTask("whole",1);	
}	

/*
* Function, which is called by sorting process (all without prime process).
* It receive block of data from prime process (DataLoader), and sorts data by proper calling 
* function compareSplit(). The information about parameters of calling compareSplit(),
* is taken by calling functions: getIdToCompSplit() and	getDirectionToCompSplit().
* Finally sorted block is sended to prime process (DataCollector).
* numprocs -  number of all processes
* myrank - id of process which call this function
*/
void BSorterWorker:: slaveAction(int numprocs, int myrank)
{
	MPI_Status status;
	MPI_Request request;
   	int bufSize,idProces,*buffer;
	bool direction;
	
   	if(Utils::mpi_recv(&bufSize, 1, 0, BUFFER_SIZE_TAG, &status))
   		Utils::exitWithError();
	buffer = new int[bufSize]; //Utils::init_recv
	if(Utils::mpi_recv(buffer, bufSize, 0, WORK_TAG, &status))
   		Utils::exitWithError(); 
	//
	//cout<<"Process #"<<myrank<<" My buffer: ";		
	//for(int i=0; i<bufSize; i++)
	//	cout<<buffer[i]<<" ";
	//cout<<endl;		
	//
	for(int i=0; i<log2(numprocs/*-1*/); ++i)
	{
		for(int j=i; j>=0; --j)
		{
			getIdToCompSplit(j,myrank,numprocs/*-1*/,&idProces);
			getDirectionToCompSplit(i,myrank,numprocs/*-1*/,
			&direction,idProces);
			if(compareSplit(idProces, myrank, direction, buffer, bufSize))
				Utils::exitWithError(); 
		}			
	}
	
	//cout<<"Sorted proces #: "<<myrank<<": ";
	//for(int i = 0; i<bufSize; ++i)
	//	cout<< buffer[i]<<" ";
	//cout<<endl;	

	if(Utils::mpi_send(buffer, bufSize, 0, END_TAG))
		Utils::exitWithError();	
	if(buffer != NULL)
		delete(buffer);
	
}


/*
* By this function process communicates with rest of processes and exchange data.
* It starts from sending its own block of data, then it receives the block of data from process 
* with idProces. It sorts the sequence of data using LocalQSorter.
* Then it save greater or lower part of sequence - witch is determined by parameter direction.

* idProces - rank of process which is parner in this calling function compareSplit
* myId - rank of process which call function
* direction - ascending or descending, information which part of data - lower or greater - should
*		be saven by process
* buffer - buffer with data
* bufSize - size of buffer
*/
int BSorterWorker:: compareSplit(int idProces, int myId, bool direction, int* buffer, int bufSize)
{	
	MPI_Request request;
	MPI_Status status; 
	int* buffer2 = new int[bufSize*2];
	LocalQSorter* lqs = new LocalQSorter(buffer2,bufSize*2);
			
	if(Utils::mpi_send(buffer,bufSize,idProces,WORK_TAG+50))
		return 1;
	if(Utils::mpi_recv(buffer2,bufSize,idProces,WORK_TAG+50,&status))
		return 1;
	
	for(int i = 0; i<bufSize; ++i)
		buffer2[i+bufSize] = buffer[i];
	
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
 * Gets direction (ASCENDING or DESCENDING)- information which part(lower or greater) of 
 * sequence (which will be sorted in CompareSplit()) should be saven in operation CompareSlit()
 *
 * etap - current phase of sorting 
 * myId - rank of process calling this function
 * coutProc - a number of sorting processes
 * direction - information which part(lower or greater) of sorted in CompareSplit()
 * sequence should be saven in operation COmpareSlit()
 * idProces - rank of process, which take part in operation CompareSplit(), and which save another part
 *		of sequence
 */
int BSorterWorker:: getDirectionToCompSplit(int etap, int myId,  int coutProc, bool* direction, int idProces)
{
	int et = (int)pow(2, etap);	
	//myId-=1; //zeby byly od zera
	//idProces -=1;	
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

/*
* Gets a rank of process, which will take part in operation CompareSplit()
* depth - the depth of current phase of sorting
* myId - rank of process calling this function
* coutProc - a number of sorting processes
* idProces - rank of process, which will take part in operation CompareSplit()
*/
int BSorterWorker:: getIdToCompSplit(int depth, int myId,  int coutProc, int* idProces)
{
	//myId-=1; //zeby byly od zera
	int et = (int)pow(2, depth);	
	int p;	
	int m = myId%(et*2);		
		
	if(m<et)p = myId+et;
	else p = myId-et;	
	*idProces = p;//+1; //		
	return 0;
}

}
