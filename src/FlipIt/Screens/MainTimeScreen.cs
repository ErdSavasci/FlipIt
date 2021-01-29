using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Windows.Forms;

namespace ScreenSaver
{
    public class MainTimeScreen : TimeScreen, IDisposable
    {
        private const int SplitWidth = 4;

        private readonly float FontScaleFactor = 5F;
        private readonly bool previewMode;
        private readonly int fontSize = 350;
        private readonly bool will24HoursWillBeShowed;
        private readonly bool willSecondsBeShowed;
        private readonly float timeBoxScale;
        private readonly Font primaryFont;
        private readonly Font primarySmallFont;
        private readonly Graphics graphics;
        private readonly Rectangle currentScreenBounds;

        // Alternative fonts:
        // * league-gothic from https://github.com/theleagueof/league-gothic
        // * http://tipotype.com/aileron/

        private const string familyName = "Oswald"; //"Texgyreheroscn";"Bebas"
        private static readonly Color backColorTop = Color.FromArgb(255, 15, 15, 15);
        private static readonly Color backColorBottom = Color.FromArgb(255, 10, 10, 10);
        private readonly Brush backFillTop = new SolidBrush(backColorTop);
        private readonly Brush backFillBottom = new SolidBrush(backColorBottom);
        private readonly Brush FontBrush = new SolidBrush(Color.FromArgb(255, 183, 183, 183));
        private readonly Pen SplitPen = new Pen(Color.Black, SplitWidth);
        private readonly Pen SmallSplitPen = new Pen(Color.Black, SplitWidth / 2);

        private bool closing;

        public MainTimeScreen(ref Graphics formGraphics, Rectangle bounds)
        {
            // in
            currentScreenBounds = bounds;
            graphics = formGraphics;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            will24HoursWillBeShowed = Utilities.Will24HoursBeShowed();
            willSecondsBeShowed = Utilities.WillSecondsBeShowed();
            timeBoxScale = Utilities.GetTimeBoxScale();

            fontSize = Convert.ToInt32(currentScreenBounds.Width / FontScaleFactor);
            fontSize = Convert.ToInt32(Math.Floor(fontSize * (timeBoxScale / 3)));
            primaryFont = new Font(familyName, fontSize, FontStyle.Bold);
            primarySmallFont = new Font(familyName, fontSize / 9, FontStyle.Bold);
        }

        public MainTimeScreen(Form parentForm, ref Graphics formGraphics, out Rectangle formRect, IntPtr formHandle, IntPtr previewWndHandle, object[] args)
        {
            if (parentForm == null)
            {
                throw new ArgumentNullException(nameof(parentForm));
            }

            will24HoursWillBeShowed = args != null && args.Length > 0 ? bool.Parse((string)args[0]) : Utilities.Will24HoursBeShowed();
            willSecondsBeShowed = args != null && args.Length > 1 ? bool.Parse((string)args[1]) : Utilities.WillSecondsBeShowed();
            timeBoxScale = args != null && args.Length > 2 ? float.Parse((string)args[2], CultureInfo.InvariantCulture) : Utilities.GetTimeBoxScale();

            // Set the preview window as the parent of this window
            NativeMethods.SetParent(formHandle, previewWndHandle);
            parentForm.Focus();

            // Make this a child window so it will close when the parent dialog closes
            _ = NativeMethods.SetWindowLong(formHandle, -16, new IntPtr(NativeMethods.GetWindowLong(formHandle, -16) | 0x40000000));

            // Place our window inside the parent
            NativeMethods.GetClientRect(previewWndHandle, out var parentRect);

            // in
            currentScreenBounds = new Rectangle(parentRect.X, parentRect.Y, parentRect.Width, parentRect.Height);
            previewMode = true;
            graphics = formGraphics;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            formRect = parentRect;

            // Make text smaller for preview window
            fontSize = Convert.ToInt32(currentScreenBounds.Width / FontScaleFactor);
            fontSize = Convert.ToInt32(Math.Floor(fontSize * (timeBoxScale / 3)));
            primaryFont = new Font(familyName, fontSize, FontStyle.Bold);
            primarySmallFont = new Font(familyName, fontSize / 9, FontStyle.Bold);
        }

