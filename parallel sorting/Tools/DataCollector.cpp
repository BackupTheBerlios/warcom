#include "DataCollector.h"

namespace tools
{

DataCollector::DataCollector(string fileName, int pcsCount, int bufferSize)
{
	this->fileName = fileName;
	this->pcsCount = pcsCount;
	this->bufferSize = bufferSize;
	fd = MyIO::my_open(fileName.c_str(),O_CREAT | O_TRUNC | O_WRONLY );
	int size = (pcsCount - 1) * bufferSize;
	MyIO::my_write(fd, &size, sizeof(int), 0, SEEK_CUR);
	
	
}

DataCollector::~DataCollector()
{
	MyIO::my_close(fd);
}

void DataCollector::collectData()
{
	int* buffer = new int[this->bufferSize];
	int start =(int)sizeof(int);
	MPI_Request request;
	MPI_Status status; 
	for(int i=1;i<pcsCount;i++)
	{
			MPI_Recv(buffer, bufferSize, MPI_INT, i, END_TAG, MPI_COMM_WORLD, &status);
			MyIO::my_write(fd, buffer, bufferSize * sizeof(int), 
				( i - 1 ) * bufferSize * sizeof(int) + start, SEEK_SET);	
	}
}

}
