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

public final class OntologyStructure implements java.lang.Cloneable
{
    public java.util.Map<java.lang.String, OntologyClass> OntologyClassMap;

    public ObjectProperty[] ObjectProperties;

    public OntologyStructure()
    {
    }

    public OntologyStructure(java.util.Map<java.lang.String, OntologyClass> OntologyClassMap, ObjectProperty[] ObjectProperties)
    {
        this.OntologyClassMap = OntologyClassMap;
        this.ObjectProperties = ObjectProperties;
    }

    public boolean
    equals(java.lang.Object rhs)
    {
        if(this == rhs)
        {
            return true;
        }
        OntologyStructure _r = null;
        try
        {
            _r = (OntologyStructure)rhs;
        }
        catch(ClassCastException ex)
        {
        }

        if(_r != null)
        {
            if(OntologyClassMap != _r.OntologyClassMap && OntologyClassMap != null && !OntologyClassMap.equals(_r.OntologyClassMap))
            {
                return false;
            }
            if(!java.util.Arrays.equals(ObjectProperties, _r.ObjectProperties))
            {
                return false;
            }

            return true;
        }

        return false;
    }

    public int
    hashCode()
    {
        int __h = 0;
        if(OntologyClassMap != null)
        {
            __h = 5 * __h + OntologyClassMap.hashCode();
        }
        if(ObjectProperties != null)
        {
            for(int __i0 = 0; __i0 < ObjectProperties.length; __i0++)
            {
                __h = 5 * __h + ObjectProperties[__i0].hashCode();
            }
        }
        return __h;
    }

    public java.lang.Object
    clone()
    {
        java.lang.Object o = null;
        try
        {
            o = super.clone();
        }
        catch(CloneNotSupportedException ex)
        {
            assert false; // impossible
        }
        return o;
    }

    public void
    __write(IceInternal.BasicStream __os)
    {
        dictionaryStringOntologyClassHelper.write(__os, OntologyClassMap);
        sequenceObjectPropertyHelper.write(__os, ObjectProperties);
    }

    public void
    __read(IceInternal.BasicStream __is)
    {
        OntologyClassMap = dictionaryStringOntologyClassHelper.read(__is);
        ObjectProperties = sequenceObjectPropertyHelper.read(__is);
    }
}
