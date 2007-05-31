#include<iostream>
#include "../Tools/myio.h"

using namespace std;
using namespace tools;

string outputFile;
int numbers;
int maxNumber = 100;
int onlyDisplay = 0;

void showUsage()
{
	cout<<"Correct usage:"<<endl;
	cout<<"fgen outputFile numberCount [numberMax (default=100)]"<<endl;
}

/*
 * Checks main program input parameters.
 */ 
bool checkInput(int argc, char* argv[])
{	
	if(argc < 2)
		return false;

	outputFile = argv[1];
	if(argc == 2)
	{
		onlyDisplay = 1;
		return true;
	}
	numbers = atoi(argv[2]);
	if(argc == 4)
	{
		int tmn = atoi(argv[3]);
		if(tmn > 0)
		maxNumber = tmn;
	}
	return true;
}

/*
 * Displays specified by filepath file's content.
 */ 
int displayFileContent(string file)
{
	int fd = MyIO::my_open(file.c_str(), O_RDONLY);
	if(fd == -1)
		return 1;
	
	int n;
	MyIO::my_read(fd, &n, sizeof(int), 0, SEEK_CUR);
	int *buffer = new int[n];
	MyIO::my_read(fd, buffer, sizeof(int) * n, 0, SEEK_CUR);
	
	for(int i = 0; i < n; i++)
		cout<<buffer[i]<<" ";
	cout<<endl;
	
	MyIO::my_close(fd);
	return 0;
}

/*
 * Generates random numbers not greater than maxNumber integers.
 */ 
int generateNumbers(int numbers, int maxNumber, string file)
{
	int fd, random;
	srand((unsigned)time(NULL));
	
	fd = MyIO::my_open(file.c_str(), O_CREAT);
	MyIO::my_close(fd);
	fd = MyIO::my_open(file.c_str(), O_WRONLY);
	
	if(fd == -1)
		return 1;	

	MyIO::my_write(fd, &numbers, sizeof(int), 0, SEEK_CUR);
	for(int i=0; i<numbers; i++)
	{
		random=rand()%(maxNumber+1);
		MyIO::my_write(fd, &random, sizeof(int), 0, SEEK_CUR);
	}
	
	MyIO::my_close(fd);
	cout<<"File "<< file << " with "<<numbers<<" integers (max = "<<maxNumber<<") to sort generated."<< endl;
	return 0;
}

int main(int argc, char* argv[])
{
	if(checkInput(argc, argv))
	{
		if(onlyDisplay)
			return displayFileContent(outputFile);
		else
			return generateNumbers(numbers, maxNumber, outputFile);
	}
	else
	{
		showUsage();
		return 1;
	}
	return 0;
}

