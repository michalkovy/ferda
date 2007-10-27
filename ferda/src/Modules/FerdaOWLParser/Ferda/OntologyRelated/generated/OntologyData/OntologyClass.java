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

public final class OntologyClass implements java.lang.Cloneable
{
    public String name;

    public String[] Annotations;

    public String[] SubClasses;

    public String[] SuperClasses;

    public String[] Instances;

    public java.util.Map<java.lang.String, String[]> DataPropertiesMap;

    public OntologyClass()
    {
    }

    public OntologyClass(String name, String[] Annotations, String[] SubClasses, String[] SuperClasses, String[] Instances, java.util.Map<java.lang.String, String[]> DataPropertiesMap)
    {
        this.name = name;
        this.Annotations = Annotations;
        this.SubClasses = SubClasses;
        this.SuperClasses = SuperClasses;
        this.Instances = Instances;
        this.DataPropertiesMap = DataPropertiesMap;
    }

    public boolean
    equals(java.lang.Object rhs)
    {
        if(this == rhs)
        {
            return true;
        }
        OntologyClass _r = null;
        try
        {
            _r = (OntologyClass)rhs;
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
            if(!java.util.Arrays.equals(SubClasses, _r.SubClasses))
            {
                return false;
            }
            if(!java.util.Arrays.equals(SuperClasses, _r.SuperClasses))
            {
                return false;
            }
            if(!java.util.Arrays.equals(Instances, _r.Instances))
            {
                return false;
            }
            if(DataPropertiesMap != _r.DataPropertiesMap && DataPropertiesMap != null && !DataPropertiesMap.equals(_r.DataPropertiesMap))
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
        if(SubClasses != null)
        {
            for(int __i1 = 0; __i1 < SubClasses.length; __i1++)
            {
                if(SubClasses[__i1] != null)
                {
                    __h = 5 * __h + SubClasses[__i1].hashCode();
                }
            }
        }
        if(SuperClasses != null)
        {
            for(int __i2 = 0; __i2 < SuperClasses.length; __i2++)
            {
                if(SuperClasses[__i2] != null)
                {
                    __h = 5 * __h + SuperClasses[__i2].hashCode();
                }
            }
        }
        if(Instances != null)
        {
            for(int __i3 = 0; __i3 < Instances.length; __i3++)
            {
                if(Instances[__i3] != null)
                {
                    __h = 5 * __h + Instances[__i3].hashCode();
                }
            }
        }
        if(DataPropertiesMap != null)
        {
            __h = 5 * __h + DataPropertiesMap.hashCode();
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
        StrSeqHelper.write(__os, SubClasses);
        StrSeqHelper.write(__os, SuperClasses);
        StrSeqHelper.write(__os, Instances);
        StrSeqMapHelper.write(__os, DataPropertiesMap);
    }

    public void
    __read(IceInternal.BasicStream __is)
    {
        name = __is.readString();
        Annotations = StrSeqHelper.read(__is);
        SubClasses = StrSeqHelper.read(__is);
        SuperClasses = StrSeqHelper.read(__is);
        Instances = StrSeqHelper.read(__is);
        DataPropertiesMap = StrSeqMapHelper.read(__is);
    }
}
