#include<iostream>
#include "Tools/Utils.h"
#include "Sorters/OemSorterWorker.h"
#include "Sorters/ShellSorterWorker.h"
#include "Sorters/ParalBSorter.h"
#include "Sorters/BSorter.h"
#include "Tools/FDataLoader.h"
using namespace std; 
using namespace tools; 
using namespace sorting;

bool bitonic = false;
bool oem = false;
bool shell = false;
bool oemlocal = false;
bool bitoniclocal = false;
string inputFile;
string outputFile;


void showUsage()
{
	cout<<"Invalid entry"<<endl;
	cout<<"psort inputFile outputFile -b -oem -s -oeml"<<endl;
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
	}
	return bitonic||oem||shell||oemlocal||bitoniclocal;
}

int main(int argc, char* args[])
{
	if(checkInput(args, argc))
	{
		if(oem)
		{
			 OemSorterWorker* osw = new OemSorterWorker(inputFile, outputFile);
			 osw->sort(); 
		}
		if(shell)
		{
			ShellSorterWorker* shsw = new ShellSorterWorker(inputFile, outputFile);
			shsw->sort();
		}	
		if(bitonic)
		{
			ParalBSorter* pbs = new ParalBSorter(inputFile, outputFile);
			pbs->sort();
		}
		if(oemlocal)
		{
			FDataLoader* fdl = new FDataLoader(inputFile); 
			OemSorter* os = new OemSorter();
			int* buffer = fdl->getBuffer();
			int bufferSize = fdl->getBufferSize();
			os->sort(buffer, bufferSize);
			for(int i=0;i<bufferSize;i++)
				cout<<buffer[i]<<" ";
			cout<<endl;
		}	
		if(bitoniclocal)
		{
			FDataLoader* fdl = new FDataLoader(inputFile); 
			BSorter* os = new BSorter();
			int* buffer = fdl->getBuffer();
			int bufferSize = fdl->getBufferSize();
			os->sort(buffer, bufferSize);
			for(int i=0;i<bufferSize;i++)
				cout<<buffer[i]<<" ";
			cout<<endl;
		}	
	}
	else
	{
		showUsage();
		return 1;
	}
	return 0;
}

