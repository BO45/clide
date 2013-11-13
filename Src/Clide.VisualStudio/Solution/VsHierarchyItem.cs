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

namespace Clide
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio;

    /// <summary>
	/// Basic representation of an item in a hierarchy in VS extensibility APIs. 
	/// Any solution item should be able to smart-cast to this interface using 
	/// the As&lt;T&gt; method.
	/// </summary>
    public class VsHierarchyItem
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="VsHierarchyItem"/> class.
        /// </summary>
        /// <param name="hierarchy">The hierarchy.</param>
        /// <param name="itemId">The item id.</param>
        public VsHierarchyItem(IVsHierarchy hierarchy, uint itemId)
        {
            this.VsHierarchy = hierarchy;
            this.ItemId = itemId;
        }

		/// <summary>
		/// Gets the hierarchy that contains the item.
		/// </summary>
        public IVsHierarchy VsHierarchy { get; private set; }

		/// <summary>
		/// Gets the item id. Can be the special value <see cref="VSConstants.VSITEMID_ROOT"/> 
		/// if the item is a root hierarchy itself, such as a project.
		/// </summary>
        public uint ItemId { get; private set; }
	}
}
