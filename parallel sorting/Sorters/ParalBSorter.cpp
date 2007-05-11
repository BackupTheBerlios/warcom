#include "ParalBSorter.h"

namespace sorting
{

int ParalBSorter:: compareSplit(int idProces, int myId, bool direction, int* buffer, int bufSize)//direction true- zachowaj wieksze, false- mniejsze
{	
	int* buffer2 = new int[bufSize*2];
	MPI_Request request;
	MPI_Status status; 
	LocalQSorter* lqs = new LocalQSorter(buffer2,bufSize*2);
	
	MPI_Isend( buffer, bufSize, MPI_INT, idProces,
		WORK_TAG+50, MPI_COMM_WORLD, &request );
	MPI_Recv(buffer2, bufSize, MPI_INT, idProces,
		WORK_TAG+50, MPI_COMM_WORLD, &status);	
	/*if(direction){
		MPI_Isend( buffer, bufSize, MPI_INT, idProces,
		WORK_TAG+100, MPI_COMM_WORLD, &request );			
		MPI_Recv(buffer2, bufSize, MPI_INT, idProces,WORK_TAG+50, MPI_COMM_WORLD, &status);			
	}
	else
	{
		MPI_Isend( buffer, bufSize, MPI_INT, idProces,
		WORK_TAG+50, MPI_COMM_WORLD, &request );
		MPI_Recv(buffer2, bufSize, MPI_INT, idProces,
		WORK_TAG+100, MPI_COMM_WORLD, &status);		
	}*/		
	for(int i = 0; i<bufSize; ++i)
		buffer2[i+bufSize] = buffer[i];//moze to dac po Isend a przed recv
	
	lqs->localSort(ASCENDING); //poprawicic zeby mozna bylo tez malejaco
	if(direction == ASCENDING)
	    for(int i = 0; i<bufSize; ++i)
		    buffer[i] = buffer2[i+bufSize];
	if(direction == DESCENDING)
		for(int i = 0; i<bufSize; ++i)
			buffer[i] = buffer2[i];
	
	/*cout<<"compsplit proces "<<myId<<": ";	
	for(int i = 0; i<bufSize*2; ++i)
		cout<< buffer2[i]<<" ";
	cout<<endl;	*/		
	if(buffer2 != NULL)
			delete(buffer2);
	return 0;	
}

int ParalBSorter:: getDirectionToCompSplit(int etap, int myId,  int coutProc,bool* direction, int idProces)
{
	int et = 1;
	int etap1 = etap;
	while(etap1>0)     //0  1  2   ... log(countProc) 
	{	et*=2; 	       //1  2  4
		etap1--;
	}	
	int et2 = et*2;    //2  4  8
	int et22 = et2*2;  //4  8  16
	
	myId-=1; //zeby byly od zera!!!!!
	idProces -=1;	
	//cout<<"QEtap: "<<etap<<" myid: "<<myId+1<<" p: "<<idProces+1;	
	if(myId%et22 < et2 )
	{
		if(myId<idProces){
			*direction = DESCENDING;			
		}
		else{
			*direction = ASCENDING;			
		}
	}		
	else
	{
		if(myId<idProces){
			*direction = ASCENDING;		
		}
		else			{
			*direction = DESCENDING;		
		}
	}	
	return 0;	
}

int ParalBSorter:: getIdToCompSplit(int depth, int myId,  int coutProc, int* idProces)
{
	myId-=1; //zeby byly od zera!!!!!
	int et = 1;
	int etap1 = depth;
	while(etap1>0)     //0  1  2   ... log(countProc) 
	{	et*=2; 	       //1  2  4
		etap1--;
	}	
	int et2 = et*2;    //2  4  8
	int et22 = et2*2;  //4  8  16
	
	int p;	
	int m = myId%et2;		
		
	if(m<et)p = myId+et;
	else p = myId-et;	
	*idProces = p+1;	
	//cout<<"DEPTH: "<<depth<<" myid: "<<myId+1<<" p: "<<*idProces;	
	return 0;
}
/*int ParalBSorter:: getIdProcesToCompSplit(int etap, int myId,  int coutProc,bool* direction, int* idProces)
{
	myId-=1; //zeby byly od zera!!!!!
	int et = 1;
	int etap1 = etap;
	while(etap1>0)     //0  1  2   ... log(countProc) 
	{	et*=2; 	       //1  2  4
		etap1--;
	}	
	int et2 = et*2;    //2  4  8
	int et22 = et2*2;  //4  8  16
	
	int p;	
	int m = myId%et2;		
	//int mm = myId%et22;
		
	if(m<et)p = myId+et;
	else p = myId-et;	
	*idProces = p+1;	
	cout<<"Etap: "<<etap<<" myid: "<<myId+1<<" p: "<<*idProces;	
	if(myId%et22 < et2 )
	{
		if(myId<p){
			*direction = DESCENDING;
			cout<<" mal"<<endl;
		}
		else{
			*direction = ASCENDING;
			cout<<" ros"<<endl;//ros
		}
	}		
	else
	{
		if(myId<p){
			*direction = ASCENDING;
			cout<<" ros"<<endl;//ros
		}
		else			{
			*direction = DESCENDING;
			cout<<" mal"<<endl;
		}
	}	
	return 0;	
}*/
ParalBSorter::ParalBSorter(string inFile, string outFile)
{
	this->inFile = inFile;
	this->outFile = outFile;
}

int ParalBSorter::sort()
{
	Status status; 
   	MPI::Init();
   	int myrank = COMM_WORLD.Get_rank(); 
   	int numprocs = COMM_WORLD.Get_size(); 
   	if(myrank == 0)
   	{
   		DataLoader dl(inFile, numprocs);
		dl.loadAndSendData();
		
		return 0;
			//TODO potem chyba normalny udział w sortowaniu?
   	}
   	else
   	{
   		MPI_Status status;
   		MPI_Request request;
   		int bufSize;
   		int* buffer;
		
   		MPI_Recv(&bufSize, 1, MPI_INT, 0, BUFFER_SIZE_TAG, MPI_COMM_WORLD, &status);
   		buffer = new int[bufSize];
		MPI_Recv(buffer, bufSize, MPI_INT, 0, WORK_TAG, MPI_COMM_WORLD, &status);
		
		cout<<"Process #"<<myrank<<": My buffer: ";
		//oem->sort(buffer, bufSize);
		for(int i=0; i<bufSize; i++)
			cout<<buffer[i]<<" ";
		cout<<endl;
	
		log2(numprocs-1); 
		bool direction;
		int idProces;
		
		for(int i=0; i<log2(numprocs-1); ++i)
		{
			for(int j=i; j>=0; --j)
			{
				getIdToCompSplit(j,myrank,numprocs-1,&idProces);
				getDirectionToCompSplit(i,myrank,numprocs-1,
			&direction,idProces);
				compareSplit(idProces, myrank, direction, buffer, bufSize);
			}			
		}		
		cout<<"!!!!Po sortowaniu proces #: "<<myrank<<": ";
		for(int i = 0; i<bufSize; ++i)
			cout<< buffer[i]<<" ";
		cout<<endl;	
		if(buffer != NULL)
			delete(buffer);
   	}
   	MPI::Finalize();
}
}
