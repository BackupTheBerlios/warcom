#include "FDataLoader.h"

namespace tools
{
	/*
	 * Constructor
	 * fileName - file with data
	 */
	FDataLoader::FDataLoader(string fileName)
	{
		this->fileName = fileName;
		
		fileh = MyIO::my_open(this->fileName.c_str(), O_RDONLY);
		if(fileh == -1)
			cout<<"File error"<<endl;
			
		MyIO::my_read(fileh, &bufferSize,  sizeof(int),  0,  SEEK_CUR);
				
		buffer = new int[bufferSize];
		if(buffer != NULL)
		{
			MyIO::my_read(fileh, buffer, bufferSize*sizeof(int), 0, SEEK_CUR);
		}
		
		if(fileh != -1)
			MyIO::my_close(fileh);
	}
	
	/*
	 * Destructor.
	 */
	FDataLoader::~FDataLoader()
	{				
		if(buffer != NULL)
			delete(buffer);
	}
}
