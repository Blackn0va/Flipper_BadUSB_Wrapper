using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Powershell_Wrapper
{
    public partial class frmHauptprogramm : Form
    {
        public frmHauptprogramm()
        {
            InitializeComponent();
            toJavascriptCheck.Checked = true; // Standardmodus
            UpdateStatus("Bereit – Dateien hierher ziehen oder per Explorer auswählen.");
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void cmdConvert_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("Bitte zuerst mindestens eine Quelldatei hinzufügen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!toJavascriptCheck.Checked && !toBadUSBCheck.Checked)
            {
                MessageBox.Show("Bitte mindestens ein Zielformat auswählen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            cmdConvert.Enabled = false;
            try
            {
                int jsOk = 0, badOk = 0, jsErr = 0, badErr = 0;
                foreach (string file in listBox1.Items)
                {
                    if (!File.Exists(file))
                    {
                        jsErr += toJavascriptCheck.Checked ? 1 : 0;
                        badErr += toBadUSBCheck.Checked ? 1 : 0;
                        continue;
                    }

                    try
                    {
                        if (toJavascriptCheck.Checked)
                        {
                            if (ConvertToJavascript(file)) jsOk++; else jsErr++;
                        }
                        if (toBadUSBCheck.Checked)
                        {
                            if (ConvertToBadUsb(file)) badOk++; else badErr++;
                        }
                    }
                    catch (Exception exPerFile)
                    {
                        // Einzeldateifehler zählen
                        if (toJavascriptCheck.Checked) jsErr++;
                        if (toBadUSBCheck.Checked) badErr++;
                        AppendStatus($"Fehler bei '{Path.GetFileName(file)}': {exPerFile.Message}");
                    }
                }

                var sb = new StringBuilder();
                if (toJavascriptCheck.Checked)
                    sb.AppendLine($"Javascript: {jsOk} erfolgreich, {jsErr} Fehler.");
                if (toBadUSBCheck.Checked)
                    sb.AppendLine($"BadUSB: {badOk} erfolgreich, {badErr} Fehler.");
                UpdateStatus(sb.ToString().TrimEnd());
                MessageBox.Show(sb.ToString(), "Fertig", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Allgemeiner Fehler: " + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Fehler – Details im Dialog.");
            }
            finally
            {
                cmdConvert.Enabled = true;
            }
        }

        private void AddFiles(IEnumerable<string> files)
        {
            int added = 0, skipped = 0;
            foreach (var f in files)
            {
                try
                {
                    if (Directory.Exists(f))
                    {
                        // Rekursiv alle txt- und ps1-Dateien hinzufügen
                        var txts = Directory.GetFiles(f, "*.txt", SearchOption.AllDirectories);
                        var ps1s = Directory.GetFiles(f, "*.ps1", SearchOption.AllDirectories);
                        AddFiles(txts.Concat(ps1s));
                        continue;
                    }
                    if (!File.Exists(f)) { skipped++; continue; }
                    string ext = Path.GetExtension(f).ToLowerInvariant();
                    if (ext != ".txt" && ext != ".ps1") { skipped++; continue; }
                    if (!listBox1.Items.Contains(f))
                    {
                        listBox1.Items.Add(f);
                        added++;
                    }
                    else skipped++;
                }
                catch
                {
                    skipped++;
                }
            }
            UpdateStatus($"Hinzugefügt: {added}, übersprungen: {skipped}.");
        }

        private bool ConvertToJavascript(string sourceFile)
        {
            try
            {
                string targetFile = Path.ChangeExtension(sourceFile, ".js");
                var lines = File.ReadAllLines(sourceFile);
                using (var newfile = new StreamWriter(targetFile, false, Encoding.UTF8))
                {
                    WriteJavascriptHeader(newfile);
                    foreach (var raw in lines)
                    {
                        var line = raw?.TrimEnd();
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string sanitized = SanitizeForJs(line);
                        newfile.WriteLine($"    badusb.altPrint(\"{sanitized}\", 10);");
                        newfile.WriteLine("    badusb.press(\"ENTER\");");
                        newfile.WriteLine("    delay(500);");
                    }
                    WriteJavascriptFooter(newfile);
                }
                return true;
            }
            catch (Exception ex)
            {
                AppendStatus($"[JS] {Path.GetFileName(sourceFile)}: {ex.Message}");
                return false;
            }
        }

        private bool ConvertToBadUsb(string sourceFile)
        {
            try
            {
                string directory = Path.GetDirectoryName(sourceFile) ?? string.Empty;
                string filenameNoExt = Path.GetFileNameWithoutExtension(sourceFile);
                string targetFile = Path.Combine(directory, filenameNoExt + "_flip.txt");
                using (var newfile = new StreamWriter(targetFile, false, Encoding.UTF8))
                {
                    WriteBadUsbHeader(newfile);
                    foreach (var raw in File.ReadLines(sourceFile))
                    {
                        var line = raw?.Trim();
                        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//")) continue;
                        newfile.WriteLine("STRING " + line);
                        newfile.WriteLine("ENTER");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                AppendStatus($"[BadUSB] {Path.GetFileName(sourceFile)}: {ex.Message}");
                return false;
            }
        }

        private static void WriteJavascriptHeader(StreamWriter sw)
        {
            sw.WriteLine("let badusb = require(\"badusb\");");
            sw.WriteLine("let notify = require(\"notification\");");
            sw.WriteLine("let dialog = require(\"dialog\");");
            sw.WriteLine("let serial = require(\"serial\");");
            sw.WriteLine();
            sw.WriteLine("badusb.setup({");
            sw.WriteLine("    vid: 0xAAAA,");
            sw.WriteLine("    pid: 0xBBBB,");
            sw.WriteLine("    mfr_name: \"Flipper\",");
            sw.WriteLine("    prod_name: \"Zero\",");
            sw.WriteLine("    layout_path: \"/ext/badusb/assets/layouts/de-DE.kl\"");
            sw.WriteLine("});");
            sw.WriteLine("dialog.message(\"Intune Onboarding\", \"Press OK zum starten\");");
            sw.WriteLine();
            sw.WriteLine("if (badusb.isConnected()) {");
            sw.WriteLine("    notify.blink(\"green\", \"short\");");
            sw.WriteLine("    print(\"USB verbunden\");");
            sw.WriteLine();
            sw.WriteLine("    badusb.print(\"powershell\", 10);");
            sw.WriteLine("    badusb.press(\"ENTER\");");
            sw.WriteLine("    delay(1000);");
        }

        private static void WriteJavascriptFooter(StreamWriter sw)
        {
            sw.WriteLine("    notify.success();");
            sw.WriteLine("} else {");
            sw.WriteLine("    print(\"USB nicht verbunden\");");
            sw.WriteLine("    notify.error();");
            sw.WriteLine("}");
            sw.WriteLine();
            sw.WriteLine("badusb.quit();");
        }

        private static void WriteBadUsbHeader(StreamWriter sw)
        {
            sw.WriteLine("DELAY 2000");
            sw.WriteLine("GUI r");
            sw.WriteLine("DELAY 800");
            sw.WriteLine("STRING powershell");
            sw.WriteLine("DELAY 400");
            sw.WriteLine("ENTER");
            sw.WriteLine("DELAY 1500");
            sw.WriteLine("STRING cls");
            sw.WriteLine("ENTER");
        }

        private static string SanitizeForJs(string line)
        {
            // Escape Sequenzen für JS String in doppelten Anführungszeichen
            var sb = new StringBuilder(line.Length * 2);
            foreach (char c in line)
            {
                switch (c)
                {
                    case '"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    default:
                        if (c < 32)
                        {
                            sb.Append("\\x").Append(((int)c).ToString("X2"));
                        }
                        else sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                AddFiles(files);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Hinzufügen: " + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toJavascriptCheck_CheckStateChanged(object sender, EventArgs e)
        {
            if (toJavascriptCheck.Checked)
            {
                // Exklusiv, aber beide abwählbar möglich – daher nur deaktivieren falls gewählt
                if (toBadUSBCheck.Checked) toBadUSBCheck.Checked = false;
            }
        }

        private void toBadUSBCheck_CheckStateChanged(object sender, EventArgs e)
        {
            if (toBadUSBCheck.Checked)
            {
                if (toJavascriptCheck.Checked) toJavascriptCheck.Checked = false;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            UpdateStatus("Liste geleert.");
        }

        private void UpdateStatus(string text)
        {
            if (lblStatus != null)
                lblStatus.Text = text;
        }

        private void AppendStatus(string line)
        {
            if (lblStatus == null) return;
            if (string.IsNullOrEmpty(lblStatus.Text))
                lblStatus.Text = line;
            else
                lblStatus.Text = lblStatus.Text + " | " + line;
        }
    }
}