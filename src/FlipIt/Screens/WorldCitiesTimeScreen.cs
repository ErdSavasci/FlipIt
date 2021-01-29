using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace ScreenSaver
{
    public class WorldCitiesTimeScreen : TimeScreen, IDisposable
    {
        private const int SplitWidth = 4;
        private const int HorizontalGapBetweenBoxesPercent = 5;
        private const int VerticalGapBetweenBoxesPercent = 10;
        private const int BoxWidthPercentage = 70;

        private readonly float FontScaleFactor = 5F;
        private readonly int fontSize = 350;
        private readonly bool will24HoursWillBeShowed;
        private readonly bool willSecondsBeShowed;
        private readonly int citiesSortOrderType;
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

        private Font cityFont;
        private Font smallCityFont;
        private bool closing;
        
        private List<City> cities;
        private List<City> Cities => cities ?? (cities = Utilities.GetCities());

        public WorldCitiesTimeScreen(ref Graphics formGraphics, Rectangle bounds)
        {
            // in
            currentScreenBounds = bounds;
            graphics = formGraphics;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            will24HoursWillBeShowed = Utilities.Will24HoursBeShowed();
            willSecondsBeShowed = Utilities.WillSecondsBeShowed();
            citiesSortOrderType = Utilities.GetCitiesSortOrder();
            timeBoxScale = Utilities.GetTimeBoxScale();
            
            fontSize = Convert.ToInt32(currentScreenBounds.Width / FontScaleFactor);
            fontSize = Convert.ToInt32(Math.Floor(fontSize * (timeBoxScale / 3)));
            primaryFont = new Font(familyName, fontSize, FontStyle.Bold);
            primarySmallFont = new Font(familyName, fontSize / 9, FontStyle.Bold);
        }

        public WorldCitiesTimeScreen(Form parentForm, ref Graphics formGraphics, out Rectangle formSize, IntPtr formHandle, IntPtr previewWndHandle, object[] args)
        {
            if (parentForm == null)
            {
                throw new ArgumentNullException(nameof(parentForm));
            }

            will24HoursWillBeShowed = args != null && args.Length > 0 ? bool.Parse((string)args[0]) : Utilities.Will24HoursBeShowed();
            willSecondsBeShowed = args != null && args.Length > 1 ? bool.Parse((string)args[1]) : Utilities.WillSecondsBeShowed();
            timeBoxScale = args != null && args.Length > 2 ? float.Parse((string)args[2], CultureInfo.InvariantCulture) : Utilities.GetTimeBoxScale();
            citiesSortOrderType = Utilities.GetCitiesSortOrder();

            // Set the preview window as the parent of this window
            NativeMethods.SetParent(formHandle, previewWndHandle);
            parentForm.Focus();

            // Make this a child window so it will close when the parent dialog closes
            _ = NativeMethods.SetWindowLong(formHandle, -16, new IntPtr(NativeMethods.GetWindowLong(formHandle, -16) | 0x40000000));

            // Place our window inside the parent
            NativeMethods.GetClientRect(previewWndHandle, out var parentRect);

            // in
            currentScreenBounds = new Rectangle(parentRect.X, parentRect.Y, parentRect.Width, parentRect.Height);
            graphics = formGraphics;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            formSize = parentRect;

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

            DrawCities();
        }

        private void DrawCities()
        {
            const int timeLengthInChars = 8;
            const int dstIndicatorLength = 1;
            const int maxBoxHeight = 160;

            var maxNameLengthInChars = Cities.Max(c => c.DisplayName.Length);
            var maxRowLengthInChars = maxNameLengthInChars + 2 + timeLengthInChars + 1 + dstIndicatorLength;

            var maxWidth = currentScreenBounds.Width - (40 * (currentScreenBounds.Width * (Math.Max(0, currentScreenBounds.Width - 1000)) / 1000 / 100));      // leave some margin
            var maxHeight = currentScreenBounds.Height - 40;    // leave some margin

            var roughBoxWidth = maxWidth / maxRowLengthInChars; // the rough max width of boxes
            var roughBoxHeight = maxHeight / Cities.Count;
            var boxHeight = Math.Min(roughBoxHeight, roughBoxWidth.PercentInv(BoxWidthPercentage));
            boxHeight -= boxHeight.Percent(VerticalGapBetweenBoxesPercent);
            boxHeight = Math.Min(boxHeight, maxBoxHeight);

            if (cityFont == null)
            {
                cityFont = new Font(familyName, Math.Max(1, boxHeight.Percent(80)), FontStyle.Regular, GraphicsUnit.Pixel);
                smallCityFont = new Font(familyName, Math.Max(1, boxHeight.Percent(25)), FontStyle.Regular, GraphicsUnit.Pixel);
            }

            var verticalGap = boxHeight.Percent(VerticalGapBetweenBoxesPercent);

            //var heightForAllRows = (boxHeight + verticalGap) * cities.Count - verticalGap;
            var heightForAllRows = CalcSize(Cities.Count, boxHeight, verticalGap);
            var y = (currentScreenBounds.Height - heightForAllRows) / 2;
            if (y < 20) {
                y = 20;
            } 
            var startingX = (currentScreenBounds.Width - maxRowLengthInChars * boxHeight.Percent(BoxWidthPercentage)).Percent(40);

            var boxSize = new Size(boxHeight.Percent(BoxWidthPercentage), boxHeight);
            var horizontalGap = boxSize.Height.Percent(HorizontalGapBetweenBoxesPercent);

            // Clear screen and fill it with black background color
            graphics.Clear(Color.Black);

            foreach (var city in (citiesSortOrderType == 1 ? Cities.OrderBy(c => c.DisplayName) : Cities.OrderBy(c => c.CurrentTime)))
            {
                city.RefreshTime(SystemTime.Now);
                var s = city.DisplayName.PadRight(maxNameLengthInChars + 2) + FormatTime(city.CurrentTime);
                DrawString(startingX, y, boxSize, horizontalGap, s, city);
                y += boxHeight + verticalGap;
            }
        }

        private static int CalcSize(int itemCount, int itemSize, int gapSize)
        {
            return (itemCount * (itemSize + gapSize)) - gapSize;
        }

        private string FormatTime(DateTime time)
        {
            var result = time.ToString(will24HoursWillBeShowed ? "HH:mm" + (willSecondsBeShowed ? ":ss" : "") : "hh:mm" + (willSecondsBeShowed ? ":ss" : "") + " tt", CultureInfo.InvariantCulture) + " ";
            return $"{result,9}";
        }

        private void DrawString(int x, int y, Size boxSize, int horizontalGap, string s, City city)
        {
            var boxRectangle = new Rectangle(new Point(x, y), boxSize);
            foreach (var c in s.ToUpperInvariant())
            {
                DrawCharInBox(boxRectangle, c);
                boxRectangle.X = boxRectangle.Right + horizontalGap;
            }

            if (city.IsDaylightSavingTime || city.DaysDifference != 0)
            {
                DrawSmallStringsInBox(boxRectangle,
                    city.IsDaylightSavingTime ? "DST" : null,
                    city.DaysDifference != 0 ? $"{city.DaysDifference:+#;-#}d" : null);
            }
            else
            {
                DrawCharInBox(boxRectangle, ' ');
            }
        }

        private void DrawCharInBox(Rectangle boxRectangle, char theChar)
        {
            DrawBox(boxRectangle);
            DrawString(theChar.ToString(), cityFont, boxRectangle);
            DrawSplitter(boxRectangle);
        }

        private void DrawSmallStringsInBox(Rectangle boxRectangle, string top, string bottom)
        {
            DrawBox(boxRectangle, top, bottom);
            DrawSplitter(boxRectangle);
        }

        private void DrawSplitter(Rectangle box)
        {
            var penY = box.Y + (box.Height / 2) - (SplitWidth / 2 / 2);
            graphics.DrawLine(SmallSplitPen, box.Left, penY, box.Right, penY);
        }

        private void DrawBox(Rectangle box, string topString = null, string bottomString = null)
        {
            // Alternative, simple way to draw box
            // Gfx.FillRectangle(backFillTop, box.X, box.Y, box.Width, box.Height / 2);
            // Gfx.FillRectangle(backFillBottom, box.X, box.Y + (box.Height / 2), box.Width, box.Height / 2);

            var radius = Math.Max(1, box.Height / 20);
            var halfRectangle = new Rectangle(box.X, box.Y, box.Width, box.Height / 2);

            DrawHalfBox(halfRectangle, radius, RectangleCorner.TopLeft | RectangleCorner.TopRight, topString);

            // Move the rect down
            halfRectangle.Y = halfRectangle.Y + halfRectangle.Height + 1;

            DrawHalfBox(halfRectangle, radius, RectangleCorner.BottomLeft | RectangleCorner.BottomRight, bottomString);
        }

        private void DrawHalfBox(Rectangle halfRectangle, int radius, RectangleCorner corners, string s)
        {
            using (var path = RoundedRectangle.Create(halfRectangle, radius, corners))
            {
                graphics.FillPath(backFillTop, path);
            }

            if (!String.IsNullOrEmpty(s))
            {
                DrawString(s, smallCityFont, halfRectangle);
            }
        }


        private void DrawString(string s, Font font, Rectangle box)
        {
            using (var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                graphics.DrawString(s, font, FontBrush, box, stringFormat);
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
                cityFont?.Dispose();
                smallCityFont?.Dispose();
                SmallSplitPen?.Dispose();
                SplitPen?.Dispose();
                FontBrush?.Dispose();
                graphics?.Dispose();
            }

            Dispose(disposing);
        }
    }
}
