﻿#region BSD License
/* 
Copyright (c) 2012, Clarius Consulting
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

namespace Clide.Diagnostics
{
    using System;
    using System.IO;
    using System.Text;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// A <see cref="TextWriter"/> that writes to 
    /// an <see cref="IVsOutputWindowPane"/>.
    /// </summary>
    internal class OutputWindowTextWriter : TextWriter
    {
        private Lazy<IUIThread> uiThread;
        private IVsOutputWindowPane outputPane;

        public OutputWindowTextWriter(Lazy<IUIThread> uiThread, IVsOutputWindowPane outputPane)
        {
            this.uiThread = uiThread;
            this.outputPane = outputPane;
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        public override void Write(string value)
        {
            uiThread.Value.BeginInvoke(() => outputPane.OutputStringThreadSafe(value));
        }

        public override void WriteLine()
        {
            uiThread.Value.BeginInvoke(() => outputPane.OutputStringThreadSafe(Environment.NewLine));
        }

        public override void WriteLine(string value)
        {
            Write(value);;
            WriteLine();
        }
    }
}
