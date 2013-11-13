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

namespace Clide.Solution
{
    using System;
    using System.Linq;
    using System.Dynamic;
    using Clide.Patterns.Adapter;
    using Clide.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio;
    using System.Collections.Generic;
    using System.Collections;

    internal class ProjectNode : SolutionTreeNode, IProjectNode
	{
        private Lazy<GlobalProjectProperties> properties;

		public ProjectNode(
			IVsSolutionHierarchyNode hierarchyNode,
			Lazy<ITreeNode> parentNode,
			ITreeNodeFactory<IVsSolutionHierarchyNode> nodeFactory,
			IAdapterService adapter)
            : base(SolutionNodeKind.Project, hierarchyNode, parentNode, nodeFactory, adapter)
		{
            Guard.NotNull(() => parentNode, parentNode);

		    this.Project = new Lazy<EnvDTE.Project>(() => (EnvDTE.Project)hierarchyNode.VsHierarchy.Properties(hierarchyNode.ItemId).ExtenderObject);
            this.properties = new Lazy<GlobalProjectProperties>(() => new GlobalProjectProperties(this));
            this.Configuration = new ProjectConfiguration(this);
		}

        public IProjectConfiguration Configuration { get; private set; }
        public string ActiveConfigurationName { get { return this.Configuration.ActiveConfiguration + "|" + this.Configuration.ActivePlatform; } }

		public Lazy<EnvDTE.Project> Project { get; private set; }

		public IFolderNode CreateFolder(string name)
		{
			Guard.NotNullOrEmpty(() => name, name);

			this.Project.Value.ProjectItems.AddFolder(name);

			var folder = this.HierarchyNode.Children
				.Single(child => child.VsHierarchy.Properties(child.ItemId).DisplayName == name);

			return this.CreateNode(folder) as IFolderNode;
		}

        public void Save()
        {
            ErrorHandler.ThrowOnFailure(this
                .HierarchyNode
                .ServiceProvider
                .GetService<SVsSolution, IVsSolution>()
                .SaveSolutionElement(
                    (uint)__VSSLNSAVEOPTIONS.SLNSAVEOPT_ForceSave, 
                    this.HierarchyNode.VsHierarchy, 
                    0));
        }

		public string PhysicalPath
		{
			get
			{
				var dteProject = this.As<EnvDTE.Project>();
				if (dteProject == null)
					return null;
				else
					return dteProject.FullName;
			}
		}

		public dynamic Properties
		{
			get { return this.properties.Value; }
		}

        public dynamic PropertiesFor(string configurationAndPlatform)
        {
            return new ConfigProjectProperties(this, configurationAndPlatform);
        }
	}
}