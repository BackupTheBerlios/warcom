#ifndef FDATALOADER_H_
#define FDATALOADER_H_

#include<iostream>
#include<fstream>
#include<string>
#include "myio.h"
#include "Utils.h"
using namespace std;


namespace tools
{
	class FDataLoader
	{
		private:
			string fileName;
			int fileh;
			
			int bufferSize;
			int* buffer;
		
			
		public:
			FDataLoader(string);
			~FDataLoader();
			
			int getBufferSize() { return bufferSize;};
			int* getBuffer() { return buffer;};
	};
}

#endif /*FDATALOADER_H_*/
