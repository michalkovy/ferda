// **********************************************************************
//
// Copyright (c) 2003-2007 ZeroC, Inc. All rights reserved.
//
// This copy of Ice is licensed to you under the terms described in the
// ICE_LICENSE file included in this distribution.
//
// **********************************************************************

// Ice version 3.2.1

package Ferda.OntologyRelated.generated;

public final class OWLParserPrxHelper extends Ice.ObjectPrxHelperBase implements OWLParserPrx
{
    public Ferda.OntologyRelated.generated.OntologyData.OntologyStructure
    parseOntology(String ontologyURL)
        throws Ferda.OntologyRelated.generated.OntologyData.WrongOntologyURL
    {
        return parseOntology(ontologyURL, null, false);
    }

    public Ferda.OntologyRelated.generated.OntologyData.OntologyStructure
    parseOntology(String ontologyURL, java.util.Map<String, String> __ctx)
        throws Ferda.OntologyRelated.generated.OntologyData.WrongOntologyURL
    {
        return parseOntology(ontologyURL, __ctx, true);
    }

    @SuppressWarnings("unchecked")
    private Ferda.OntologyRelated.generated.OntologyData.OntologyStructure
    parseOntology(String ontologyURL, java.util.Map<String, String> __ctx, boolean __explicitCtx)
        throws Ferda.OntologyRelated.generated.OntologyData.WrongOntologyURL
    {
        if(__explicitCtx && __ctx == null)
        {
            __ctx = _emptyContext;
        }
        int __cnt = 0;
        while(true)
        {
            Ice._ObjectDel __delBase = null;
            try
            {
                __checkTwowayOnly("parseOntology");
                __delBase = __getDelegate();
                _OWLParserDel __del = (_OWLParserDel)__delBase;
                return __del.parseOntology(ontologyURL, __ctx);
            }
            catch(IceInternal.LocalExceptionWrapper __ex)
            {
                __handleExceptionWrapper(__delBase, __ex);
            }
            catch(Ice.LocalException __ex)
            {
                __cnt = __handleException(__delBase, __ex, __cnt);
            }
        }
    }

    public static OWLParserPrx
    checkedCast(Ice.ObjectPrx __obj)
    {
        OWLParserPrx __d = null;
        if(__obj != null)
        {
            try
            {
                __d = (OWLParserPrx)__obj;
            }
            catch(ClassCastException ex)
            {
                if(__obj.ice_isA("::Ferda::OntologyRelated::generated::OWLParser"))
                {
                    OWLParserPrxHelper __h = new OWLParserPrxHelper();
                    __h.__copyFrom(__obj);
                    __d = __h;
                }
            }
        }
        return __d;
    }

    public static OWLParserPrx
    checkedCast(Ice.ObjectPrx __obj, java.util.Map<String, String> __ctx)
    {
        OWLParserPrx __d = null;
        if(__obj != null)
        {
            try
            {
                __d = (OWLParserPrx)__obj;
            }
            catch(ClassCastException ex)
            {
                if(__obj.ice_isA("::Ferda::OntologyRelated::generated::OWLParser", __ctx))
                {
                    OWLParserPrxHelper __h = new OWLParserPrxHelper();
                    __h.__copyFrom(__obj);
                    __d = __h;
                }
            }
        }
        return __d;
    }

    public static OWLParserPrx
    checkedCast(Ice.ObjectPrx __obj, String __facet)
    {
        OWLParserPrx __d = null;
        if(__obj != null)
        {
            Ice.ObjectPrx __bb = __obj.ice_facet(__facet);
            try
            {
                if(__bb.ice_isA("::Ferda::OntologyRelated::generated::OWLParser"))
                {
                    OWLParserPrxHelper __h = new OWLParserPrxHelper();
                    __h.__copyFrom(__bb);
                    __d = __h;
                }
            }
            catch(Ice.FacetNotExistException ex)
            {
            }
        }
        return __d;
    }

    public static OWLParserPrx
    checkedCast(Ice.ObjectPrx __obj, String __facet, java.util.Map<String, String> __ctx)
    {
        OWLParserPrx __d = null;
        if(__obj != null)
        {
            Ice.ObjectPrx __bb = __obj.ice_facet(__facet);
            try
            {
                if(__bb.ice_isA("::Ferda::OntologyRelated::generated::OWLParser", __ctx))
                {
                    OWLParserPrxHelper __h = new OWLParserPrxHelper();
                    __h.__copyFrom(__bb);
                    __d = __h;
                }
            }
            catch(Ice.FacetNotExistException ex)
            {
            }
        }
        return __d;
    }

    public static OWLParserPrx
    uncheckedCast(Ice.ObjectPrx __obj)
    {
        OWLParserPrx __d = null;
        if(__obj != null)
        {
            OWLParserPrxHelper __h = new OWLParserPrxHelper();
            __h.__copyFrom(__obj);
            __d = __h;
        }
        return __d;
    }

    public static OWLParserPrx
    uncheckedCast(Ice.ObjectPrx __obj, String __facet)
    {
        OWLParserPrx __d = null;
        if(__obj != null)
        {
            Ice.ObjectPrx __bb = __obj.ice_facet(__facet);
            OWLParserPrxHelper __h = new OWLParserPrxHelper();
            __h.__copyFrom(__bb);
            __d = __h;
        }
        return __d;
    }

    protected Ice._ObjectDelM
    __createDelegateM()
    {
        return new _OWLParserDelM();
    }

    protected Ice._ObjectDelD
    __createDelegateD()
    {
        return new _OWLParserDelD();
    }

    public static void
    __write(IceInternal.BasicStream __os, OWLParserPrx v)
    {
        __os.writeProxy(v);
    }

    public static OWLParserPrx
    __read(IceInternal.BasicStream __is)
    {
        Ice.ObjectPrx proxy = __is.readProxy();
        if(proxy != null)
        {
            OWLParserPrxHelper result = new OWLParserPrxHelper();
            result.__copyFrom(proxy);
            return result;
        }
        return null;
    }
}
