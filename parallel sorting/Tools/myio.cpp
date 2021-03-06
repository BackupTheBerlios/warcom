#include "myio.h"

/*************************************************************************************************************************************
Functions with precondition "my_-" are system functions' wrappers.
They get the same parameters and returns the same values like the original ones. Otherwise, it is specified in comments before definition. 
*************************************************************************************************************************************/
namespace tools
{
	//fd, buffer, n, offset - like in write, offset, start - like in lseek
	size_t MyIO::my_read(int fd, void* buffer, size_t n, off_t offset, int start)
	{
		size_t res=0, overall=0;
		if(!(offset==0 &&  start==SEEK_CUR))
			if(lseek(fd,offset,start)==-1)
			{
				perror("lseek error");
				exit(1);
			}
		do
		{
			res=TEMP_FAILURE_RETRY(read(fd, buffer, n));
			overall+=res;
			//printf("n=%d, res=%d, overall=%d\n", (int)n, (int)res, (int)overall);
			if(res==-1)
			{
				perror("read error");
				return (size_t)-1;
			}
			n-=res;
		}while(res>0 && n>0);
		return overall;
	}
	
	//fd, buffer, n, offset - like in write, offset, start - like in lseek
	size_t MyIO::my_write(int fd, const void *buffer, size_t n, off_t offset, int start)
	{
		size_t res=0, overall=0;
		if(!(offset==0 &&  start==SEEK_CUR))
			if(lseek(fd,offset,start)==-1)
			{
				perror("lseek error");
				exit(1);
			} 
		do
		{
			res=TEMP_FAILURE_RETRY(write(fd, buffer, n));
			overall+=res;
			//printf("n=%d, res=%d, overall=%d\n", (int)n, (int)res, (int)overall);
			if(res==-1)
			{
				perror("write error");
				return (size_t)-1;
			}
			n-=res;
		}while(res>0 && n>0);
		return overall;
	}
	
	off_t MyIO::my_lseek(int fd, off_t offset, int start)
	{
		off_t res=lseek(fd, offset, start);
		if(res==-1)
			perror("my_lseek error");
			
		return res;
	}
	
	int MyIO::my_close(int fd)
	{
		int res=TEMP_FAILURE_RETRY(close(fd));
		if(res==-1)
			perror("my_close error");
		return res;
	}
	
	//path - filepath
	int MyIO::my_filesize(char* path)
	{
		static struct stat statbuf;
		if(stat(path, &statbuf)==-1)
		{
			printf("my_filesize(%s) :", path);
			perror("stat error");
			exit(1);
		}
	        return statbuf.st_size;
	}
	
	int MyIO::my_open(const char* path, int flags)
	{
		int fd;
		do fd=open(path, flags, 0700);
		while(fd==-1 && errno==EINTR);
	
		if(fd==-1)
		{
			perror("open_file error");
			//exit(1);
		}
		return fd;
	}
	
	int MyIO::my_chdir(char* path)
	{
		int res=chdir(path);
		if(res<0)
			perror("my_chdir error");
		return res;
	}
	
	void MyIO::my_fatal(const char* log)
	{
		perror(log);
		exit(1);
	}
}
