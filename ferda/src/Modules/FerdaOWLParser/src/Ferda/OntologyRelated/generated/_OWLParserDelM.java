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

public final class _OWLParserDelM extends Ice._ObjectDelM implements _OWLParserDel
{
    public Ferda.OntologyRelated.generated.OntologyData.OntologyStructure
    parseOntology(String ontologyURL, java.util.Map<String, String> __ctx)
        throws IceInternal.LocalExceptionWrapper,
               Ferda.OntologyRelated.generated.OntologyData.WrongOntologyURL
    {
        IceInternal.Outgoing __og = __connection.getOutgoing(__reference, "parseOntology", Ice.OperationMode.Normal, __ctx, __compress);
        try
        {
            try
            {
                IceInternal.BasicStream __os = __og.os();
                __os.writeString(ontologyURL);
            }
            catch(Ice.LocalException __ex)
            {
                __og.abort(__ex);
            }
            boolean __ok = __og.invoke();
            try
            {
                IceInternal.BasicStream __is = __og.is();
                if(!__ok)
                {
                    try
                    {
                        __is.throwException();
                    }
                    catch(Ferda.OntologyRelated.generated.OntologyData.WrongOntologyURL __ex)
                    {
                        throw __ex;
                    }
                    catch(Ice.UserException __ex)
                    {
                        throw new Ice.UnknownUserException(__ex.ice_name());
                    }
                }
                Ferda.OntologyRelated.generated.OntologyData.OntologyStructure __ret;
                __ret = new Ferda.OntologyRelated.generated.OntologyData.OntologyStructure();
                __ret.__read(__is);
                return __ret;
            }
            catch(Ice.LocalException __ex)
            {
                throw new IceInternal.LocalExceptionWrapper(__ex, false);
            }
        }
        finally
        {
            __connection.reclaimOutgoing(__og);
        }
    }
}
