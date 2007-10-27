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

public final class ObjectProperty implements java.lang.Cloneable
{
    public String name;

    public String[] Annotations;

    public String[] Domains;

    public String[] Ranges;

    public ObjectProperty()
    {
    }

    public ObjectProperty(String name, String[] Annotations, String[] Domains, String[] Ranges)
    {
        this.name = name;
        this.Annotations = Annotations;
        this.Domains = Domains;
        this.Ranges = Ranges;
    }

    public boolean
    equals(java.lang.Object rhs)
    {
        if(this == rhs)
        {
            return true;
        }
        ObjectProperty _r = null;
        try
        {
            _r = (ObjectProperty)rhs;
        }
        catch(ClassCastException ex)
        {
        }

        if(_r != null)
        {
            if(name != _r.name && name != null && !name.equals(_r.name))
            {
                return false;
            }
            if(!java.util.Arrays.equals(Annotations, _r.Annotations))
            {
                return false;
            }
            if(!java.util.Arrays.equals(Domains, _r.Domains))
            {
                return false;
            }
            if(!java.util.Arrays.equals(Ranges, _r.Ranges))
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
        if(name != null)
        {
            __h = 5 * __h + name.hashCode();
        }
        if(Annotations != null)
        {
            for(int __i0 = 0; __i0 < Annotations.length; __i0++)
            {
                if(Annotations[__i0] != null)
                {
                    __h = 5 * __h + Annotations[__i0].hashCode();
                }
            }
        }
        if(Domains != null)
        {
            for(int __i1 = 0; __i1 < Domains.length; __i1++)
            {
                if(Domains[__i1] != null)
                {
                    __h = 5 * __h + Domains[__i1].hashCode();
                }
            }
        }
        if(Ranges != null)
        {
            for(int __i2 = 0; __i2 < Ranges.length; __i2++)
            {
                if(Ranges[__i2] != null)
                {
                    __h = 5 * __h + Ranges[__i2].hashCode();
                }
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
        __os.writeString(name);
        StrSeqHelper.write(__os, Annotations);
        StrSeqHelper.write(__os, Domains);
        StrSeqHelper.write(__os, Ranges);
    }

    public void
    __read(IceInternal.BasicStream __is)
    {
        name = __is.readString();
        Annotations = StrSeqHelper.read(__is);
        Domains = StrSeqHelper.read(__is);
        Ranges = StrSeqHelper.read(__is);
    }
}
