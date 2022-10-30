using System;
using MyClassLib;
using System.Text;
using System.Windows.Forms;
using EncryptingClasses;
using ClassLibs.StreamGenerators;

namespace Encode
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
                    System.IO.File.WriteAllText(saveFileDialog.FileName, encrypter.Encrypt(inputField.Text), Encoding.Unicode);
        }

        private void decodeButton_Click(object sender, EventArgs e)
        {

            if (successfulEncrypterGenerated(keyInput.Text, numericUpDown1.Value))
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    System.IO.File.WriteAllText(saveFileDialog.FileName, encrypter.Decrypt(inputField.Text), Encoding.Unicode);
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

        private void EnableParts(bool inp, bool num)
        {
            keyInput.Enabled = inp;
            numericUpDown1.Enabled = num;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                encrypter = new CeasarEncrypter();
                EnableParts(true, true);
            }
            if (comboBox1.SelectedIndex == 1)
            {
                encrypter = new VigenereEncrypter();
                EnableParts(true, false);
            }
            if (comboBox1.SelectedIndex == 2)
            {
                encrypter = new NOEKEON();
                EnableParts(true, false);
            }
            if (comboBox1.SelectedIndex == 3)
            {
                encrypter = new StreamBasedEncrypter(new BBS());
                EnableParts(true, false);
            }
            if (comboBox1.SelectedIndex == 4)
            {
                encrypter = new StreamBasedEncrypter(new LFSR());
                EnableParts(false, false);
            }
        }

        private void bBSKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string key;
            try
            {
                key = BBS.GenerateKey(keyInput.Text);
            }
            catch (WrongKeyValue err)
            {
                MessageBox.Show(err.Message);
                return;
            }
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                System.IO.File.WriteAllText(saveFileDialog.FileName, key, Encoding.Unicode);
            keyInput.Text = key;
        }
    }
}
