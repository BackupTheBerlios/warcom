FILES = Tools/Utils.cpp Tools/Utils.h Tools/DataLoader.cpp Tools/DataLoader.h Tools/myio.cpp Tools/myio.h Sorters/Sorter.h Sorters/Sorter.cpp Sorters/OemSorter.h Sorters/OemSorter.cpp Sorters/BSorter.cpp Sorters/BSorter.h Sorters/OemSorterWorker.cpp Sorters/OemSorterWorker.h

all: psort

clean:
	-rm -f psort dlt

psort: psort.cpp
	g++ -o2 -Wall psort.cpp $(FILES) -o psort
	
lam: psort.cpp
	mpic++ -o2 psort.cpp $(FILES) -o psort

dlt: dataLoaderTest.cpp
	mpic++ -o2 dataLoaderTest.cpp $(FILES) -o dlt
