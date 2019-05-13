﻿#pragma checksum "..\..\..\UserControls\LuseUserControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "06439F20565C3F07AA244CA7AE45935E1B3013DC"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Dragablz;
using ECR_System_v2.UserControls;
using LiveCharts.Wpf;
using MahApps.Metro.IconPacks;
using MaterialDesignExtensions.Controls;
using MaterialDesignExtensions.Model;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ECR_System_v2.UserControls {
    
    
    /// <summary>
    /// LuseUserControl
    /// </summary>
    public partial class LuseUserControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 28 "..\..\..\UserControls\LuseUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock LuseValue;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\UserControls\LuseUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock LuseChangeValue;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\UserControls\LuseUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock LuseChangePer;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\UserControls\LuseUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox SharesTimelineCombo;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\UserControls\LuseUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button GoBackBtn;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\UserControls\LuseUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button GoForwardBtn;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\UserControls\LuseUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.Transitions.Transitioner EquitySection;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\UserControls\LuseUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView SharePricesListView;
        
        #line default
        #line hidden
        
        
        #line 194 "..\..\..\UserControls\LuseUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LiveCharts.Wpf.CartesianChart EquityValueCartesianChart;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ECR System v2;component/usercontrols/luseusercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControls\LuseUserControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.LuseValue = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.LuseChangeValue = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.LuseChangePer = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.SharesTimelineCombo = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.GoBackBtn = ((System.Windows.Controls.Button)(target));
            
            #line 54 "..\..\..\UserControls\LuseUserControl.xaml"
            this.GoBackBtn.Click += new System.Windows.RoutedEventHandler(this.GoBack);
            
            #line default
            #line hidden
            return;
            case 6:
            this.GoForwardBtn = ((System.Windows.Controls.Button)(target));
            
            #line 60 "..\..\..\UserControls\LuseUserControl.xaml"
            this.GoForwardBtn.Click += new System.Windows.RoutedEventHandler(this.GoForward);
            
            #line default
            #line hidden
            return;
            case 7:
            this.EquitySection = ((MaterialDesignThemes.Wpf.Transitions.Transitioner)(target));
            return;
            case 8:
            this.SharePricesListView = ((System.Windows.Controls.ListView)(target));
            return;
            case 10:
            this.EquityValueCartesianChart = ((LiveCharts.Wpf.CartesianChart)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 9:
            
            #line 103 "..\..\..\UserControls\LuseUserControl.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SelectEquity);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

