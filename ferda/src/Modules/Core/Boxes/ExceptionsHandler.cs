using System;
using System.Diagnostics;

namespace Ferda.Modules.Boxes
{
    /// <summary>
    /// This class provides some static methods that helps to invokes
    /// (nearly) any methods, with specified error handling behaviour.
    /// </summary>
    public static class ExceptionsHandler
    {
        /// <summary>
        /// The method delegate.
        /// </summary>
        /// <typeparam name="ResultType"></typeparam>
        /// <returns></returns>
        public delegate ResultType MethodDelegate<ResultType>();

        /// <summary>
        /// Invokes the method delegate. On error no exception is thrown, 
        /// but default init delegate is invoked to return result.
        /// </summary>
        /// <param name="methodDelegate">The method delegate.</param>
        /// <param name="defaultInit">The default init.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Invokes the method delegate. On error the exception is thrown.
        /// Exceptions that are not compatible with <c>BoxRuntimeError</c>
        /// are converted to this one.
        /// </summary>
        /// <param name="methodDelegate">The method delegate.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns></returns>
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
                throw Exceptions.BoxRuntimeError(e, boxIdentity, "Unexpected Ice exception." + e.Message);
            }
            catch (Exception e)
            {
                Debug.Assert(false);
                throw Exceptions.BoxRuntimeError(e, boxIdentity, "Unexpected exception." + e.Message);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Gets the result with parameter <c>fallOnError</c> specifying
        /// the behaviour in case of error i.e. default init invocation to return
        /// default value or exception (<c>BoxRuntimeError</c>) throwing.
        /// </summary>
        /// <param name="fallOnError">if set to <c>true</c> exception is thrown on error.</param>
        /// <param name="methodDelegate">The method delegate.</param>
        /// <param name="defaultInit">The default init.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns></returns>
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