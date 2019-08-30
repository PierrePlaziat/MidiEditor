﻿#pragma checksum "..\..\..\MVC\Vue.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "41A2C9480CC2F37618941919D1EA7F671E4E6B99"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Sanford.Multimedia.Midi.UI;
using ScoreApp.MVC;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace ScoreApp.MVC {
    
    
    /// <summary>
    /// Vue
    /// </summary>
    public partial class Vue : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 33 "..\..\..\MVC\Vue.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem OpenMenuItem;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\MVC\Vue.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem AddTrackMenuItem;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\MVC\Vue.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem DeleteTrackMenuItem;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\MVC\Vue.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button startButton;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\MVC\Vue.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button stopButton;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\MVC\Vue.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button continueButton;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\MVC\Vue.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel TracksPanel;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\MVC\Vue.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas ProgressViewer;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\MVC\Vue.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle ProgressViewerBar;
        
        #line default
        #line hidden
        
        
        #line 109 "..\..\..\MVC\Vue.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar ProgressionBar;
        
        #line default
        #line hidden
        
        
        #line 114 "..\..\..\MVC\Vue.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ScrollBar positionScrollBar;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\..\MVC\Vue.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Sanford.Multimedia.Midi.UI.PianoControl Piano;
        
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
            System.Uri resourceLocater = new System.Uri("/ScoreApp;component/mvc/vue.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MVC\Vue.xaml"
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
            
            #line 11 "..\..\..\MVC\Vue.xaml"
            ((ScoreApp.MVC.Vue)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.OpenMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 33 "..\..\..\MVC\Vue.xaml"
            this.OpenMenuItem.Click += new System.Windows.RoutedEventHandler(this.OpenMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.AddTrackMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 38 "..\..\..\MVC\Vue.xaml"
            this.AddTrackMenuItem.Click += new System.Windows.RoutedEventHandler(this.AddTrackMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.DeleteTrackMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 39 "..\..\..\MVC\Vue.xaml"
            this.DeleteTrackMenuItem.Click += new System.Windows.RoutedEventHandler(this.DeleteTrackMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.startButton = ((System.Windows.Controls.Button)(target));
            
            #line 68 "..\..\..\MVC\Vue.xaml"
            this.startButton.Click += new System.Windows.RoutedEventHandler(this.StartButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.stopButton = ((System.Windows.Controls.Button)(target));
            
            #line 69 "..\..\..\MVC\Vue.xaml"
            this.stopButton.Click += new System.Windows.RoutedEventHandler(this.StopButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.continueButton = ((System.Windows.Controls.Button)(target));
            
            #line 70 "..\..\..\MVC\Vue.xaml"
            this.continueButton.Click += new System.Windows.RoutedEventHandler(this.ContinueButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.TracksPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 9:
            this.ProgressViewer = ((System.Windows.Controls.Canvas)(target));
            return;
            case 10:
            this.ProgressViewerBar = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 11:
            this.ProgressionBar = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 12:
            this.positionScrollBar = ((System.Windows.Controls.Primitives.ScrollBar)(target));
            return;
            case 13:
            this.Piano = ((Sanford.Multimedia.Midi.UI.PianoControl)(target));
            
            #line 129 "..\..\..\MVC\Vue.xaml"
            this.Piano.PianoKeyDown += new System.EventHandler<Sanford.Multimedia.Midi.UI.PianoKeyEventArgs>(this.PianoControl_PianoKeyDown);
            
            #line default
            #line hidden
            
            #line 130 "..\..\..\MVC\Vue.xaml"
            this.Piano.PianoKeyUp += new System.EventHandler<Sanford.Multimedia.Midi.UI.PianoKeyEventArgs>(this.PianoControl_PianoKeyUp);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

