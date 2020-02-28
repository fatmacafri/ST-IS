using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Threading;
using System.Data.SqlClient;

namespace KullaniciGirisi
{
    public partial class frm_kullanicigiris_yüztanima : Form//YÜZ TANIMA FORMU
    {
        public frm_kullanicigiris_yüztanima()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=NOTEBOOK\\SQLEXPRESS;Initial Catalog=KullanıcıGiris;Integrated Security=True");
        public static int ID;
        private async void button1_Click(object sender, EventArgs e)
        //Görüldüğü üzere tanımlanan yüzler pictureBox2 nesnesine atanmaktadır. Haliyle yakalanan bu yüz resimleri SaveTrainingData metoduna gönderilmekte ve program yüzlere özel eğitilmektedirler.
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    if (!recognition.SaveTrainingData(pictureBox2.Image, textBox1.Text)) MessageBox.Show("Hata", "Profil alınırken beklenmeyen bir hata oluştu.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Thread.Sleep(100);
                    label1.Text = (i + 1) + " adet profil.";
                }
                recognition = null;
                train = null;

                recognition = new BusinessRecognition();
                train = new Classifier_Train();//Eğitilen Yüzlerle Tanımlanan Yüzü Kıyaslama
            });
        }
        BusinessRecognition recognition = new BusinessRecognition();
        Classifier_Train train = new Classifier_Train();

        private void frm_kullanicigiris_yüztanima_Load(object sender, EventArgs e)
        //Tanımlanan ve Tanınan Yüzün Vurgulanıp İsminin Yazılması
        {
            Capture capture = new Capture();
            capture.Start();
            capture.ImageGrabbed += (a, b) =>
            {
                var image = capture.RetrieveBgrFrame();
                var image2 = image.Convert<Gray, byte>();
                HaarCascade haaryuz = new HaarCascade("haarcascade_frontalface_alt2.xml");
                MCvAvgComp[][] Yuzler = image2.DetectHaarCascade(haaryuz, 1.2, 5, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(15, 15));
                MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 0.5, 0.5);
                foreach (MCvAvgComp yuz in Yuzler[0])
                {
                    var sadeyuz = image2.Copy(yuz.rect).Convert<Gray, byte>().Resize(100, 100, INTER.CV_INTER_CUBIC);
                    //Resimler aynı boyutta olmalıdır. O yüzden Resize ile yeniden boyutlandırma yapılmıştır. Aksi taktirde Classifier_Train sınıfının 245. satırında hata alınacaktır.
                    pictureBox2.Image = sadeyuz.ToBitmap();
                    if (train != null)
                        if (train.IsTrained)
                        {
                            string name = train.Recognise(sadeyuz);
                            int match_value = (int)train.Get_Eigen_Distance;
                            image.Draw(name + " ", ref font, new Point(yuz.rect.X - 2, yuz.rect.Y - 2), new Bgr(Color.LightGreen));
                        }
                    image.Draw(yuz.rect, new Bgr(Color.Red), 2);
                    textBox2.Text= train.Recognise(sadeyuz);
                  
                }
                pictureBox1.Image = image.ToBitmap();
            };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            recognition.DeleteTrains();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from kullanici where KULLANICIAD='"+textBox2.Text+ "'", baglanti);
            SqlDataReader oku = komut.ExecuteReader();
            if (oku.Read())
            {
                ID = Convert.ToInt32(oku["KULLANICIID"].ToString());
                frm_tanimlar f4 = new frm_tanimlar();

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
                f4.Show();
               
            } 
            else
            {
                MessageBox.Show("Kullanıcı Adı veya Şifre Hatalı");
            }
           
           baglanti.Close();
             this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
