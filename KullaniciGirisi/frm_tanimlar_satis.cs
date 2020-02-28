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
using System.Media;
using System.Net.Mail;
using System.Collections;

namespace KullaniciGirisi
{
    public partial class frm_tanimler_satis : Form//SATIŞ FORMU
    {
        string cs = "Data Source=NOTEBOOK\\SQLEXPRESS;Initial Catalog=KullanıcıGiris;Integrated Security=True";
        SqlConnection baglanti = new SqlConnection("Data Source=NOTEBOOK\\SQLEXPRESS;Initial Catalog=KullanıcıGiris;Integrated Security=True");

        public string[] parcala(string text)//BARKOD OKUMA KISMI
        {
            int adet = 1;
            string barkod = "";
            string[] donus = { adet.ToString(), barkod, "" };
            char[] ayrac = { '*' };
            string[] barkodarray = text.Split(ayrac);
            if (barkodarray.Length > 2)//BARKOD UZUNLUĞU 2DEN FAZLA İSE
            {
                MessageBox.Show("BARKOD BULUNAMADI.", "HATA");
                donus[2] = "hata";
                return donus;
            }
            if (barkodarray.Length == 2)//BARKOD UZUNLUĞU 2 İSE 2YE AYIRIR İLK KISIM ADET DİĞER KISIM BARKOD NUMARASIDIR.YANİ 3*5 DEMEK 3 ADET 5 NUMARALI ÜRÜNDEN EKLE
            {
                try
                {
                    adet = Convert.ToInt32(barkodarray[0]);
                    barkod = barkodarray[1].ToString();
                    donus[0] = adet.ToString();
                    donus[1] = barkod;
                }
                catch
                {
                    MessageBox.Show("BARKOD GİRİŞİ HATALI.", "HATA");
                    donus[2] = "hata";
                    return donus;
                }
                return donus;
            }
            if (barkodarray.Length == 1)//BARKOD UZUNLUĞU 1 İSE
            {
                barkod = barkodarray[0];
                donus[1] = barkod;
                return donus;
            }
            MessageBox.Show("BİLİNMEYEN HATA.", "HATA");
            donus[2] = "hata";
            return donus;
        }
        public int satirekle(int productid, string name, int quantity, int price, int stock)//ALINAN ÜRÜNLERİN DATAGRİDE EKLENMESİ
        {
            int rowindex = -1;
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                if (productid == Convert.ToInt32(DT.Rows[i]["ÜRÜN ID"]))
                {
                    rowindex = i;
                    break;
                }
            }

            if (rowindex == -1)
            {
                DT.Rows.Add(productid, name, quantity, price * quantity, stock);
                rowindex = DT.Rows.Count - 1;
            }
            else
            {
                DT.Rows[rowindex]["ÜRÜN MİKTAR"] = Convert.ToInt32(DT.Rows[rowindex]["ÜRÜN MİKTAR"]) + quantity;
                DT.Rows[rowindex]["TUTAR"] = Convert.ToInt32(DT.Rows[rowindex]["TUTAR"]) + quantity * price;
            }

            int tutar = 0;//TUTAR HESAPLAMA
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                tutar += Convert.ToInt32(DT.Rows[i]["TUTAR"]);
            }

            textBox2.Text = tutar.ToString();

