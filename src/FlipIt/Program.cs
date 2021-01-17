/* Originally based on project by Frank McCown in 2010 */

using System;
using System.Globalization;
using System.Windows.Forms;

namespace ScreenSaver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                string firstArgument = args[0].ToUpperInvariant().Trim();
                string secondArgument = args.Length > 1 ? args[1].Trim() : null;

                // Handle cases where arguments are separated by colon.
                // Examples: /C:1234567 or /P:1234567
                if (firstArgument.Length > 2)
                {
                    secondArgument = firstArgument.Substring(3).Trim();
                    firstArgument = firstArgument.Substring(0, 2);
                }

                if (firstArgument == "/C")           // Configuration mode
                {
                    using (SettingsForm settingsForm = new SettingsForm())
                    {
                        Application.Run(settingsForm);
                    }
                }
                else if (firstArgument == "/P")      // Preview mode
                {
                    IntPtr previewWndHandle;
                    if (secondArgument == null)
                    {
                        previewWndHandle = NativeMethods.GetDesktopWindow();
                    }
                    else
                    {
                        previewWndHandle = new IntPtr(long.Parse(secondArgument, CultureInfo.InvariantCulture));
                    }

                    using (MainForm mainForm = new MainForm(previewWndHandle))
                    {
                        Application.Run(mainForm);
                    }
                }
                else if (firstArgument == "/S")      // Full-screen mode
                {
                    ShowScreenSaver();
                    Application.Run();
                }
                else    // Undefined argument
                {
                    MessageBox.Show("Sorry, but the command line argument \"" + firstArgument +
                        "\" is not valid.", "FlipIt",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else    // No arguments - treat like /C
            {
                using (SettingsForm settingsForm = new SettingsForm())
                {
                    Application.Run(settingsForm);
                }
            }
        }

        /// <summary>
        /// Display the form on each of the computer's monitors.
        /// </summary>
        static void ShowScreenSaver()
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                #pragma warning disable CA2000 // Dispose objects before losing scope
                MainForm screensaver = new MainForm(screen.Bounds, screen.Primary);
                #pragma warning restore CA2000 // Dispose objects before losing scope
                screensaver.Show();
            }
        }
    }
}
