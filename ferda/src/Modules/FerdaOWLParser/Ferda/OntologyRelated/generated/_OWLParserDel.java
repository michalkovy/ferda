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

public interface _OWLParserDel extends Ice._ObjectDel
{
    Ferda.OntologyRelated.generated.OntologyData.OntologyStructure parseOntology(String ontologyURL, java.util.Map<String, String> __ctx)
        throws IceInternal.LocalExceptionWrapper,
               Ferda.OntologyRelated.generated.OntologyData.WrongOntologyURL;
}
