#ifndef DATALOADER_H_
#define DATALOADER_H_

#include<iostream>
#include<fstream>
#include<string>
#include<mpi.h>
#include "myio.h"
#include "Utils.h"
using namespace std;

#define BUFFER_SIZE_TAG 142
#define WORK_TAG 143

namespace tools
{
	class DataLoader
	{
		private:
			string fileName;
			int fileh;
			bool extended;
			
			int bufferSize;
			//int* buffer;
			int** buffer;
			int pcsCount;
			
			MPI_Request* request;
		
			int* loadData(int bufferNo);
			
		public:
			DataLoader(string, int, bool);
			~DataLoader();

			int loadAndSendData();
			int* loadPrimeProcessData();
			int getBufferSize() { return bufferSize;};
	};
}

#endif /*DATALOADER_H_*/
