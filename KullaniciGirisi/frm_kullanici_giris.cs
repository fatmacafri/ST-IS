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

namespace KullaniciGirisi
{
    public partial class frm_kullanici_giris : Form //KULLANICI GİRİŞİ FORMU
    {
        public frm_kullanici_giris()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=NOTEBOOK\\SQLEXPRESS;Initial Catalog=KullanıcıGiris;Integrated Security=True");//SQL'E BAĞLANMAK İÇİN
        public static int ID;
        public static int yetki;
        public static string name;
        private void button2_Click(object sender, EventArgs e)//KAPAT BUTONU
        {
            Dispose();
        }
        private void Form1_Load(object sender, EventArgs e)//FORM YÜKLENİRKEN HİÇBİR ŞEY OLMICAK SADECE FORM AÇILACAK BOŞ O YÜZDEN.
        {

        }
        
        private void button1_Click(object sender, EventArgs e)//GİRİŞ BUTONU 
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from kullanici where KULLANICIAD='" + textBox1.Text + "'and SIFRE='" + textBox2.Text + "'", baglanti);//KOMUT OLUŞTURDUK KARŞILAŞTIRMA YAPABİLMEK İÇİN SQL'E BAKACAZ KULLANICI ADI YERİNE GİRİLEN İSİM DATABASEDE VAR MI DİYE VE ŞİFRE İÇİNDE
            SqlDataReader oku = komut.ExecuteReader();//SQLDEN VERİLERİ OKUMAK İÇİN
            if (oku.Read())//OKUDUĞUMUZDA ID LER EŞİTSE SQLDEKİ İLE YENİ FORM AÇILACAK.
            {
                ID = Convert.ToInt32(oku["KULLANICIID"].ToString());
                frm_tanimlar f4 = new frm_tanimlar();
                f4.Show();
                this.Hide();

            if (Convert.ToBoolean(oku["MUSTERILISTE"].ToString()) == false)//YETKİLERİ KONTROL ETMEK İÇİN YANİ BİR KULLANICININ HANGİ YETKİSİ VARSA O BUTONLAR AKTİF OLARAK YENİ FORMDA AÇILACAK.
            {
                f4.button4.Enabled = false;
            }
            if (Convert.ToBoolean(oku["STOKLISTE"]) == false)
            {
                f4.button1.Enabled = false;
            }
            if (Convert.ToBoolean(oku["DEPOLISTE"]) == false)
            {
                f4.button3.Enabled = false;
            }
            if (Convert.ToBoolean(oku["SATISLISTE"]) == false)
            {
                f4.button6.Enabled = false;
            }
            if (Convert.ToBoolean(oku["SATIS"]) == false)
            {
                f4.button2.Enabled = false;
            }
            if (Convert.ToBoolean(oku["KULLANICILISTE"]) == false)
            {
                f4.button5.Enabled = false;
            }   
            }
            else
            {
                HATACAL();//EĞER ID LER EŞİT DEĞİLSE SES ÇALACAK VE MESAJ GÖZÜKECEK.
                MessageBox.Show("Kullanıcı Adı veya Şifre Hatalı");
            } 
            baglanti.Close();

            baglanti.Open();
            SqlCommand komut1 = new SqlCommand("select * from musteri where MUSTERIAD='" + textBox1.Text + "'and MUSTERISIFRE='" + textBox2.Text + "'", baglanti);
            SqlDataReader oku1 = komut1.ExecuteReader();
            if (oku1.Read())
            {
                name = textBox1.Text;
                ID = Convert.ToInt32(oku1["MUSTERIID"].ToString());
                Form1 f1 = new Form1();
                f1.Show();
                this.Hide();
            }
            baglanti.Close();
        }
        void HATACAL()//SES ÇALMA FONKSİYONU.
        {
            SoundPlayer ses = new SoundPlayer();
            ses.SoundLocation = @"C:\Users\Toshiba\Documents\visual studio 2015\Projects\KullaniciGirisi\KullaniciGirisi\bin\Debug\BARKODHATA.WAV";
            ses.Play();
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)//ENTER'A BASARAK GİRİŞ YAPMAK.NORMAL GİRİŞ BUTONUNDAKİ HER ŞEY BURDADA AYNI
        {
            if (e.KeyChar == 13)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("select * from kullanici where KULLANICIAD='" + textBox1.Text + "'and SIFRE='" + textBox2.Text + "'", baglanti);
                SqlDataReader oku = komut.ExecuteReader();        
                if (oku.Read())
                {
                    ID = Convert.ToInt32(oku["KULLANICIID"].ToString());
                    frm_tanimlar f4 = new frm_tanimlar();
                    f4.Show();
                    this.Hide();

                    if (Convert.ToBoolean(oku["MUSTERILISTE"].ToString()) == false)
                    {
                        f4.button4.Enabled = false;
                    }
                    if (Convert.ToBoolean(oku["STOKLISTE"]) == false)
                    {
                        f4.button1.Enabled = false;
                    }
                    if (Convert.ToBoolean(oku["DEPOLISTE"]) == false)
                    {
                        f4.button3.Enabled = false;
                    }
                    if (Convert.ToBoolean(oku["SATISLISTE"]) == false)
                    {
                        f4.button6.Enabled = false;
                    }
                    if (Convert.ToBoolean(oku["SATIS"]) == false)
                    {
                        f4.button2.Enabled = false;
                    }
                    if (Convert.ToBoolean(oku["KULLANICILISTE"]) == false)
                    {
                        f4.button5.Enabled = false;
                    } 
                }
               // else
                //{
                  // HATACAL();
                  // MessageBox.Show("Kullanıcı Adı veya Şifre Hatalı");
               // }
                baglanti.Close();

                baglanti.Open();
                SqlCommand komut1 = new SqlCommand("select * from musteri where MUSTERIAD='" + textBox1.Text + "'and MUSTERISIFRE='" + textBox2.Text + "'", baglanti);
                SqlDataReader oku1 = komut1.ExecuteReader();
                if (oku1.Read())
                {
                    name = textBox1.Text;
                    ID = Convert.ToInt32(oku1["MUSTERIID"].ToString());
                    Form1 f1 = new Form1();
                    f1.Show();
                    this.Hide();
                }
                baglanti.Close();
            }
    
        }
        
        private void button3_Click(object sender, EventArgs e)//YÜZ TANIMA BUTONUYLA GİRİŞ
        {  
            frm_kullanicigiris_yüztanima f = new frm_kullanicigiris_yüztanima();
            f.ShowDialog();
            this.Hide();
          
        }
    }
        }

      
    

