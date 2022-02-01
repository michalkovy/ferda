//
// Copyright (c) ZeroC, Inc. All rights reserved.
//
//
// Ice version 3.7.7
//
// <auto-generated>
//
// Generated from file `BoxType.ice'
//
// Warning: do not edit this file.
//
// </auto-generated>
//


using _System = global::System;

#pragma warning disable 1591

namespace Ferda
{
    namespace Modules
    {
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1722")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724")]
        [global::System.Serializable]
        public partial class NeededSocket : global::System.ICloneable
        {
            #region Slice data members

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public string socketName;

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public string functionIceId;

            #endregion

            partial void ice_initialize();

            #region Constructors

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public NeededSocket()
            {
                this.socketName = "";
                this.functionIceId = "";
                ice_initialize();
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public NeededSocket(string socketName, string functionIceId)
            {
                this.socketName = socketName;
                this.functionIceId = functionIceId;
                ice_initialize();
            }

            #endregion

            #region ICloneable members

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public object Clone()
            {
                return MemberwiseClone();
            }

            #endregion

            #region Object members

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public override int GetHashCode()
            {
                int h_ = 5381;
                global::IceInternal.HashUtil.hashAdd(ref h_, "::Ferda::Modules::NeededSocket");
                global::IceInternal.HashUtil.hashAdd(ref h_, socketName);
                global::IceInternal.HashUtil.hashAdd(ref h_, functionIceId);
                return h_;
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public override bool Equals(object other)
            {
                if(object.ReferenceEquals(this, other))
                {
                    return true;
                }
                if(other == null)
                {
                    return false;
                }
                if(GetType() != other.GetType())
                {
                    return false;
                }
                NeededSocket o = (NeededSocket)other;
                if(this.socketName == null)
                {
                    if(o.socketName != null)
                    {
                        return false;
                    }
                }
                else
                {
                    if(!this.socketName.Equals(o.socketName))
                    {
                        return false;
                    }
                }
                if(this.functionIceId == null)
                {
                    if(o.functionIceId != null)
                    {
                        return false;
                    }
                }
                else
                {
                    if(!this.functionIceId.Equals(o.functionIceId))
                    {
                        return false;
                    }
                }
                return true;
            }

            #endregion

            #region Comparison members

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public static bool operator==(NeededSocket lhs, NeededSocket rhs)
            {
                return Equals(lhs, rhs);
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public static bool operator!=(NeededSocket lhs, NeededSocket rhs)
            {
                return !Equals(lhs, rhs);
            }

            #endregion

            #region Marshaling support

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public void ice_writeMembers(global::Ice.OutputStream ostr)
            {
                ostr.writeString(this.socketName);
                ostr.writeString(this.functionIceId);
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public void ice_readMembers(global::Ice.InputStream istr)
            {
                this.socketName = istr.readString();
                this.functionIceId = istr.readString();
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public static void ice_write(global::Ice.OutputStream ostr, NeededSocket v)
            {
                if(v == null)
                {
                    _nullMarshalValue.ice_writeMembers(ostr);
                }
                else
                {
                    v.ice_writeMembers(ostr);
                }
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public static NeededSocket ice_read(global::Ice.InputStream istr)
            {
                var v = new NeededSocket();
                v.ice_readMembers(istr);
                return v;
            }

            private static readonly NeededSocket _nullMarshalValue = new NeededSocket();

            #endregion
        }

        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1722")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724")]
        [global::System.Serializable]
        public partial class BoxType : global::System.ICloneable
        {
            #region Slice data members

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public string functionIceId;

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public NeededSocket[] neededSockets;

            #endregion

            partial void ice_initialize();

            #region Constructors

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public BoxType()
            {
                this.functionIceId = "";
                ice_initialize();
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public BoxType(string functionIceId, NeededSocket[] neededSockets)
            {
                this.functionIceId = functionIceId;
                this.neededSockets = neededSockets;
                ice_initialize();
            }

            #endregion

            #region ICloneable members

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public object Clone()
            {
                return MemberwiseClone();
            }

            #endregion

            #region Object members

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public override int GetHashCode()
            {
                int h_ = 5381;
                global::IceInternal.HashUtil.hashAdd(ref h_, "::Ferda::Modules::BoxType");
                global::IceInternal.HashUtil.hashAdd(ref h_, functionIceId);
                global::IceInternal.HashUtil.hashAdd(ref h_, neededSockets);
                return h_;
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public override bool Equals(object other)
            {
                if(object.ReferenceEquals(this, other))
                {
                    return true;
                }
                if(other == null)
                {
                    return false;
                }
                if(GetType() != other.GetType())
                {
                    return false;
                }
                BoxType o = (BoxType)other;
                if(this.functionIceId == null)
                {
                    if(o.functionIceId != null)
                    {
                        return false;
                    }
                }
                else
                {
                    if(!this.functionIceId.Equals(o.functionIceId))
                    {
                        return false;
                    }
                }
                if(this.neededSockets == null)
                {
                    if(o.neededSockets != null)
                    {
                        return false;
                    }
                }
                else
                {
                    if(!IceUtilInternal.Arrays.Equals(this.neededSockets, o.neededSockets))
                    {
                        return false;
                    }
                }
                return true;
            }

            #endregion

            #region Comparison members

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public static bool operator==(BoxType lhs, BoxType rhs)
            {
                return Equals(lhs, rhs);
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public static bool operator!=(BoxType lhs, BoxType rhs)
            {
                return !Equals(lhs, rhs);
            }

            #endregion

            #region Marshaling support

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public void ice_writeMembers(global::Ice.OutputStream ostr)
            {
                ostr.writeString(this.functionIceId);
                NeededSocketSeqHelper.write(ostr, this.neededSockets);
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public void ice_readMembers(global::Ice.InputStream istr)
            {
                this.functionIceId = istr.readString();
                this.neededSockets = NeededSocketSeqHelper.read(istr);
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public static void ice_write(global::Ice.OutputStream ostr, BoxType v)
            {
                if(v == null)
                {
                    _nullMarshalValue.ice_writeMembers(ostr);
                }
                else
                {
                    v.ice_writeMembers(ostr);
                }
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
            public static BoxType ice_read(global::Ice.InputStream istr)
            {
                var v = new BoxType();
                v.ice_readMembers(istr);
                return v;
            }

            private static readonly BoxType _nullMarshalValue = new BoxType();

            #endregion
        }
    }
}

namespace Ferda
{
    namespace Modules
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
        public sealed class NeededSocketSeqHelper
        {
            public static void write(global::Ice.OutputStream ostr, NeededSocket[] v)
            {
                if(v == null)
                {
                    ostr.writeSize(0);
                }
                else
                {
                    ostr.writeSize(v.Length);
                    for(int ix = 0; ix < v.Length; ++ix)
                    {
                        (v[ix] == null ? new NeededSocket() : v[ix]).ice_writeMembers(ostr);
                    }
                }
            }

            public static NeededSocket[] read(global::Ice.InputStream istr)
            {
                NeededSocket[] v;
                {
                    int szx = istr.readAndCheckSeqSize(2);
                    v = new NeededSocket[szx];
                    for(int ix = 0; ix < szx; ++ix)
                    {
                        v[ix] = new NeededSocket();
                        v[ix].ice_readMembers(istr);
                    }
                }
                return v;
            }
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.7")]
        public sealed class BoxTypeSeqHelper
        {
            public static void write(global::Ice.OutputStream ostr, BoxType[] v)
            {
                if(v == null)
                {
                    ostr.writeSize(0);
                }
                else
                {
                    ostr.writeSize(v.Length);
                    for(int ix = 0; ix < v.Length; ++ix)
                    {
                        (v[ix] == null ? new BoxType() : v[ix]).ice_writeMembers(ostr);
                    }
                }
            }

            public static BoxType[] read(global::Ice.InputStream istr)
            {
                BoxType[] v;
                {
                    int szx = istr.readAndCheckSeqSize(2);
                    v = new BoxType[szx];
                    for(int ix = 0; ix < szx; ++ix)
                    {
                        v[ix] = new BoxType();
                        v[ix].ice_readMembers(istr);
                    }
                }
                return v;
            }
        }
    }
}
