// **********************************************************************
//
// Copyright (c) 2003-2007 ZeroC, Inc. All rights reserved.
//
// This copy of Ice is licensed to you under the terms described in the
// ICE_LICENSE file included in this distribution.
//
// **********************************************************************

// Ice version 3.2.1
// Generated from file `OntologyData.ice'

using _System = System;
using _Microsoft = Microsoft;

namespace Ferda
{
    namespace OntologyRelated
    {
        namespace generated
        {
            namespace OntologyData
            {
                public class WrongOntologyURL : Ice.UserException
                {
                    #region Slice data members

                    public string ontologyURL;

                    #endregion

                    #region Constructors

                    public WrongOntologyURL()
                    {
                    }

                    public WrongOntologyURL(_System.Exception ex__) : base(ex__)
                    {
                    }

                    private void initDM__(string ontologyURL)
                    {
                        this.ontologyURL = ontologyURL;
                    }

                    public WrongOntologyURL(string ontologyURL)
                    {
                        initDM__(ontologyURL);
                    }

                    public WrongOntologyURL(string ontologyURL, _System.Exception ex__) : base(ex__)
                    {
                        initDM__(ontologyURL);
                    }

                    #endregion

                    #region Object members

                    public override int GetHashCode()
                    {
                        int h__ = 0;
                        if((object)ontologyURL != null)
                        {
                            h__ = 5 * h__ + ontologyURL.GetHashCode();
                        }
                        return h__;
                    }

                    public override bool Equals(object other__)
                    {
                        if(other__ == null)
                        {
                            return false;
                        }
                        if(object.ReferenceEquals(this, other__))
                        {
                            return true;
                        }
                        if(!(other__ is WrongOntologyURL))
                        {
                            return false;
                        }
                        if(ontologyURL == null)
                        {
                            if(((WrongOntologyURL)other__).ontologyURL != null)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if(!ontologyURL.Equals(((WrongOntologyURL)other__).ontologyURL))
                            {
                                return false;
                            }
                        }
                        return true;
                    }

                    #endregion

                    #region Comparison members

                    public static bool operator==(WrongOntologyURL lhs__, WrongOntologyURL rhs__)
                    {
                        return Equals(lhs__, rhs__);
                    }

                    public static bool operator!=(WrongOntologyURL lhs__, WrongOntologyURL rhs__)
                    {
                        return !Equals(lhs__, rhs__);
                    }

                    #endregion

                    #region Marshaling support

                    public override void write__(IceInternal.BasicStream os__)
                    {
                        os__.writeString("::Ferda::OntologyRelated::generated::OntologyData::WrongOntologyURL");
                        os__.startWriteSlice();
                        os__.writeString(ontologyURL);
                        os__.endWriteSlice();
                    }

                    public override void read__(IceInternal.BasicStream is__, bool rid__)
                    {
                        if(rid__)
                        {
                            /* string myId = */ is__.readString();
                        }
                        is__.startReadSlice();
                        ontologyURL = is__.readString();
                        is__.endReadSlice();
                    }

                    public override void write__(Ice.OutputStream outS__)
                    {
                        Ice.MarshalException ex = new Ice.MarshalException();
                        ex.reason = "exception Ferda::OntologyRelated::generated::OntologyData::WrongOntologyURL was not generated with stream support";
                        throw ex;
                    }

                    public override void read__(Ice.InputStream inS__, bool rid__)
                    {
                        Ice.MarshalException ex = new Ice.MarshalException();
                        ex.reason = "exception Ferda::OntologyRelated::generated::OntologyData::WrongOntologyURL was not generated with stream support";
                        throw ex;
                    }

                    public override bool usesClasses__()
                    {
                        return true;
                    }

                    #endregion
                }

                public class StrSeqMap : _System.Collections.DictionaryBase, _System.ICloneable
                {
                    #region StrSeqMap members

                    public void AddRange(StrSeqMap d__)
                    {
                        foreach(_System.Collections.DictionaryEntry e in d__)
                        {
                            try
                            {
                                InnerHashtable.Add(e.Key, e.Value);
                            }
                            catch(_System.ArgumentException)
                            {
                                // ignore
                            }
                        }
                    }

