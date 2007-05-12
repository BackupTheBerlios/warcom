FILES = psort.cpp Sorters/Sorter.h Sorters/Sorter.cpp

TOOLS = Tools/Utils.cpp Tools/Utils.h Tools/DataLoader.cpp Tools/DataLoader.h Tools/myio.cpp Tools/myio.h

OEM = Sorters/OemSorter.h Sorters/OemSorter.cpp Sorters/OemSorterWorker.cpp Sorters/OemSorterWorker.h

SHELL = Sorters/ShellSorter.h Sorters/ShellSorter.cpp Sorters/ShellSorterWorker.cpp Sorters/ShellSorterWorker.h

BITONIC = Sorters/BSorter.cpp Sorters/BSorter.h

all: lam

clean:
	-rm -f psort

lam: psort.cpp
	mpic++ -o2 $(FILES) $(TOOLS) $(SHELL) $(OEM) $(BITONIC) -o psort
