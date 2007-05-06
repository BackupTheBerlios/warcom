#ifndef DATALOADER_H_
#define DATALOADER_H_
#include<iostream>
#include<string>
using namespace std;

namespace tools
{

class DataLoader
{
private:
	string fileName;	

public:
	DataLoader(string);
	int* loadData();
	void saveData(int*, int size, string);
};

}

#endif /*DATALOADER_H_*/
