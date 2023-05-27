using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace İnternet_Cafe_Otomasyonu
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        Button btn;
        private void SecileneGore(object sender, MouseEventArgs e)
        {
            btn = sender as Button;
            if (btn.BackColor == Color.Orange)
            {
                süreliMasaAçmaİsteğiGönderToolStripMenuItem.Visible = false;
                süresizMasaAçmaToolStripMenuItem.Visible = false;
            }
            if (btn.BackColor == Color.LightGreen)
            {
                süreliMasaAçmaİsteğiGönderToolStripMenuItem.Visible = true;
                süresizMasaAçmaToolStripMenuItem.Visible = true;
            }
        }

        RadioButton radio;
        private void RadioButtonSeciliyeGore(object sender, EventArgs e)
        {
            radio = sender as RadioButton;
        }
        public void yenile()
        {
            Veritabani.sepetlistele(dataGridView1);
            Veritabani.ListeviewdeKayitlariGoster(listView1);
            Veritabani.ComboyaBosMasaGetir(cmbBosMasalar);
            foreach (Control item in Controls)
            {
                if (item is Button)
                {
                    if (item.Name != btnMasaAc.Name)
                    {
                        Veritabani.baglanti.Open();
                        SqlCommand komut = new SqlCommand("select * from TBL_MASALAR", Veritabani.baglanti);
                        SqlDataReader read = komut.ExecuteReader();
                        while (read.Read())
                        {
                            if (read["Durumu"].ToString() == "BOŞ" && read["MASALAR"].ToString() == item.Text)
                            {
                                item.BackColor = Color.LightGreen;
                            }

                            if (read["Durumu"].ToString() == "DOLU" && read["MASALAR"].ToString() == item.Text)
                            {
                                item.BackColor = Color.OrangeRed;
                            }
                        }
                        Veritabani.baglanti.Close();
                    }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'internet_cafeDataSet.TBL_SaatUcreti' table. You can move, or remove it, as needed.
            this.tBL_SaatUcretiTableAdapter.Fill(this.internet_cafeDataSet.TBL_SaatUcreti);
            radiobtnsuresız.Checked = true;
            yenile();
            cmbBosMasalar.Text = "";
            timer1.Start();
        }

        private void btnMasaAc_Click(object sender, EventArgs e)
        {
            if (Kullanici.KullaniciID == 1)
            {
                string sqlsorgu = "insert into TBL_SEPET(MasaID,Masa,AcilisTuru,Baslangic,Tarih) values('" + cmbBosMasalar.Text.Substring(5) + "','" + cmbBosMasalar.Text + "','" + radio.Text + "',@Baslangic,'" + DateTime.Now.ToString() + "')";
                SqlCommand komut = new SqlCommand("");
                komut.Parameters.AddWithValue("@Baslangic", DateTime.Parse(DateTime.Now.ToString()));
                komut.Parameters.AddWithValue("@Tarih", DateTime.Parse(DateTime.Now.ToString()));
                Veritabani.ESG(komut, sqlsorgu);
                MessageBox.Show(cmbBosMasalar.Text.Substring(5) + "Nolu Masa Açildı ", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                yenile();
                radiobtnsuresız.Checked = true;

            }
            else
            {
                MessageBox.Show("Böyle Bir Yetkiniz Bulunmuyor ", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Hesapla"].Index)
            {
                if (cmbSaatUcreti.Text != "")
                {
                    DateTime BitisTarihi = DateTime.Now;
                    DateTime BaslangicTarihi = DateTime.Parse(dataGridView1.CurrentRow.Cells["BaslamaSaati"].Value.ToString());
                    TimeSpan saatfarki = BitisTarihi - BaslangicTarihi;
                    double saatfarki2 = saatfarki.TotalHours;
                    dataGridView1.CurrentRow.Cells["Sure"].Value = saatfarki2.ToString("0.00");
                    double toplamtutar = saatfarki2 * double.Parse(cmbSaatUcreti.Text);
                    dataGridView1.CurrentRow.Cells["Tutar"].Value = toplamtutar.ToString("0.00");
                    dataGridView1.CurrentRow.Cells["BitisSaati"].Value = BitisTarihi;
                }
                if (cmbSaatUcreti.Text == "")
                {
                    MessageBox.Show("Önce Saat Ücreti Belirtilmeli", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.ColumnIndex == dataGridView1.Columns["MasaKapat"].Index)
            {
                if (dataGridView1.CurrentRow.Cells["BitisSaati"].Value != null)
                {
                    FrmMasaKapat masakpt = new FrmMasaKapat();
                    masakpt.txtID.Text = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();
                    masakpt.txtMasaID.Text = dataGridView1.CurrentRow.Cells["MASA_ID"].Value.ToString();
                    masakpt.txtMasa.Text = dataGridView1.CurrentRow.Cells["_MASA"].Value.ToString();
                    masakpt.TxtAcilisTuru.Text = dataGridView1.CurrentRow.Cells["AcilisTuru"].Value.ToString();
                    masakpt.txtBaslamaSaati.Text = dataGridView1.CurrentRow.Cells["BaslamaSaati"].Value.ToString();
                    masakpt.txtBitisSaati.Text = dataGridView1.CurrentRow.Cells["BitisSaati"].Value.ToString();
                    masakpt.txtSaatUcreti.Text = cmbSaatUcreti.Text;
                    masakpt.txtSure.Text = dataGridView1.CurrentRow.Cells["Sure"].Value.ToString();
                    masakpt.txtTutar.Text = dataGridView1.CurrentRow.Cells["Tutar"].Value.ToString();
                    masakpt.txtTarih.Text = dataGridView1.CurrentRow.Cells["_Tarih"].Value.ToString();
                    masakpt.ShowDialog();
                }
                else if (dataGridView1.CurrentRow.Cells["BitisSaati"].Value == null)
                {
                    MessageBox.Show("Önce Hesaplama Yapılmalıdır", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }

        string istek = "";
        private void yöneticiÇağırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            istek = "Yöneticiyi Çağırıyor";
            Istekler();

        }

        private void Istekler()
        {
            string sqlsorgu = ("insert into TBL_HAREKETLER(KullaniciID,MasaID,Masa,İstekTuru,Aciklama,Tarih) values('" + Kullanici.KullaniciID + "','" + btn.Text.Substring(5) + "','" + btn.Text + "','" + istek + "','Yapılmadı',@Tarih)");
            SqlCommand komut = new SqlCommand();
            komut.Parameters.AddWithValue("@Tarih", DateTime.Parse(DateTime.Now.ToString()));
            Veritabani.ESG(komut, sqlsorgu);
            Veritabani.ListeviewdeKayitlariGoster(listView1);

        }

        private void süresizMasaAçmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            istek = "Süresiz Masa Açma İsteği Gönderdi";
            Istekler();
        }

        private void masaDeğiştirmeİsteğiGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            istek = "Masa Değiştirme İsteği Gönderdi";
            Istekler();
        }

        ToolStripMenuItem item;
        private void Surelilistesecilirse(object sender, EventArgs e)
        {
            item = new ToolStripMenuItem();
            istek = item.Text + "Dakika Süre ile Masa Açma İsteği Gönderdi";
            Istekler();
        }

        int sayac = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            sayac++;
            if (sayac > 29)
            {
                if (cmbSaatUcreti.Text != "")
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        DateTime BitisTarihi = DateTime.Now;
                        DateTime BaslangicTarihi = DateTime.Parse(dataGridView1.Rows[i].Cells["BaslamaSaati"].Value.ToString());
                        TimeSpan saatfarki = BitisTarihi - BaslangicTarihi;
                        double saatfarki2 = saatfarki.TotalHours;
                        dataGridView1.Rows[i].Cells["Sure"].Value = saatfarki2.ToString("0.00");
                        double toplamtutar = saatfarki2 * double.Parse(cmbSaatUcreti.Text);
                        dataGridView1.Rows[i].Cells["Tutar"].Value = toplamtutar.ToString("0.00");
                        dataGridView1.Rows[i].Cells["BitisSaati"].Value = BitisTarihi;
                    }

                }
                if (cmbSaatUcreti.Text == "")
                {
                    MessageBox.Show("Önce Saat Ücreti Belirtilmeli", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnMasaDegistir_Click(object sender, EventArgs e)
        {
            int SepetID = int.Parse(dataGridView1.CurrentRow.Cells["ID"].Value.ToString());
            int MasaID = int.Parse(dataGridView1.CurrentRow.Cells["Masa_ID"].Value.ToString());
            string sql = "update TBL_SEPET set MasaID='" + int.Parse(cmbBosMasalar.Text.Substring(5)) + "',Masa='" + cmbBosMasalar.Text + "' where SepetID='" + SepetID + "'";
            SqlCommand cmd = new SqlCommand();
            Veritabani.ESG(cmd, sql);

            string sql2 = "update TBL_MASALAR set durumu='BOS' where MasaID='" + MasaID + "'";
            SqlCommand cmd2 = new SqlCommand();
            Veritabani.ESG(cmd2, sql2);

            string sql3 = "update TBL_MASALAR set durumu='DOLU' where MasaID='" + int.Parse(cmbBosMasalar.Text.Substring(5)) + "'";
            SqlCommand cmd3 = new SqlCommand();
            Veritabani.ESG(cmd3, sql3);
            yenile();
            MessageBox.Show("Masa Değiştirme İşlemi Başarılı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["Sure"].Value != null)
                {
                    if (dataGridView1.Rows[i].Cells["AcilisTuru"].Value.ToString() != "Süresiz")
                    {
                        double sure = double.Parse(dataGridView1.Rows[i].Cells["Sure"].Value.ToString()) * 60;
                        double AcilisTuru = double.Parse(dataGridView1.Rows[i].Cells["AcilisTuru"].Value.ToString());
                        if (AcilisTuru - sure <= 5.0)
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        }
                    }
                }

            }
        }

        private void btnGeriAl_Click(object sender, EventArgs e)
        {
            frmSatislariListele frm = new frmSatislariListele();
            frm.btnGeriAl.Enabled = true;
            frm.ShowDialog();
        }

        private void btnSatisRaporu_Click(object sender, EventArgs e)
        {
            frmSatisRaporu frm = new frmSatisRaporu();
            frm.ShowDialog();
        }

        private void btnHareketRapor_Click(object sender, EventArgs e)
        {
            frmHareket frmHareket = new frmHareket();
            frmHareket.ShowDialog();
        }
    }
}