                    #endregion

                    #region IDictionary members

                    public bool IsFixedSize
                    {
                        get
                        {
                            return false;
                        }
                    }

                    public bool IsReadOnly
                    {
                        get
                        {
                            return false;
                        }
                    }

                    public _System.Collections.ICollection Keys
                    {
                        get
                        {
                            return InnerHashtable.Keys;
                        }
                    }

                    public _System.Collections.ICollection Values
                    {
                        get
                        {
                            return InnerHashtable.Values;
                        }
                    }

                    #region Indexer

                    public string[] this[string key]
                    {
                        get
                        {
                            return (string[])InnerHashtable[key];
                        }
                        set
                        {
                            InnerHashtable[key] = value;
                        }
                    }

                    #endregion

                    public void Add(string key, string[] value)
                    {
                        InnerHashtable.Add(key, value);
                    }

                    public void Remove(string key)
                    {
                        InnerHashtable.Remove(key);
                    }

                    public bool Contains(string key)
                    {
                        return InnerHashtable.Contains(key);
                    }

                    #endregion

                    #region ICollection members

                    public bool IsSynchronized
                    {
                        get
                        {
                            return false;
                        }
                    }

                    public object SyncRoot
                    {
                        get
                        {
                            return this;
                        }
                    }

                    #endregion

                    #region ICloneable members

                    public object Clone()
                    {
                        StrSeqMap d = new StrSeqMap();
                        foreach(_System.Collections.DictionaryEntry e in InnerHashtable)
                        {
                            d.InnerHashtable.Add(e.Key, e.Value);
                        }
                        return d;
                    }

                    #endregion

                    #region Object members

                    public override int GetHashCode()
                    {
                        int hash = 0;
                        foreach(_System.Collections.DictionaryEntry e in InnerHashtable)
                        {
                            hash = 5 * hash + e.Key.GetHashCode();
                            if(e.Value != null)
                            {
                                hash = 5 * hash + e.Value.GetHashCode();
                            }
                        }
                        return hash;
                    }

                    public override bool Equals(object other)
                    {
                        if(object.ReferenceEquals(this, other))
                        {
                            return true;
                        }
                        if(!(other is StrSeqMap))
                        {
                            return false;
                        }
                        if(Count != ((StrSeqMap)other).Count)
                        {
                            return false;
                        }
                        string[] klhs__ = new string[Count];
                        Keys.CopyTo(klhs__, 0);
                        _System.Array.Sort(klhs__);
                        string[] krhs__ = new string[((StrSeqMap)other).Count];
                        ((StrSeqMap)other).Keys.CopyTo(krhs__, 0);
                        _System.Array.Sort(krhs__);
                        for(int i = 0; i < Count; ++i)
                        {
                            if(!klhs__[i].Equals(krhs__[i]))
                            {
                                return false;
                            }
                        }
                        string[][] vlhs__ = new string[Count][];
                        Values.CopyTo(vlhs__, 0);
                        _System.Array.Sort(vlhs__);
                        string[][] vrhs__ = new string[((StrSeqMap)other).Count][];
                        ((StrSeqMap)other).Values.CopyTo(vrhs__, 0);
                        _System.Array.Sort(vrhs__);
                        for(int i = 0; i < Count; ++i)
                        {
                            if(vlhs__[i] == null)
                            {
                                if(vrhs__[i] != null)
                                {
                                    return false;
                                }
                            }
                            else if(!vlhs__[i].Equals(vrhs__[i]))
                            {
                                return false;
                            }
                        }
                        return true;
                    }

                    #endregion

                    #region Comparison members

                    public static bool operator==(StrSeqMap lhs__, StrSeqMap rhs__)
                    {
                        return Equals(lhs__, rhs__);
                    }

                    public static bool operator!=(StrSeqMap lhs__, StrSeqMap rhs__)
                    {
                        return !Equals(lhs__, rhs__);
                    }

                    #endregion
                }

                public class OntologyClass : _System.ICloneable
                {
                    #region Slice data members

                    public string name;

                    public string[] Annotations;

                    public string[] SubClasses;

