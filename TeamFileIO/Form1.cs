using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeamFileIO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open Text File";
            openFileDialog.Filter = "TXT files|*.txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader fileIn = new StreamReader(openFileDialog.FileName))
                {
                    txtFilename.Text = openFileDialog.FileName;
                    string fileContents = fileIn.ReadToEnd();
                    txtFileContents.Text = fileContents;
                }
            }
            else
            {
                MessageBox.Show("File could not be opened.");
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            //TODO Insert encrypt code

            //TODO Update txtFileContents.Text to show the encrypted data
            txtEncryptedFileContents.Text = "This is encrypted text";

            //Save file as "encrypted_filename.txt"
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = "Save Text File";
            savefile.FileName = "encrypted_file.txt";
            savefile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(savefile.FileName))
                    sw.Write(txtEncryptedFileContents.Text);
            }
            else
            {
                MessageBox.Show("File could not be saved.");
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            //TO Insert decrypt code

            //TODO Update txtFileContents.Text to show the decrypted data
            txtDecryptedFileContents.Text = "This is decrypted text";

            //Save file as "decrypted_filename.txt"
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = "Save Text File";
            savefile.FileName = "decrypted_file.txt";
            savefile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(savefile.FileName))
                    sw.Write(txtDecryptedFileContents.Text);
            }
            else
            {
                MessageBox.Show("File could not be saved.");
            }
        }
    }
}
