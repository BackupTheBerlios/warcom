#include "OemSorterWorker.h"

namespace sorting
{

OemSorterWorker::OemSorterWorker(string inFile, string outFile)
{
	this->inFile = inFile;
	this->outFile = outFile;
}

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

int OemSorterWorker::canTransferInThisStep(int k ,int i ,int j)
{
	if(j > 0 )
	{
		int forbiden = (int)pow(2,j - 1);
		int block = 2 << i;
		int modulo = ((k - 1) % block) + 1;
		if(modulo <= forbiden || modulo > (block - forbiden)) 
			return 0;
	}
	return 1;
}

int OemSorterWorker::findPartner(int k , int i ,int j)
{
	int block = 2 << i;
	int partner =  k + i + 1 - j;
	int expr = (( k - 1 ) % block) + i + 2 - j;
	if(expr > block || !canTransferInThisStep(partner, i,j))
		partner = k - i - 1 + j;
	return partner;
}

void OemSorterWorker::supervisorAction(int numprocs)
{
	TaskTimer* tt = new TaskTimer();
   	tt->startTask("whole");
	tt->startTask("load");
   	DataLoader dl(inFile, numprocs);
   	dl.loadAndSendData();
	tt->endTask("load",1);
	DataCollector dc(outFile, numprocs,dl.getBufferSize());
	dc.collectData();
	tt->endTask("whole",1);
}	

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
	for(int i=0;i<log2(numprocs - 1);i++)
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

int OemSorterWorker::sort()
{
	MPI::Init();
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
