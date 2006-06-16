namespace Design
{
    public interface ISingleOperandEntitySetting : IEntitySetting
    {
        EntityImporatancePair Operand { get; set; }
    }
}