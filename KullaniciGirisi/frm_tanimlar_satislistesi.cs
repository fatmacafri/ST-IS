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
    public partial class frm_tanimlar_satislistesi : Form//SATIŞ LİSTESİ FORMU
    {
        public frm_tanimlar_satislistesi()
        {
            InitializeComponent();
        }

        public static SqlConnection baglanti = new SqlConnection("Data Source=NOTEBOOK\\SQLEXPRESS;Initial Catalog=KullanıcıGiris;Integrated Security=True");
        DataTable dt = new DataTable();
        public void verılerıgoster()//SQLDEN VERİLERİ ÇEKMEK İÇİN
        {
            dt.Clear();
            SqlDataAdapter da = new SqlDataAdapter("select satis.TARIH[Tarih],convert(varchar,satis.SAAT,24) as Saat,satis.EVRAKNO[Evrak NO],musteri.MUSTERIAD[Müşteri Adı],musteri.MUSTERISOYAD[Müşteri Soyadı],depotanim.ISIM[Depo Adı],depotanim.KOD[Depo NO],depotanim.ADRES[Depo Adresi],satis.SATISID[Satış ID] from satis inner join depotanim on depotanim.DEPOID=satis.DEPOID inner join musteri on satis.MUSTERIID=musteri.MUSTERIID where TARIH between @tr1 and @tr2", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@tr1",dateTimePicker1.Value);
            da.SelectCommand.Parameters.AddWithValue("@tr2", dateTimePicker2.Value);  
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            baglanti.Close();
        }
   
        private void SatisListesi_Load(object sender, EventArgs e)//FORM AÇILDIĞINDA GÖZÜKECEK KISIMLAR
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("select satis.TARIH[Tarih],convert(varchar,satis.SAAT,24) as Saat,satis.EVRAKNO[Evrak NO],musteri.MUSTERIAD[Müşteri Adı],musteri.MUSTERISOYAD[Müşteri Soyadı],depotanim.ISIM[Depo Adı],depotanim.KOD[Depo NO],depotanim.ADRES[Depo Adresi],satis.SATISID[Satış ID] from satis inner join depotanim on depotanim.DEPOID=satis.DEPOID inner join musteri on satis.MUSTERIID=musteri.MUSTERIID ", baglanti); 
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)//SORGULA BUTONU
        {
           
            verılerıgoster(); 
        }
       
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)//ÜSTTEKİ DATAGRİDE ÇİFT TIKLAYINCA O KİŞİYLE ALAKALI DETAYLI BİLGİLER AŞAĞIDAKİ DATAGRİDDE GÖZÜKÜR.
        {

            int satir = Convert.ToInt32(dataGridView1.CurrentCell.RowIndex);
            int id = Convert.ToInt32(dataGridView1.Rows[satir].Cells["Satış ID"].Value.ToString());
            try { 
            baglanti.Open();
                
            SqlCommand komut = new SqlCommand("select stok.PRODUCTID[Ürün ID],stok.NAME[Ürün Adı],stok.PRICE[Ürün Fiyatı],satisdetay.MIKTAR[Alınan Miktar],satisdetay.FIYAT[Toplam Fiyat] from satisdetay inner join stok on stok.STOKID=satisdetay.STOKID WHERE satisdetay.SATISID=@SATISID", baglanti);
            komut.Parameters.AddWithValue("@SATISID", id);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            dt.Clear();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView2.ReadOnly = true;
            baglanti.Close();
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)//KAPAT BUTONU
        {
          
            this.Dispose();
        }
    }
}