                    public string[] SuperClasses;

                    public string[] Instances;

                    public Ferda.OntologyRelated.generated.OntologyData.StrSeqMap DataPropertiesMap;

                    #endregion

                    #region Constructors

                    public OntologyClass()
                    {
                    }

                    public OntologyClass(string name, string[] Annotations, string[] SubClasses, string[] SuperClasses, string[] Instances, Ferda.OntologyRelated.generated.OntologyData.StrSeqMap DataPropertiesMap)
                    {
                        this.name = name;
                        this.Annotations = Annotations;
                        this.SubClasses = SubClasses;
                        this.SuperClasses = SuperClasses;
                        this.Instances = Instances;
                        this.DataPropertiesMap = DataPropertiesMap;
                    }

                    #endregion

                    #region ICloneable members

                    public object Clone()
                    {
                        return MemberwiseClone();
                    }

                    #endregion

                    #region Object members

                    public override int GetHashCode()
                    {
                        int h__ = 0;
                        if(name != null)
                        {
                            h__ = 5 * h__ + name.GetHashCode();
                        }
                        if(Annotations != null)
                        {
                            h__ = 5 * h__ + Annotations.GetHashCode();
                        }
                        if(SubClasses != null)
                        {
                            h__ = 5 * h__ + SubClasses.GetHashCode();
                        }
                        if(SuperClasses != null)
                        {
                            h__ = 5 * h__ + SuperClasses.GetHashCode();
                        }
                        if(Instances != null)
                        {
                            h__ = 5 * h__ + Instances.GetHashCode();
                        }
                        if(DataPropertiesMap != null)
                        {
                            h__ = 5 * h__ + DataPropertiesMap.GetHashCode();
                        }
                        return h__;
                    }

                    public override bool Equals(object other__)
                    {
                        if(object.ReferenceEquals(this, other__))
                        {
                            return true;
                        }
                        if(other__ == null)
                        {
                            return false;
                        }
                        if(GetType() != other__.GetType())
                        {
                            return false;
                        }
                        OntologyClass o__ = (OntologyClass)other__;
                        if(name == null)
                        {
                            if(o__.name != null)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if(!name.Equals(o__.name))
                            {
                                return false;
                            }
                        }
                        if(Annotations == null)
                        {
                            if(o__.Annotations != null)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if(!Annotations.Equals(o__.Annotations))
                            {
                                return false;
                            }
                        }
                        if(SubClasses == null)
                        {
                            if(o__.SubClasses != null)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if(!SubClasses.Equals(o__.SubClasses))
                            {
                                return false;
                            }
                        }
                        if(SuperClasses == null)
                        {
                            if(o__.SuperClasses != null)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if(!SuperClasses.Equals(o__.SuperClasses))
                            {
                                return false;
                            }
                        }
                        if(Instances == null)
                        {
                            if(o__.Instances != null)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if(!Instances.Equals(o__.Instances))
                            {
                                return false;
                            }
                        }
                        if(DataPropertiesMap == null)
                        {
                            if(o__.DataPropertiesMap != null)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if(!DataPropertiesMap.Equals(o__.DataPropertiesMap))
                            {
                                return false;
                            }
                        }
                        return true;
                    }

                    #endregion

                    #region Comparison members

                    public static bool operator==(OntologyClass lhs__, OntologyClass rhs__)
                    {
                        return Equals(lhs__, rhs__);
                    }

                    public static bool operator!=(OntologyClass lhs__, OntologyClass rhs__)
                    {
                        return !Equals(lhs__, rhs__);
                    }

                    #endregion

                    #region Marshalling support

                    public void write__(IceInternal.BasicStream os__)
                    {
                        os__.writeString(name);
                        os__.writeStringSeq(Annotations);
                        os__.writeStringSeq(SubClasses);
                        os__.writeStringSeq(SuperClasses);
                        os__.writeStringSeq(Instances);
                        Ferda.OntologyRelated.generated.OntologyData.StrSeqMapHelper.write(os__, DataPropertiesMap);
                    }

