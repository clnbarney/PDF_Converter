using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pdf_Converter
{
    class Open_File
    {
        public static String Open_PDF(OpenFileDialog openfile)
        {
            //Sets the initial file directory that opens as the c:\
            openfile.InitialDirectory = "C:\\";


            //Opens the file dialog box for the user to select a PDF file, checks which button was pressed
            DialogResult res = openfile.ShowDialog();

            //if cancel is pressed, or no file is selected, end the button click event so the rest of the code doesn't error out
            if (res == DialogResult.Cancel)
            {
                String cancel = "Cancelled";
                return cancel;
            }
            else
            {
                //Stores the filepath in a string variable
                string source = openfile.FileName;

                return source;
            }

        }

        public static String Create_CSV(String source)
        {
            string file_name = (source).Substring(0, (source).LastIndexOf('.'));


            string path = file_name + ".csv";

            Random rand = new Random();

            if (File.Exists(path))
            {
                path = file_name + "_" + rand.Next() + ".csv";
            }

            return path;
        }
    }
}
