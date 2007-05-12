FILES = Sorters/Sorter.h Sorters/Sorter.cpp
TOOLS = Tools/Utils.cpp Tools/Utils.h Tools/DataLoader.cpp Tools/DataLoader.h Tools/myio.cpp Tools/myio.h
OEM = Sorters/OemSorter.h Sorters/OemSorter.cpp Sorters/OemSorterWorker.cpp Sorters/OemSorterWorker.h
BITONIC = Sorters/BSorter.cpp Sorters/BSorter.h
SHELLS = Sorters/ShellSorter.h Sorters/ShellSorter.cpp Sorters/ShellSorterWorker.cpp Sorters/ShellSorterWorker.h

all: psort

clean:
	-rm -f psort
	
lam: psort.cpp
	mpic++ -o2 psort.cpp $(FILES) $(TOOLS) $(OEM) $(BITONIC) $(SHELLS) -o psort


