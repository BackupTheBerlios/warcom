#include "TaskTimer.h"

TaskTimer::TaskTimer()
{
	time_t seconds;

  seconds = time (NULL);
}

void TaskTimer::startTask(string name)
{
	tasks[name] = time(NULL);
}

int TaskTimer::endTask(string name, int showInfo)
{
	time_t now = time(NULL);
	if(tasks.find(name) != tasks.end())
	{
		time_t past = tasks[name];
		if(showInfo)
			cout<<name<<" time: "<<now - past<<" seconds"<<endl;
		return (now - past);
	}
	if(showInfo)
		cout<<"Not found task: "<<name<<endl;
	return -1;
}

