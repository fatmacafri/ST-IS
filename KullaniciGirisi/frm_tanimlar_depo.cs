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
    public partial class frm_tanimlar_depo : Form //DEPO FORMU
    {
        public frm_tanimlar_depo()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=NOTEBOOK\\SQLEXPRESS;Initial Catalog=KullanıcıGiris;Integrated Security=True");
        public static DataRow MODALRESULT = null;
        public static DataRow ac()
        {
            frm_tanimlar_depo f = new frm_tanimlar_depo();
            f.ShowDialog();
            f.Dispose();
            return MODALRESULT;
        }
        public void verılerıgoster()//VERİLERİ GÖSTERME FONKSİYONU SQLDEN VERİLERİ ÇEKMEK İÇİN
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select DEPOID[Depo ID],KOD[Depo Kodu],ISIM[Depo Adı],ADRES[Depo Adresi],AKTIF[Depo Durum] from depotanim", baglanti);
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
        private void frm_tanim_depo_Load(object sender, EventArgs e)
        {
            verılerıgoster();//SQLDEN ÇEKTİĞİMİZ VERİLERİ DATAGRİDDE GÖSTERMEK İÇİN.
            dataGridView1.MouseClick += new MouseEventHandler(dataGridView1_MouseClick);
        }

        private void aCToolStripMenuItem_Click(object sender, EventArgs e)//SAĞ TIK YAPARAK AÇ KISMI
        {
            int id = (dataGridView1.CurrentCell.RowIndex) + 1;
            frm_tanimlar_depo_depokayit.AC(id);
            verılerıgoster();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)//MOUSE UN CLICK BASARAK SAĞ CLICKSE AÇ YENİ KISMININ ÇIKMASI
        {

            if (e.Button == MouseButtons.Left)
            {
            }
            else
            {
                ContextMenuStrip menu = new System.Windows.Forms.ContextMenuStrip();
            }
        }

        private void yENIToolStripMenuItem_Click(object sender, EventArgs e)//SAĞ TIK İLE YENİ BUTONUNUN AÇILMASI
        {
            frm_tanimlar_depo_depokayit.AC(0);
            verılerıgoster();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MODALRESULT = (dataGridView1.DataSource as DataTable).Rows[dataGridView1.CurrentCell.RowIndex];
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
    }

