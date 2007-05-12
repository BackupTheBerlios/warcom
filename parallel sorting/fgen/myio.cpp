#include "myio.h"

//Projekt
/*************************************************************************************************************************************
Funkcje o nazwach rozpoczynajacych sie przedrostkiem "my_-" sa wrapperami funkcji systemowych. Jeli nie zaznaczono przy ich definicji
inaczej, przyjmuja takie argumenty i zwracaja taka wartosc jak funkcja opakowywana.
*************************************************************************************************************************************/
namespace tools
{
	//fd, buffer, n, offset - jak w write, offset, start - jak w lseek
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
				return -1;
			}
			n-=res;
		}while(res>0 && n>0);
		return overall;
	}
	
	//fd, buffer, n, offset - jak w write, offset, start - jak w lseek
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
				return -1;
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
	
	//path - sciezka do pliku
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
