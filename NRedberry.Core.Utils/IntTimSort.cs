using System.Diagnostics;

namespace NRedberry.Core.Utils;

public class IntTimSort
{
    private static readonly int MIN_MERGE = 32;
    private readonly int[] a;
    private readonly int[] b;
    private static readonly int MIN_GALLOP = 7;
    private int minGallop = MIN_GALLOP;
    private static readonly int INITIAL_TMP_STORAGE_LENGTH = 256;
    private int[] tmp; // Actual runtime type will be int[], regardless of T
    private int[] tmpB; // Actual runtime type will be int[], regardless of T
    private int stackSize; // Number of pending runs on stack
    private readonly int[] runBase;
    private readonly int[] runLen;

    private IntTimSort(int[] a, int[] b)
    {
        this.a = a;
        this.b = b;

        // Allocate temp storage (which may be increased later if necessary)
        int len = a.Length;

        tmp = new int[len < 2 * INITIAL_TMP_STORAGE_LENGTH
            ? len >> 1 : INITIAL_TMP_STORAGE_LENGTH];

        tmpB = new int[len < 2 * INITIAL_TMP_STORAGE_LENGTH
            ? len >> 1 : INITIAL_TMP_STORAGE_LENGTH];

        int stackLen = len < 120 ? 5
            : len < 1542 ? 10
            : len < 119151 ? 19 : 40;
        runBase = new int[stackLen];
        runLen = new int[stackLen];
    }

    public static void Sort(int[] a, int[] b)
    {
        if (a.Length != b.Length)
        {
            throw new ArgumentException();
        }

        Sort(a, 0, a.Length, b);
    }

    public static void Sort(int[] a, int lo, int hi, int[] b)
    {
        RangeCheck(a.Length, lo, hi);
        int nRemaining = hi - lo;
        if (nRemaining < 2)
        {
            return; // Arrays of size 0 and 1 are always sorted
        }

        // If array is small, do a "mini-TimSort" with no merges
        if (nRemaining < MIN_MERGE)
        {
            int initRunLen = CountRunAndMakeAscending(a, lo, hi, b);
            BinarySort(a, lo, hi, lo + initRunLen, b);
            return;
        }

        IntTimSort ts = new IntTimSort(a, b);
        int minRun = MinRunLength(nRemaining);
        do
        {
            // Identify next run
            int runLen = CountRunAndMakeAscending(a, lo, hi, b);

            // If run is short, extend to min(minRun, nRemaining)
            if (runLen < minRun)
            {
                int force = nRemaining <= minRun ? nRemaining : minRun;
                BinarySort(a, lo, lo + force, lo + runLen, b);
                runLen = force;
            }

            // Push run onto pending-run stack, and maybe merge
            ts.PushRun(lo, runLen);
            ts.MergeCollapse();

            // Advance to find next run
            lo += runLen;
            nRemaining -= runLen;
        } while (nRemaining != 0);

        // Merge all remaining runs to complete sort
        Debug.Assert(lo == hi);
        ts.MergeForceCollapse();
        Debug.Assert(ts.stackSize == 1);
    }

    private static void BinarySort(int[] a, int lo, int hi, int start, int[] b)
    {
        Debug.Assert(lo <= start && start <= hi);
        if (start == lo)
        {
            start++;
        }

        for (; start < hi; start++)
        {
            int pivot = a[start];
            int pivotB = b[start];

            // Set left (and right) to the index where a[start] (pivot) belongs
            int left = lo;
            int right = start;
            Debug.Assert(left <= right);

            while (left < right)
            {
                int mid = (left + right) >> 1;
                if (pivot < a[mid])
                {
                    right = mid;
                }
                else
                {
                    left = mid + 1;
                }
            }

            Debug.Assert(left == right);

            int n = start - left;  // The number of elements to move
            // Switch is just an optimization for arraycopy in default case
            switch (n)
            {
                case 2:
                {
                    a[left + 2] = a[left + 1];
                    b[left + 2] = b[left + 1];
                    goto case 1;
                }

                case 1:
                {
                    a[left + 1] = a[left];
                    b[left + 1] = b[left];
                    break;
                }

                default:
                {
                    Array.Copy(a, left, a, left + 1, n);
                    Array.Copy(b, left, b, left + 1, n);
                    break;
                }
            }

            a[left] = pivot;
            b[left] = pivotB;
        }
    }

