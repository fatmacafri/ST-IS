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
    public partial class frm_tanimlar_satis_malzemelistesi : Form//MALZEME LİSTESİ FORMU
    {
        public frm_tanimlar_satis_malzemelistesi()
        {
            InitializeComponent();
        }
        public static DataRow MODALRESULT = null;
        public static int MIKTAR = 1;

        public static DataRow ac()
        {
            frm_tanimlar_satis_malzemelistesi f = new frm_tanimlar_satis_malzemelistesi();
            f.ShowDialog();
            f.Dispose();
            return MODALRESULT;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)//ÜRÜN ARAMASI YAPMA KISMI
        {
            
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select PRODUCTID,NAME,PRICE,STOK from stok where PRODUCTID like'" + textBox2.Text+"%' or NAME like'"+textBox2.Text+"%'", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglanti.Close();
            
        }
   
        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)//DATAGRİDE ÇİFT TIKLAYINCA O ÜRÜNDEN SEÇER
        {
           int index = dataGridView1.SelectedCells[0].RowIndex;
           textBox1.Text =dataGridView1.Rows[0].Cells[0].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)//KAYDET BUTONUNA BASINCA TIKLADIĞIMIZ ÜRÜNDEN EKLERİZ
        {
            MIKTAR = Convert.ToInt32(textBox1.Text);
            MODALRESULT = (dataGridView1.DataSource as DataTable).Rows[dataGridView1.CurrentRow.Index];
            this.Close();

            frm_kullanici_giris frm1 = new frm_kullanici_giris();
           
            Close();

        }

        SqlConnection baglanti = new SqlConnection("Data Source=NOTEBOOK\\SQLEXPRESS;Initial Catalog=KullanıcıGiris;Integrated Security=True");

        private void Form3_Load_1(object sender, EventArgs e)//FORM YÜKLENİRKEN GÖZÜKECEK KISIMLAR.
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select PRODUCTID,NAME,PRICE,STOK from stok", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            baglanti.Close(); 
        }

        private void button2_Click(object sender, EventArgs e)//KAPAT BUTONU
        {
            Dispose();
        }
    }
    }

