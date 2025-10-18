namespace NRedberry.Core.Utils;

/// <summary>
/// Mutable array of bits with fixed size.
/// </summary>
[Obsolete("use System.Collections.BitArray instead")]
public interface IBitArray
{
 /**
    * Logical {@code and} operation.
    *
    * @param bitArray bit array
    * @throws IllegalArgumentException if size of this not equals to size of specified array.
    */
 void And(IBitArray bitArray);

 /**
         * Number of nonzero bits in this array.
         *
         * @return number of nonzero bits in this array
         */
 int BitCount();

 /**
         * Sets i-th bit to zero.
         *
         * @param i position in array
         */
 void Clear(int i);

 /**
         * Sets all bits in this array to zero.
         */
 void ClearAll();

 /**
         * Returns a deep clone of this.
         *
         * @return a deep clone of this
         */
 IBitArray Clone();

 /**
         * Returns an array with positions of the nonzero bits.
         *
         * @return an array with positions of the nonzero bits
         */
 int[] GetBits();

 /**
         * Returns {@code true} if there are at least two nonzero bits at same positions in
         * this and specified array.
         *
         * @param bitArray bit array
         * @return {@code true} if there are at least two nonzero bits at the same positions in
         *         this array and specified array
         * @throws IllegalArgumentException if size of this not equal to the size pf specified array
         */
 bool Intersects(IBitArray bitArray);

 /**
         * This will set all bits in this array to bits from specified array.
         *
         * @param bitArray bit array
         * @throws IllegalArgumentException if size of this not equals to size of specified array
         */
 void LoadValueFrom(IBitArray bitArray);

 /**
         * Logical or operation.
         *
         * @param bitArray bit array
         * @throws IllegalArgumentException if size of this not equals to size of specified array
         */
 void Or(IBitArray bitArray);

 ///<summary>
 /// Gets or sets thge i-th bit.
 /// </summary>
 ///<param name="i">position of bit</param>
 bool this[int i]
 {
  get;
  set;
 }

 /**
         * Sets all bits in this to 1.
         */
 void SetAll();
 void Set(int i);

 /// <summary>Size of array.</summary>
 /// <returns>size of array</returns>
 int Size { get; }

 /**
         * Logical xor operation.
         *
         * @param bitArray bit array
         * @throws IllegalArgumentException if size of this not equals to size of specified array
         */
 void Xor(IBitArray bitArray);

 int NextTrailingBit(int position);
}