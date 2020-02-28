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

namespace KullaniciGirisi
{
    public partial class frm_tanimlar_musteri_musterikayit : Form//MÜŞTERİ KAYIT FORMU
    {
        public frm_tanimlar_musteri_musterikayit()
        {
            InitializeComponent();
        }
        public static SqlConnection baglanti = new SqlConnection("Data Source=NOTEBOOK\\SQLEXPRESS;Initial Catalog=KullanıcıGiris;Integrated Security=True");

        public static int item;
        public static int ac(int id)
        {
            frm_tanimlar_musteri_musterikayit f = new frm_tanimlar_musteri_musterikayit();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from musteri where MUSTERIID=@ID", baglanti);
            komut.Parameters.AddWithValue("@ID", id);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            item = id;
            if (id != 0)//VAROLAN MÜŞTERİYİ AÇMAK İÇİN 
            {
                f.textBox1.Text = dt.Rows[0]["MUSTERIID"].ToString();
                f.textBox2.Text = dt.Rows[0]["MUSTERIAD"].ToString();
                f.textBox3.Text = dt.Rows[0]["MUSTERISOYAD"].ToString();
                f.textBox4.Text = dt.Rows[0]["MUSTERIADRES"].ToString();
                f.textBox5.Text = dt.Rows[0]["MUSTERIMAIL"].ToString();
            }
            else
            {
                MessageBox.Show("YENİ MÜŞTERİ EKLEYİNİZ");
            }

            baglanti.Close();
            da.Dispose();
            dt.Dispose();
            f.ShowDialog();
            f.Dispose();
            return id;
        }

        private void frm_tanimlar_musteri_müsterikayit_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            if (item != 0)//MÜŞTERİ GÜNCELLEMEK İÇİN
            {
                SqlCommand komut = new SqlCommand("UPDATE musteri SET MUSTERIAD=@2,MUSTERISOYAD=@3,MUSTERIADRES=@4,MUSTERIMAIL=@musterimail WHERE MUSTERIID=@1", baglanti);
                komut.Parameters.AddWithValue("@1", textBox1.Text);
                komut.Parameters.AddWithValue("@2", textBox2.Text);
                komut.Parameters.AddWithValue("@3", textBox3.Text);
                komut.Parameters.AddWithValue("@4", textBox4.Text);
                komut.Parameters.AddWithValue("@musterimail", textBox5.Text);
                komut.ExecuteNonQuery();
            }
            else//MÜŞTERİ EKLEMEK İÇİN
            {
                SqlCommand komut = new SqlCommand("INSERT INTO musteri(MUSTERIAD,MUSTERISOYAD,MUSTERIADRES,MUSTERIMAIL) VALUES(@2,@3,@4,@musterimail) ", baglanti);
               // komut.Parameters.AddWithValue("@1", textBox1.Text);
                komut.Parameters.AddWithValue("@2", textBox2.Text);
                komut.Parameters.AddWithValue("@3", textBox3.Text);
                komut.Parameters.AddWithValue("@4", textBox4.Text);
                komut.Parameters.AddWithValue("@musterimail", textBox5.Text);
                komut.ExecuteNonQuery();
            }

            baglanti.Close();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)//KAPAT BUTONU
        {
            Dispose();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