                    public void read__(IceInternal.BasicStream is__)
                    {
                        name = is__.readString();
                        Annotations = is__.readStringSeq();
                        SubClasses = is__.readStringSeq();
                        SuperClasses = is__.readStringSeq();
                        Instances = is__.readStringSeq();
                        DataPropertiesMap = Ferda.OntologyRelated.generated.OntologyData.StrSeqMapHelper.read(is__);
                    }

                    #endregion
                }

                public class dictionaryStringOntologyClass : _System.Collections.DictionaryBase, _System.ICloneable
                {
                    #region dictionaryStringOntologyClass members

                    public void AddRange(dictionaryStringOntologyClass d__)
                    {
                        foreach(_System.Collections.DictionaryEntry e in d__)
                        {
                            try
                            {
                                InnerHashtable.Add(e.Key, e.Value);
                            }
                            catch(_System.ArgumentException)
                            {
                                // ignore
                            }
                        }
                    }

                    #endregion

                    #region IDictionary members

                    public bool IsFixedSize
                    {
                        get
                        {
                            return false;
                        }
                    }

                    public bool IsReadOnly
                    {
                        get
                        {
                            return false;
                        }
                    }

                    public _System.Collections.ICollection Keys
                    {
                        get
                        {
                            return InnerHashtable.Keys;
                        }
                    }

                    public _System.Collections.ICollection Values
                    {
                        get
                        {
                            return InnerHashtable.Values;
                        }
                    }

                    #region Indexer

                    public Ferda.OntologyRelated.generated.OntologyData.OntologyClass this[string key]
                    {
                        get
                        {
                            return (Ferda.OntologyRelated.generated.OntologyData.OntologyClass)InnerHashtable[key];
                        }
                        set
                        {
                            InnerHashtable[key] = value;
                        }
                    }

                    #endregion

                    public void Add(string key, Ferda.OntologyRelated.generated.OntologyData.OntologyClass value)
                    {
                        InnerHashtable.Add(key, value);
                    }

                    public void Remove(string key)
                    {
                        InnerHashtable.Remove(key);
                    }

                    public bool Contains(string key)
                    {
                        return InnerHashtable.Contains(key);
                    }

                    #endregion

                    #region ICollection members

                    public bool IsSynchronized
                    {
                        get
                        {
                            return false;
                        }
                    }

                    public object SyncRoot
                    {
                        get
                        {
                            return this;
                        }
                    }

                    #endregion

                    #region ICloneable members

                    public object Clone()
                    {
                        dictionaryStringOntologyClass d = new dictionaryStringOntologyClass();
                        foreach(_System.Collections.DictionaryEntry e in InnerHashtable)
                        {
                            d.InnerHashtable.Add(e.Key, e.Value);
                        }
                        return d;
                    }

                    #endregion

                    #region Object members

                    public override int GetHashCode()
                    {
                        int hash = 0;
                        foreach(_System.Collections.DictionaryEntry e in InnerHashtable)
                        {
                            hash = 5 * hash + e.Key.GetHashCode();
                            if(e.Value != null)
                            {
                                hash = 5 * hash + e.Value.GetHashCode();
                            }
                        }
                        return hash;
                    }

                    public override bool Equals(object other)
                    {
                        if(object.ReferenceEquals(this, other))
                        {
                            return true;
                        }
                        if(!(other is dictionaryStringOntologyClass))
                        {
                            return false;
                        }
                        if(Count != ((dictionaryStringOntologyClass)other).Count)
                        {
                            return false;
                        }
                        string[] klhs__ = new string[Count];
                        Keys.CopyTo(klhs__, 0);
                        _System.Array.Sort(klhs__);
                        string[] krhs__ = new string[((dictionaryStringOntologyClass)other).Count];
                        ((dictionaryStringOntologyClass)other).Keys.CopyTo(krhs__, 0);
                        _System.Array.Sort(krhs__);
                        for(int i = 0; i < Count; ++i)
                        {
                            if(!klhs__[i].Equals(krhs__[i]))
                            {
                                return false;
                            }
                        }
                        Ferda.OntologyRelated.generated.OntologyData.OntologyClass[] vlhs__ = new Ferda.OntologyRelated.generated.OntologyData.OntologyClass[Count];
                        Values.CopyTo(vlhs__, 0);
                        _System.Array.Sort(vlhs__);
                        Ferda.OntologyRelated.generated.OntologyData.OntologyClass[] vrhs__ = new Ferda.OntologyRelated.generated.OntologyData.OntologyClass[((dictionaryStringOntologyClass)other).Count];
                        ((dictionaryStringOntologyClass)other).Values.CopyTo(vrhs__, 0);
                        _System.Array.Sort(vrhs__);
                        for(int i = 0; i < Count; ++i)
                        {
                            if(vlhs__[i] == null)
                            {
                                if(vrhs__[i] != null)
                                {
                                    return false;
                                }
                            }
                            else if(!vlhs__[i].Equals(vrhs__[i]))
                            {
                                return false;
                            }
                        }
                        return true;
                    }

