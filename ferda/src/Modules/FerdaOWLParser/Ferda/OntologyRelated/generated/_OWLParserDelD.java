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

public final class _OWLParserDelD extends Ice._ObjectDelD implements _OWLParserDel
{
    public Ferda.OntologyRelated.generated.OntologyData.OntologyStructure
    parseOntology(String ontologyURL, java.util.Map<String, String> __ctx)
        throws IceInternal.LocalExceptionWrapper,
               Ferda.OntologyRelated.generated.OntologyData.WrongOntologyURL
    {
        Ice.Current __current = new Ice.Current();
        __initCurrent(__current, "parseOntology", Ice.OperationMode.Normal, __ctx);
        while(true)
        {
            IceInternal.Direct __direct = new IceInternal.Direct(__current);
            try
            {
                OWLParser __servant = null;
                try
                {
                    __servant = (OWLParser)__direct.servant();
                }
                catch(ClassCastException __ex)
                {
                    Ice.OperationNotExistException __opEx = new Ice.OperationNotExistException();
                    __opEx.id = __current.id;
                    __opEx.facet = __current.facet;
                    __opEx.operation = __current.operation;
                    throw __opEx;
                }
                try
                {
                    return __servant.parseOntology(ontologyURL, __current);
                }
                catch(Ice.LocalException __ex)
                {
                    throw new IceInternal.LocalExceptionWrapper(__ex, false);
                }
            }
            finally
            {
                __direct.destroy();
            }
        }
    }
}
