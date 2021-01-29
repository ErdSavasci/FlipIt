using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSaver
{
    public abstract class TimeScreen
    {
        public abstract void DrawIt(bool? drawSeconds);
        public abstract void Closing();
    }
}