    private static int CountRunAndMakeAscending(int[] a, int lo, int hi, int[] b)
    {
        Debug.Assert(lo < hi);
        int runHi = lo + 1;
        if (runHi == hi)
        {
            return 1;
        }

        // Find end of run, and reverse range if descending
        if (a[runHi++] < a[lo])
        { // Descending
            while (runHi < hi && a[runHi] < a[runHi - 1])
                runHi++;
            ReverseRange(a, lo, runHi, b);
        }
        else // Ascending
        {
            while (runHi < hi && a[runHi] >= a[runHi - 1])
                runHi++;
        }

        return runHi - lo;
    }

    private static void ReverseRange(int[] a, int lo, int hi, int[] b)
    {
        hi--;
        while (lo < hi)
        {
            int t = a[lo];
            int e = b[lo];
            b[lo] = b[hi];
            a[lo++] = a[hi];
            b[hi] = e;
            a[hi--] = t;
        }
    }

    private static int MinRunLength(int n)
    {
        Debug.Assert(n >= 0);
        int r = 0; // Becomes 1 if any 1 bits are shifted off
        while (n >= MIN_MERGE)
        {
            r |= (n & 1);
            n >>= 1;
        }

        return n + r;
    }

    private void PushRun(int runBase, int runLen)
    {
        this.runBase[stackSize] = runBase;
        this.runLen[stackSize] = runLen;
        stackSize++;
    }

    private void MergeCollapse()
    {
        while (stackSize > 1)
        {
            int n = stackSize - 2;
            if (n > 0 && runLen[n - 1] <= runLen[n] + runLen[n + 1])
            {
                if (runLen[n - 1] < runLen[n + 1])
                {
                    n--;
                }

                MergeAt(n);
            }
            else if (runLen[n] <= runLen[n + 1])
            {
                MergeAt(n);
            }
            else
            {
                break; // Invariant is established
            }
        }
    }

    private void MergeForceCollapse()
    {
        while (stackSize > 1)
        {
            int n = stackSize - 2;
            if (n > 0 && runLen[n - 1] < runLen[n + 1])
            {
                n--;
            }

            MergeAt(n);
        }
    }

    private void MergeAt(int i)
    {
        Debug.Assert(stackSize >= 2);
        Debug.Assert(i >= 0);
        Debug.Assert(i == stackSize - 2 || i == stackSize - 3);

        int base1 = runBase[i];
        int len1 = runLen[i];
        int base2 = runBase[i + 1];
        int len2 = runLen[i + 1];
        Debug.Assert(len1 > 0 && len2 > 0);
        Debug.Assert(base1 + len1 == base2);

        // Record the length of the combined runs; if i is the 3rd-last run now, also slide over the last run
        runLen[i] = len1 + len2;
        if (i == stackSize - 3)
        {
            runBase[i + 1] = runBase[i + 2];
            runLen[i + 1] = runLen[i + 2];
        }

        stackSize--;

        // Find where the first element of run2 goes in run1
        int k = GallopRight(a[base2], a, base1, len1, 0);
        Debug.Assert(k >= 0);
        base1 += k;
        len1 -= k;
        if (len1 == 0)
        {
            return;
        }

        // Find where the last element of run1 goes in run2
        len2 = GallopLeft(a[base1 + len1 - 1], a, base2, len2, len2 - 1);
        Debug.Assert(len2 >= 0);
        if (len2 == 0)
        {
            return;
        }

        // Merge remaining runs, using tmp array with min(len1, len2) elements
        if (len1 <= len2)
        {
            MergeLo(base1, len1, base2, len2);
        }
        else
        {
            MergeHi(base1, len1, base2, len2);
        }
    }

