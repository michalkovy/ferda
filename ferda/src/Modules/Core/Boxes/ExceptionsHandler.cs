using System;
using System.Diagnostics;

namespace Ferda.Modules.Boxes
{
    public static class ExceptionsHandler
    {
        public delegate ResultType MethodDelegate<ResultType>();

        public static ResultType TryCatchMethodNoThrow<ResultType>(
            MethodDelegate<ResultType> methodDelegate,
            MethodDelegate<ResultType> defaultInit)
        {
            try
            {
                return methodDelegate();
            }
            catch (BoxRuntimeError)
            {
                return defaultInit();
            }
            catch (Exception)
            {
                Debug.Assert(false);
                return defaultInit();
            }
            finally
            {
            }
        }

        public static ResultType TryCatchMethodThrow<ResultType>(
            MethodDelegate<ResultType> methodDelegate,
            string boxIdentity)
        {
            ResultType result;
            try
            {
                result = methodDelegate();
                return result;
            }
            catch (BoxRuntimeError e)
            {
                // na vyber jestli poslad vyjimku nebo implicitni konstrukci vysledku
                if (String.IsNullOrEmpty(e.boxIdentity))
                    e.boxIdentity = boxIdentity;
                Debug.Assert(!String.IsNullOrEmpty(e.boxIdentity));
                Debug.Assert(!String.IsNullOrEmpty(e.userMessage));
                throw;
            }
            catch (Ice.Exception e)
            {
                Debug.Assert(false);
                throw Exceptions.BoxRuntimeError(e, boxIdentity, "Unexpected Ice exception.");
            }
            catch (Exception e)
            {
                Debug.Assert(false);
                throw Exceptions.BoxRuntimeError(e, boxIdentity, "Unexpected exception.");
            }
            finally
            {
            }
        }

        public static ResultType GetResult<ResultType>(
            bool fallOnError,
            MethodDelegate<ResultType> methodDelegate,
            MethodDelegate<ResultType> defaultInit,
            string boxIdentity)
        {
            if (fallOnError)
            {
                return TryCatchMethodThrow(methodDelegate, boxIdentity);
            }
            else
            {
                return TryCatchMethodNoThrow(methodDelegate, defaultInit);
            }
        }
    }
#if DEBUG
    /// <summary>
    /// An example of usage.
    /// </summary>
    internal class A
    {
        public string I = "Charles";
        public int J = 4;
        public string S = "Emperor by the grace of God";

        public String MyMethod()
        {
            return I + " " + J.ToString() + " " + S;
        }

        public String GetResult_NamedMethod(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<String>(
                fallOnError,
                MyMethod,
                delegate
                    {
                        return String.Empty;
                    },
                "String Ice Identity"
                );
        }
    }
#endif
}