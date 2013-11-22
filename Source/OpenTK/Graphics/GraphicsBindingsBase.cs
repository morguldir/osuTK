#region License
//
// The Open Toolkit Library License
//
// Copyright (c) 2006 - 2009 the Open Toolkit library.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//
#endregion

using System;
using System.Runtime.InteropServices;

namespace OpenTK.Graphics
{
    /// <summary>
    /// Implements BindingsBase for the OpenTK.Graphics namespace (OpenGL and OpenGL|ES).
    /// </summary>
    public abstract class GraphicsBindingsBase : BindingsBase
    {
        /// <summary>
        /// Retrieves an unmanaged function pointer to the specified function.
        /// </summary>
        /// <param name="funcname">
        /// A <see cref="System.String"/> that defines the name of the function.
        /// </param>
        /// <returns>
        /// A <see cref="IntPtr"/> that contains the address of funcname or IntPtr.Zero,
        /// if the function is not supported by the drivers.
        /// </returns>
        /// <remarks>
        /// Note: some drivers are known to return non-zero values for unsupported functions.
        /// Typical values include 1 and 2 - inheritors are advised to check for and ignore these
        /// values.
        /// </remarks>
        protected override IntPtr GetAddress(string funcname)
        {
            var context = GraphicsContext.CurrentContext as IGraphicsContextInternal;
            if (context == null)
                throw new GraphicsContextMissingException();
            return context != null ? context.GetAddress(funcname) : IntPtr.Zero;
        }

        internal static Delegate GetExtensionDelegateStatic(string funcname, Type signature)
        {
            var context = GraphicsContext.CurrentContext as IGraphicsContextInternal;
            if (context == null)
                throw new GraphicsContextMissingException();

            IntPtr address = context.GetAddress(funcname);
            if (address == IntPtr.Zero ||
                address == new IntPtr(1) ||     // Workaround for buggy nvidia drivers which return
                address == new IntPtr(2))       // 1 or 2 instead of IntPtr.Zero for some extensions.
            {
                return null;
            }
            else
            {
                return Marshal.GetDelegateForFunctionPointer(address, signature);
            }
        }
    }
}