    private static int GallopLeft(int key, int[] a, int @base, int len, int hint)
{
    Debug.Assert(len > 0 && hint >= 0 && hint < len);
    int lastOfs = 0;
    int ofs = 1;
    if (key > a[@base + hint])
    {
        int maxOfs = len - hint;
        while (ofs < maxOfs && key > a[@base + hint + ofs])
        {
            lastOfs = ofs;
            ofs = (ofs << 1) + 1;
            if (ofs <= 0)
            {
                ofs = maxOfs; // int overflow
            }
        }

        if (ofs > maxOfs)
        {
            ofs = maxOfs;
        }

        lastOfs += hint;
        ofs += hint;
    }
    else
    {
        int maxOfs = hint + 1;
        while (ofs < maxOfs && key <= a[@base + hint - ofs])
        {
            lastOfs = ofs;
            ofs = (ofs << 1) + 1;
            if (ofs <= 0)
            {
                ofs = maxOfs; // int overflow
            }
        }

        if (ofs > maxOfs)
        {
            ofs = maxOfs;
        }

        int tmp = lastOfs;
        lastOfs = hint - ofs;
        ofs = hint - tmp;
    }

    Debug.Assert(-1 <= lastOfs && lastOfs < ofs && ofs <= len);

    lastOfs++;
    while (lastOfs < ofs)
    {
        int m = lastOfs + ((ofs - lastOfs) >> 1);

        if (key > a[@base + m])
        {
            lastOfs = m + 1;
        }
        else
        {
            ofs = m;
        }
    }

    Debug.Assert(lastOfs == ofs);
    return ofs;
}

private static int GallopRight(int key, int[] a, int @base, int len, int hint)
{
    Debug.Assert(len > 0 && hint >= 0 && hint < len);

    int ofs = 1;
    int lastOfs = 0;
    if (key < a[@base + hint])
    {
        int maxOfs = hint + 1;
        while (ofs < maxOfs && key < a[@base + hint - ofs])
        {
            lastOfs = ofs;
            ofs = (ofs << 1) + 1;
            if (ofs <= 0)
            {
                ofs = maxOfs; // int overflow
            }
        }

        if (ofs > maxOfs)
        {
            ofs = maxOfs;
        }

        int tmp = lastOfs;
        lastOfs = hint - ofs;
        ofs = hint - tmp;
    }
    else
    {
        int maxOfs = len - hint;
        while (ofs < maxOfs && key >= a[@base + hint + ofs])
        {
            lastOfs = ofs;
            ofs = (ofs << 1) + 1;
            if (ofs <= 0)
            {
                ofs = maxOfs; // int overflow
            }
        }

        if (ofs > maxOfs)
        {
            ofs = maxOfs;
        }

        lastOfs += hint;
        ofs += hint;
    }

    Debug.Assert(-1 <= lastOfs && lastOfs < ofs && ofs <= len);

    lastOfs++;
    while (lastOfs < ofs)
    {
        int m = lastOfs + ((ofs - lastOfs) >> 1);

        if (key < a[@base + m])
        {
            ofs = m;
        }
        else
        {
            lastOfs = m + 1;
        }
    }

    Debug.Assert(lastOfs == ofs);
    return ofs;
}

private void MergeLo(int base1, int len1, int base2, int len2)
{
    // len1 > 0 && len2 > 0 && base1 + len1 == base2

    // Copy first run into temp array
    int[] a = this.a; // For performance
    int[] b = this.b;
    int[] tmp = EnsureCapacity(len1);
    Array.Copy(a, base1, tmp, 0, len1);
    int[] tmpB = this.tmpB;
    Array.Copy(b, base1, tmpB, 0, len1);

    int cursor1 = 0; // Indices into tmp array
    int cursor2 = base2; // Indices int a
    int dest = base1; // Indices int a

    // Move first element of second run and deal with degenerate cases
    b[dest] = b[cursor2];
    a[dest++] = a[cursor2++];
    if (--len2 == 0)
    {
        Array.Copy(tmp, cursor1, a, dest, len1);
        Array.Copy(tmpB, cursor1, b, dest, len1);
        return;
    }

    if (len1 == 1)
    {
        Array.Copy(a, cursor2, a, dest, len2);
        Array.Copy(b, cursor2, b, dest, len2);
        a[dest + len2] = tmp[cursor1]; // Last elt of run 1 to end of merge
        b[dest + len2] = tmpB[cursor1];
        return;
    }

    int minGallop = this.minGallop; //  "    "       "     "      "
    while (true)
    {
        int count1 = 0; // Number of times in a row that first run won
        int count2 = 0; // Number of times in a row that second run won

        do
        {
            // len1 > 1 && len2 > 0
            if (a[cursor2] < tmp[cursor1])
            {
                b[dest] = b[cursor2];
                a[dest++] = a[cursor2++];
                count2++;
                count1 = 0;
                if (--len2 == 0)
                {
                    break;
                }
            }
            else
            {
                b[dest] = tmpB[cursor1];
                a[dest++] = tmp[cursor1++];
                count1++;
                count2 = 0;
                if (--len1 == 1)
                {
                    break;
                }
            }
        } while ((count1 | count2) < minGallop);

        do
        {
            // len1 > 1 && len2 > 0
            count1 = GallopRight(a[cursor2], tmp, cursor1, len1, 0);
            if (count1 != 0)
            {
                Array.Copy(tmp, cursor1, a, dest, count1);
                Array.Copy(tmpB, cursor1, b, dest, count1);
                dest += count1;
                cursor1 += count1;
                len1 -= count1;
                if (len1 <= 1) // len1 == 1 || len1 == 0
                {
                    break;
                }
            }

            b[dest] = b[cursor2];
            a[dest++] = a[cursor2++];
            if (--len2 == 0)
            {
                break;
            }

            count2 = GallopLeft(tmp[cursor1], a, cursor2, len2, 0);
            if (count2 != 0)
            {
                Array.Copy(a, cursor2, a, dest, count2);
                Array.Copy(b, cursor2, b, dest, count2);
                dest += count2;
                cursor2 += count2;
                len2 -= count2;
                if (len2 == 0)
                {
                    break;
                }
            }

            b[dest] = tmpB[cursor1];
            a[dest++] = tmp[cursor1++];
            if (--len1 == 1)
            {
                break;
            }

            minGallop--;
        } while (count1 >= MIN_GALLOP | count2 >= MIN_GALLOP);

        if (minGallop < 0)
        {
            minGallop = 0;
        }

        minGallop += 2; // Penalize for leaving gallop mode

        if (len1 == 1)
        {
            // len2 > 0
            Array.Copy(a, cursor2, a, dest, len2);
            Array.Copy(b, cursor2, b, dest, len2);
            a[dest + len2] = tmp[cursor1]; //  Last elt of run 1 to end of merge
            b[dest + len2] = tmpB[cursor1]; //  Last elt of run 1 to end of merge
        }
        else if (len1 == 0)
        {
            throw new ArgumentException("Comparison method violates its general contract!");
        }
        else
        {
            // len2 == 0
            // len1 > 1
            Array.Copy(tmp, cursor1, a, dest, len1);
            Array.Copy(tmpB, cursor1, b, dest, len1);
        }

        break;
    }
}

