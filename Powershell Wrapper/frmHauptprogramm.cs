using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Powershell_Wrapper
{
    public partial class frmHauptprogramm : Form
    {
        System.IO.StreamWriter file;
            
        public frmHauptprogramm()
        {
            InitializeComponent();
        }
        
        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            //get filepath of dropped item and add it to the listbox
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            listBox1.Items.Add(files[0]);


            String Datei = listBox1.Items[0].ToString();
            Datei = Datei.Replace(".txt", "_flip.txt");




            //open file from listbox an read line for line
            file = new System.IO.StreamWriter(Datei, true);
            file.WriteLine("DELAY 2000");
            file.WriteLine("GUI r");
            file.WriteLine("DELAY 1000");
            file.WriteLine("STRING powershell");
            file.WriteLine("DELAY 500");
            file.WriteLine("ENTER");
            file.WriteLine("DELAY 2000");
            file.WriteLine("STRING cls");
            file.WriteLine("ENTER");
            file.Close();


            string[] lines = System.IO.File.ReadAllLines(listBox1.Items[0].ToString());
            foreach (string line in lines)
            {
                //make sure the line is not empty
                if (line != "")
                {
                    if (!line.StartsWith("//"))
                    {
                        //split the line into two parts
                        string[] parts = line.Split(';');

                        file = new System.IO.StreamWriter(Datei, true);
                        file.WriteLine("STRING " + line);
                        file.WriteLine("ENTER");
                        file.Close();
                    }

                }


            }

        }
 

    }
}
