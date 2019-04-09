using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pdf_Converter
{
    public partial class Main_Window : Form
    {
        public Main_Window()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Open_File_Btn_Click(object sender, EventArgs e)
        {
            //create an open file dialog variable
            OpenFileDialog openfile = new OpenFileDialog();

            String source = Open_File.Open_PDF(openfile);

            if(source == "Cancelled")
            {
                return;
            }

            String path = Open_File.Create_CSV(source);


            StreamWriter new_file_writer = new StreamWriter(path);

            PdfReader reader = new PdfReader(source);

            //Creates an instance of the MyLocationTextExtractionStrategy to be used to get the rectangular coordinates of each text chunk
            var text_strat = new Text_Extraction_Strategy();

            Header[] header_objects;
            List<string> header_name_list = new List<string>();

            int YLocation = 0;
            var text_switch = false;
            var headers_switch = false;
            int counter = 0;
            int first_line = 0;


            if(Now_RBtn.Checked == true)
            {
                Now_Row[] now_row = new Now_Row[1];

                now_row[0] = new Now_Row();
                header_objects = new Header[9];

                var item = new Header();
                var shipped = new Header();
                var um = new Header();
                var retail = new Header();
                var sale_rtl = new Header();
                var reg_whsl = new Header();
                var cost = new Header();
                var disc = new Header();
                var extension = new Header();


                item.name = ("Item No / Description");
                shipped.name = ("Shipped");
                um.name = ("U/M");
                retail.name = ("Retail");
                sale_rtl.name = ("Sale RTL.");
                reg_whsl.name = ("Reg Whsl");
                cost.name = ("Cost");
                disc.name = ("Disc%");
                extension.name = ("Extension");

                header_objects[0] = item;
                header_objects[1] = shipped;
                header_objects[2] = um;
                header_objects[3] = retail;
                header_objects[4] = sale_rtl;
                header_objects[5] = reg_whsl;
                header_objects[6] = cost;
                header_objects[7] = disc;
                header_objects[8] = extension;


                var check = header_objects[0].name;

                //adds the name of each header into a list of strings
                //this will allow us to check if the current text chunk is one of the headers
                foreach (var header in header_objects)
                {
                    header_name_list.Add(header.name);
                    new_file_writer.Write(header.name + ",");
                }

                new_file_writer.WriteLine("");

                //Loops through each page of the PDF file until the last page has been finished
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    //stores all of the text chunks and their coordinates for the current page as rectangles inside the mypoints list of the t variable
                    PdfTextExtractor.GetTextFromPage(reader, i, text_strat);
                }








                //Loop through each rectangle stored in the myPoints list of our MyLocationTextExtractionStrategy t variable
                foreach (var chunk in text_strat.coordinate_list)
                {
                    //stores the current text and coordinates of the RectAndText object in local variables 
                    string current_text = chunk.Text;
                    int current_XLocation = Convert.ToInt32(chunk.Rect.Left);
                    int current_YLocation = Convert.ToInt32(chunk.Rect.Bottom);


                    //Sets the text_switch and header_switch to true if the current text chunk matches one of the items in our headers list
                    if (header_name_list.Contains(current_text))
                    {

                        text_switch = true;
                        headers_switch = true;

                    }



                    //turns off the text_switch and header_switch and resets all of the current header location variables back to 0 every time one of those two strings are reached in a page
                    //this is because each page contains "Manufacturer of Quality Natural Foods and Vitamins" at the beginning of the page and the last page ends with "Your total savings over catalog price"
                    //So when one of those phrases is reached, the program will stop applying the text to parameters of our row objects until the next time all of the headers have been gone through
                    else if ((current_text.Contains("Your total savings over catalog price is:")) || (current_text.Contains("Manufacturer of Quality Natural Foods and Vitamins")))
                    {
                        first_line++;
                        counter = 0;
                        text_switch = false;
                        headers_switch = false;
                        foreach (var header in header_objects)
                        {
                            header.X = 0;
                        }



                    }

                    //when the vertical location of the current text element is within the specified range, do not add a new line
                    if (((current_YLocation == YLocation) || (current_YLocation == (YLocation - 1))) && (YLocation != 0))
                    {

                    }


                    // when the vertical location is significantly different than the previous vertical location, start a new object
                    else
                    {
                        //Makes sure that the headers have all been gone through (counter>9) or if the text is the very first text on the second page
                        //if either of those conditions are true, it will add a new object to our array
                        if (counter > 9 || (current_text.Contains("Manufacturer of Quality Natural Foods and Vitamins") && first_line > 1))
                        {
                            Array.Resize(ref now_row, now_row.Length + 1);
                            now_row[now_row.Length - 1] = new Now_Row();
                            //store the current vertical location as the new Ylocation to compare to on the next cycle
                            YLocation = current_YLocation;
                        }
                        //if neither of the previous conditions are true, store the current new Y location into a variable
                        else
                        {
                            //store the current vertical location as the new Ylocation to compare to on the next cycle
                            YLocation = current_YLocation;
                        }
                        //Console.WriteLine($"Location: X={location.X:0.00}, Y={location.Y:0.00}");
                    }


                    if (counter == 9)
                    {
                        counter++;
                    }
                    //evaluates if the text_switch and header_switch are true, meaning that one of the items has been reached on the headers list
                    //this stores the current X and Y value of each header object and increases our counter by one

                    if (text_switch)
                    {
                        if (headers_switch)
                        {
                            foreach (var header in header_objects.Where(x => x.X == 0))
                            {


                                if (header.name == current_text)
                                {
                                    header.X = current_XLocation;
                                    header.Y = current_YLocation;
                                    counter++;
                                    break;
                                }
                            }
                        }



                        //checks the counter to make sure that all of the headers have been passed and that the current text is not one of our headers
                        //if both conditions are true, it will check each chunk of text to see what range it falls in
                        //depending upon the text chunk's coordinates, this will determine what column it is under and which object parameter it should be applied to
                        //it will then apply it to the last object in our list 
                        if (counter >= 9 && !(header_name_list.Contains(current_text)))
                        {
                            headers_switch = false;

                            if ((current_XLocation >= 0) && (current_XLocation <= shipped.X - 1))
                            {
                                now_row[now_row.Length - 1].item = current_text;

                            }

                            if ((current_XLocation >= shipped.X) && (current_XLocation <= um.X - 1))
                            {
                                now_row[now_row.Length - 1].shipped = current_text;
                            }

                            else if ((current_XLocation >= um.X) && (current_XLocation <= retail.X - 1))
                            {
                                now_row[now_row.Length - 1].um = current_text;

                            }

                            else if ((current_XLocation >= retail.X) && (current_XLocation <= sale_rtl.X - 1))
                            {
                                now_row[now_row.Length - 1].retail = current_text;
                            }

                            else if ((current_XLocation >= sale_rtl.X) && (current_XLocation <= reg_whsl.X - 1))
                            {
                                now_row[now_row.Length - 1].sale_rtl = current_text;
                            }

                            else if ((current_XLocation >= reg_whsl.X) && (current_XLocation <= cost.X - 1))
                            {
                                now_row[now_row.Length - 1].reg_whsl = current_text;
                            }

                            else if ((current_XLocation >= cost.X) && (current_XLocation <= disc.X - 1))
                            {
                                now_row[now_row.Length - 1].cost = current_text;
                            }

                            else if ((current_XLocation >= disc.X) && (current_XLocation <= extension.X - 1))
                            {
                                now_row[now_row.Length - 1].disc = current_text;
                            }

                            else if ((current_XLocation >= extension.X) && (current_XLocation <= 999))
                            {
                                now_row[now_row.Length - 1].extension = current_text;
                            }


                        }

                    }







                }





                //formats each row in our now_row object list to be in cvs format
                foreach (var row in now_row)
                {


                    new_file_writer.Write("\"" + row.item + "\"" + ",");

                    new_file_writer.Write("\"" + row.shipped + "\"" + ",");

                    new_file_writer.Write("\"" + row.um + "\"" + ",");

                    new_file_writer.Write("\"" + row.retail + "\"" + ",");

                    new_file_writer.Write("\"" + row.sale_rtl + "\"" + ",");

                    new_file_writer.Write("\"" + row.reg_whsl + "\"" + ",");

                    new_file_writer.Write("\"" + row.cost + "\"" + ",");

                    new_file_writer.Write("\"" + row.disc + "\"" + ",");

                    new_file_writer.WriteLine("\"" + row.extension + "\"" + ",");

                }
                new_file_writer.WriteLine("");

                new_file_writer.Flush();

            }
            else if(Nutraceutical_RBtn.Checked == true)
            {
                var product_number = new Header();
                var ship = new Header();
                var adv = new Header();
                var brand = new Header();
                var product = new Header();
                var lot = new Header();
                var srp = new Header();
                var whp = new Header();
                var discount = new Header();
                var net = new Header();
                var pay = new Header();




                List<string> header_list = new List<string>();

                header_objects = new Header[11];

                Nutraceutical_Row[] nutraceutical_row = new Nutraceutical_Row[1];

                nutraceutical_row[0] = new Nutraceutical_Row();

                product_number.name = ("Prod#");
                ship.name = ("Ship*");
                adv.name = ("Adv");
                brand.name = ("Brand");
                product.name = ("Product");
                lot.name = ("Lot#");
                srp.name = ("SRP");
                whp.name = ("WHP");
                discount.name = ("Discount");
                net.name = ("Net");
                pay.name = ("Pay");

                header_objects[0] = product_number;
                header_objects[1] = ship;
                header_objects[2] = adv;
                header_objects[3] = brand;
                header_objects[4] = product;
                header_objects[5] = lot;
                header_objects[6] = srp;
                header_objects[7] = whp;
                header_objects[8] = discount;
                header_objects[9] = net;
                header_objects[10] = pay;

                var check = header_objects[0].name;

                //adds the name of each header into a list of strings
                //this will allow us to check if the current text chunk is one of the header_objects
                foreach (var header in header_objects)
                {
                    header_list.Add(header.name);
                    new_file_writer.Write(header.name + ",");
                }

                new_file_writer.WriteLine("");



                StringBuilder sb = new StringBuilder();



  


                int previous_YLocation = 0;
                int length_modifier = 1;


                //Loops through each page of the PDF file until the last page has been finished
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    //stores all of the text chunks and their coordinates for the current page as rectangles inside the mypoints list of the t variable
                    PdfTextExtractor.GetTextFromPage(reader, i, text_strat);
                }

                foreach (var p in text_strat.coordinate_list)
                {

                    //stores the current text and coordinates of the RectAndText object in local variables 
                    string current_text = p.Text;
                    int current_XLocation = Convert.ToInt32(p.Rect.Left);
                    int current_YLocation = Convert.ToInt32(p.Rect.Bottom);

                


                    //Sets the text_switch and header_switch to true if the current text chunk matches one of the items in our headers list
                    if (header_list.Contains(current_text))
                    {

                        text_switch = true;
                        headers_switch = true;

                    }
                    //turns off the text_switch and header_switch and resets all of the current header location variables back to 0 every time one of those two strings are reached in a page
                    //this is because each page contains "Manufacturer of Quality Natural Foods and Vitamins" at the beginning of the page and the last page ends with "Your total savings over catalog price"
                    //So when one of those phrases is reached, the program will stop applying the text to parameters of our row objects until the next time all of the headers have been gone through
                    else if (current_text.Contains("Please Pay This Amount:") || current_text.Contains("This purchase is subject in all respects to Terms and Conditions of Sale "))
                    {
                        first_line++;
                        counter = 0;
                        text_switch = false;
                        headers_switch = false;
                        foreach (var header in header_objects)
                        {
                            header.X = 0;
                        }



                    }
                    //when the vertical location of the current text element is within the specified range, do not add a new line
                    if (((current_YLocation == YLocation) || ((current_YLocation >= (YLocation - 2)) && (current_YLocation <= (YLocation + 2)))) && (YLocation != 0))
                    {
                        previous_YLocation = current_YLocation;
                    }
                    // when the vertical location is significantly different than the previous vertical location, start a new object
                    else
                    {
                        //Makes sure that the headers have all been gone through (counter>9) or if the text is the very first text on the second page
                        //if either of those conditions are true, it will add a new object to our array
                        if (counter > 11 || current_text.Contains("This purchase is subject in all respects to Terms and Conditions of Sale"))
                        {
                            if(previous_YLocation == current_YLocation )
                            {
                                length_modifier = 2;
                                YLocation = current_YLocation;
                            }
                            else
                            {
                                length_modifier = 1;
                                Array.Resize(ref nutraceutical_row, nutraceutical_row.Length + 1);
                                nutraceutical_row[nutraceutical_row.Length - 1] = new Nutraceutical_Row();
                                //store the current vertical location as the new Ylocation to compare to on the next cycle
                                YLocation = current_YLocation;
                            }

                        }
                        //if neither of the previous conditions are true, store the current new Y location into a variable
                        else
                        {
                            //store the current vertical location as the new Ylocation to compare to on the next cycle
                            YLocation = current_YLocation;
                        }
                        //Console.WriteLine($"Location: X={location.X:0.00}, Y={location.Y:0.00}");
                    }


                    if (counter == 11)
                    {
                        counter++;
                    }
                    //evaluates if the text_switch and header_switch are true, meaning that one of the items has been reached on the headers list
                    //this stores the current X and Y value of each header object and increases our counter by one

                    if (text_switch)
                    {
                        if (headers_switch)
                        {
                            foreach (var header in header_objects.Where(x => x.X == 0))
                            {


                                if (header.name == current_text.Trim())
                                {
                                    header.X = current_XLocation;
                                    header.Y = current_YLocation;
                                    counter++;
                                    break;
                                }
                            }
                        }



                        //checks the counter to make sure that all of the headers have been passed and that the current text is not one of our headers
                        //if both conditions are true, it will check each chunk of text to see what range it falls in
                        //depending upon the text chunk's coordinates, this will determine what column it is under and which object parameter it should be applied to
                        //it will then apply it to the last object in our list 
                        if (counter >= 11 && !(header_list.Contains(current_text)))
                        {
                            headers_switch = false;
                        
                            if ((current_XLocation >= 0) && (current_XLocation <= header_objects[1].X - 1))
                            {
                                nutraceutical_row[nutraceutical_row.Length - length_modifier].product_number = current_text;

                            }

                            else if ((current_XLocation >= header_objects[1].X) && (current_XLocation <= header_objects[2].X - 1))
                            {
                                nutraceutical_row[nutraceutical_row.Length - length_modifier].ship = current_text;
                            }

                            else if ((current_XLocation >= header_objects[2].X) && (current_XLocation <= header_objects[3].X - 1))
                            {
                                nutraceutical_row[nutraceutical_row.Length - length_modifier].adv = current_text;
                            }

                            else if ((current_XLocation >= header_objects[3].X) && (current_XLocation <= header_objects[4].X - 1))
                            {
                                nutraceutical_row[nutraceutical_row.Length - length_modifier].brand = current_text;
                            }

                            else if ((current_XLocation >= header_objects[4].X) && (current_XLocation <= header_objects[5].X - 1))
                            {
                                nutraceutical_row[nutraceutical_row.Length - length_modifier].product = current_text;
                            }

                            else if ((current_XLocation >= header_objects[5].X) && (current_XLocation <= header_objects[6].X - 30))
                            {
                                nutraceutical_row[nutraceutical_row.Length - length_modifier].lot = current_text;
                            }

                            else if ((current_XLocation >= (header_objects[6].X -29)) && (current_XLocation <= header_objects[7].X - 30))
                            {
                                nutraceutical_row[nutraceutical_row.Length - length_modifier].srp = current_text;
                            }

                            else if ((current_XLocation >= header_objects[7].X - 29) && (current_XLocation <= header_objects[8].X - 1))
                            {
                                nutraceutical_row[nutraceutical_row.Length - length_modifier].whp = current_text;
                            }

                            else if ((current_XLocation >= header_objects[8].X) && (current_XLocation <= header_objects[9].X - 30))
                            {
                                nutraceutical_row[nutraceutical_row.Length - length_modifier].discount = current_text;
                            }

                            else if ((current_XLocation >= header_objects[9].X - 29) && (current_XLocation <= header_objects[10].X - 30))
                            {
                                nutraceutical_row[nutraceutical_row.Length - length_modifier].net = current_text;
                            }

                            else if ((current_XLocation >= header_objects[10].X - 29) && (current_XLocation <= 999))
                            {
                                nutraceutical_row[nutraceutical_row.Length - length_modifier].pay = current_text;
                            }

                        }

                    }

                }

                foreach (var row in nutraceutical_row)
                {
                
                    if (row.product_number == null)
                    {
                        row.product_number = " ";
                    }

                    if (row.ship == null)
                    {
                        row.ship = " ";
                    }

                    if (row.adv == null)
                    {
                        row.adv = " ";
                    }

                    if (row.brand == null)
                    {
                        row.brand = " ";
                    }

                    if (row.product == null)
                    {
                        row.product = " ";
                    }

                    if (row.lot == null)
                    {
                        row.lot = " ";
                    }

                    if (row.srp == null)
                    {
                        row.srp = " ";
                    }

                    if (row.whp == null)
                    {
                        row.whp = " ";
                    }

                    if (row.discount == null)
                    {
                        row.discount = " ";
                    }

                    if (row.net == null)
                    {
                        row.net = " ";
                    }

                    if (row.pay == null)
                    {
                        row.pay = " ";
                    }




                    new_file_writer.Write("\"" + row.product_number + "\"" + ",");

                    new_file_writer.Write("\"" + row.ship + "\"" + ",");

                    new_file_writer.Write("\"" + row.adv + "\"" + ",");

                    new_file_writer.Write("\"" + row.brand + "\"" + ",");

                    new_file_writer.Write("\"" + row.product + "\"" + ",");

                    new_file_writer.Write("\"" + row.lot + "\"" + ",");

                    new_file_writer.Write("\"" + row.srp + "\"" + ",");

                    new_file_writer.Write("\"" + row.whp + "\"" + ",");

                    new_file_writer.Write("\"" + row.discount + "\"" + ",");

                    new_file_writer.Write("\"" + row.net + "\"" + ",");

                    new_file_writer.WriteLine("\"" + row.pay + "\"" + ",");

                }
                new_file_writer.WriteLine("");

                new_file_writer.Flush();


            }
        }
    }
}
