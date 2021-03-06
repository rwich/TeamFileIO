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
        byte[] cipherText;
        Aes newAes = Aes.Create();
        byte[] key = new byte[] { 0x12, 0xB7, 0xCF, 0x4D, 0x06, 0x87, 0x04, 0x41, 0x03, 0xC4, 0xAE, 0x1E, 0x51, 0x4E, 0x92, 0x18, 0xDE, 0x48, 0x63, 0x1F, 0x8A, 0xD0, 0x33, 0xAD, 0x90, 0xDF, 0xB1, 0xC3, 0x2C, 0x23, 0xF0, 0x6C };
        byte[] vector = new byte[] { 0x12, 0xB7, 0xCF, 0x4D, 0x06, 0x87, 0x04, 0x41, 0x03, 0xC4, 0xAE, 0x1E, 0x51, 0x4E, 0x92, 0x18 };

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
                }
            }
            else
            {
                MessageBox.Show("File could not be opened.");
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            cipherText = EncryptText(fileContents, key, vector);

            string cipherTextAsString = Convert.ToBase64String(cipherText);

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = "Save Text File";
            savefile.FileName = fileName.Substring(0, fileName.Length - 4) + "ENCRYPTED";
            savefile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(savefile.FileName))
                    sw.Write(cipherTextAsString);
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
                        
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
            return encrypted;
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            cipherText = Convert.FromBase64String(fileContents);
            string decrypted = DecryptText(cipherText, key, vector);

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = "Save Text File";
            savefile.FileName = fileName.Substring(0, fileName.Length - 4) + "DECRYPTED";
            savefile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(savefile.FileName))
                    sw.Write(decrypted);
            }
            else
            {
                MessageBox.Show("File could not be saved.");
            }
        }

        private string DecryptText(byte[] encrypted, byte[] key, byte[] iV)
        {
            if (encrypted == null || encrypted.Length < 1)
                throw new ArgumentNullException("encrypted");
            if (key == null || key.Length < 1)
                throw new ArgumentNullException("key");
            if (iV == null || iV.Length < 1)
                throw new ArgumentNullException("iV");

            string plainText = string.Empty;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encrypted))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plainText;
        }
    }
}