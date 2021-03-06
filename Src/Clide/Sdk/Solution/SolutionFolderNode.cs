﻿#region BSD License
/* 
Copyright (c) 2012, Clarius Consulting
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

* Neither the name of Clarius Consulting nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

namespace Clide.Sdk.Solution
{
    using Clide.Patterns.Adapter;
    using Clide.Solution;
    using Clide.VisualStudio;
    using EnvDTE;
    using EnvDTE80;
    using System;
    using System.Linq;

    /// <summary>
    /// Default implementation of a solution folder node in a managed project.
    /// </summary>
    public class SolutionFolderNode : SolutionTreeNode, ISolutionFolderNode
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionFolderNode"/> class.
        /// </summary>
        /// <param name="hierarchyNode">The underlying hierarchy represented by this node.</param>
        /// <param name="parentNode">The parent node accessor.</param>
        /// <param name="nodeFactory">The factory for child nodes.</param>
        /// <param name="adapter">The adapter service that implements the smart cast <see cref="ITreeNode.As{T}"/>.</param>
        public SolutionFolderNode(
			IVsSolutionHierarchyNode hierarchyNode,
			Lazy<ITreeNode> parentNode,
			ITreeNodeFactory<IVsSolutionHierarchyNode> nodeFactory,
			IAdapterService adapter)
            : base(SolutionNodeKind.SolutionFolder, hierarchyNode, parentNode, nodeFactory, adapter)
		{
            Guard.NotNull(() => parentNode, parentNode);

			this.SolutionFolder = new Lazy<SolutionFolder>(
                () => (SolutionFolder)((Project)hierarchyNode.VsHierarchy.Properties(hierarchyNode.ItemId).ExtenderObject).Object);
		}

        /// <summary>
        /// Creates a nested solution folder.
        /// </summary>
        /// <param name="name">The name of the folder to create.</param>
        public virtual ISolutionFolderNode CreateSolutionFolder(string name)
		{
			Guard.NotNullOrEmpty(() => name, name);

			this.SolutionFolder.Value.AddSolutionFolder(name);

			var solutionfolder =
				this.HierarchyNode.Children.Single(child => child.VsHierarchy.Properties(child.ItemId).DisplayName == name);

			return this.CreateNode(solutionfolder) as ISolutionFolderNode;
		}

        /// <summary>
        /// Accepts the specified visitor for traversal.
        /// </summary>
        public override bool Accept(ISolutionVisitor visitor)
        {
            return SolutionVisitable.Accept(this, visitor);
        }

		/// <summary>
		/// Tries to smart-cast this node to the give type.
		/// </summary>
		/// <typeparam name="T">Type to smart-cast to.</typeparam>
		/// <returns>
		/// The casted value or null if it cannot be converted to that type.
		/// </returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public override T As<T>()
		{
			return this.Adapter.Adapt(this).As<T>();
		}

        /// <summary>
        /// Gets the solution folder represented by this node.
        /// </summary>
        internal Lazy<SolutionFolder> SolutionFolder { get; private set; }
	}
}