                    #endregion

                    #region Comparison members

                    public static bool operator==(dictionaryStringOntologyClass lhs__, dictionaryStringOntologyClass rhs__)
                    {
                        return Equals(lhs__, rhs__);
                    }

                    public static bool operator!=(dictionaryStringOntologyClass lhs__, dictionaryStringOntologyClass rhs__)
                    {
                        return !Equals(lhs__, rhs__);
                    }

                    #endregion
                }

                public class ObjectProperty : _System.ICloneable
                {
                    #region Slice data members

                    public string name;

                    public string[] Annotations;

                    public string[] Domains;

                    public string[] Ranges;

                    #endregion

                    #region Constructors

                    public ObjectProperty()
                    {
                    }

                    public ObjectProperty(string name, string[] Annotations, string[] Domains, string[] Ranges)
                    {
                        this.name = name;
                        this.Annotations = Annotations;
                        this.Domains = Domains;
                        this.Ranges = Ranges;
                    }

                    #endregion

                    #region ICloneable members

                    public object Clone()
                    {
                        return MemberwiseClone();
                    }

                    #endregion

                    #region Object members

                    public override int GetHashCode()
                    {
                        int h__ = 0;
                        if(name != null)
                        {
                            h__ = 5 * h__ + name.GetHashCode();
                        }
                        if(Annotations != null)
                        {
                            h__ = 5 * h__ + Annotations.GetHashCode();
                        }
                        if(Domains != null)
                        {
                            h__ = 5 * h__ + Domains.GetHashCode();
                        }
                        if(Ranges != null)
                        {
                            h__ = 5 * h__ + Ranges.GetHashCode();
                        }
                        return h__;
                    }

                    public override bool Equals(object other__)
                    {
                        if(object.ReferenceEquals(this, other__))
                        {
                            return true;
                        }
                        if(other__ == null)
                        {
                            return false;
                        }
                        if(GetType() != other__.GetType())
                        {
                            return false;
                        }
                        ObjectProperty o__ = (ObjectProperty)other__;
                        if(name == null)
                        {
                            if(o__.name != null)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if(!name.Equals(o__.name))
                            {
                                return false;
                            }
                        }
                        if(Annotations == null)
                        {
                            if(o__.Annotations != null)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if(!Annotations.Equals(o__.Annotations))
                            {
                                return false;
                            }
                        }
                        if(Domains == null)
                        {
                            if(o__.Domains != null)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if(!Domains.Equals(o__.Domains))
                            {
                                return false;
                            }
                        }
                        if(Ranges == null)
                        {
                            if(o__.Ranges != null)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if(!Ranges.Equals(o__.Ranges))
                            {
                                return false;
                            }
                        }
                        return true;
                    }

                    #endregion

                    #region Comparison members

                    public static bool operator==(ObjectProperty lhs__, ObjectProperty rhs__)
                    {
                        return Equals(lhs__, rhs__);
                    }

                    public static bool operator!=(ObjectProperty lhs__, ObjectProperty rhs__)
                    {
                        return !Equals(lhs__, rhs__);
                    }

                    #endregion

                    #region Marshalling support

                    public void write__(IceInternal.BasicStream os__)
                    {
                        os__.writeString(name);
                        os__.writeStringSeq(Annotations);
                        os__.writeStringSeq(Domains);
                        os__.writeStringSeq(Ranges);
                    }

