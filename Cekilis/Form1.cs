using Cekilis.Properties;
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

namespace Cekilis
{
    public partial class Form1 : Form
    {
        List<string> adlar;
        Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
            Icon = Resources.cekilis;
            VerileriOku();
            Listele();
        }
        private void VerileriOku()
        {
            try
            {
                adlar = File.ReadAllLines("adlar.txt").ToList();
            }
            catch (Exception)
            {
                adlar = new List<string>();
            }
        }
        void Karistir(List<string> liste)
        {
            int talihliIndex;
            string gecici;
            for (int i = 0; i < liste.Count; i++)
            {
                talihliIndex = rnd.Next(i, liste.Count);
                gecici = liste[i];
                liste[i] = liste[talihliIndex];
                liste[talihliIndex] = gecici;
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            int sid = lstAdlar.SelectedIndex;
            // Hata kontrolü
            string ad = txtAd.Text.Trim();
            if (ad == "")
            {
                MessageBox.Show("Eklemek için bir isim girmelisiniz.");
                return;
            }
            // Verinin eklenme
            adlar.Add(ad);

            // formun temizlenmesi
            txtAd.Clear();
            txtAd.Focus();
            // Listbox güncelleme
            Listele();
            lstAdlar.SelectedIndex = sid;
        }

        private void Listele()
        {
            lstAdlar.Items.Clear();
            foreach (string item in adlar)
                lstAdlar.Items.Add(item);
        }

        private void txtAd_KeyDown(object sender, KeyEventArgs e)
        {
            // Text Box'a veri girdikten sonra Enter'a basınca btnEkle click oluyor.
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // tuşa basılmamış gibi algıla demektir.
                btnEkle.PerformClick();
            }
        }

        private void btnCekilis_Click(object sender, EventArgs e)
        {
            if (adlar.Count == 0) return;
            Karistir(adlar);
            lblSonuc.Text = adlar[0];
            if (chkKaldir.Checked)
                adlar.RemoveAt(0);
            Listele();
        }

        private void lstAdlar_KeyDown(object sender, KeyEventArgs e)
        {
            // Hiçbirşey seçili değilse, selected index = -1;
            int sid = lstAdlar.SelectedIndex;
            if (e.KeyCode == Keys.Delete && sid > -1)
            {
                adlar.RemoveAt(sid);
                Listele();
                lstAdlar.SelectedIndex = sid < lstAdlar.Items.Count ? sid : lstAdlar.Items.Count - 1;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Program Kapatılacaktır. Değişiklikleri kaydetmek istiyor musunuz ?", "Çekiliş", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
            switch (dr)
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                case DialogResult.Yes:
                    File.WriteAllLines("adlar.txt", adlar);
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void btnYukari_Click(object sender, EventArgs e)
        {
            string temp;
            int tasinacakKisi = lstAdlar.SelectedIndex;
            if (lstAdlar.SelectedIndex != 0)
            {
                temp = adlar[tasinacakKisi];
                adlar[tasinacakKisi] = adlar[tasinacakKisi - 1];
                adlar[tasinacakKisi - 1] = temp;
                Listele();
                lstAdlar.SelectedIndex = tasinacakKisi - 1;
            }
            else return;
        }

        private void btnAsagi_Click(object sender, EventArgs e)
        {
            string temp;
            int tasinacakKisi = lstAdlar.SelectedIndex;

            if (lstAdlar.SelectedIndex != adlar.Count - 1)
            {
                temp = adlar[tasinacakKisi];
                adlar[tasinacakKisi] = adlar[tasinacakKisi + 1];
                adlar[tasinacakKisi + 1] = temp;
                Listele();
                lstAdlar.SelectedIndex = tasinacakKisi + 1;
            }
            else return;
        }

        private void lstAdlar_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnYukari.Enabled = lstAdlar.SelectedIndex > 0;
            btnAsagi.Enabled = lstAdlar.SelectedIndex != lstAdlar.Items.Count - 1;
        }
    }
}
