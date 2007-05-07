#ifndef MY_IO_HEADER
#define MY_IO_HEADER

#include <stdio.h>
#include <stdlib.h>
//#include <string.h>

//#include <sys/stat.h>
//#include <sys/types.h>
//#include <sys/wait.h>
#include <unistd.h>
#include <errno.h>

#include <fcntl.h>

namespace tools
{

class MyIO
{
	public:
		static size_t my_read(int fd, void* buffer, size_t n, off_t offset, int start);
		static size_t my_write(int fd, const void *buffer, size_t n, off_t offset, int start);
		static off_t my_lseek(int fd, off_t offset, int start);
		static int my_close(int fd);
		
		static int my_create();
		static int my_open(const char* path, int flags);
		static int my_chdir(char* path);
		
		static int my_filesize(char* path);
		
		static void my_fatal(const char* log);
};
}

#endif /*MY_IO_H_*/

