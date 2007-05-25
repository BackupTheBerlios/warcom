#include "ShellLocalSorter.h"

namespace sorting
{

ShellLocalSorter::ShellLocalSorter()
{
}

/* Performs an insertion sort on elements of a[] with the given gap.
 * If gap == 1, performs an ordinary insertion sort.
 * If gap >= length, does nothing.
 */
void ShellLocalSorter::shellSortPhase(int a[], int length, int gap) 
{
    int i;
    for (i = gap; i < length; ++i) 
    {
        int value = a[i];
        int j;
        for (j = i - gap; j >= 0 && a[j] > value; j -= gap) {
            a[j + gap] = a[j];
        }
        a[j + gap] = value;
    }
}
    
int* ShellLocalSorter::sort(int a[], int length) 
{
    /*
     * gaps[] should approximate a geometric progression.
     * The following sequence is the best known in terms of
     * the average number of key comparisons made [1]
     */
    static const int gaps[] = {
        1, 4, 10, 23, 57, 132, 301, 701
    };
    
    int sizeIndex;
    
    for (sizeIndex = sizeof(gaps)/sizeof(gaps[0]) - 1;
               sizeIndex >= 0;
               --sizeIndex)
        shellSortPhase(a, length, gaps[sizeIndex]);
}

}
