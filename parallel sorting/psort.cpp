#include<iostream>
#include "Tools/Utils.h"
#include "Sorters/OemSorterWorker.h"
using namespace std; 
using namespace tools; 
using namespace sorting;

bool bitonic = false;
bool oem = false;
bool shell = false;
string inputFile;
string outputFile;


void showUsage()
{
	cout<<"Invalid entry"<<endl;
	cout<<"psort inputFile outputFile -b -oem -s"<<endl;
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

bool checkInput(string args[], int argc)
{
	if(argc < 4)
		return false;
	inputFile = args[1];
	outputFile = args[2];
	for(int i=3;i<argc;i++)
	{
		if(args[i].compare("-b"))
			bitonic = true;
		else if(args[i].compare("-oem"))
			oem = true;
		else if(args[i].compare("-s"))
			shell = true;
	}
	return bitonic||oem||shell;
}

int main(int argc, string args[])
{
	if(checkInput(args, argc))
	{
		if(oem)
		{
			 cout<<"Sortowanie algorytmem oem rozpoczete"<<endl;
			 OemSorterWorker* osw = new OemSorterWorker(inputFile, outputFile);
			 osw->sort(); 
			 cout<<"Sortowanie zakonczone"<<endl;
		}
		if(shell)
		{
			
		}	
		if(bitonic)
		{
			
		}	
	}
	else
	{
		showUsage();
		return 1;
	}
	return 0;
}

