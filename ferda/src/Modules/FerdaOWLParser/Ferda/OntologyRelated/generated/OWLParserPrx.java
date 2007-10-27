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

public interface OWLParserPrx extends Ice.ObjectPrx
{
    public Ferda.OntologyRelated.generated.OntologyData.OntologyStructure parseOntology(String ontologyURL)
        throws Ferda.OntologyRelated.generated.OntologyData.WrongOntologyURL;
    public Ferda.OntologyRelated.generated.OntologyData.OntologyStructure parseOntology(String ontologyURL, java.util.Map<String, String> __ctx)
        throws Ferda.OntologyRelated.generated.OntologyData.WrongOntologyURL;
}
