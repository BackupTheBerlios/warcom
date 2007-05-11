#include "ShellSorter.h"

namespace sorting
{

ShellSorter::ShellSorter()
{
}

int* ShellSorter::sort(int a[], int size)
{
	this->a = a;
	this->size = size;
	//ShellSort(0, getValue(size));
	return a;
}

int ShellSorter::getValue(int value)
{
	int i=1;
	while(i < value)
		i *= 2;
	return i;
}

void ShellSorter::display()
{
	for(int i = 0; i < size; ++i)
	{
		std::cout<<a[i];
		std::cout<<" ";
	}
	std::cout<<"\n";
}


}
