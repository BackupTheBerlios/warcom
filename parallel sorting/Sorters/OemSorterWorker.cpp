#include "OemSorterWorker.h"

namespace sorting
{

/*
 * Constructor
 * inFile - file with data to save
 * outFile - file where to save data
 * argc - entry of the program
 * args - number of arguments
 */
OemSorterWorker::OemSorterWorker(string inFile, string outFile, int argc, char** args)
{
	this->inFile = inFile;
	this->outFile = outFile;
	this->argc = argc;
	this->args = args;
}

/*
 * Compare split operation
 * idProcess - id of process with which compare split operation is process
 * myId - id of process with which compare split operation is process
 * buffer - to send, it also contains data after operation is finished
 * bufSize - size of a buffer to send
 */
int OemSorterWorker::compareSplit(int idProcess, int myId, int* buffer, int bufSize)
{	
	
	int* buffer2 = new int[bufSize*2];
	MPI_Status status; 
	OemSorter* sorter = new OemSorter();
	if(Utils::mpi_send(buffer,bufSize,idProcess,WORK_TAG+50))
		return 1;
	if(Utils::mpi_recv(buffer2,bufSize,idProcess,WORK_TAG+50,&status))
		return 1;
	for(int i = 0; i<bufSize; ++i)
		buffer2[i+bufSize] = buffer[i];
	sorter->sort(buffer2, bufSize * 2);
	int shift = 0;
	if(myId > idProcess)
		shift = bufSize;
	for(int i = 0; i<bufSize; ++i)
		    buffer[i] = buffer2[i+shift];
	if(buffer2 != NULL)
			delete(buffer2);
	return 0;	
}

/*
 * Checks whether process can transfer data in stage describe by i and j
 * k - process id
 * i and j - stage description
 * return - boolean information whether can transfer
 */
int OemSorterWorker::canTransferInThisStep(int k ,int i ,int j)
{
	k = k + 1;
	if(j > 0 )
	{
		int forbiden = (int)pow(2,i - j);
		int block = 2 << i;
		int modulo = ((k - 1) % block) + 1;
		if(modulo <= forbiden || modulo > (block - forbiden)) 
			return 0;
	}
	return 1;
}

/*
 * Finds partner for a given process with which compare operation will be process
 * k - process id
 * i and j - stage description
 * return - partner id
 */
int OemSorterWorker::findPartner(int k , int i ,int j)
{
	k = k + 1;
	int block = 2 << i;
	int partner =  k + (int)pow(2, i - j);
	int expr = (( k - 1 ) % block) + (int)pow(2, i - j) + 1;
	if(expr > block || !canTransferInThisStep(partner - 1, i,j) || ((k % 2 == 1) && ( i >=2 ) && (i == j)))
		partner = k - (int)pow(2, i - j);
	return partner - 1;
}

/*
 * supervisor process action - load data , then sends and receives data, after all save data
 */
void OemSorterWorker::supervisorAction(int numprocs)
{
	int partner, bufSize;
	TaskTimer* tt = new TaskTimer();
	OemSorter* oem = new OemSorter();
   	tt->startTask("whole");
	tt->startTask("load");
   	DataLoader dl(inFile, numprocs, true);
   	bufSize = dl.getBufferSize();
   	dl.loadAndSendData();
	tt->endTask("load",1);
	DataCollector dc(outFile, numprocs, bufSize);
	int* buffer = dl.loadPrimeProcessData();
	oem->sort(buffer, bufSize);
	for(int i=0;i<log2(numprocs);i++)
		for(int j=0;j<=i;j++)
				if(canTransferInThisStep(0, i, j))
				{
					partner = findPartner(0, i, j);
					if(compareSplit(partner, 0, buffer, bufSize))
						Utils::exitWithError();
				}
	dc.collectData(buffer);
	tt->endTask("whole",1);
}	

/*
 * Slave process action - only sends and receives data
 */
void OemSorterWorker::slaveAction(int numprocs, int myrank)
{
	OemSorter* oem = new OemSorter();
   	MPI_Status status;
   	int bufSize, partner, *buffer;
   	if(Utils::mpi_recv(&bufSize, 1, 0, BUFFER_SIZE_TAG, &status))
   		Utils::exitWithError();
   	buffer = new int[bufSize];
   	if(Utils::mpi_recv(buffer, bufSize, 0, WORK_TAG, &status))
   		Utils::exitWithError(); 
	oem->sort(buffer, bufSize);
	for(int i=0;i<log2(numprocs);i++)
		for(int j=0;j<=i;j++)
				if(canTransferInThisStep(myrank, i, j))
				{
					partner = findPartner(myrank, i, j);
					if(compareSplit(partner, myrank, buffer, bufSize))
						Utils::exitWithError();
				}
	if(Utils::mpi_send(buffer, bufSize, 0, END_TAG))
		Utils::exitWithError();
	if(buffer != NULL)
		delete(buffer);
	
}

/*
 * Start process, decide which process make which action
 */
int OemSorterWorker::sort()
{
	MPI::Init(argc, args);
   	int myrank = COMM_WORLD.Get_rank(); 
   	int numprocs = COMM_WORLD.Get_size(); 
   	if(myrank == 0)
   		supervisorAction(numprocs);
   	else
   		slaveAction(numprocs, myrank);
	MPI::Finalize();
   	return 0;
}
}
