﻿#pragma checksum "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5CAFEC9FBABA1BAD43D25F6852F34C64794C903DE789AF52A7F7CB2FAFECA6ED"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using LaboratoryBook.ModifyDatabaseWindow;
using Microsoft.Windows.Controls;
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


namespace LaboratoryBook.ModifyDatabaseWindow {
    
    
    /// <summary>
    /// ModifyDatabaseWindow
    /// </summary>
    public partial class ModifyDatabaseWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 253 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CbxListValues;
        
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
            System.Uri resourceLocater = new System.Uri("/LaboratoryBook;component/modifydatabasewindow/modifydatabasewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
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
            
            #line 17 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.СloseCommand_CanExecute);
            
            #line default
            #line hidden
            
            #line 18 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CloseCommand_Executed);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 21 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.AddUserCommand_CanExecute);
            
            #line default
            #line hidden
            
            #line 22 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.AddUserCommand_Executed);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 25 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.RemoveUserCommand_CanExecute);
            
            #line default
            #line hidden
            
            #line 26 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.RemoveUserCommand_Executed);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 29 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.AddColumnCommand_CanExecute);
            
            #line default
            #line hidden
            
            #line 30 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.AddColumnCommand_Executed);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 33 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.RemoveColumnCommand_CanExecute);
            
            #line default
            #line hidden
            
            #line 34 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.RemoveColumnCommand_Executed);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 37 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.AddListValueCommand_CanExecute);
            
            #line default
            #line hidden
            
            #line 38 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.AddListValueCommand_Executed);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 41 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.RemoveListValueCommand_CanExecute);
            
            #line default
            #line hidden
            
            #line 42 "..\..\..\ModifyDatabaseWindow\ModifyDatabaseWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.RemoveListValueCommand_Executed);
            
            #line default
            #line hidden
            return;
            case 8:
            this.CbxListValues = ((System.Windows.Controls.ComboBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

