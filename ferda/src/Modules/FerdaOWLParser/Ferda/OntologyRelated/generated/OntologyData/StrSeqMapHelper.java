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

public final class StrSeqMapHelper
{
    public static void
    write(IceInternal.BasicStream __os, java.util.Map<java.lang.String, String[]> __v)
    {
        if(__v == null)
        {
            __os.writeSize(0);
        }
        else
        {
            __os.writeSize(__v.size());
            for(java.util.Map.Entry<java.lang.String, String[]> __e : __v.entrySet())
            {
                __os.writeString(__e.getKey());
                StrSeqHelper.write(__os, __e.getValue());
            }
        }
    }

    public static java.util.Map<java.lang.String, String[]>
    read(IceInternal.BasicStream __is)
    {
        java.util.Map<java.lang.String, String[]> __v;
        __v = new java.util.HashMap<java.lang.String, String[]>();
        int __sz0 = __is.readSize();
        for(int __i0 = 0; __i0 < __sz0; __i0++)
        {
            String __key;
            __key = __is.readString();
            String[] __value;
            __value = StrSeqHelper.read(__is);
            __v.put(__key, __value);
        }
        return __v;
    }
}
