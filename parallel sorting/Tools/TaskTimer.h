#ifndef TASKTIMER_H_
#define TASKTIMER_H_
#include<iostream>
#include<map>
#include<time.h>
using namespace std;

class TaskTimer
{
public:
	TaskTimer();
	//rozpoczyna liczenie czasu dla nowego zadania
	void startTask(string name); 
	//konczy liczenie czasu dla danego zadania i zwraca czas w sekundach
	int endTask(string name, int showInfo); 
private:
	map<string,time_t> tasks;
};

#endif /*TASKTIMER_H_*/
