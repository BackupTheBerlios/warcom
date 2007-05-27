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
		//cout<<"Process #"<<myId<<" before compareSplit with #"<<otherPId<<" at stage "<<stage<<endl;
		compareSplit(otherPId, myId, a, size);
		//cout<<"Process #"<<myId<<" after compareSplit at stage "<<stage<<endl;;
	}
	
	//in worker - before managing stop condition
	//cout<<"Process #"<<myId<<" before 2nd phase barrier"<<endl;
	MPI_Barrier(MPI_COMM_WORLD);
	//cout<<"Process #"<<myId<<" after 2nd phase barrier"<<endl;
	
	//second phase - odd-even
	//cout<<"odd-even phase started"<<endl;
	//while(!done)
	for(int i=1; i<=allSortersCount; i++)
	{
		//cout<<"stage "<<i<<endl;
		bool changed = false;
		if(i%2 ==1)
		{
			if(myId%2 == 1 && myId+1 < allSortersCount+1) // gdy wszystkie sortują <allSortersCount
			{
				//cout<<"Process #"<<myId<<" before odd with #"<<myId+1<<" at stage #"<<i<<endl;
				//compareSplit(myId+1, myId, a, size);
				compareSplit(myId+1, myId, a, size, changed);
			}
			else if (myId-1 > 0 /*&& myId!=allSortersCount*/) // gdy wszystkie sortują >=0
			{
				//cout<<"Process #"<<myId<<" before odd with #"<<myId-1<<" at stage #"<<i<<endl;
				//compareSplit(myId-1, myId, a, size);
				compareSplit(myId-1, myId, a, size, changed);
			}
		}
		else
		{
			if(myId%2 == 0 && myId+1 < allSortersCount+1) // gdy wszystkie sortują <allSortersCount
			{
				//cout<<"Process #"<<myId<<" before even with #"<<myId+1<<" at stage #"<<i<<endl;
				//compareSplit(myId+1, myId, a, size);
				compareSplit(myId+1, myId, a, size, changed);
			}
			else if (myId-1 > 0 && myId!=allSortersCount) // gdy wszystkie sortują >=0
			{
				//cout<<"Process #"<<myId<<" before even with #"<<myId-1<<" at stage #"<<i<<endl;
				//compareSplit(myId-1, myId, a, size);
				compareSplit(myId-1, myId, a, size, changed);
			}
		}
		
		//cout<<"Process #"<<myId<<" localset changed = "<<changed<<endl;
		
		sendLocalSetChanged(myId, changed);
		int stopCond = receiveStopCondition(myId);
		if(stopCond == OE_SORTING_DONE)
		{
			MPI_Barrier(MPI_COMM_WORLD);
			break;
			//done = true;
		}
		else if(stopCond != OE_SORTING_UNDONE)
			cout<<"Stop condition receiving error in pcs #"<<myId<<"!!!"<<endl;
		MPI_Barrier(MPI_COMM_WORLD);
	}
	
	//cout<<"Process #"<<myId<<" completed sorting. "<<endl;
	//display();
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

void ShellSorter::sendLocalSetChanged(int myId, bool localSetChanged)
{
	int setChanged = OE_NO_CHANGES;
	if(localSetChanged)
	{
		//cout<<"#"<<myId<<"changes reported"<<endl;
		setChanged = OE_CHANGED;
	}
	//else
	//	cout<<"#"<<myId<<"no changes reported"<<endl;
		
	//MPI_Send(&setChanged, 1, MPI_INT, 0, OE_RESULT, MPI_COMM_WORLD);
	if(Utils::mpi_send(&setChanged, 1, 0, OE_RESULT)!=0)
		Utils::exitWithError();
		
	//cout<<"#"<<myId<<" changes report sent."<<endl;
}

int ShellSorter::receiveStopCondition(int myId)
{
	MPI_Status mpi_status;
	int stopCondition;
	//cout<<"Process #"<<myId<<" waiting for stop condition..."<<endl;
	
//	MPI_Recv(&stopCondition, 1, MPI_INT, 0, OE_STOP_CONDITION, MPI_COMM_WORLD, &mpi_status);
	if(Utils::mpi_recv(&stopCondition, 1, 0, OE_STOP_CONDITION, &mpi_status)!=0)
		Utils::exitWithError();

	/*if(stopCondition == OE_SORTING_DONE)
		cout<<"Process #"<<myId<<" received stop condition: OE_SORTING_DONE"<<endl;
	else if(stopCondition == OE_SORTING_UNDONE)
		cout<<"Process #"<<myId<<" received stop condition: OE_SORTING_UNDONE"<<endl;
	else
		cout<<"Process #"<<myId<<" received stop condition: ERROR"<<endl;*/
	//cout<<"Process #"<<myId<<" received stop condition = "<<stopCondition<<endl;
	return stopCondition;
}

int ShellSorter::compareSplit(int otherPId, int myId, int* buffer, int bufferSize, bool& localSetChanged)
{	
	MPI_Request request;
	MPI_Status status;
	
	bool getGreaterPart = (myId > otherPId) ? true : false;
	int* buffer2 = new int[bufferSize*2];
	
	LocalQSorter* lqs = new LocalQSorter(buffer2,bufferSize*2);
	
	MPI_Isend( buffer, bufferSize, MPI_INT, otherPId,
		COMPARE_SPLIT, MPI_COMM_WORLD, &request );
	
	//MPI_Recv(buffer2, bufferSize, MPI_INT, otherPId,
	//	COMPARE_SPLIT, MPI_COMM_WORLD, &status);	
	
	if(Utils::mpi_recv(buffer2, bufferSize, otherPId,
	 COMPARE_SPLIT, &status)!= 0)
		Utils::exitWithError();

	for(int i = 0; i<bufferSize; ++i)
		buffer2[i+bufferSize] = buffer[i];
	
	lqs->localSort(true);
	

		localSetChanged = false;
		if(getGreaterPart)
			for(int i=0; i<bufferSize; ++i)
				if(buffer[i] != buffer2[i+bufferSize])
				{
					localSetChanged = true;
					break;
				}
		else
			for(int i=0; i<bufferSize; ++i)
				if(buffer[i] != buffer2[i])
				{
					localSetChanged = true;
					break;
				}
	
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

int ShellSorter::compareSplit(int otherPId, int myId, int* buffer, int bufferSize)
{	
	MPI_Request request;
	MPI_Status status;
	
	bool getGreaterPart = (myId > otherPId) ? true : false;
	int* buffer2 = new int[bufferSize*2];
	
	LocalQSorter* lqs = new LocalQSorter(buffer2,bufferSize*2);
	
	MPI_Isend( buffer, bufferSize, MPI_INT, otherPId,
		COMPARE_SPLIT + myId, MPI_COMM_WORLD, &request );
	
	//MPI_Recv(buffer2, bufferSize, MPI_INT, otherPId,
		//COMPARE_SPLIT + otherPId, MPI_COMM_WORLD, &status);
		
	if(Utils::mpi_recv(buffer2, bufferSize, otherPId,
	 COMPARE_SPLIT + otherPId, &status)!= 0)
		Utils::exitWithError();	

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
