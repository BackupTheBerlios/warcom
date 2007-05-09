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
	}
	return bitonic||oem||shell;
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

