using System.Diagnostics;
using System.Windows.Interop;
using EY.SpyDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Automation = System.Windows.Automation;
using System.Windows.Automation;
using System.Windows.Threading;
using System.Windows.Forms;

/// <summary>
/// Application is to SPY other process
/// </summary>

namespace WPFDriver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly TreeWalker ControlViewWalker;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region WPF Automation

        private delegate void SetMessageCallback(string message);

        private void btnStartAutomation_Click(object sender, RoutedEventArgs e)
        {
            Thread automateThread = new Thread(new ThreadStart(Automate));
            automateThread.Start();
        }

        private void Automate()
        {
            LogMessage("Getting RootElement...");
            AutomationElement rootElement = AutomationElement.RootElement;
            if (rootElement != null)
            {
                LogMessage("OK." + Environment.NewLine);

                Automation.Condition condition = new PropertyCondition(AutomationElement.NameProperty, "Show Visual Tree");

                LogMessage("Searching for Dialog Box Window...");
                AutomationElement appElement = rootElement.FindFirst(TreeScope.Children, condition);

                //ControlProxy proxy = new ControlProxy();
                //int[] a = proxy.FindCountofWindow((char*)appElement.Current.NativeWindowHandle);

                //List<IntPtr> childWIndows = ChildWIndowFInder.GetChildWindows((IntPtr)appElement.Current.NativeWindowHandle);

                //IntPtr hWnd = ChildWIndowFInder.SearchForWindow(appElement.Current.ClassName, "btnShowVisualTree");

                if (appElement != null)
                {
                    LogMessage("OK " + Environment.NewLine);
                    LogMessage("Searching for Button Show Visual Tree control...");
                    AutomationElement btnElementA = GetChildElement(appElement, "btnShowVisualTree");
                    if (btnElementA != null)
                    {
                        LogMessage("OK " + Environment.NewLine);
                        LogMessage("Setting Button 1 value...");
                        try
                        {
                            // Control the element by EY Spy Driver
                            //ControlProxy controlProxy = ControlProxy.WPFFromHandle(new IntPtr(appElement.Current.NativeWindowHandle), (uint)appElement.Current.ProcessId);
                            

                            //controlProxy.Invoke("Hide", null);

                            LogMessage("OK " + Environment.NewLine);
                        }
                        catch
                        {
                            WriteLogError();
                        }
                    }
                    else
                    {
                        WriteLogError();
                    }

                    LogMessage("Searching for TextBox 1 control...");
                    AutomationElement txtElement = GetChildElement(appElement, "txt1");
                    if (txtElement != null)
                    {
                        LogMessage("OK " + Environment.NewLine);
                        LogMessage("Setting TextBox 1 value...");
                        try
                        {
                            ValuePattern valuePatternB = txtElement.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
                            valuePatternB.SetValue("text changed by AUM");
                            LogMessage("OK " + Environment.NewLine);
                        }
                        catch
                        {
                            WriteLogError();
                        }
                    }
                    else
                    {
                        WriteLogError();
                    }
                }
                else
                {
                    WriteLogError();
                }
            }
        }

        private AutomationElement GetChildElement(AutomationElement parentElement, string value)
        {
            Automation.Condition condition = new PropertyCondition(AutomationElement.AutomationIdProperty, value);
            AutomationElement childElement = parentElement.FindFirst(TreeScope.Descendants, condition);
            return childElement;
        }

        private void DisplayLogMessage(string message)
        {
            txtLogs.Text += message;
        }
        private void LogMessage(string message)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new SetMessageCallback(DisplayLogMessage), message);
        }
        private void WriteLogError()
        {
            LogMessage("ERROR." + Environment.NewLine);
        }

        public string GetText(AutomationElement element)
        {
            object patternObj;
            if (element.TryGetCurrentPattern(ValuePattern.Pattern, out patternObj))
            {
                var valuePattern = (ValuePattern)patternObj;
                return valuePattern.Current.Value;
            }
            else if (element.TryGetCurrentPattern(TextPattern.Pattern, out patternObj))
            {
                var textPattern = (TextPattern)patternObj;
                return textPattern.DocumentRange.GetText(-1).TrimEnd('\r'); // often there is an extra '\r' hanging off the end.
            }
            else
            {
                return element.Current.Name;
            }
        }

        #endregion

        #region Commeted Code

        //public MainWindow()
        //{
        //    InitializeComponent();
        //    this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        //}

        //void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //WindowInteropHelper helper = new WindowInteropHelper(this);
        //    //IntPtr ptr = helper.Handle;

        //    IntPtr ptr = this.WindowHandle();

        //    HwndSource source = (HwndSource)HwndSource.FromVisual(btnStartAutomation);
        //    IntPtr btn_hWnd = source.Handle;

        //    //NativeWindowHandle  855534


        //    ControlProxy controlProxy = ControlProxy.FromHandle(new IntPtr(855534));
        //}

        //private IntPtr WindowHandle()
        //{
        //    HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;

        //    IntPtr handle = IntPtr.Zero;
        //    if (hwndSource != null)
        //    {
        //        handle = hwndSource.Handle;
        //    }
        //    return handle;
        //}

        #endregion
    }
}
