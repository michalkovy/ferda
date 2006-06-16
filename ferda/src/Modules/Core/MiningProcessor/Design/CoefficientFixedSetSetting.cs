using System;

namespace Design
{
    public class CoefficientFixedSetSetting : ILeafEntitySetting
    {
        public string[] CategoriesIds
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