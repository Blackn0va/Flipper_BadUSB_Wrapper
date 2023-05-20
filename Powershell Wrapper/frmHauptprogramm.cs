using System;
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


            //AllowDrop multible file drips on listbox
            e.Effect = DragDropEffects.All;

        }

        private void cmdConvert_Click(object sender, EventArgs e)
        {

            //foreach file in listbox
            foreach (string filefromListbox in listBox1.Items)
            {

                String Datei = filefromListbox.ToString();
                Datei = Datei.Replace(".txt", "_flip.txt");

                //write the newfile Datei
                System.IO.StreamWriter newfile = new System.IO.StreamWriter(Datei);

                //read the file
                System.IO.StreamReader reader = new System.IO.StreamReader(filefromListbox);

                //add some lines
                newfile.WriteLine("DELAY 2000");
                newfile.WriteLine("GUI r");
                newfile.WriteLine("DELAY 1000");
                newfile.WriteLine("STRING powershell");
                newfile.WriteLine("DELAY 500");
                newfile.WriteLine("ENTER");
                newfile.WriteLine("DELAY 2000");
                newfile.WriteLine("STRING cls");
                newfile.WriteLine("ENTER");
                newfile.Close();



                    string[] lines = System.IO.File.ReadAllLines(filefromListbox);
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

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {

            //get filepath of dropped item and add it to the listbox
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            //for each file in files
            foreach (string file in files)
            {
                //add file to listbox
                listBox1.Items.Add(file);

                //AllowDrop multible file drips on listbox
                e.Effect = DragDropEffects.All;
            }


        }
    }
}
