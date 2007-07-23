// **********************************************************************
//
// Copyright (c) 2003-2007 ZeroC, Inc. All rights reserved.
//
// This copy of Ice is licensed to you under the terms described in the
// ICE_LICENSE file included in this distribution.
//
// **********************************************************************

// Ice version 3.2.0
// Generated from file `OntologyMapping.ice'

using _System = System;
using _Microsoft = Microsoft;

namespace Ferda
{
    namespace Modules
    {
        namespace Boxes
        {
            namespace OntologyRelated
            {
                namespace OntologyMapping
                {
                    public interface OntologyMappingFunctions : Ice.Object, OntologyMappingFunctionsOperations_, OntologyMappingFunctionsOperationsNC_
                    {
                    }
                }
            }
        }
    }
}

namespace Ferda
{
    namespace Modules
    {
        namespace Boxes
        {
            namespace OntologyRelated
            {
                namespace OntologyMapping
                {
                    public interface OntologyMappingFunctionsPrx : Ice.ObjectPrx
                    {
                        string HelloWorld();
                        string HelloWorld(Ice.Context context__);
                    }
                }
            }
        }
    }
}

namespace Ferda
{
    namespace Modules
    {
        namespace Boxes
        {
            namespace OntologyRelated
            {
                namespace OntologyMapping
                {
                    public interface OntologyMappingFunctionsOperations_
                    {
                        string HelloWorld(Ice.Current current__);
                    }

                    public interface OntologyMappingFunctionsOperationsNC_
                    {
                        string HelloWorld();
                    }
                }
            }
        }
    }
}

namespace Ferda
{
    namespace Modules
    {
        namespace Boxes
        {
            namespace OntologyRelated
            {
                namespace OntologyMapping
                {
                    public sealed class OntologyMappingFunctionsPrxHelper : Ice.ObjectPrxHelperBase, OntologyMappingFunctionsPrx
                    {
                        #region Synchronous operations

                        public string HelloWorld()
                        {
                            return HelloWorld(null, false);
                        }

                        public string HelloWorld(Ice.Context context__)
                        {
                            return HelloWorld(context__, true);
                        }

                        private string HelloWorld(Ice.Context context__, bool explicitContext__)
                        {
                            if(explicitContext__ && context__ == null)
                            {
                                context__ = emptyContext_;
                            }
                            int cnt__ = 0;
                            while(true)
                            {
                                Ice.ObjectDel_ delBase__ = null;
                                try
                                {
                                    checkTwowayOnly__("HelloWorld");
                                    delBase__ = getDelegate__();
                                    OntologyMappingFunctionsDel_ del__ = (OntologyMappingFunctionsDel_)delBase__;
                                    return del__.HelloWorld(context__);
                                }
                                catch(IceInternal.LocalExceptionWrapper ex__)
                                {
                                    cnt__ = handleExceptionWrapperRelaxed__(delBase__, ex__, cnt__);
                                }
                                catch(Ice.LocalException ex__)
                                {
                                    cnt__ = handleException__(delBase__, ex__, cnt__);
                                }
                            }
                        }

                        #endregion

                        #region Checked and unchecked cast operations

                        public static OntologyMappingFunctionsPrx checkedCast(Ice.ObjectPrx b)
                        {
                            if(b == null)
                            {
                                return null;
                            }
                            if(b is OntologyMappingFunctionsPrx)
                            {
                                return (OntologyMappingFunctionsPrx)b;
                            }
                            if(b.ice_isA("::Ferda::Modules::Boxes::OntologyRelated::OntologyMapping::OntologyMappingFunctions"))
                            {
                                OntologyMappingFunctionsPrxHelper h = new OntologyMappingFunctionsPrxHelper();
                                h.copyFrom__(b);
                                return h;
                            }
                            return null;
                        }

