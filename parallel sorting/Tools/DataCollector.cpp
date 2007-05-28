#include "DataCollector.h"

namespace tools
{

DataCollector::DataCollector(string fileName, int pcsCount, int bufferSize)
{
	this->fileName = fileName;
	this->pcsCount = pcsCount;
	this->bufferSize = bufferSize;
	fd = MyIO::my_open(fileName.c_str(),O_CREAT | O_TRUNC | O_WRONLY );	
}

DataCollector::~DataCollector()
{
}

int DataCollector::removeGuars(int *buffer, int size)
{
	int newSize = size;
	for(int i = 0; i < size; i++)
		if(buffer[i] == -1)
			newSize--;
	return size - newSize;
}

void DataCollector::collectData()
{
	int* buffer = new int[this->bufferSize];
	int start =(int)sizeof(int);
	MPI_Request request;
	MPI_Status status; 
	int firstTask = 1;
	int globalVal = 0;
	TaskTimer *tt = new TaskTimer();
	for(int i=1;i<pcsCount;i++)
	{
			MPI_Recv(buffer, bufferSize, MPI_INT, i, END_TAG, MPI_COMM_WORLD, &status);
			int val = removeGuars(buffer, bufferSize);
			if(firstTask)
			{
				firstTask = 0;
				tt->startTask("collect");
			}
			globalVal += val;
			int shift = (( i - 1 ) * (bufferSize) - globalVal) * sizeof(int) + start;
			if(shift < start)
				shift = start;
			MyIO::my_write(fd, buffer + val , 
				(bufferSize - val) * sizeof(int), shift, SEEK_SET);	
	}
	int size = (pcsCount - 1) * bufferSize - globalVal;
	MyIO::my_write(fd, &size, sizeof(int), 0, SEEK_SET);
	tt->endTask("collect",1);
	MyIO::my_close(fd);
}

}
