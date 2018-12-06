﻿/*
    This file is part of the d# project.
    Copyright (c) 2016-2018 ei8
    Authors: ei8
     This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License version 3
    as published by the Free Software Foundation with the addition of the
    following permission added to Section 15 as permitted in Section 7(a):
    FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
    EI8. EI8 DISCLAIMS THE WARRANTY OF NON INFRINGEMENT OF THIRD PARTY RIGHTS
     This program is distributed in the hope that it will be useful, but
    WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
    or FITNESS FOR A PARTICULAR PURPOSE.
    See the GNU Affero General Public License for more details.
    You should have received a copy of the GNU Affero General Public License
    along with this program; if not, see http://www.gnu.org/licenses or write to
    the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
    Boston, MA, 02110-1301 USA, or download the license from the following URL:
    https://github.com/ei8/cortex-diary/blob/master/LICENSE
     The interactive user interfaces in modified source and object code versions
    of this program must display Appropriate Legal Notices, as required under
    Section 5 of the GNU Affero General Public License.
     You can be released from the requirements of the license by purchasing
    a commercial license. Buying such a license is mandatory as soon as you
    develop commercial activities involving the d# software without
    disclosing the source code of your own applications.
     For more information, please contact ei8 at this address: 
     support@ei8.works
 */

using DynamicData.Binding;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using works.ei8.Cortex.Diary.Port.Adapter.UI.ViewModels.Docking;
using works.ei8.Cortex.Diary.Port.Adapter.UI.ViewModels.Neurons;

namespace works.ei8.Cortex.Diary.Port.Adapter.UI.ViewModels.Peripheral
{
    public class NeuronGraphViewModel : ToolViewModel
    {
        private IExtendedSelectionService selectionService;
        private IExtendedSelectionService highlightService;

        public NeuronGraphViewModel(IExtendedSelectionService selectionService = null, IExtendedSelectionService highlightService = null) : base("Graph")
        {
            this.selectionService = selectionService ?? Locator.Current.GetService<IExtendedSelectionService>(SelectionContract.Select.ToString());
            this.highlightService = highlightService ?? Locator.Current.GetService<IExtendedSelectionService>(SelectionContract.Highlight.ToString());

            this.LayoutOptions = new string[] { "Top to bottom", "Left to right", "Bottom to top", "Right to left" };

            this.SelectCommand = ReactiveCommand.Create(() => this.UpdateHighlightService());

            this.selectionService.WhenPropertyChanged(a => a.SelectedComponents)
                .Subscribe(p =>
                {
                    if (p.Sender.PrimarySelection is NeuronViewModelBase)
                    {
                        this.ExternallySelectedNeuron = (NeuronViewModelBase)p.Sender.PrimarySelection;
                        this.InternallySelectedNeuronId = null;
                        this.UpdateHighlightService();
                    }
                });
        }

        private void UpdateHighlightService()
        {
            this.highlightService.SetSelectedComponents(new object[] { this.InternallySelectedNeuronId });
        }

        public ReactiveCommand<Unit, Unit> SelectCommand { get; }

        private NeuronViewModelBase externallySelectedNeuron;

        public NeuronViewModelBase ExternallySelectedNeuron
        {
            get => this.externallySelectedNeuron;
            set => this.RaiseAndSetIfChanged(ref this.externallySelectedNeuron, value);
        }

        private string internallySelectedNeuronId;

        public string InternallySelectedNeuronId
        {
            get => this.internallySelectedNeuronId;
            set => this.RaiseAndSetIfChanged(ref this.internallySelectedNeuronId, value);
        }

        private string[] layoutOptions;

        public string[] LayoutOptions
        {
            get => this.layoutOptions;
            set => this.RaiseAndSetIfChanged(ref this.layoutOptions, value);
        }

        private int layout;

        public int Layout
        {
            get => this.layout;
            set => this.RaiseAndSetIfChanged(ref this.layout, value);
        }
    }
}