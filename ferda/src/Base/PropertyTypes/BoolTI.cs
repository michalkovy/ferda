using Ice;
using System;

namespace Ferda.Modules
{
    public class BoolTI : BoolT, BoolTInterfaceOperations_, IValue
    {
        public ValueT getValueT()
        {
            BoolValueT result = new BoolValueT();
            result.Value = this;
            return result;
        }

        public BoolTI()
        { }

        public BoolTI(bool boolValue)
        {
            this.boolValue = boolValue;
        }

        public BoolTI(BoolTInterfacePrx iface)
        {
        	if (iface != null)
            	this.boolValue = iface.getBoolValue();
        }

        public static implicit operator bool(BoolTI v)
        {
            return v.boolValue;
        }

        public static implicit operator BoolTI(bool v)
        {
            return new BoolTI(v);
        }
        
        /// <summary>
        /// Method getBoolValue
        /// </summary>
        /// <returns>A bool</returns>
        /// <param name="__current">An Ice.Current</param>
        public virtual bool getBoolValue(Current __current)
        {
            return this.boolValue;
        }

        public virtual short getShortValue(Current __current)
        {
            return this.boolValue ? (short)1 : (short)0;
        }

        public virtual int getIntValue(Current __current)
        {
            return this.boolValue ? 1 : 0;
        }

        public virtual long getLongValue(Current __current)
        {
            return this.boolValue ? 1 : 0;
        }

        public virtual float getFloatValue(Current __current)
        {
            return this.boolValue ? 1 : 0;
        }

        public virtual double getDoubleValue(Current __current)
        {
            return this.boolValue ? 1 : 0;
        }

        public virtual String getStringValue(Current __current)
        {
            return this.boolValue.ToString();
        }
    }
}
