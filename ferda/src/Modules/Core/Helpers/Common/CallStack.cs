// CallStack.cs - functions for .NET call stack
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

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
            
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(true);
            return stackTrace.ToString();

            /*
            StringBuilder stringBuilder = new StringBuilder(255);
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
             */
        }
    }
}
