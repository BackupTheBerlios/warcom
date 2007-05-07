#include "DataLoader.h"

namespace tools
{
	DataLoader::DataLoader(string fileName, int pcsCount)
	{
		this->fileName = fileName;
		cout<<"File to perform: "<<this->fileName << endl; 
		this->pcsCount = pcsCount;
		
		fileh = MyIO::my_open(this->fileName.c_str(), O_RDONLY);
		if(fileh == -1)
			cerr<<"File error"<<endl;
			
		int setSize = 0;
		MyIO::my_read(fileh, &setSize,  sizeof(int),  0,  SEEK_CUR);
		cout<<"setSize: "<<setSize<<endl;
		
		//TODO: to ustalenie wielkości bufora jest złe, oczywiście ;-)
		//moze zrobic tak, by I proces czytal size/pcsCount+size%pcsCount?
		//moze czytaja po rowno size/pcsCount a reszte zaniedbujemy?
		this->bufferSize = (setSize%pcsCount != 0) ? setSize/pcsCount+1 : setSize/pcsCount;
		cout<<"bufferSize: "<<this->bufferSize<<endl;
		
		buffer = new int[bufferSize];
		cout<<"Initialization for data loading done."<<endl<<"-------------"<<endl;
	}
	
	DataLoader::~DataLoader()
	{
		if(buffer != NULL)
			delete(buffer);
		if(fileh != -1)
			MyIO::my_close(fileh);
	}
	
	int* DataLoader::loadData()
	{
		if(fileh == -1)
			return NULL;
		
		MyIO::my_read(fileh, buffer, bufferSize*sizeof(int), 0, SEEK_CUR);
		/*for(int j=0; j< bufferSize; j++)
			cout<<buffer[j]<<" ";
		cout<<endl;*/ 
		
		//TODO obmyślic jak przydzielac pamiec bufora (nie mozna chyba do tego samego - czekanie zmarnuje asynchr) i co zwracac
		// moze dwa bufory, raz korzystac z jednego raz z drugiego?
		return buffer;
	}
	
	void DataLoader::saveData(int* array, int size, string outputFile)
	{
	}

	int DataLoader::loadAndSendData()
	{
		MPI_Status status[bufferSize];
		MPI_Request request[bufferSize];
		
		//TODO ustalić tagi
		//TODO czy przed rozsylaniem danych wyslac wszystkim rozmiar bloku?
		cout<<"Sending bufferSize"<<endl;
		for(int i=1; i< pcsCount; i++)
			MPI_Send( &this->bufferSize, 1, MPI_INT, i, BUFFER_SIZE_TAG, MPI_COMM_WORLD);
			
		
		//TODO dla i = 0 - przekaz dane do posortowania dla procesu głównego (na końcu?)
		for(int i=1; i< pcsCount; i++)
		{
			loadData();
			cout<<"Sending buffer to "<<i<<". Buffer content: ";
			for(int j=0; j<bufferSize; j++)
				cout<<buffer[j]<<" ";
			cout<<endl;
			MPI_Isend( buffer, bufferSize, MPI_INT, i, WORK_TAG, MPI_COMM_WORLD, &request[i] );
			//wyslij odpowiedniemu procesowi
		}
		//do kazdego senda jakis test?
		return 0;
	}

}