                        public static OntologyMappingFunctionsPrx checkedCast(Ice.ObjectPrx b, Ice.Context ctx)
                        {
                            if(b == null)
                            {
                                return null;
                            }
                            if(b is OntologyMappingFunctionsPrx)
                            {
                                return (OntologyMappingFunctionsPrx)b;
                            }
                            if(b.ice_isA("::Ferda::Modules::Boxes::OntologyRelated::OntologyMapping::OntologyMappingFunctions", ctx))
                            {
                                OntologyMappingFunctionsPrxHelper h = new OntologyMappingFunctionsPrxHelper();
                                h.copyFrom__(b);
                                return h;
                            }
                            return null;
                        }

                        public static OntologyMappingFunctionsPrx checkedCast(Ice.ObjectPrx b, string f)
                        {
                            if(b == null)
                            {
                                return null;
                            }
                            Ice.ObjectPrx bb = b.ice_facet(f);
                            try
                            {
                                if(bb.ice_isA("::Ferda::Modules::Boxes::OntologyRelated::OntologyMapping::OntologyMappingFunctions"))
                                {
                                    OntologyMappingFunctionsPrxHelper h = new OntologyMappingFunctionsPrxHelper();
                                    h.copyFrom__(bb);
                                    return h;
                                }
                            }
                            catch(Ice.FacetNotExistException)
                            {
                            }
                            return null;
                        }

                        public static OntologyMappingFunctionsPrx checkedCast(Ice.ObjectPrx b, string f, Ice.Context ctx)
                        {
                            if(b == null)
                            {
                                return null;
                            }
                            Ice.ObjectPrx bb = b.ice_facet(f);
                            try
                            {
                                if(bb.ice_isA("::Ferda::Modules::Boxes::OntologyRelated::OntologyMapping::OntologyMappingFunctions", ctx))
                                {
                                    OntologyMappingFunctionsPrxHelper h = new OntologyMappingFunctionsPrxHelper();
                                    h.copyFrom__(bb);
                                    return h;
                                }
                            }
                            catch(Ice.FacetNotExistException)
                            {
                            }
                            return null;
                        }

                        public static OntologyMappingFunctionsPrx uncheckedCast(Ice.ObjectPrx b)
                        {
                            if(b == null)
                            {
                                return null;
                            }
                            OntologyMappingFunctionsPrxHelper h = new OntologyMappingFunctionsPrxHelper();
                            h.copyFrom__(b);
                            return h;
                        }

                        public static OntologyMappingFunctionsPrx uncheckedCast(Ice.ObjectPrx b, string f)
                        {
                            if(b == null)
                            {
                                return null;
                            }
                            Ice.ObjectPrx bb = b.ice_facet(f);
                            OntologyMappingFunctionsPrxHelper h = new OntologyMappingFunctionsPrxHelper();
                            h.copyFrom__(bb);
                            return h;
                        }

                        #endregion

                        #region Marshaling support

                        protected override Ice.ObjectDelM_ createDelegateM__()
                        {
                            return new OntologyMappingFunctionsDelM_();
                        }

                        protected override Ice.ObjectDelD_ createDelegateD__()
                        {
                            return new OntologyMappingFunctionsDelD_();
                        }

                        public static void write__(IceInternal.BasicStream os__, OntologyMappingFunctionsPrx v__)
                        {
                            os__.writeProxy(v__);
                        }

                        public static OntologyMappingFunctionsPrx read__(IceInternal.BasicStream is__)
                        {
                            Ice.ObjectPrx proxy = is__.readProxy();
                            if(proxy != null)
                            {
                                OntologyMappingFunctionsPrxHelper result = new OntologyMappingFunctionsPrxHelper();
                                result.copyFrom__(proxy);
                                return result;
                            }
                            return null;
                        }

                        #endregion
                    }
                }
            }
        }
    }
}

namespace Ferda
{
    namespace Modules
    {
        namespace Boxes
        {
            namespace OntologyRelated
            {
                namespace OntologyMapping
                {
                    public interface OntologyMappingFunctionsDel_ : Ice.ObjectDel_
                    {
                        string HelloWorld(Ice.Context context__);
                    }
                }
            }
        }
    }
}

