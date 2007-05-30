#include "TaskTimer.h"

TaskTimer::TaskTimer()
{
}

/*
 * Starts counting time for a task with given name
 * name - name of a task
 */
void TaskTimer::startTask(string name)
{
	tasks[name] = time(NULL);
}

/*
 * Ends counting time for a task with given name
 * name - name of a task
 * showInfo - whether to display info about elapsed time
 * return - number of elapsed time in seconds or -1 if task with given
 * 			name doesn't exist
 */
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

