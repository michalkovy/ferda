namespace Design
{
    public interface IMultipleOperandEntitySetting : IEntitySetting
    {
        ClassOfEquivalence[] ClassesOfEquivalence { get; set; }

        int MinimalLength { get; set; }

        int MaximalLength { get; set; }

        EntityImporatancePair[] Operands { get; set; }
    }
}