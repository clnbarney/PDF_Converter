using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf_Converter
{
    class Text_Rectangle
    {
        public iTextSharp.text.Rectangle Rect;
        public String Text;
        public Text_Rectangle(iTextSharp.text.Rectangle rect, String text)
        {
            this.Rect = rect;
            this.Text = text;
        }
    }
}
