/* Originally based on project by Frank McCown in 2010 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenSaver
{
    public sealed partial class MainForm : Form
    {
        private readonly bool persistentFrame;
        private readonly TimeScreen timeScreen;
        private readonly Graphics graphics;

        private int lastMinute = -1;
        private Point mouseLocation;

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(Screen currentScreen)
        {
            if (currentScreen == null)
            {
                throw new ArgumentNullException(nameof(currentScreen));
            }

            InitializeComponent();

            Bounds = currentScreen.Bounds;
            graphics = CreateGraphics();

            if (currentScreen.Primary)
            {
                if (!Utilities.ShowWorldCitiesAtMainScreen())
                {
                    timeScreen = new MainTimeScreen(ref graphics, Bounds);
                }
                else
                {
                    timeScreen = new WorldCitiesTimeScreen(ref graphics, Bounds);
                }
            }
            else
            {
                if (!Utilities.ShowWorldCitiesAtMainScreen())
                {
                    timeScreen = new WorldCitiesTimeScreen(ref graphics, Bounds);
                }
                else
                {
                    timeScreen = new MainTimeScreen(ref graphics, Bounds);
                }
            }
        }

        public MainForm(Form parentForm, IntPtr previewWndHandle, bool showCities, bool persistentFrame, object[] args)
        {
            InitializeComponent();

            this.persistentFrame = persistentFrame;
            Location = new Point(0, 0);
            graphics = CreateGraphics();
            Rectangle rect;

            if (parentForm == null)
            {
                parentForm = this;
            }

            if (!showCities)
            {
                
                timeScreen = new MainTimeScreen(parentForm, ref graphics, out rect, Handle, previewWndHandle, args);
                Size = rect.Size;
            }
            else
            {
                timeScreen = new WorldCitiesTimeScreen(parentForm, ref graphics, out rect, Handle, previewWndHandle, args);
                Size = rect.Size;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!persistentFrame)
            {
                Cursor.Hide();
            }

            TopMost = true;

            MoveTimer_Tick(null, null);
            moveTimer.Interval = 1000;
            moveTimer.Tick += MoveTimer_Tick;
            moveTimer.Start();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            // Some test times
            /* if (Debugger.IsAttached)
            {
                // SystemTime.NowForTesting = new DateTime(2020, 6, 7, 1, 23, 45);
                // SystemTime.NowForTesting = new DateTime(2020, 6, 7, 23, 45, 57);
            } 
            */
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            var now = SystemTime.Now;

            var minute = now.Minute;
            if (lastMinute != minute)
            {
                // Update every minute
                lastMinute = minute;
                timeScreen.DrawIt(false);
            }

            timeScreen.DrawIt(true);

            // Move text to new location
            //			if (now.Second % 5 == 0)
            //			{
            //		        textLabel.Left = rand.Next(Math.Max(1, Bounds.Width - textLabel.Width));
            //		        textLabel.Top = rand.Next(Math.Max(1, Bounds.Height - textLabel.Height));
            //	        }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseLocation.IsEmpty)
            {
                // Terminate if mouse is moved a significant distance
                if (Math.Abs(mouseLocation.X - e.X) > 5 ||
                    Math.Abs(mouseLocation.Y - e.Y) > 5)
                {
                    if (!persistentFrame)
                    {
                        timeScreen.Closing();
                        Close();
                        Application.Exit();
                    }
                }
            }

            // Update current mouse location
            mouseLocation = e.Location;
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!persistentFrame)
            {
                timeScreen.Closing();
                Close();
                Application.Exit();
            }
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!persistentFrame)
            {
                timeScreen.Closing();
                Close();
                Application.Exit();
            }
        }

        #pragma warning disable IDE0051 // Remove unused private members
        private new void Close()
        #pragma warning restore IDE0051 // Remove unused private members
        {
            timeScreen.Closing();
            CustomDispose(true);
            Dispose();
            base.Close();
        }

        private void CustomDispose(bool disposing)
        {
            if (disposing)
            {
                graphics.Dispose();
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            timeScreen.DrawIt(null);
        }
    }
}
