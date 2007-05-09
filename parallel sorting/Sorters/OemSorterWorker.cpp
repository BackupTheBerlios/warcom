#include "OemSorterWorker.h"

namespace sorting
{

OemSorterWorker::OemSorterWorker(string inFile, string outFile)
{
	this->inFile = inFile;
	this->outFile = outFile;
}

void OemSorterWorker::sort()
{
	Status status; 
   	MPI::Init();
   	int myrank = COMM_WORLD.Get_rank(); 
   	int numprocs = COMM_WORLD.Get_size(); 
   	if(myrank == 0)
   	{
   		DataLoader dl(inFile, numprocs);
		dl.loadAndSendData();
		
			//TODO potem chyba normalny udział w sortowaniu?
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
		cout<<"Process #"<<myrank<<": My buffer: ";
		oem->sort(buffer, bufSize);
		for(int i=0; i<bufSize; i++)
			cout<<buffer[i]<<" ";
		cout<<endl;
		if(buffer != NULL)
			delete(buffer);
   	}
   	MPI::Finalize();
}
}
