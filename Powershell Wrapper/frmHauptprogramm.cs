using System;
using System.Windows.Forms;

namespace Powershell_Wrapper
{
    public partial class frmHauptprogramm : Form
    {
        public frmHauptprogramm()
        {
            InitializeComponent();
        }

        [STAThread]
        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            // AllowDrop multiple file drops on listbox
            e.Effect = DragDropEffects.All;
        }
        [STAThread]
        private void cmdConvert_Click(object sender, EventArgs e)
        {
            if (toJavascriptCheck.Checked)
            {
                toJavascript();
            }

            if (toBadUSBCheck.Checked)
            {
                toBADUSB();
            }
        }
        [STAThread]
        public void toJavascript()
        {
            // Für jede Datei in der Listbox
            foreach (string filefromListbox in listBox1.Items)
            {
                string Datei = filefromListbox.Replace(".txt", ".js");

                // Neue Datei erstellen
                using (System.IO.StreamWriter newfile = new System.IO.StreamWriter(Datei))
                {
                    // Datei lesen
                    string[] lines = System.IO.File.ReadAllLines(filefromListbox);

                    // JavaScript-Code schreiben
                    newfile.WriteLine("let badusb = require(\"badusb\");");
                    newfile.WriteLine("let notify = require(\"notification\");");
                    newfile.WriteLine("let dialog = require(\"dialog\");");
                    newfile.WriteLine("let serial = require(\"serial\"); // Load \"serial\" module");
                    newfile.WriteLine();
                    newfile.WriteLine("badusb.setup({");
                    newfile.WriteLine("    vid: 0xAAAA,");
                    newfile.WriteLine("    pid: 0xBBBB,");
                    newfile.WriteLine("    mfr_name: \"Flipper\",");
                    newfile.WriteLine("    prod_name: \"Zero\",");
                    newfile.WriteLine("    layout_path: \"/ext/badusb/assets/layouts/de-DE.kl\"");
                    newfile.WriteLine("});");
                    newfile.WriteLine("dialog.message(\"Intune Onboarding\", \"Press OK zum starten\");");
                    newfile.WriteLine();
                    newfile.WriteLine("if (badusb.isConnected()) {");
                    newfile.WriteLine("    notify.blink(\"green\", \"short\");");
                    newfile.WriteLine("    print(\"USB verbunden\");");
                    newfile.WriteLine();
                    newfile.WriteLine("    badusb.print(\"powershell\", 10);");
                    newfile.WriteLine("    badusb.press(\"ENTER\");");
                    newfile.WriteLine("    delay(1000);");

                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            newfile.WriteLine($"    badusb.altPrint(\"{line.Replace("\"", "\\\"")}\", 10);");
                            newfile.WriteLine("    badusb.press(\"ENTER\");");
                            newfile.WriteLine("    delay(1000);");
                        }
                    }

                    newfile.WriteLine("    notify.success();");
                    newfile.WriteLine("} else {");
                    newfile.WriteLine("    print(\"USB nicht verbunden\");");
                    newfile.WriteLine("    notify.error();");
                    newfile.WriteLine("}");
                    newfile.WriteLine();
                    newfile.WriteLine("// Optional, but allows to interchange with usbdisk");
                    newfile.WriteLine("badusb.quit();");
                }
            }
        }
        [STAThread]
        public void toBADUSB()
        {
            // Für jede Datei in der Listbox
            foreach (string filefromListbox in listBox1.Items)
            {
                string Datei = filefromListbox.Replace(".txt", "_flip.txt");

                // Neue Datei erstellen
                using (System.IO.StreamWriter newfile = new System.IO.StreamWriter(Datei))
                {
                    // Datei lesen
                    string[] lines = System.IO.File.ReadAllLines(filefromListbox);

                    // Initiale Zeilen schreiben
                    newfile.WriteLine("DELAY 2000");
                    newfile.WriteLine("GUI r");
                    newfile.WriteLine("DELAY 1000");
                    newfile.WriteLine("STRING powershell");
                    newfile.WriteLine("DELAY 500");
                    newfile.WriteLine("ENTER");
                    newfile.WriteLine("DELAY 2000");
                    newfile.WriteLine("STRING cls");
                    newfile.WriteLine("ENTER");

                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("//"))
                        {
                            newfile.WriteLine("STRING " + line);
                            newfile.WriteLine("ENTER");
                        }
                    }
                }
            }
        }
        [STAThread]
        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            // Get filepath of dropped item and add it to the listbox
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // For each file in files
            foreach (string file in files)
            {
                // Add file to listbox
                listBox1.Items.Add(file);
            }
        }
        [STAThread]
        private void toJavascriptCheck_CheckStateChanged(object sender, EventArgs e)
        {
            if (toJavascriptCheck.Checked)
            {
                toBadUSBCheck.Checked = false;
            }
        }
        [STAThread]
        private void toBadUSBCheck_CheckStateChanged(object sender, EventArgs e)
        {
            if (toBadUSBCheck.Checked)
            {
                toJavascriptCheck.Checked = false;
            }
        }
    }
}