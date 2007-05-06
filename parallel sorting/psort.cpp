#include<iostream>
#include "Tools/DataLoader.h"
#include "Tools/Utils.h"
#include "Sorters/Sorter.h"
#include "Sorters/OemSorter.h"
using namespace std; 
using namespace tools; 
using namespace sorting;

bool bitonic = false;
bool oem = false;
bool shell = false;
string inputFile;
string outputFile;

void testOem();

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

int main(string args[], int argc)
{
	testOem();
	if(checkInput(args, argc))
	{
		//TODO jak bedzie wiecej kodu to napisze cos co na podstawie
		//wejscia wybierze odpowiednie sortowania i wywola wszystkie funkcje		
	}
	else
	{
		showUsage();
		return 1;
	}
	return 0;
}

void testOem()
{
	int test[] = { 4, 1, 7, 2, 4, 9, 3};
	OemSorter* oem = new OemSorter();
	oem->sort(test, 7);
	oem->display();
	
}
