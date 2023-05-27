using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace İnternet_Cafe_Otomasyonu
{
    internal class Kullanici
    {
        public static int KullaniciID = 0;
        public static bool durum = false;
        public static SqlDataReader KullaniciGirisi(TextBox KullaniciAdi, TextBox Sifre)
        {
            Veritabani.baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from TBL_KULLANİCİ where KullaniciAdi = '" + KullaniciAdi.Text + "' and Sifre='" + Sifre.Text + "'", Veritabani.baglanti);
            SqlDataReader read = komut.ExecuteReader();
            if (read.Read())
            {
                durum = true;
                KullaniciID = int.Parse(read["KullaniciID"].ToString());
            }
            else
            {
                durum = false;
            }
            Veritabani.baglanti.Close();
            return read;
        }
    }
}
