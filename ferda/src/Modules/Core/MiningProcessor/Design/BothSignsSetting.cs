using System;

namespace Design
{
    public class BothSignsSetting : ISingleOperandEntitySetting
    {
        #region ISingleOperandEntitySetting Members

        public EntityImporatancePair Operand
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