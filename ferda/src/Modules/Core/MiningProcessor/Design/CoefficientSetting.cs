using System;

namespace Design
{
    public class CoefficientSetting : ILeafEntitySetting
    {
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

        public CoefficientType CoefficientType
        {
            get { throw new NotImplementedException(); }
            set
            {
            }
        }

        #region ILeafEntitySetting Members

        public BitStringGeneratorPrx BitStringGeneratorPrx
        {
            get { throw new Exception("The method or operation is not implemented."); }
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