        public override void DrawIt(bool? drawSeconds)
        {
            if (closing || (drawSeconds.HasValue && drawSeconds.Value && !willSecondsBeShowed))
            {
                return;
            }

            DrawCurrentTime();
        }

        private void DrawCurrentTime()
        {
            var height = Convert.ToInt32(Math.Floor((primaryFont.Height * 10 * (timeBoxScale / 3) / 8)));
            var width = !willSecondsBeShowed ? Convert.ToInt32(2.05 * height) : Convert.ToInt32(3.1 * height);

            var x = (currentScreenBounds.Width - width) / 2;
            var y = (currentScreenBounds.Height - height) / 2;

            var pm = SystemTime.Now.Hour >= 12;
            DrawIt(x, y, height, SystemTime.Now.ToString(will24HoursWillBeShowed ? "HH" : "hh", CultureInfo.InvariantCulture), will24HoursWillBeShowed ? null : (pm ? null : "AM"), will24HoursWillBeShowed ? null : (pm ? "PM" : null)); // The % avoids a FormatException https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx#UsingSingleSpecifiers

            x += height + (height / 20);
            DrawIt(x, y, height, SystemTime.Now.ToString("mm", CultureInfo.InvariantCulture));

            if (willSecondsBeShowed)
            {
                x += height + (height / 20);
                DrawIt(x, y, height, SystemTime.Now.ToString("ss", CultureInfo.InvariantCulture));
            }
        }

        private void DrawIt(int x, int y, int size, string s, string topString = null, string bottomString = null)
        {
            // Draw the background
            var diff = size / 10;
            var textRect = new Rectangle(x - diff, y + diff / 2, size + diff * 2, size);

            var radius = size / 20;
            var diameter = radius * 2;
            graphics.FillEllipse(backFillTop, x, y, diameter, diameter); // top left
            graphics.FillEllipse(backFillTop, x + size - diameter, y, diameter, diameter); // top right
            graphics.FillEllipse(backFillBottom, x, y + size - diameter, diameter, diameter); // bottom left
            graphics.FillEllipse(backFillBottom, x + size - diameter, y + size - diameter, diameter, diameter); //bottom right

            graphics.FillRectangle(backFillTop, x + radius, y, size - diameter, diameter);
            graphics.FillRectangle(backFillBottom, x + radius, y + size - diameter, size - diameter, diameter);

            var linGrBrush = new LinearGradientBrush(
                new Point(10, y + radius),
                new Point(10, y + size - radius),
                backColorTop,
                backColorBottom);
            graphics.FillRectangle(linGrBrush, x, y + radius, size, size - diameter);
            linGrBrush.Dispose();

            //	if (s.Length == 1)
            //	{
            //		s = "\u2002" + s; // Add an EN SPACE which is 1/2 em
            //	}

            // Draw the text
            using (var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.None, FormatFlags = StringFormatFlags.NoWrap })
            {
                graphics.DrawString(s, primaryFont, FontBrush, textRect, stringFormat);
            }

            if (topString != null)
            {
                //Gfx.DrawString(bottomString, SmallFont, FontBrush, textRect.X, textRect.Bottom - SmallFont.Height);
                graphics.DrawString(topString, primarySmallFont, FontBrush, x + diameter, y + diameter / 2);
            }
            if (bottomString != null)
            {
                graphics.DrawString(bottomString, primarySmallFont, FontBrush, x + diameter, y + size - diameter - primarySmallFont.Height / 2);
            }

            // Horizontal dividing line
            if (!previewMode)
            {
                var penY = y + (size / 2) - (SplitWidth / 2);
                graphics.DrawLine(SplitPen, x, penY, x + size, penY);
            }
            else
            {
                graphics.DrawLine(Pens.Black, x, y + (size / 2), x + size, y + (size / 2));
            }
        }

        public override void Closing()
        {
            closing = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            CustomDispose(disposing);
        }

        private void CustomDispose(bool disposing)
        {
            if (disposing)
            {
                backFillTop?.Dispose();
                backFillBottom?.Dispose();
                primaryFont?.Dispose();
                primarySmallFont?.Dispose();
                SmallSplitPen?.Dispose();
                SplitPen?.Dispose();
                FontBrush?.Dispose();
                graphics?.Dispose();
            }

            Dispose(disposing);
        }
    }
}
