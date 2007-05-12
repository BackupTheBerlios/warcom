#include<iostream>
#include "myio.h"

using namespace std;
using namespace tools;

string outputFile;
int numbers;
int minNumber = 0;
int maxNumber = 100;

void showUsage()
{
	cout<<"Invalid entry"<<endl;
	cout<<"fgen outputFile numberCount [numberMax]"<<endl;
}


bool checkInput(int argc, char* argv[])
{	
	if(argc < 2 || argc > 4)
	{
		showUsage();
		return false;
	}

	outputFile = argv[1];
	numbers = atoi(argv[2]);
	if(argc == 3)
	{
		int tmn = atoi(argv[3]);
		if(tmn > 0)
		maxNumber = tmn;
	}
	return true;
}

int main(int argc, char* argv[])
{
	int random;
	int fd;
	if(checkInput(argc, argv))
	{
		srand((unsigned)time(NULL));

		fd = MyIO::my_open(outputFile.c_str(), O_CREAT);
		MyIO::my_close(fd);
		fd = MyIO::my_open(outputFile.c_str(), O_WRONLY);

		if(fd == -1)
			return 1;

		MyIO::my_write(fd, &numbers, sizeof(int), 0, SEEK_CUR);
		for(int i=0; i<numbers; i++)
		{
			random=rand()%maxNumber+minNumber;
			MyIO::my_write(fd, &random, sizeof(int), 0, SEEK_CUR);
		}
		MyIO::my_close(fd);
		cout<<"File "<< outputFile << " with "<<numbers<<" integers to sort generated."<< endl;
	}
	else
	{
		showUsage();
		return 1;
	}
	return 0;
}

