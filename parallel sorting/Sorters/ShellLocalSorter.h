#ifndef SHELLLOCALSORTER_H_
#define SHELLLOCALSORTER_H_

#include "../Tools/Utils.h"
using namespace std;
using namespace tools;

namespace sorting
{

class ShellLocalSorter
{
private:
	void shellSortPhase(int a[], int length, int gap);
	
public:
	ShellLocalSorter();
	int* sort(int a[], int size);
};

}
#endif /*SHELLLOCALSORTER_H_*/
