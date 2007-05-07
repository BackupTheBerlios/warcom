#ifndef OEMSORTERWORKER_H_
#define OEMSORTERWORKER_H_

namespace sorting
{

class OemSorterWorker
{
public:
	OemSorterWorker(string inFile, string outFile);
	void sort();
	
private:
	string inFile;
	string outFile; 
};
}
#endif /*OEMSORTERWORKER_H_*/