namespace Ferda
{
    namespace Modules
    {
        namespace Boxes
        {
            namespace OntologyRelated
            {
                namespace OntologyMapping
                {
                    public sealed class OntologyMappingFunctionsDelM_ : Ice.ObjectDelM_, OntologyMappingFunctionsDel_
                    {
                        public string HelloWorld(Ice.Context context__)
                        {
                            IceInternal.Outgoing og__ = getOutgoing("HelloWorld", Ice.OperationMode.Nonmutating, context__);
                            try
                            {
                                bool ok__ = og__.invoke();
                                try
                                {
                                    IceInternal.BasicStream is__ = og__.istr();
                                    if(!ok__)
                                    {
                                        try
                                        {
                                            is__.throwException();
                                        }
                                        catch(Ice.UserException ex)
                                        {
                                            throw new Ice.UnknownUserException(ex);
                                        }
                                    }
                                    string ret__;
                                    ret__ = is__.readString();
                                    return ret__;
                                }
                                catch(Ice.LocalException ex__)
                                {
                                    throw new IceInternal.LocalExceptionWrapper(ex__, false);
                                }
                            }
                            finally
                            {
                                reclaimOutgoing(og__);
                            }
                        }
                    }
                }
            }
        }
    }
}

namespace Ferda
{
    namespace Modules
    {
        namespace Boxes
        {
            namespace OntologyRelated
            {
                namespace OntologyMapping
                {
                    public sealed class OntologyMappingFunctionsDelD_ : Ice.ObjectDelD_, OntologyMappingFunctionsDel_
                    {
                        public string HelloWorld(Ice.Context context__)
                        {
                            Ice.Current current__ = new Ice.Current();
                            initCurrent__(ref current__, "HelloWorld", Ice.OperationMode.Nonmutating, context__);
                            while(true)
                            {
                                IceInternal.Direct direct__ = new IceInternal.Direct(current__);
                                object servant__ = direct__.servant();
                                if(servant__ is OntologyMappingFunctions)
                                {
                                    try
                                    {
                                        return ((Ferda.Modules.Boxes.OntologyRelated.OntologyMapping.OntologyMappingFunctions)servant__).HelloWorld(current__);
                                    }
                                    catch(Ice.LocalException ex__)
                                    {
                                        throw new IceInternal.LocalExceptionWrapper(ex__, false);
                                    }
                                    finally
                                    {
                                        direct__.destroy();
                                    }
                                }
                                else
                                {
                                    direct__.destroy();
                                    Ice.OperationNotExistException opEx__ = new Ice.OperationNotExistException();
                                    opEx__.id = current__.id;
                                    opEx__.facet = current__.facet;
                                    opEx__.operation = current__.operation;
                                    throw opEx__;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

namespace Ferda
{
    namespace Modules
    {
        namespace Boxes
        {
            namespace OntologyRelated
            {
                namespace OntologyMapping
                {
                    public abstract class OntologyMappingFunctionsDisp_ : Ice.ObjectImpl, OntologyMappingFunctions
                    {
                        #region Slice operations

                        public string HelloWorld()
                        {
                            return HelloWorld(Ice.ObjectImpl.defaultCurrent);
                        }

                        public abstract string HelloWorld(Ice.Current current__);

                        #endregion

                        #region Slice type-related members

                        public static new string[] ids__ = 
                        {
                            "::Ferda::Modules::Boxes::OntologyRelated::OntologyMapping::OntologyMappingFunctions",
                            "::Ice::Object"
                        };

                        public override bool ice_isA(string s)
                        {
                            if(IceInternal.AssemblyUtil.runtime_ == IceInternal.AssemblyUtil.Runtime.Mono)
                            {
                                // Mono bug: System.Array.BinarySearch() uses the wrong collation sequence,
                                // so we do a linear search for the time being
                                int pos = 0;
                                while(pos < ids__.Length)
                                {
                                    if(ids__[pos] == s)
                                    {
                                        break;
                                    }
                                    ++pos;
                                }
                                if(pos == ids__.Length)
                                {
                                    pos = -1;
                                }
                                return pos >= 0;
                            }
                            else
                            {
                                return _System.Array.BinarySearch(ids__, s, _System.Collections.Comparer.DefaultInvariant) >= 0;
                            }
                        }

                        public override bool ice_isA(string s, Ice.Current current__)
                        {
                            if(IceInternal.AssemblyUtil.runtime_ == IceInternal.AssemblyUtil.Runtime.Mono)
                            {
                                // Mono bug: System.Array.BinarySearch() uses the wrong collation sequence,
                                // so we do a linear search for the time being
                                int pos = 0;
                                while(pos < ids__.Length)
                                {
                                    if(ids__[pos] == s)
                                    {
                                        break;
                                    }
                                    ++pos;
                                }
                                if(pos == ids__.Length)
                                {
                                    pos = -1;
                                }
                                return pos >= 0;
                            }
                            else
                            {
                                return _System.Array.BinarySearch(ids__, s, _System.Collections.Comparer.DefaultInvariant) >= 0;
                            }
                        }

                        public override string[] ice_ids()
                        {
                            return ids__;
                        }

                        public override string[] ice_ids(Ice.Current current__)
                        {
                            return ids__;
                        }

                        public override string ice_id()
                        {
                            return ids__[0];
                        }

                        public override string ice_id(Ice.Current current__)
                        {
                            return ids__[0];
                        }

                        public static new string ice_staticId()
                        {
                            return ids__[0];
                        }

                        #endregion

                        #region Operation dispatch

                        public static IceInternal.DispatchStatus HelloWorld___(OntologyMappingFunctions obj__, IceInternal.Incoming inS__, Ice.Current current__)
                        {
                            checkMode__(Ice.OperationMode.Nonmutating, current__.mode);
                            IceInternal.BasicStream os__ = inS__.ostr();
                            string ret__ = obj__.HelloWorld(current__);
                            os__.writeString(ret__);
                            return IceInternal.DispatchStatus.DispatchOK;
                        }

                        private static string[] all__ =
                        {
                            "HelloWorld",
                            "ice_id",
                            "ice_ids",
                            "ice_isA",
                            "ice_ping"
                        };

                        public override IceInternal.DispatchStatus dispatch__(IceInternal.Incoming inS__, Ice.Current current__)
                        {
                            int pos;
                            if(IceInternal.AssemblyUtil.runtime_ == IceInternal.AssemblyUtil.Runtime.Mono)
                            {
                                // Mono bug: System.Array.BinarySearch() uses the wrong collation sequence,
                                // so we do a linear search for the time being
                                pos = 0;
                                while(pos < all__.Length)
                                {
                                    if(all__[pos] == current__.operation)
                                    {
                                        break;
                                    }
                                    ++pos;
                                }
                                if(pos == all__.Length)
                                {
                                    pos = -1;
                                }
                            }
                            else
                            {
                                pos = _System.Array.BinarySearch(all__, current__.operation, _System.Collections.Comparer.DefaultInvariant);
                            }
                            if(pos < 0)
                            {
                                return IceInternal.DispatchStatus.DispatchOperationNotExist;
                            }

                            switch(pos)
                            {
                                case 0:
                                {
                                    return HelloWorld___(this, inS__, current__);
                                }
                                case 1:
                                {
                                    return ice_id___(this, inS__, current__);
                                }
                                case 2:
                                {
                                    return ice_ids___(this, inS__, current__);
                                }
                                case 3:
                                {
                                    return ice_isA___(this, inS__, current__);
                                }
                                case 4:
                                {
                                    return ice_ping___(this, inS__, current__);
                                }
                            }

                            _System.Diagnostics.Debug.Assert(false);
                            return IceInternal.DispatchStatus.DispatchOperationNotExist;
                        }

                        #endregion
                    }
                }
            }
        }
    }
}
