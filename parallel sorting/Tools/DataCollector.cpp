#include "DataCollector.h"

namespace tools
{

/*
 * Constructor
 * fileName - file where to save all data
 * pcsCount - number of all process
 * bufferSize - size of buffer for a single process
 */
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

/*
 * Removes guards from given array - guards help to secure the same size of all buffers
 * buffer - array in which we remove guards
 * size - size of the array
 */
int DataCollector::removeGuards(int *buffer, int size)
{
	int newSize = size;
	for(int i = 0; i < size; i++)
		if(buffer[i] == -1)
			newSize--;
	return size - newSize;
}

/*
 * Collects data from all processes and save it to the file
 * numProcs - number of processes which send data to the collector
 * buffer2 - array with data from the process who calls CollectData - can be null
 */
void DataCollector::commonAction(int numProcs, int* buffer2)
{
	int* buffer = new int[this->bufferSize];
	int start =(int)sizeof(int), firstTask = 1, globalVal = 0, move = 1;
	MPI_Request request;
	MPI_Status status; 
	TaskTimer *tt = new TaskTimer();
	if(buffer2 != NULL)	{
		move = 0;
		globalVal = removeGuards(buffer2, bufferSize);
		MyIO::my_write(fd, buffer2 + globalVal , 
				(bufferSize - globalVal) * sizeof(int), start, SEEK_SET);	
	}
	for(int i=1;i<pcsCount;i++)	{
			MPI_Recv(buffer, bufferSize, MPI_INT, i, END_TAG, MPI_COMM_WORLD, &status);
			int val = removeGuards(buffer, bufferSize);
			if(firstTask) {
				firstTask = 0;
				tt->startTask("collect");
			}
			globalVal += val;
			int shift = (( i - move) * bufferSize - globalVal) * sizeof(int) + start;
			if(shift < start)
				shift = start;
			MyIO::my_write(fd, buffer + val , 
				(bufferSize - val) * sizeof(int), shift, SEEK_SET);	
	}
	int size = numProcs * bufferSize - globalVal;
	MyIO::my_write(fd, &size, sizeof(int), 0, SEEK_SET);
	tt->endTask("collect",1);
	MyIO::my_close(fd);
}

/*
 * Collects data from processes, used when supervisor process doesn't sort data
 */
void DataCollector::collectData()
{
	commonAction(pcsCount - 1, NULL);
}

/*
 * Collects data from processes, used when supervisor process also sorts data
 */
void DataCollector::collectData(int *buffer)
{
	commonAction(pcsCount, buffer);
}

}
