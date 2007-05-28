#include<iostream>
#include "Tools/Utils.h"
#include "Sorters/OemSorterWorker.h"
#include "Sorters/ShellSorterWorker.h"
#include "Sorters/ParalBSorter.h"
#include "Sorters/BSorter.h"
#include "Sorters/BSorterWorker.h"
#include "Sorters/ShellLocalSorter.h"
#include "Tools/FDataLoader.h"
#include "Tools/TaskTimer.h"
using namespace std; 
using namespace tools; 
using namespace sorting;

bool bitonic = false;
bool oem = false;
bool shell = false;
bool oemlocal = false;
bool bitoniclocal = false;
bool shelllocal = false;
string inputFile;
string outputFile;


void showUsage()
{
	cout<<"Invalid entry"<<endl;
	cout<<"psort inputFile outputFile -b -oem -s -bl -oeml -sl"<<endl;
}

void showAuthors()
{
	cout<<"Programowanie rownolegle i rozproszone 2"<<endl;
	cout<<"Sortowania rownolegle:"<<endl;
	cout<<"Bitonic sort"<<endl;
	cout<<"Odd-Even mergesort"<<endl;
	cout<<"Shell sort"<<endl;
	cout<<endl<<"Autorzy"<<endl;
	cout<<"Urszula Florianczyk"<<endl;
	cout<<"Marcin Nowinski"<<endl;
	cout<<"Piotr Olejnik"<<endl;
}

void localOem()
{
	TaskTimer* tt = new TaskTimer();
	tt->startTask("load");
	FDataLoader* fdl = new FDataLoader(inputFile); 
	int* buffer = fdl->getBuffer();
	int bufferSize = fdl->getBufferSize();
	tt->endTask("load", 1);
	tt->startTask("sort");
	OemSorter* os = new OemSorter();
	os->sort(buffer, bufferSize);
	tt->endTask("sort", 1);
	tt->startTask("save");
	int fd = MyIO::my_open(outputFile.c_str(),O_CREAT | O_TRUNC | O_WRONLY );
	if(fd != -1)
	{
		MyIO::my_write(fd, &bufferSize, sizeof(int), 0, SEEK_CUR);
		MyIO::my_write(fd, buffer, bufferSize * sizeof(int), 
			sizeof(int), SEEK_SET);
		MyIO::my_close(fd);
	}
	tt->endTask("save",1);
}

void localBitonic()
{
	TaskTimer* tt = new TaskTimer();
	tt->startTask("load");
	FDataLoader* fdl = new FDataLoader(inputFile); 
	int* buffer = fdl->getBuffer();
	int bufferSize = fdl->getBufferSize();
	tt->endTask("load", 1);
	tt->startTask("sort");			
	BSorter* bs = new BSorter();
	bs->sort(buffer, bufferSize);
	tt->endTask("sort", 1);
	tt->startTask("save");
	
	int fd = MyIO::my_open(outputFile.c_str(),O_CREAT | O_TRUNC | O_WRONLY );
        if(fd != -1)
        {
                MyIO::my_write(fd, &bufferSize, sizeof(int), 0, SEEK_CUR);
                MyIO::my_write(fd, buffer, bufferSize * sizeof(int),
                        sizeof(int), SEEK_SET);
                MyIO::my_close(fd);
        }

	tt->endTask("save",1);
}

void localShell()
{
	TaskTimer* tt = new TaskTimer();
	tt->startTask("load");
	FDataLoader* fdl = new FDataLoader(inputFile); 
	int* buffer = fdl->getBuffer();
	int bufferSize = fdl->getBufferSize();
	tt->endTask("load", 1);
	tt->startTask("sort");
	ShellLocalSorter* shlocs = new ShellLocalSorter();
	shlocs->sort(buffer, bufferSize);
	tt->endTask("sort", 1);
	tt->startTask("save");
	int fd = MyIO::my_open(outputFile.c_str(),O_CREAT | O_TRUNC | O_WRONLY );
        if(fd != -1)
        {
                MyIO::my_write(fd, &bufferSize, sizeof(int), 0, SEEK_CUR);
                MyIO::my_write(fd, buffer, bufferSize * sizeof(int),
                        sizeof(int), SEEK_SET);
                MyIO::my_close(fd);
        }
	tt->endTask("save",1);
}

bool checkInput(char* args[], int argc)
{
	if(argc < 4)
		return false;
	inputFile = args[1];
	outputFile = args[2];
	for(int i=3;i<argc;i++)
	{
		string temp = args[i];
		if(!temp.compare("-b"))
			bitonic = true;
		else if(!temp.compare("-oem"))
			oem = true;
		else if(!temp.compare("-s"))
			shell = true;
		else if(!temp.compare("-oeml"))
			oemlocal = true;
		else if(!temp.compare("-bl"))
			bitoniclocal = true;
		else if(!temp.compare("-sl"))
			shelllocal = true;
	}
	return bitonic||oem||shell||oemlocal||bitoniclocal||shelllocal;
}

int main(int argc, char* args[])
{
	if(checkInput(args, argc))
	{
		if(oem)
		{
			 OemSorterWorker* osw = new OemSorterWorker(inputFile, outputFile, argc, args);
			 osw->sort(); 
		}
		if(shell)
		{
			ShellSorterWorker* shsw = new ShellSorterWorker(inputFile, outputFile, argc, args);
			shsw->sort();
		}	
		if(bitonic)
		{
			BSorterWorker* bsw = new BSorterWorker(inputFile, outputFile, argc, args);
			bsw->sort();
		}
		if(oemlocal)
			localOem();
		if(bitoniclocal)
			localBitonic();
		if(shelllocal)
			localShell();
	}
	else
	{
		showUsage();
		return 1;
	}
	return 0;
}

