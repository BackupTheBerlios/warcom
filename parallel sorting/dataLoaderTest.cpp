#include<iostream>
#include "mpi.h"
#include "Tools/DataLoader.h"

using namespace std; 
using namespace tools;

string inputFile;
string outputFile;

void showUsage()
{
	cout<<"Invalid entry"<<endl;
	cout<<"dlt inputFile"<<endl;
}


bool checkInput(int argc, char* argv[])
{
	/*cout<<"argc = " << argc << endl;
	for(int i=0; i< argc; i++)
		cout<<"arg["<<i<<"]: "<<argv[i] << endl;*/
		
	if(argc < 2)
		return false;

	inputFile = argv[1];
	//outputFile = argv[2];
	return true;
}

int main(int argc, char* argv[])
{
	if(checkInput(argc, argv))
	{
		MPI::Status status; 
 
    	MPI::Init();
    	int myrank = MPI::COMM_WORLD.Get_rank(); 
    	int numprocs = MPI::COMM_WORLD.Get_size(); 
    	//cout<<"numprocs: " << numprocs << " myrank: " << myrank << endl;
    	
    	if(myrank == 0)
    	{
    		cout<<"There are "<< numprocs <<" procs" << endl<<endl;
			DataLoader dl(inputFile, numprocs);
			dl.loadAndSendData();
			
			//TODO potem chyba normalny udział w sortowaniu?
    	}
    	else
    	{
    		MPI_Status status;
    		MPI_Request request;
    		int bufSize;
    		int* buffer;
    		MPI_Recv(&bufSize, 1, MPI_INT, 0, BUFFER_SIZE_TAG, MPI_COMM_WORLD, &status);
    		cout<<"Process #"<<myrank<<": I received bufferSize="<<bufSize<<endl;
    		buffer = new int[bufSize];
			MPI_Recv(buffer, bufSize, MPI_INT, 0, WORK_TAG, MPI_COMM_WORLD, &status);
			cout<<"Process #"<<myrank<<": My buffer: ";
			for(int i=0; i<bufSize; i++)
				cout<<buffer[i]<<" ";
			cout<<endl;
			if(buffer != NULL)
				delete(buffer);
    	}
    	
    	MPI::Finalize();
	}
	else
	{
		showUsage();
		return 1;
	}
	return 0;
}

