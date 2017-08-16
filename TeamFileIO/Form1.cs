﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeamFileIO
{
    public partial class Form1 : Form
    {
        string fileContents = string.Empty;
        string fileName = string.Empty;

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
                    fileName = txtFilename.Text;
                    fileContents = fileIn.ReadToEnd();
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
            //TODO Ensure fileContents is not null or empty
            Aes newAes = Aes.Create();
            byte[] cipherText = EncryptText(fileContents, newAes.Key, newAes.IV);

            //TODO Update txtFileContents.Text to show the encrypted data
            txtEncryptedFileContents.Text = BitConverter.ToString(cipherText).Replace("-", string.Empty).ToLower();

            //Save file as "encrypted_filename.txt"
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = "Save Text File";
            savefile.FileName = fileName.Substring(0, fileName.Length - 4) + "ENCRYPTED";
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

        private byte[] EncryptText(string fileContents, byte[] key, byte[] iV)
        {
            if(fileContents == null || fileContents.Length < 1)
                throw new ArgumentNullException("fileContents");
            if (key == null || key.Length < 1)
                throw new ArgumentNullException("key");
            if (iV == null || iV.Length < 1)
                throw new ArgumentNullException("iv");

            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(fileContents);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}