                    public void read__(IceInternal.BasicStream is__)
                    {
                        name = is__.readString();
                        Annotations = is__.readStringSeq();
                        Domains = is__.readStringSeq();
                        Ranges = is__.readStringSeq();
                    }

                    #endregion
                }

                public class OntologyStructure : _System.ICloneable
                {
                    #region Slice data members

                    public Ferda.OntologyRelated.generated.OntologyData.dictionaryStringOntologyClass OntologyClassMap;

                    public Ferda.OntologyRelated.generated.OntologyData.ObjectProperty[] ObjectProperties;

                    #endregion

                    #region Constructors

                    public OntologyStructure()
                    {
                    }

                    public OntologyStructure(Ferda.OntologyRelated.generated.OntologyData.dictionaryStringOntologyClass OntologyClassMap, Ferda.OntologyRelated.generated.OntologyData.ObjectProperty[] ObjectProperties)
                    {
                        this.OntologyClassMap = OntologyClassMap;
                        this.ObjectProperties = ObjectProperties;
                    }

                    #endregion

                    #region ICloneable members

                    public object Clone()
                    {
                        return MemberwiseClone();
                    }

                    #endregion

                    #region Object members

                    public override int GetHashCode()
                    {
                        int h__ = 0;
                        if(OntologyClassMap != null)
                        {
                            h__ = 5 * h__ + OntologyClassMap.GetHashCode();
                        }
                        if(ObjectProperties != null)
                        {
                            h__ = 5 * h__ + ObjectProperties.GetHashCode();
                        }
                        return h__;
                    }

                    public override bool Equals(object other__)
                    {
                        if(object.ReferenceEquals(this, other__))
                        {
                            return true;
                        }
                        if(other__ == null)
                        {
                            return false;
                        }
                        if(GetType() != other__.GetType())
                        {
                            return false;
                        }
                        OntologyStructure o__ = (OntologyStructure)other__;
                        if(OntologyClassMap == null)
                        {
                            if(o__.OntologyClassMap != null)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if(!OntologyClassMap.Equals(o__.OntologyClassMap))
                            {
                                return false;
                            }
                        }
                        if(ObjectProperties == null)
                        {
                            if(o__.ObjectProperties != null)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if(!ObjectProperties.Equals(o__.ObjectProperties))
                            {
                                return false;
                            }
                        }
                        return true;
                    }

                    #endregion

                    #region Comparison members

                    public static bool operator==(OntologyStructure lhs__, OntologyStructure rhs__)
                    {
                        return Equals(lhs__, rhs__);
                    }

                    public static bool operator!=(OntologyStructure lhs__, OntologyStructure rhs__)
                    {
                        return !Equals(lhs__, rhs__);
                    }

                    #endregion

                    #region Marshalling support

                    public void write__(IceInternal.BasicStream os__)
                    {
                        Ferda.OntologyRelated.generated.OntologyData.dictionaryStringOntologyClassHelper.write(os__, OntologyClassMap);
                        if(ObjectProperties == null)
                        {
                            os__.writeSize(0);
                        }
                        else
                        {
                            os__.writeSize(ObjectProperties.Length);
                            for(int ix__ = 0; ix__ < ObjectProperties.Length; ++ix__)
                            {
                                ObjectProperties[ix__].write__(os__);
                            }
                        }
                    }

                    public void read__(IceInternal.BasicStream is__)
                    {
                        OntologyClassMap = Ferda.OntologyRelated.generated.OntologyData.dictionaryStringOntologyClassHelper.read(is__);
                        {
                            int szx__ = is__.readSize();
                            is__.startSeq(szx__, 4);
                            ObjectProperties = new Ferda.OntologyRelated.generated.OntologyData.ObjectProperty[szx__];
                            for(int ix__ = 0; ix__ < szx__; ++ix__)
                            {
                                ObjectProperties[ix__] = new Ferda.OntologyRelated.generated.OntologyData.ObjectProperty();
                                ObjectProperties[ix__].read__(is__);
                                is__.checkSeq();
                                is__.endElement();
                            }
                            is__.endSeq(szx__);
                        }
                    }

