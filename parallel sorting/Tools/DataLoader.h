#ifndef DATALOADER_H_
#define DATALOADER_H_

#include<iostream>
#include<fstream>
#include<string>
#include<mpi.h>
#include "myio.h"
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
			
			int bufferSize;
			int* buffer;
			int pcsCount;
		
		public:
			DataLoader(string, int);
			~DataLoader();
			int* loadData();
			void saveData(int*, int size, string);
			int loadAndSendData();
	};
}

#endif /*DATALOADER_H_*/
