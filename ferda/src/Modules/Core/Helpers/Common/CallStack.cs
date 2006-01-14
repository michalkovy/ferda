using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Helpers.Common
{
    /// <summary>
    /// Provides some static functions that facilitates working
    /// witch .NET call stack.
    /// </summary>
    public static class CallStack
    {
        /// <summary>
        /// Dumps the stack.
        /// </summary>
        /// <returns>Stack dump as formated string.</returns>
        public static string DumpStack()
        {
            StringBuilder stringBuilder = new StringBuilder(255);
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(true);
            string stackIndent = "";
            // skip dumping this function
            for (int i = 1; i < stackTrace.FrameCount; i++)
            {
                // Create a StackTrace that captures method, filename and line number information.
                System.Diagnostics.StackFrame stackFrame = stackTrace.GetFrame(i);
                stringBuilder.AppendLine(stackIndent + " Method: " + stackFrame.GetMethod());
                stringBuilder.AppendLine(stackIndent + " File: " + stackFrame.GetFileName());
                stringBuilder.AppendLine(stackIndent + " Line Number: " + stackFrame.GetFileLineNumber());
                stackIndent += "  ";
            }
            return stringBuilder.ToString();
        }
    }
}
