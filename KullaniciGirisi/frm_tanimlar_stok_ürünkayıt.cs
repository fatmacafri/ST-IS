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
    public partial class frm_tanimlar_stok_ürünkayıt : Form//STOK KAYIT FORMU
    {
        public static SqlConnection baglanti = new SqlConnection("Data Source=NOTEBOOK\\SQLEXPRESS;Initial Catalog=KullanıcıGiris;Integrated Security=True");

        public frm_tanimlar_stok_ürünkayıt()
        {
            InitializeComponent();
        }
        public static int item;
        public static int AC(int id)//YENİ ÜRÜN MÜ YOKSA VAROLAN ÜRÜNÜ MÜ AÇIYORUZ BURDA KONTROL EDERİZ
        {
            frm_tanimlar_stok_ürünkayıt f6 = new frm_tanimlar_stok_ürünkayıt();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select PRODUCTID,NAME,PRICE,STOK from stok where PRODUCTID=@ID", baglanti);
            komut.Parameters.AddWithValue("@ID", id);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable tablo = new DataTable();           
            da.Fill(tablo);          
            item = id;
            if (id!=0)//VAROLAN ÜRÜNÜ AÇARKEN ÇALIŞIR
            {
                f6.textBox2.Text = tablo.Rows[0]["PRODUCTID"].ToString();
                f6.textBox3.Text = tablo.Rows[0]["NAME"].ToString();
                f6.textBox4.Text = tablo.Rows[0]["PRICE"].ToString();
                f6.textBox1.Text = tablo.Rows[0]["STOK"].ToString();
            }
            else
            {
                MessageBox.Show("YENİ ÜRÜN EKLEYİNİZ");
            }
          
            baglanti.Close();
            da.Dispose();
            tablo.Dispose();
            f6.ShowDialog();
            f6.Dispose();
            return id;
        }
    
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            if (item!=0)//SQLDEKİ ÜRÜNÜ GÜNCELLER
            {
                SqlCommand komut = new SqlCommand("UPDATE stok SET PRODUCTID=@ID,NAME=@NAME,PRICE=@PRICE,STOK=@STOK where PRODUCTID=@ID", baglanti);
                komut.Parameters.AddWithValue("@ID", textBox2.Text);
                komut.Parameters.AddWithValue("@NAME", textBox3.Text);
                komut.Parameters.AddWithValue("@PRICE", textBox4.Text);
                komut.Parameters.AddWithValue("@STOK", textBox1.Text);
                komut.ExecuteNonQuery();
            }
            else//SQLE YENİ ÜRÜN EKLER
            {
                SqlCommand komut = new SqlCommand("insert into stok(PRODUCTID,NAME,PRICE,STOK) values(@ID,@NAME,@PRICE,@STOK) ", baglanti);
                komut.Parameters.AddWithValue("@ID", textBox2.Text);
                komut.Parameters.AddWithValue("@NAME", textBox3.Text);
                komut.Parameters.AddWithValue("@PRICE", textBox4.Text);
                komut.Parameters.AddWithValue("@STOK", textBox1.Text);
                komut.ExecuteNonQuery();
            }

            baglanti.Close();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)//KAPAT BUTONU
        {
            this.Close();

        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

