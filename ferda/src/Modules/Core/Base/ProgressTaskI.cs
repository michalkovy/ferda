using System;
using System.Collections.Generic;
using System.Text;
using Ice;

namespace Ferda.Modules
{
    public abstract class ProgressTaskI : ProgressTaskDisp_
    {
        public static ProgressTaskPrx Create(ObjectAdapter adapter, ProgressTaskDisp_ progressTaskI)
        {
            return ProgressTaskPrxHelper.uncheckedCast(adapter.addWithUUID(progressTaskI));
        }

        public static void Destroy(ObjectAdapter adapter, ProgressTaskPrx prx)
        {
            adapter.remove(prx.ice_getIdentity());
        }

        //public override float getValue(out string message, Current current__)
        //{
        //    throw new System.Exception("The method or operation is not implemented.");
        //}

        //public override void stop(Current current__)
        //{
        //    throw new System.Exception("The method or operation is not implemented.");
        //}
    }
}
