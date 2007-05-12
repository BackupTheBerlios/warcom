#include "ShellSorter.h"

namespace sorting
{

/*ShellSorter::ShellSorter()
{
}*/

ShellSorter::ShellSorter(int myId, int allSortersCount)
{
	this->myId = myId;
	this->allSortersCount = allSortersCount;
	this->stages = (int)log2(allSortersCount);
}

int* ShellSorter::sort(int a[], int size)
{
	this->a = a;
	this->size = size;
	
	if(a == NULL)
	{
		cout<<"ShellSorter #"<<myId<<": data table is NULL!"<<endl;
		return NULL;
	}
	
	//first phase
	for(int stage=1; stage<=stages; stage++)
	{
		int otherPId = getPIDForCompSplit(myId-1, allSortersCount, stage)+1;
		//int otherPId = getPIDForCompSplit(myId, allSortersCount, stage);
		//cout<<"Process #"<<myId<<" before compareSplit with #"<<otherPId<<" at stage "<<stage<<endl;
		//display();
		//compareSplit(otherPId);
		compareSplit(otherPId, myId, a, size);
		//cout<<"Process #"<<myId<<" after compareSplit at stage "<<stage<<endl;
		//display();
	}
	
	//second phase - odd-even
	//cout<<"odd-even phase started"<<endl;
	//while(!done)
	for(int i=1; i<=allSortersCount; i++)
	{
		//cout<<"stage "<<i<<endl;
		if(i%2 ==1)
		{
			if(myId%2 == 1 && myId+1 < allSortersCount+1) // gdy wszystkie sortują <allSortersCount
			{
				//cout<<"Process #"<<myId<<" before odd with #"<<myId+1<<" at stage #"<<i<<endl;
				compareSplit(myId+1, myId, a, size);
			}
			else if (myId-1 > 0 && myId!=allSortersCount) // gdy wszystkie sortują >=0
			{
				//cout<<"Process #"<<myId<<" before odd with #"<<myId-1<<" at stage #"<<i<<endl;
				compareSplit(myId-1, myId, a, size);
			}
		}
		else
		{
			if(myId%2 == 0 && myId+1 < allSortersCount+1) // gdy wszystkie sortują <allSortersCount
			{
				//cout<<"Process #"<<myId<<" before even with #"<<myId+1<<" at stage #"<<i<<endl;
				compareSplit(myId+1, myId, a, size);
			}
			else if (myId-1 > 0) // gdy wszystkie sortują >=0
			{
				//cout<<"Process #"<<myId<<" before even with #"<<myId-1<<" at stage #"<<i<<endl;
				compareSplit(myId-1, myId, a, size);
			}
		}
		
		//MPI_Barrier(MPI_COMM_WORLD);
	}
	
	cout<<"Process #"<<myId<<" completed sorting. "<<endl;
	display();
	return a;
}

void ShellSorter::display()
{
	cout<<"Process #"<<myId<<" buffer content:"<<endl;
	for(int i = 0; i < size; ++i)
	{
		std::cout<<a[i];
		std::cout<<" ";
	}
	std::cout<<"\n";
}

//int ShellSorter::getPIDForCompSplit(int stage)
//{
//	int stageTabSize = allSortersCount/stage;
//	int tabs = allSortersCount/(int)pow(2, stage - 1);
//	
//	int temp = stageTabSize, i;
//
//	for(i=0; i<=tabs; i++)
//	{
//		if(myId >= temp)
//			temp+=stageTabSize;
//		else
//			break;
//	}
//	
//	int tabStart = temp - stageTabSize;
//	int myIdPosInStageTab = myId - tabStart;
//	
//	return stageTabSize - myIdPosInStageTab + tabStart - 1;
//}

// używać tylko gdy wszystkie procesy sortują
//int ShellSorter::compareSplit(int otherPId)
//{	
//	MPI_Request request;
//	MPI_Status status;
//	
//	bool getGreaterPart = (myId > otherPId) ? true : false;
//	int* buffer2 = new int[size*2];
//	
//	LocalQSorter* lqs = new LocalQSorter(buffer2,size*2);
//	
//	MPI_Isend(a, size, MPI_INT, otherPId,
//		COMPARE_SPLIT + myId, MPI_COMM_WORLD, &request );
//	
//	MPI_Recv(buffer2, size, MPI_INT, otherPId,
//		COMPARE_SPLIT + otherPId, MPI_COMM_WORLD, &status);	
//
//	for(int i = 0; i<size; ++i)
//		buffer2[i+size] = a[i];
//	
//	lqs->localSort(true);
//	if(getGreaterPart)
//	    for(int i = 0; i<size; ++i)
//		    a[i] = buffer2[i+size];
//	else
//		for(int i = 0; i<size; ++i)
//			a[i] = buffer2[i];
//	
//	/*cout<<"compsplit proces "<<myId<<": ";	
//	for(int i = 0; i<size*2; ++i)
//		cout<< buffer2[i]<<" ";
//	cout<<endl;	*/		
//	if(buffer2 != NULL)
//			delete(buffer2);
//	return 0;
//}

int ShellSorter::compareSplit(int otherPId, int myId, int* buffer, int bufferSize)
{	
	MPI_Request request;
	MPI_Status status;
	
	bool getGreaterPart = (myId > otherPId) ? true : false;
	int* buffer2 = new int[bufferSize*2];
	
	LocalQSorter* lqs = new LocalQSorter(buffer2,bufferSize*2);
	
	MPI_Isend( buffer, bufferSize, MPI_INT, otherPId,
		COMPARE_SPLIT + myId, MPI_COMM_WORLD, &request );
	
	MPI_Recv(buffer2, bufferSize, MPI_INT, otherPId,
		COMPARE_SPLIT + otherPId, MPI_COMM_WORLD, &status);	

	for(int i = 0; i<bufferSize; ++i)
		buffer2[i+bufferSize] = buffer[i];
	
	lqs->localSort(true);
	if(getGreaterPart)
	    for(int i = 0; i<bufferSize; ++i)
		    buffer[i] = buffer2[i+bufferSize];
	else
		for(int i = 0; i<bufferSize; ++i)
			buffer[i] = buffer2[i];
		
	if(buffer2 != NULL)
			delete(buffer2);
	return 0;
}

//int ShellSorter::getPIDForCompSplit(int myId, int sortPcsCount, int stage)
//{
//	int stageTabSize = sortPcsCount/stage;
//	//int tabs = sortPcsCount/(int)pow(2, stage - 1);
//	int tabs = (int)pow(2, stage - 1);
//	
//	int temp = stageTabSize, i;
//
//	for(i=0; i<=tabs; i++)
//	{
//		if(myId >= temp)
//			temp+=stageTabSize;
//		else
//			break;
//	}
//	
//	int tabStart = temp - stageTabSize;
//	int myIdPosInStageTab = myId - tabStart;
//	
//	return stageTabSize - myIdPosInStageTab + tabStart - 1;
//}

int ShellSorter::getPIDForCompSplit(int myPId, int sortPcsCount, int stage)
{
	int stageTabSize = sortPcsCount/stage;
	int tabs = (int)pow(2, stage - 1);
	
	int temp = stageTabSize, i;

	for(i=0; i<=tabs; i++)
	{
		if(myPId >= temp)
			temp+=stageTabSize;
		else
			break;
	}
	
	int tabStart = temp - stageTabSize;
	int myIdPosInStageTab = myPId - tabStart;
	
	return stageTabSize - myIdPosInStageTab + tabStart - 1;
}

}
