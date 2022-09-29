using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EncryptingClasses;

namespace Ceasar
{
    public partial class EncodeDecodeForm : Form
    {
        public EncodeDecodeForm()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                    inputField.Text = System.IO.File.ReadAllText(openFileDialog.FileName);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputField.Text = "";
            keyInput.Text = "";
            numericUpDown1.Value = 0;
        }

        private void encodeButton_Click(object sender, EventArgs e)
        {
            if (successfulEncrypterGenerated(keyInput.Text, numericUpDown1.Value))
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    System.IO.File.WriteAllText(saveFileDialog.FileName, encrypter.Encrypt(inputField.Text.ToLower()));
        }

        private void decodeButton_Click(object sender, EventArgs e)
        {

            if (successfulEncrypterGenerated(keyInput.Text, numericUpDown1.Value))
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    System.IO.File.WriteAllText(saveFileDialog.FileName, encrypter.Decrypt(inputField.Text.ToLower()));
        }

        private bool successfulEncrypterGenerated(string key, decimal val)
        {
            try
            {
                encrypter.SetKey(key.ToLower(), (int)val);
                return true;
            }
            catch (WrongKeyValue err)
            {
                MessageBox.Show(err.Message);
                return false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                encrypter = new CeasarEncrypter();
                numericUpDown1.Enabled = true;
            }
            if (comboBox1.SelectedIndex == 1)
            {
                encrypter = new VigenereEncrypter();
                numericUpDown1.Enabled = false;
            }
            }
        }
}
