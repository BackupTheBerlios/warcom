#include "DataLoader.h"

namespace tools
{
	DataLoader::DataLoader(string fileName, int pcsCount)
	{
		this->fileName = fileName;
		//cout<<"File to perform: "<<this->fileName << endl;
		this->pcsCount = pcsCount;
		
		request = new MPI_Request[pcsCount];
		
		fileh = MyIO::my_open(this->fileName.c_str(), O_RDONLY);
		if(fileh == -1)
			cout<<"File error"<<endl;
			
		int setSize = 0;
		MyIO::my_read(fileh, &setSize,  sizeof(int),  0,  SEEK_CUR);
		cout<<"setSize: "<<setSize<<endl;
		
		//this->bufferSize = (setSize%(pcsCount-1) != 0) ? setSize/pcsCount : setSize/(pcsCount-1);
		this->bufferSize = (setSize%(pcsCount-1) != 0) ? setSize/(pcsCount-1) + 1 : setSize/(pcsCount-1);
		cout<<"bufferSize: "<<this->bufferSize<<endl;
		
		buffer = new int*[bufferSize];
		for(int i=0; i< pcsCount; i++)
			buffer[i] = new int[bufferSize];
	//	cout<<"Initialization for data loading done."<<endl<<"-------------"<<endl;
	}
	
	DataLoader::~DataLoader()
	{
		MPI_Status s;
		for(int i=1; i<pcsCount; i++)
			MPI_Wait(&request[i], &s);
		
		for(int i=0; i<pcsCount; i++)
			if(buffer[i] != NULL)
				delete(buffer[i]);
				
		if(buffer != NULL)
			delete(buffer);
			
		if(request != NULL)
			delete(request);
			
		if(fileh != -1)
			MyIO::my_close(fileh);
	}
	
	int* DataLoader::loadData(int bufferNo)
	{
		if(fileh == -1 || bufferNo < 0 || bufferNo > pcsCount)
			return NULL;
			
		if(buffer == NULL)
			return NULL;
		
		if(buffer[bufferNo] == NULL)
		 buffer[bufferNo] = new int[bufferSize];
		
		int res = MyIO::my_read(fileh, buffer[bufferNo], bufferSize*sizeof(int), 0, SEEK_CUR);
		if (res == -1)
			Utils::exitWithError();
		if(res < bufferSize*sizeof(int))
		{
			int sentinelsCount = (bufferSize*sizeof(int) - res)/sizeof(int);
			for(int i = bufferSize - sentinelsCount; i<bufferSize; i++)
				buffer[bufferNo][i] = -1; 
		}
		
		for(int j=0; j< bufferSize; j++)
			cout<<buffer[bufferNo][j]<<" ";
		cout<<endl; 
		
		return buffer[bufferNo];
	}
	
	int* DataLoader::loadPrimeProcessData()
	{		
		if(buffer[0] != NULL)
			for(int i=0; i<bufferSize; i++)
				buffer[0] = 0;
		return loadData(0);
	}
	
	void DataLoader::saveData(int* array, int size, string outputFile)
	{
	}

	int DataLoader::loadAndSendData()
	{
		int* buff;
		
		//cout<<"Sending bufferSize"<<endl;
//		for(int i=0; i< pcsCount; i++)
//			MPI_Send( &this->bufferSize, 1, MPI_INT, i, BUFFER_SIZE_TAG, MPI_COMM_WORLD);
		for(int i=0; i< pcsCount; i++)
			if(Utils::mpi_send( &this->bufferSize, 1, i, BUFFER_SIZE_TAG)!=0)
				Utils::exitWithError();
			
		
		for(int i=pcsCount-1; i> 0; i--)
		{
			buff = loadData(i);
			if(buff != NULL)
			{
				//cout<<"Sending buffer to "<<i<<endl;
				/*cout<<". Buffer content: ";
				for(int j=0; j<bufferSize; j++)
					cout<<buff[j]<<" ";
				cout<<endl;*/
				MPI_Isend( buff, bufferSize, MPI_INT, i, WORK_TAG, MPI_COMM_WORLD, &request[i] );
			}
		}
		
		return 0;
	}

}
