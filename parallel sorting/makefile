GENERAL_SORTERS = Sorters/LocalQSorter.h Sorters/LocalQSorter.cpp
TOOLS = Tools/Utils.cpp Tools/Utils.h Tools/DataLoader.cpp Tools/DataLoader.h Tools/myio.cpp Tools/myio.h Tools/FDataLoader.h Tools/FDataLoader.cpp Tools/TaskTimer.h Tools/TaskTimer.cpp Tools/DataCollector.h Tools/DataCollector.cpp 
OEM = Sorters/OemSorter.h Sorters/OemSorter.cpp Sorters/OemSorterWorker.cpp Sorters/OemSorterWorker.h
BITONIC = Sorters/BSorter.cpp Sorters/BSorter.h Sorters/BSorterWorker.cpp Sorters/BSorterWorker.h
SHELLS = Sorters/ShellSorter.h Sorters/ShellSorter.cpp Sorters/ShellSorterWorker.cpp Sorters/ShellSorterWorker.h Sorters/ShellLocalSorter.h Sorters/ShellLocalSorter.cpp

all: lam

clean:
	-rm -f psort
	
lam: psort.cpp
	mpic++ -o2 psort.cpp $(GENERAL_SORTERS) $(TOOLS) $(OEM) $(BITONIC) $(SHELLS) -o psort