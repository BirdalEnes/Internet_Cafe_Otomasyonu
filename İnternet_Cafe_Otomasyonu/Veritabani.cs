using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace İnternet_Cafe_Otomasyonu
{
    internal class Veritabani
    {
        public static SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=internet_cafe;Integrated Security=True");
        public static DataTable sepetlistele(DataGridView gridview)
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from TBL_SEPET", baglanti);
            DataTable tbl = new DataTable();
            adtr.Fill(tbl);
            gridview.DataSource = tbl;
            return tbl;
        }
        public static DataTable listele(DataGridView gridview, string sorgu)
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter(sorgu, baglanti);
            DataTable tbl = new DataTable();
            adtr.Fill(tbl);
            gridview.DataSource = tbl;
            return tbl;
        }
        public static DataTable ComboyaBosMasaGetir(ComboBox combo)
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from TBL_MASALAR where Durumu='BOŞ'", baglanti);
            DataTable tbl = new DataTable();
            adtr.Fill(tbl);
            combo.DataSource = tbl;
            combo.DisplayMember = "MASALAR";
            combo.ValueMember = "MasaID";
            baglanti.Close();
            return tbl;
        }
        public static void ESG(SqlCommand command, string sorgu)
        {
            baglanti.Close();
            command.Connection = baglanti;
            command.CommandText = sorgu;
            command.ExecuteNonQuery();
            baglanti.Close();
        }
        public static SqlDataReader ListeviewdeKayitlariGoster(ListView list)
        {
            list.Items.Clear();
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("select * from TBL_HAREKETLER where tarih>=@Tarih ", baglanti);
            cmd.Parameters.AddWithValue("@Tarih", DateTime.Parse(DateTime.Now.ToShortDateString()));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem ekle = new ListViewItem();
                ekle.Text = dr[0].ToString();
                ekle.SubItems.Add(dr[1].ToString());
                ekle.SubItems.Add(dr[2].ToString());
                ekle.SubItems.Add(dr[3].ToString());
                ekle.SubItems.Add(dr[4].ToString());
                ekle.SubItems.Add(dr[5].ToString());
                ekle.SubItems.Add(dr[6].ToString());
                list.Items.Add(ekle);
            }
            baglanti.Close();
            return dr;
        }

    }
}
