// **********************************************************************
//
// Copyright (c) 2003-2007 ZeroC, Inc. All rights reserved.
//
// This copy of Ice is licensed to you under the terms described in the
// ICE_LICENSE file included in this distribution.
//
// **********************************************************************

// Ice version 3.2.1

package Ferda.OntologyRelated.generated.OntologyData;

public class WrongOntologyURL extends Ice.UserException
{
    public WrongOntologyURL()
    {
    }

    public WrongOntologyURL(String ontologyURL)
    {
        this.ontologyURL = ontologyURL;
    }

    public String
    ice_name()
    {
        return "Ferda::OntologyRelated::generated::OntologyData::WrongOntologyURL";
    }

    public String ontologyURL;

    public void
    __write(IceInternal.BasicStream __os)
    {
        __os.writeString("::Ferda::OntologyRelated::generated::OntologyData::WrongOntologyURL");
        __os.startWriteSlice();
        __os.writeString(ontologyURL);
        __os.endWriteSlice();
    }

    public void
    __read(IceInternal.BasicStream __is, boolean __rid)
    {
        if(__rid)
        {
            __is.readString();
        }
        __is.startReadSlice();
        ontologyURL = __is.readString();
        __is.endReadSlice();
    }

    public void
    __write(Ice.OutputStream __outS)
    {
        Ice.MarshalException ex = new Ice.MarshalException();
        ex.reason = "exception Ferda::OntologyRelated::generated::OntologyData::WrongOntologyURL was not generated with stream support";
        throw ex;
    }

    public void
    __read(Ice.InputStream __inS, boolean __rid)
    {
        Ice.MarshalException ex = new Ice.MarshalException();
        ex.reason = "exception Ferda::OntologyRelated::generated::OntologyData::WrongOntologyURL was not generated with stream support";
        throw ex;
    }
}
