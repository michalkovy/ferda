using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    public interface IEmptyBitString : IBitString
    {
    }

    public interface IBitString : IBitStringBase
    {
        BooleanAttributeFormula Identifier { get; }

        /// <summary>
        /// Performs the bitwise AND operation on current BitString against the specified BitString.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        IBitString And(IBitString source);

        ///// <summary>
        ///// Performs the bitwise AND operation on 
        ///// copy of current BitString against the specified BitString.
        ///// </summary>
        ///// <param name="source">The second BitString operand.</param>
        //IBitString AndCloned(IBitString source);

        /// <summary>
        /// Performs the bitwise OR operation on current BitString against the specified BitString.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        IBitString Or(IBitString source);

        ///// <summary>
        ///// Performs the bitwise OR operation on 
        ///// copy of current BitString against the specified BitString.
        ///// </summary>
        ///// <param name="source">The second BitString operand.</param>
        //IBitString OrCloned(IBitString source);

        /// <summary>
        /// Performs the bitwise NOT on current BitString.
        /// </summary>
        IBitString Not();

        ///// <summary>
        ///// Performs the bitwise NOT on copy of current BitString.
        ///// </summary>
        //IBitString NotCloned();

        /// <summary>
        /// Performs the bitwise SUM operation on current BitString.
        /// </summary>
        /// <returns>The number of bits set to 1 in current BitString.</returns>
        int Sum { get; }
    }

    public interface IBitStringCreate : IBitStringBase
    {
        /// <summary>
        /// Fills the whole BitString with the specified value.
        /// </summary>
        /// <param name="value">Value to be filled into every bit of the BitString.</param>
        /// <remarks>
        /// <para>BitString are filled with zeroes when created, so there is no need to call Fill(false) after create() method.</para>
        /// </remarks>
        void Fill(bool value);

        /// <summary>
        /// Gets a value of the specified bit from the BitString.
        /// </summary>
        /// <param name="index">Index of the bit to be retrieved.</param>
        /// <returns>Value of the specified bit from the BitString.</returns>
        bool GetBit(int index);

        /// <summary>
        /// Sets a specified bit in the BitString.
        /// </summary>
        /// <param name="index">Index of the bit to be set.</param>
        /// <param name="value">New value of the bit.</param>        
        void SetBit(int index, bool value);
    }

    public interface IBitStringBase
    {
        int Length { get; }
        string ToString();
    }
}