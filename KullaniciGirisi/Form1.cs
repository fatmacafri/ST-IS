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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=NOTEBOOK\\SQLEXPRESS;Initial Catalog=KullanıcıGiris;Integrated Security=True");//SQL'E BAĞLANMAK İÇİN

        public void verilerigöster()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("select satis.TARIH[Tarih],convert(varchar,satis.SAAT,24) as Saat,satis.EVRAKNO[Evrak NO],stok.NAME[Ürün Adı],stok.PRICE[Birim Fiyat],satisdetay.MIKTAR[Miktar],satisdetay.FIYAT[Toplam Fiyat],depotanim.ISIM[Depo Adı] from satisdetay inner join stok on stok.STOKID=satisdetay.STOKID inner join satis on satisdetay.SATISID=satis.SATISID inner join depotanim on depotanim.DEPOID=satis.DEPOID inner join musteri on MUSTERIAD='"+ textBox1.Text +"'", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            baglanti.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = frm_kullanici_giris.name;
            verilerigöster();
        }
    }
}
