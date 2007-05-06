all: psort

clean:
	-rm -f psort

psort: psort.cpp
	g++ -o2 -Wall psort.cpp Tools/Utils.cpp Tools/Utils.h Tools/DataLoader.cpp Tools/DataLoader.h Sorters/Sorter.h Sorters/Sorter.cpp Sorters/OemSorter.h Sorters/OemSorter.cpp -o psort
	
lam: psort.cpp
	mpic++ -o2 psort.cpp Tools/Utils.cpp Tools/Utils.h Tools/DataLoader.cpp Tools/DataLoader.h Sorters/Sorter.h Sorters/Sorter.cpp Sorters/OemSorter.h Sorters/OemSorter.cpp -o psort