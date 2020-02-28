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
    public partial class frm_tanimlar_stok : Form//STOK FORMU
    {
        public frm_tanimlar_stok()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=NOTEBOOK\\SQLEXPRESS;Initial Catalog=KullanıcıGiris;Integrated Security=True");
       
        public void verılerıgoster()//SQLDEN VERİLERİ ÇEKMEK İÇİN
        {
            baglanti.Open();
            SqlCommand komut= new SqlCommand("select PRODUCTID[Ürün ID],NAME[Ürün Adı],PRICE[Ürün Fiyatı],STOK[Stok] from stok", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataSet ds = new DataSet();
            da.Fill(ds); 
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            baglanti.Close();
        }
        
        private void Form5_Load(object sender, EventArgs e)
        {
            verılerıgoster();
            dataGridView1.MouseClick += new MouseEventHandler(dataGridView1_MouseClick_1);
        }

        private void aCToolStripMenuItem_Click_1(object sender, EventArgs e)//SAĞTIK İLE AÇ BUTONU
        {
            int id = (dataGridView1.CurrentCell.RowIndex)+1; 
            frm_tanimlar_stok_ürünkayıt.AC(id);  
            verılerıgoster();
        }
    
        private void yENIToolStripMenuItem_Click(object sender, EventArgs e)//SAĞ TIK İLE YENİ BUTONU
        {
            frm_tanimlar_stok_ürünkayıt.AC(0);
            verılerıgoster();
        }

        private void dataGridView1_MouseClick_1(object sender, MouseEventArgs e)//SAĞ TIKI KONTROL ETME
        {   
            if (e.Button == MouseButtons.Left)
            {              
            }
            else
            {
                ContextMenuStrip menu = new System.Windows.Forms.ContextMenuStrip();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)//KAPAT BUTONU
        {
            this.Dispose();
        }
    }
}
