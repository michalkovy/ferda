using System;

namespace Design
{
    public class ConjunctionSetting : IMultipleOperandEntitySetting
    {
        #region IMultipleOperandEntitySetting Members

        public ClassOfEquivalence[] ClassesOfEquivalence
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public int MinimalLength
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public int MaximalLength
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public EntityImporatancePair[] Operands
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IEntitySettingBase Members

        public Guid Id
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }
}