    private void MergeHi(int base1, int len1, int base2, int len2)
    {
        Debug.Assert(len1 > 0 && len2 > 0 && base1 + len1 == base2);

        // Copy second run into temp array
        int[] a = this.a; // For performance
        int[] b = this.b; // For performance
        int[] tmp = EnsureCapacity(len2);
        int[] tmpB = this.tmpB;
        Array.Copy(a, base2, tmp, 0, len2);
        Array.Copy(b, base2, tmpB, 0, len2);

        int cursor1 = base1 + len1 - 1;  // Indices into a
        int cursor2 = len2 - 1;          // Indices into tmp array
        int dest = base2 + len2 - 1;     // Indices into a

        // Move last element of first run and deal with degenerate cases
        b[dest] = b[cursor1];
        a[dest--] = a[cursor1--];
        if (--len1 == 0)
        {
            Array.Copy(tmp, 0, a, dest - (len2 - 1), len2);
            Array.Copy(tmpB, 0, b, dest - (len2 - 1), len2);
            return;
        }

        if (len2 == 1)
        {
            dest -= len1;
            cursor1 -= len1;
            Array.Copy(a, cursor1 + 1, a, dest + 1, len1);
            Array.Copy(b, cursor1 + 1, b, dest + 1, len1);
            a[dest] = tmp[cursor2];
            b[dest] = tmpB[cursor2];
            return;
        }

        int minGallop = this.minGallop;
        while (true)
        {
            int count1 = 0; // Number of times in a row that first run won
            int count2 = 0; // Number of times in a row that second run won

            do
            {
                Debug.Assert(len1 > 0 && len2 > 1);
                if (tmp[cursor2] < a[cursor1])
                {
                    b[dest] = b[cursor1];
                    a[dest--] = a[cursor1--];
                    count1++;
                    count2 = 0;
                    if (--len1 == 0)
                    {
                        break;
                    }
                }
                else
                {
                    b[dest] = tmpB[cursor2];
                    a[dest--] = tmp[cursor2--];
                    count2++;
                    count1 = 0;
                    if (--len2 == 1)
                    {
                        break;
                    }
                }
            } while ((count1 | count2) < minGallop);

            do
            {
                Debug.Assert(len1 > 0 && len2 > 1);
                count1 = len1 - GallopRight(tmp[cursor2], a, base1, len1, len1 - 1);
                if (count1 != 0)
                {
                    dest -= count1;
                    cursor1 -= count1;
                    len1 -= count1;
                    Array.Copy(a, cursor1 + 1, a, dest + 1, count1);
                    Array.Copy(b, cursor1 + 1, b, dest + 1, count1);
                    if (len1 == 0)
                    {
                        break;
                    }
                }

                b[dest] = tmpB[cursor2];
                a[dest--] = tmp[cursor2--];
                if (--len2 == 1)
                {
                    break;
                }

                count2 = len2 - GallopLeft(a[cursor1], tmp, 0, len2, len2 - 1);
                if (count2 != 0)
                {
                    dest -= count2;
                    cursor2 -= count2;
                    len2 -= count2;
                    Array.Copy(tmpB, cursor2 + 1, b, dest + 1, count2);
                    Array.Copy(tmp, cursor2 + 1, a, dest + 1, count2);
                    if (len2 <= 1)  // len2 == 1 || len2 == 0
                    {
                        break;
                    }
                }

                b[dest] = b[cursor1];
                a[dest--] = a[cursor1--];
                if (--len1 == 0)
                {
                    break;
                }

                minGallop--;
            } while (count1 >= MIN_GALLOP | count2 >= MIN_GALLOP);
            if (minGallop < 0)
            {
                minGallop = 0;
            }

            minGallop += 2;  // Penalize for leaving gallop mode
        }  // End of "outer" loop

        this.minGallop = minGallop < 1 ? 1 : minGallop;  // Write back to field

        if (len2 == 1)
        {
            Debug.Assert(len1 > 0);
            dest -= len1;
            cursor1 -= len1;
            Array.Copy(a, cursor1 + 1, a, dest + 1, len1);
            Array.Copy(b, cursor1 + 1, b, dest + 1, len1);
            a[dest] = tmp[cursor2];  // Move first elt of run2 to front of merge
            b[dest] = tmpB[cursor2];  // Move first elt of run2 to front of merge
        }
        else if (len2 == 0)
        {
            throw new ArgumentException("Comparison method violates its general contract!");
        }
        else
        {
            Debug.Assert(len1 == 0);
            Debug.Assert(len2 > 0);
            Array.Copy(tmp, 0, a, dest - (len2 - 1), len2);
            Array.Copy(tmpB, 0, b, dest - (len2 - 1), len2);
        }
    }

    private int[] EnsureCapacity(int minCapacity)
    {
        if (tmp.Length < minCapacity)
        {
            // Compute smallest power of 2 > minCapacity
            int newSize = minCapacity;
            newSize |= newSize >> 1;
            newSize |= newSize >> 2;
            newSize |= newSize >> 4;
            newSize |= newSize >> 8;
            newSize |= newSize >> 16;
            newSize++;

            newSize = newSize < 0
                ? minCapacity
                : Math.Min(newSize, a.Length / 2); // Right shift >>> is equivalent to divide by 2 in C#

            // Not bloody likely!
            tmp = new int[newSize];
            tmpB = new int[newSize];
        }

        return tmp;
    }

    private static void RangeCheck(int arrayLen, int fromIndex, int toIndex)
    {
        if (fromIndex > toIndex)
        {
            throw new ArgumentException("fromIndex(" + fromIndex + ") > toIndex(" + toIndex + ")");
        }

        if (fromIndex < 0)
        {
            throw new IndexOutOfRangeException("fromIndex is out of bounds: " + fromIndex);
        }

        if (toIndex > arrayLen)
        {
            throw new IndexOutOfRangeException("toIndex is out of bounds: " + toIndex);
        }
    }
}
