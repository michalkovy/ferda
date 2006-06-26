using System;
using System.Collections.ObjectModel;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    public interface IEmptyBitString : IBitString
    {}
    
    /// <summary>
    /// And, Not, Or returns new IBitString
    /// </summary>
    public interface IBitString : IBitStringBase
    {
        Formulas.BooleanAttributeFormula Identifier { get; }

        IBitString And(IBitString source);
        IBitString Not();
        IBitString Or(IBitString source);
        int Sum { get; }
    }

    public interface IBitStringCreate : IBitStringBase
    {
        void Fill(bool value);
        bool GetBit(int index);
        void SetBit(int index, bool value);
    }

    public interface IBitStringBase
    {
        bool Equals(object obj);
        int GetHashCode();
        int Length { get; }
        string ToString();
    }
}