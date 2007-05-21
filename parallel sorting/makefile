GENERAL_SORTERS = Sorters/Sorter.h Sorters/Sorter.cpp Sorters/LocalQSorter.h Sorters/LocalQSorter.cpp
TOOLS = Tools/Utils.cpp Tools/Utils.h Tools/DataLoader.cpp Tools/DataLoader.h Tools/myio.cpp Tools/myio.h Tools/FDataLoader.h Tools/FDataLoader.cpp Tools/TaskTimer.h Tools/TaskTimer.cpp Tools/DataCollector.h Tools/DataCollector.cpp 
OEM = Sorters/OemSorter.h Sorters/OemSorter.cpp Sorters/OemSorterWorker.cpp Sorters/OemSorterWorker.h
BITONIC = Sorters/BSorter.cpp Sorters/BSorter.h Sorters/ParalBSorter.cpp Sorters/ParalBSorter.h
SHELLS = Sorters/ShellSorter.h Sorters/ShellSorter.cpp Sorters/ShellSorterWorker.cpp Sorters/ShellSorterWorker.h

all: lam

clean:
	-rm -f psort
	
lam: psort.cpp
	mpic++ -o2 psort.cpp $(GENERAL_SORTERS) $(TOOLS) $(OEM) $(BITONIC) $(SHELLS) -o psort
