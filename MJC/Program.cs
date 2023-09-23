using Microsoft.Reporting.NETCore;
using MJC.common;
using MJC.forms;
using MJC.forms.login;
using System.Collections;
using System.Data;
using System.Reflection;
using MJC.model;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Data.SqlClient;
using MJC.common;
using MJC.config;
using System.IO;
using System.Drawing;
using Microsoft.Extensions.Logging;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using LogLevel = Microsoft.AppCenter.LogLevel;

namespace MJC
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 
        // TODO: @Nobel, please move the following code to Sessions.
        public static bool permissionOrders { get; set; }
        public static bool permissionInventory { get; set; }
        public static bool permissionReceivables { get; set; }
        public static bool permissionSetting { get; set; }
        public static bool permissionUsers { get; set; }
        public static bool permissionQuickBooks { get; set; }

        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.EnableVisualStyles();
            ApplicationConfiguration.Initialize();
            AppCenter.Start("80bb0e62-814b-4f68-9a48-3786a507b4b0",
                  typeof(Analytics), typeof(Crashes));
            AppCenter.LogLevel = LogLevel.Verbose;

            Sentry.SentrySdk.Init(o =>
            {
                // Tells which project in Sentry to send events to:
                o.Dsn = "https://4b7926db913b708af6e2bdde51bc6243@o382651.ingest.sentry.io/4505844276527104";
                // When configuring for the first time, to see what the SDK is doing:
                o.Debug = true;
                // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
                // We recommend adjusting this value in production.
                o.TracesSampleRate = 1.0;
                o.IsGlobalModeEnabled = true;
                o.Release = "mjc-desktop@v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
#if DEBUG
                o.Environment = "development";
#else
                o.Environment = "production";
#endif
            });
            // Configure WinForms to throw exceptions so Sentry can capture them.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

            ShowSplash();
            Session.Initialize();

            Login login = new Login();
            Application.Run(login);
        }

        private static void ShowSplash()
        {
            Splash sp = new Splash();
            sp.Show();
            
            Application.DoEvents();
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 500;
            t.Tick += new EventHandler((sender, ea) =>
            {
                sp.BeginInvoke(new Action(() =>
                {
                    
                    if (sp != null && Application.OpenForms.Count > 1)
                    {
                        sp.Close();
                        sp.Dispose();
                        sp = null;
                        t.Stop();
                        t.Dispose();
                        t = null;
                    }
                }));
            });
            t.Start();
        }
    }
}