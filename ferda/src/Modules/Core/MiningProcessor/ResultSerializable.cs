using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    [Serializable()]
    public class SerializableFormula
    {
        public enum FormulaType
        {
            AtomFormula,
            NegationFormula,
            ConjunctionFormula,
            DisjunctionFormula,
            CategorialFormula,
        }
        public FormulaType Type;
        public SerializableFormula[] Operands;
        public Guid AttributeId;
        public string CategoryId;

        public static Formula Build(SerializableFormula input)
        {
            if (input == null)
                return null;
            BooleanAttributeFormula[] inner = null;
            if (input.Operands != null && input.Operands.Length > 0)
            {
                inner = new BooleanAttributeFormula[input.Operands.Length];
                for (int i = 0; i < input.Operands.Length; i++)
                {
                    inner[i] = (BooleanAttributeFormula)Build(input.Operands[i]);
                }
            }

            switch (input.Type)
            {
                case FormulaType.AtomFormula:
                    return new AtomFormula(new BitStringIdentifier(input.AttributeId, input.CategoryId));
                case FormulaType.NegationFormula:
                    return new NegationFormula(inner[0]);
                case FormulaType.ConjunctionFormula:
                    return new ConjunctionFormula(inner);
                case FormulaType.DisjunctionFormula:
                    return new DisjunctionFormula(inner);
                case FormulaType.CategorialFormula:
                    return new CategorialAttributeFormula(input.AttributeId);
                default:
                    throw new NotImplementedException();
            }
        }

        public static SerializableFormula Build(Formula input)
        {
            if (input == null)
                return null;
            SerializableFormula result = new SerializableFormula();
            {
                AtomFormula af = input as AtomFormula;
                if (af != null)
                {
                    result.AttributeId = af.BitStringIdentifier.AttributeId;
                    result.CategoryId = af.BitStringIdentifier.CategoryId;
                    result.Type = FormulaType.AtomFormula;
                    return result;
                }
            }
            {
                ConjunctionFormula cf = input as ConjunctionFormula;
                if (cf != null)
                {
                    SerializableFormula[] inners = new SerializableFormula[cf.Operands.Length];
                    for (int i = 0; i < cf.Operands.Length; i++)
                    {
                        inners[i] = Build(cf.Operands[i]);
                    }
                    result.Operands = inners;
                    result.Type = FormulaType.ConjunctionFormula;
                    return result;
                }
            }
            {
                DisjunctionFormula df = input as DisjunctionFormula;
                if (df != null)
                {
                    SerializableFormula[] inners = new SerializableFormula[df.Operands.Length];
                    for (int i = 0; i < df.Operands.Length; i++)
                    {
                        inners[i] = Build(df.Operands[i]);
                    }
                    result.Operands = inners;
                    result.Type = FormulaType.DisjunctionFormula;
                    return result;
                }
            }
            {
                NegationFormula nf = input as NegationFormula;
                if (nf != null)
                {
                    result.Operands = new SerializableFormula[] { Build(nf.Operand) };
                    result.Type = FormulaType.NegationFormula;
                    return result;
                }
            }
            {
                CategorialAttributeFormula caf = input as CategorialAttributeFormula;
                if (caf != null)
                {
                    result.AttributeId = caf.AttributeId;
                    result.Type = FormulaType.CategorialFormula;
                    return result;
                }
            }
            throw new NotImplementedException();
        }
    }

    public class Hypothesis
    {
        internal const int CountOfFormulas = 8;

        public double[][] ContingencyTableA;
        public double[][] ContingencyTableB;

        public Guid NumericValuesAttributeId
        {
            get
            {
                CategorialAttributeFormula f = GetFormula(MarkEnum.Attribute) as CategorialAttributeFormula;
                if (f == null)
                {
                    Debug.Assert(false);
                    return new Guid();
                }
                return f.AttributeId;
            }
        }

        internal Formula[] _formulas = new Formula[CountOfFormulas];

        public Formula GetFormula(MarkEnum semantic)
        {
            switch (semantic)
            {
                case MarkEnum.Antecedent:
                    return _formulas[0];
                case MarkEnum.Succedent:
                    return _formulas[1];
                case MarkEnum.Condition:
                    return _formulas[2];
                case MarkEnum.FirstSet:
                    return _formulas[3];
                case MarkEnum.SecondSet:
                    return _formulas[4];
                case MarkEnum.Attribute:
                    return _formulas[5];
                case MarkEnum.RowAttribute:
                    return _formulas[6];
                case MarkEnum.ColumnAttribute:
                    return _formulas[7];
                default:
                    throw new NotImplementedException();
            }
        }

        public void SetFormula(MarkEnum semantic, Formula formula)
        {
            switch (semantic)
            {
                case MarkEnum.Antecedent:
                    _formulas[0] = formula;
                    break;
                case MarkEnum.Succedent:
                    _formulas[1] = formula;
                    break;
                case MarkEnum.Condition:
                    _formulas[2] = formula;
                    break;
                case MarkEnum.FirstSet:
                    _formulas[3] = formula;
                    break;
                case MarkEnum.SecondSet:
                    _formulas[4] = formula;
                    break;
                case MarkEnum.Attribute:
                    _formulas[5] = formula;
                    break;
                case MarkEnum.RowAttribute:
                    _formulas[6] = formula;
                    break;
                case MarkEnum.ColumnAttribute:
                    _formulas[7] = formula;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }

    [Serializable()]
    public class SerializableHypothesis
    {
        public double[][] ContingencyTableA;
        public double[][] ContingencyTableB;
        public SerializableFormula[] Formulas = new SerializableFormula[Hypothesis.CountOfFormulas];

        public static Hypothesis Build(SerializableHypothesis input)
        {
            Hypothesis result = new Hypothesis();
            result.ContingencyTableA = input.ContingencyTableA;
            result.ContingencyTableB = input.ContingencyTableB;
            for (int i = 0; i < Hypothesis.CountOfFormulas; i++)
            {
                result._formulas[i] = SerializableFormula.Build(input.Formulas[i]);
            }
            return result;
        }

        public static SerializableHypothesis Build(Hypothesis input)
        {
            SerializableHypothesis result = new SerializableHypothesis();
            result.ContingencyTableA = input.ContingencyTableA;
            result.ContingencyTableB = input.ContingencyTableB;
            for (int i = 0; i < Hypothesis.CountOfFormulas; i++)
            {
                result.Formulas[i] = SerializableFormula.Build(input._formulas[i]);
            }
            return result;
        }
    }

    public class Result
    {
        public long AllObjectsCount;
        public List<Hypothesis> Hypotheses = new List<Hypothesis>();
        public TaskTypeEnum TaskTypeEnum;
        
        public bool TwoContingencyTables
        {
            get
            {
                switch (TaskTypeEnum)
                {
                    case TaskTypeEnum.FourFold:
                    case TaskTypeEnum.CF:
                    case TaskTypeEnum.KL:
                        return false;
                    case TaskTypeEnum.SDFourFold:
                    case TaskTypeEnum.SDCF:
                    case TaskTypeEnum.SDKL:
                        return true;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public MarkEnum[] GetSemanticMarks()
        {
            switch (TaskTypeEnum)
            {
                case TaskTypeEnum.FourFold:
                    return new MarkEnum[] { MarkEnum.Antecedent, MarkEnum.Succedent, MarkEnum.Condition };
                case TaskTypeEnum.SDFourFold:
                    return new MarkEnum[] { MarkEnum.Antecedent, MarkEnum.Succedent, MarkEnum.Condition, MarkEnum.FirstSet, MarkEnum.SecondSet };
                case TaskTypeEnum.KL:
                    return new MarkEnum[] { MarkEnum.RowAttribute, MarkEnum.ColumnAttribute, MarkEnum.Condition };
                case TaskTypeEnum.SDKL:
                    return new MarkEnum[] { MarkEnum.RowAttribute, MarkEnum.ColumnAttribute, MarkEnum.Condition, MarkEnum.FirstSet, MarkEnum.SecondSet };
                case TaskTypeEnum.CF:
                    return new MarkEnum[] { MarkEnum.Attribute, MarkEnum.Condition };
                case TaskTypeEnum.SDCF:
                    return new MarkEnum[] { MarkEnum.Attribute, MarkEnum.Condition, MarkEnum.FirstSet, MarkEnum.SecondSet };
                default:
                    throw new NotImplementedException();
            }
        }
    }


    [Serializable()]
    public class SerializableResult
    {
        public long AllObjectsCount;
        public SerializableHypothesis[] Hypotheses;
        public TaskTypeEnum TaskTypeEnum;

        public SerializableResult()
        {
        }

        public static Result Build(SerializableResult input)
        {
            Result result = new Result();
            result.AllObjectsCount = input.AllObjectsCount;
            result.TaskTypeEnum = input.TaskTypeEnum;
            if (input.Hypotheses == null)
                result.Hypotheses = new List<Hypothesis>();
            else
            {
                result.Hypotheses = new List<Hypothesis>(input.Hypotheses.Length);
                for (int i = 0; i < input.Hypotheses.Length; i++)
                {
                    result.Hypotheses.Add(SerializableHypothesis.Build(input.Hypotheses[i]));
                }
            }
            return result;
        }

        public static SerializableResult Build(Result input)
        {
            SerializableResult result = new SerializableResult();
            result.AllObjectsCount = input.AllObjectsCount;
            result.TaskTypeEnum = input.TaskTypeEnum;
            if (input.Hypotheses == null)
                result.Hypotheses = new SerializableHypothesis[0];
            else
            {
                result.Hypotheses = new SerializableHypothesis[input.Hypotheses.Count];
                for (int i = 0; i < input.Hypotheses.Count; i++)
                {
                    result.Hypotheses[i] = SerializableHypothesis.Build(input.Hypotheses[i]);
                }
            }
            return result;
        }

        public static string Serialize(Result input)
        {
            SerializableResult serializable = SerializableResult.Build(input);
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableResult));
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            serializer.Serialize(writer, serializable);
            return sb.ToString();
        }

        public static Result DeSerialize(string input)
        {
            StringReader reader = new StringReader(input);
            XmlSerializer deserealizer = new XmlSerializer(typeof(SerializableResult));
            object deserealized = deserealizer.Deserialize(reader);
            SerializableResult serializable = (SerializableResult)deserealized;
            return SerializableResult.Build(serializable);
        }
    }
}