            return rowindex;
        }

         void HATACAL()//HATALI BARKOD GİRİLDİĞİ ZAMAN SES ÇALMASI.
        {
            SoundPlayer ses = new SoundPlayer();
            ses.SoundLocation = @"C:\Users\Toshiba\Documents\visual studio 2015\Projects\KullaniciGirisi\KullaniciGirisi\bin\Debug\BARKODHATA.WAV";
            ses.Play();
        }
        public frm_tanimler_satis()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)//ENTERE BASINCA BARKOD GİRİŞİ
        {
            if (e.KeyChar == 13)
            {
                if (textBox1.Text == "")
                {
                    HATACAL();
                    textBox1.Clear();
                    return;
                }
                string hatakontrol = parcala(textBox1.Text)[2];
                if (hatakontrol == "hata")
                {
                    HATACAL();
                    textBox1.Clear();
                    return;
                }
                else
                {
                    int adet = Convert.ToInt32(parcala(textBox1.Text)[0].ToString());
                    string barkod = parcala(textBox1.Text)[1];


                    DataTable dt = doquery("select * from stok where PRODUCTID='" + barkod.Replace("'", "") + "'");

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Barkod Bulunamadı!");
                        textBox1.Text = "";
                        textBox1.Focus();

                        return;
                    }

                    satirekle(Convert.ToInt32(dt.Rows[0]["STOKID"]),dt.Rows[0]["NAME"].ToString(),adet,Convert.ToInt32(dt.Rows[0]["PRICE"]),Convert.ToInt32(dt.Rows[0]["STOK"]));
                    dt.Dispose();
                    textBox1.Text = "";
                    textBox1.Focus();
                }
            }
        }

        public DataTable doquery(string sql)//???????????????????????
        {
            SqlCommand ca = baglanti.CreateCommand();
            ca.CommandText = sql;
            SqlDataAdapter da = new SqlDataAdapter(ca);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ca.Dispose();
            da.Dispose();
            return dt;
        }

        private void button1_Click(object sender, EventArgs e)//LİSTE BUTONUYLA ÜRÜN SEÇME(ÜRÜN LİSTESİNİ AÇAR)
        {
            DataRow dr = frm_tanimlar_satis_malzemelistesi.ac();
            if (dr == null)
                return;
            int productid = Convert.ToInt32(dr["PRODUCTID"]);
            string name = dr["NAME"].ToString();
            int price = Convert.ToInt32(dr["PRICE"]);
            int stock = Convert.ToInt32(dr["STOK"]);
            satirekle(productid, name, frm_tanimlar_satis_malzemelistesi.MIKTAR, price, stock);
            return;
            MessageBox.Show(frm_tanimlar_satis_malzemelistesi.MIKTAR.ToString());
            return;
            frm_tanimlar_satis_malzemelistesi liste = new frm_tanimlar_satis_malzemelistesi();
            liste.Show();
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
        }

        DataTable DT = new DataTable();
        private void Form2_Load(object sender, EventArgs e)//DATAGRİDDE COLOUMN ADI BELİRLEME
        {
            nextEvrakNo();//EVRAK NOYU DOLU GETİRME

            DT.Columns.Add("ÜRÜN ID", typeof(int));
            DT.Columns.Add("ÜRÜN AD", typeof(string));
            DT.Columns.Add("ÜRÜN MİKTAR", typeof(int));
            DT.Columns.Add("TUTAR", typeof(int));
            DT.Columns.Add("STOK", typeof(int));

            dataGridView1.DataSource = DT;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
        }

        private void nextEvrakNo()//GELECEK EVRAK NO BELİRLEME
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select MAX(CAST(EVRAKNO AS INT)) as EvrakNo from dbo.satis", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            textBox5.Text = (Convert.ToInt32(dt.Rows[0]["EvrakNo"].ToString()) + 1).ToString();
            baglanti.Close();
        }

        private void button5_Click(object sender, EventArgs e)//SEÇ BUTONUYLA DEPO SEÇME
        {
            DataRow dr = frm_tanimlar_depo.ac();
            if (dr == null)
            {
                return;
            }
            textBox3.Text = dr["Depo Kodu"].ToString();
            textBox3.Tag = dr["Depo ID"].ToString();
            textBox4.Text = dr["Depo Adı"].ToString();
            return;
        }

        private void button4_Click(object sender, EventArgs e)//FATURA KAYDETME SQL'E
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Satılacak ürün yok.");
                return;
            }
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into satis(DEPOID,TARIH,SAAT,EVRAKNO,MUSTERIID) values(@1,@2,@3,@4,@5) select scope_identity() as ID ", baglanti);
            komut.Parameters.AddWithValue("@1", textBox3.Tag);
            komut.Parameters.AddWithValue("@2", DateTime.Now.Date);
            komut.Parameters.AddWithValue("@3", DateTime.Now.TimeOfDay);
            komut.Parameters.AddWithValue("@4", textBox5.Text);
            komut.Parameters.AddWithValue("@5", textBox6.Tag);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int satisid = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                komut = new SqlCommand("insert into satisdetay(SATISID,STOKID,MIKTAR,FIYAT) values(@1,@2,@3,@4) ", baglanti);
                komut.Parameters.AddWithValue("@1", satisid);
                komut.Parameters.AddWithValue("@2", dataGridView1.Rows[i].Cells["ÜRÜN ID"].Value);
                komut.Parameters.AddWithValue("@3", dataGridView1.Rows[i].Cells["ÜRÜN MİKTAR"].Value);
                komut.Parameters.AddWithValue("@4", dataGridView1.Rows[i].Cells["TUTAR"].Value);
                komut.ExecuteNonQuery();
            }
            da.Dispose();
            dt.Dispose();
            MessageBox.Show("fatura kaydedildi.");
           
            baglanti.Close();

        }

        private void button2_Click_1(object sender, EventArgs e)//SEÇ BUTONUYLA MÜŞTERİ SEÇME
        {
            DataRow dr = frm_tanimlar_musteri .ac();
            if (dr == null)
            {
                return;
            }
            textBox6.Text = dr["Müşteri Ad"].ToString();
            textBox6.Tag = dr["Müşteri ID"].ToString();
            textBox7.Text = dr["Müşteri Soyad"].ToString();
            return;
        }

        private void button6_Click(object sender, EventArgs e)//KAPAT BUTONU
        {
            this.Dispose();
        }

        private void button3_Click_1(object sender, EventArgs e)//SİL BUTONU
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {

                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index); //seçili satırı siliyoruz.
            }
            else
            {
                MessageBox.Show("Lütfen Silinecek Satırı Seçin!");
            }
        }

        private void button7_Click(object sender, EventArgs e)//MÜŞTERİYE MAİL GÖNDERME
        {
            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select MUSTERIMAIL from musteri where MUSTERIAD='" + textBox6.Text + "'";
            con.Open(); MessageBox.Show("baglantı");
            SqlDataReader dr = cmd.ExecuteReader();
            ArrayList isimler = new ArrayList();

            while (dr.Read())
            {
                isimler.Add(dr["MUSTERIMAIL"]);
            }
            dr.Close();
            con.Close();
            listBox1.Items.AddRange(isimler.ToArray());
            MessageBox.Show(listBox1.Items.ToString());
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string s = (listBox1.Items[i].ToString());
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("spixspix8@gmail.com", "Fatma");
                    mail.To.Add(s);
                    mail.Subject = "FATURA";
                    mail.Body = "Toplam tutarı "+textBox2.Text+" TL olan ve evrak numarası "+textBox5.Text+" olan faturanız oluşturulmuştur.";
                    mail.IsBodyHtml = true;
                    SmtpClient smtpserver = new SmtpClient();
                    smtpserver.Host = "smtp.gmail.com";
                    smtpserver.Port = 587;
                    smtpserver.Credentials = new System.Net.NetworkCredential("spixspix8@gmail.com", "123456789#+");
                    smtpserver.EnableSsl = true;
                    smtpserver.Send(mail);
                    MessageBox.Show("mail gönderildi");
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                    textBox7.Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("error:" + ex.Message);
                }
            }

        }
    }
}