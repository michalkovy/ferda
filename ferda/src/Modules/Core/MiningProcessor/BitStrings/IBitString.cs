using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// And, Not, Or vraci novy IBitString
    /// </summary>
    public interface IBitString : IBitStringBase
    {
        IFormula Identifier { get; }

        ReadOnlyCollection<Guid> UsedAttributes { get; }

        IBitString And(IBitString source);
        IBitString Not();
        IBitString Or(IBitString source);
        int Sum { get; }
    }

    public interface IBitStringCreate
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