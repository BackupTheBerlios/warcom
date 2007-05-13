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
	MPI_Request request;
	MPI_Status status; 
	OemSorter* sorter = new OemSorter();
	MPI_Isend( buffer, bufSize, MPI_INT, idProcess,
		WORK_TAG+50, MPI_COMM_WORLD, &request );
	MPI_Recv(buffer2, bufSize, MPI_INT, idProcess,
		WORK_TAG+50, MPI_COMM_WORLD, &status);	
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

int OemSorterWorker::sort()
{
	Status status; 
   	MPI::Init();
   	int myrank = COMM_WORLD.Get_rank(); 
   	int numprocs = COMM_WORLD.Get_size(); 
   	if(myrank == 0)
   	{
   		DataLoader dl(inFile, numprocs);
		dl.loadAndSendData();
		MPI_Barrier(COMM_WORLD);
   	}
   	else
   	{
   		OemSorter* oem = new OemSorter();
   		MPI_Status status;
   		MPI_Request request;
   		int bufSize;
   		int* buffer;
   		MPI_Recv(&bufSize, 1, MPI_INT, 0, BUFFER_SIZE_TAG, MPI_COMM_WORLD, &status);
   		buffer = new int[bufSize];
		MPI_Recv(buffer, bufSize, MPI_INT, 0, WORK_TAG, MPI_COMM_WORLD, &status);
		oem->sort(buffer, bufSize);
		for(int i=0;i<log2(numprocs - 1);i++)
			for(int j=0;j<=i;j++)
					if(canTransferInThisStep(myrank, i, j))
					{
						int partner = findPartner(myrank, i, j);
						compareSplit(partner, myrank, buffer, bufSize);
					}
		MPI_Barrier(COMM_WORLD);
		cout<<"Moj bufor "<<myrank<<endl;
		for(int i=0; i<bufSize; i++)
			cout<<buffer[i]<<" ";
		cout<<endl;
		if(buffer != NULL)
			delete(buffer);
   	}
   	MPI::Finalize();
   	return 0;
}
}