                    #endregion
                }
            }
        }
    }
}

namespace Ferda
{
    namespace OntologyRelated
    {
        namespace generated
        {
            namespace OntologyData
            {
                public sealed class StrSeqHelper
                {
                    public static void write(IceInternal.BasicStream os__, string[] v__)
                    {
                        os__.writeStringSeq(v__);
                    }

                    public static string[] read(IceInternal.BasicStream is__)
                    {
                        string[] v__;
                        v__ = is__.readStringSeq();
                        return v__;
                    }
                }

                public sealed class StrSeqMapHelper
                {
                    public static void write(IceInternal.BasicStream os__, StrSeqMap v__)
                    {
                        if(v__ == null)
                        {
                            os__.writeSize(0);
                        }
                        else
                        {
                            os__.writeSize(v__.Count);
                            foreach(_System.Collections.DictionaryEntry e__ in v__)
                            {
                                os__.writeString(((string)e__.Key));
                                os__.writeStringSeq(((string[])e__.Value));
                            }
                        }
                    }

                    public static StrSeqMap read(IceInternal.BasicStream is__)
                    {
                        int sz__ = is__.readSize();
                        StrSeqMap r__ = new StrSeqMap();
                        for(int i__ = 0; i__ < sz__; ++i__)
                        {
                            string k__;
                            k__ = is__.readString();
                            string[] v__;
                            v__ = is__.readStringSeq();
                            r__[k__] = v__;
                        }
                        return r__;
                    }
                }

                public sealed class dictionaryStringOntologyClassHelper
                {
                    public static void write(IceInternal.BasicStream os__, dictionaryStringOntologyClass v__)
                    {
                        if(v__ == null)
                        {
                            os__.writeSize(0);
                        }
                        else
                        {
                            os__.writeSize(v__.Count);
                            foreach(_System.Collections.DictionaryEntry e__ in v__)
                            {
                                os__.writeString(((string)e__.Key));
                                ((Ferda.OntologyRelated.generated.OntologyData.OntologyClass)e__.Value).write__(os__);
                            }
                        }
                    }

                    public static dictionaryStringOntologyClass read(IceInternal.BasicStream is__)
                    {
                        int sz__ = is__.readSize();
                        dictionaryStringOntologyClass r__ = new dictionaryStringOntologyClass();
                        for(int i__ = 0; i__ < sz__; ++i__)
                        {
                            string k__;
                            k__ = is__.readString();
                            Ferda.OntologyRelated.generated.OntologyData.OntologyClass v__;
                            v__ = new Ferda.OntologyRelated.generated.OntologyData.OntologyClass();
                            v__.read__(is__);
                            r__[k__] = v__;
                        }
                        return r__;
                    }
                }

                public sealed class sequenceObjectPropertyHelper
                {
                    public static void write(IceInternal.BasicStream os__, Ferda.OntologyRelated.generated.OntologyData.ObjectProperty[] v__)
                    {
                        if(v__ == null)
                        {
                            os__.writeSize(0);
                        }
                        else
                        {
                            os__.writeSize(v__.Length);
                            for(int ix__ = 0; ix__ < v__.Length; ++ix__)
                            {
                                v__[ix__].write__(os__);
                            }
                        }
                    }

                    public static Ferda.OntologyRelated.generated.OntologyData.ObjectProperty[] read(IceInternal.BasicStream is__)
                    {
                        Ferda.OntologyRelated.generated.OntologyData.ObjectProperty[] v__;
                        {
                            int szx__ = is__.readSize();
                            is__.startSeq(szx__, 4);
                            v__ = new Ferda.OntologyRelated.generated.OntologyData.ObjectProperty[szx__];
                            for(int ix__ = 0; ix__ < szx__; ++ix__)
                            {
                                v__[ix__] = new Ferda.OntologyRelated.generated.OntologyData.ObjectProperty();
                                v__[ix__].read__(is__);
                                is__.checkSeq();
                                is__.endElement();
                            }
                            is__.endSeq(szx__);
                        }
                        return v__;
                    }
                }
            }
        }
    }
}
