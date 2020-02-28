using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace KullaniciGirisi

{
    class Classifier_Train : IDisposable
    {
        /// <summary>
        /// Örnek Kullanım,
        /// D:\\, D:\\Klasor\\
        /// </summary>
        string Dizin;
        string KlasorAdi;
        string XmlVeriDosyasi;
        public Classifier_Train(string Dizin, string KlasorAdi)
        {
            this.Dizin = Dizin + KlasorAdi;


            termCrit = new MCvTermCriteria(ContTrain, 0.001);
            _IsTrained = LoadTrainingData(this.Dizin);
        }
        public Classifier_Train(string Dizin, string KlasorAdi, string XmlVeriDosyasi)
        {
            this.Dizin = Dizin + KlasorAdi;
            this.XmlVeriDosyasi = XmlVeriDosyasi;


            termCrit = new MCvTermCriteria(ContTrain, 0.001);
            _IsTrained = LoadTrainingData(this.Dizin);
        }

        #region Variables

        //Eigen
        MCvTermCriteria termCrit;
        EigenObjectRecognizer recognizer;

        //training variables
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();//Images
        List<string> Names_List = new List<string>(); //labels
        int ContTrain, NumLabels;
        float Eigen_Distance = 0;
        string Eigen_label;
        int Eigen_threshold = 0;

        //Class Variables
        string Error;
        bool _IsTrained = false;

        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor, Looks in (Dizin") for traing data.
        /// </summary>
        public Classifier_Train()
        {
            KlasorAdi = "TrainedFaces";
            Dizin = Application.StartupPath + "\\" + KlasorAdi;
            XmlVeriDosyasi = "TrainedLabels.xml";

            termCrit = new MCvTermCriteria(ContTrain, 0.001);
            _IsTrained = LoadTrainingData(Dizin);
        }

        /// <summary>
        /// Takes String input to a different location for training data
        /// </summary>
        /// <param name="Training_Folder"></param>
        public Classifier_Train(string Training_Folder)
        {
            termCrit = new MCvTermCriteria(ContTrain, 0.001);
            _IsTrained = LoadTrainingData(Training_Folder);
        }
        #endregion

        #region Public
        /// <summary>
        /// <para>Return(True): If Training data has been located and Eigen Recogniser has been trained</para>
        /// <para>Return(False): If NO Training data has been located of error in training has occured</para>
        /// </summary>
        public bool IsTrained
        {
            get { return _IsTrained; }
        }

        /// <summary>
        /// Recognise a Grayscale Image using the trained Eigen Recogniser
        /// </summary>
        /// <param name="Input_image"></param>
        /// <returns></returns>
        public string Recognise(Image<Gray, byte> Input_image, int Eigen_Thresh = -1)
            //Bu metot, gönderilen image nesnesini hafızadakilerle kıyaslayarak eşleşen kaydın bilgilerini dönmektedir.
        {
            if (_IsTrained)
            {
                EigenObjectRecognizer.RecognitionResult ER = recognizer.Recognize(Input_image);
                //handle if recognizer.EigenDistanceThreshold is set as a null will be returned
                //NOTE: This is still not working correctley 
                if (ER == null)
                {
                    Eigen_label = "Tanımsız";
                    Eigen_Distance = 0;
                    return Eigen_label;
                }
                else
                {
                    Eigen_label = ER.Label;
                    Eigen_Distance = ER.Distance;
                    if (Eigen_Thresh > -1) Eigen_threshold = Eigen_Thresh;
                    if (Eigen_Distance > Eigen_threshold) return Eigen_label;
                    else return "Tanımsız";
                }

            }
            else return "";
        }

        /// <summary>
        /// Sets the threshold confidence value for string Recognise(Image<Gray, byte> Input_image) to be used.
        /// </summary>
        public int Set_Eigen_Threshold
        {
            set
            {
                //NOTE: This is still not working correctley 
                //recognizer.EigenDistanceThreshold = value;
                Eigen_threshold = value;
            }
        }

        /// <summary>
        /// Returns a string containg the recognised persons name
        /// </summary>
        public string Get_Eigen_Label
        {
            get
            {
                return Eigen_label;
            }
        }

        /// <summary>
        /// Returns a float confidence value for potential false clasifications
        /// </summary>
        public float Get_Eigen_Distance
        {
            get
            {
                //get eigenDistance
                return Eigen_Distance;
            }
        }

        /// <summary>
        /// Returns a string contatining any error that has occured
        /// </summary>
        public string Get_Error
        {
            get { return Error; }
        }

        /// <summary>
        /// Saves the trained Eigen Recogniser to specified location
        /// </summary>
        /// <param name="filename"></param>
        public void Save_Eigen_Recogniser(string filename)
        {
            StringBuilder sb = new StringBuilder();

            (new XmlSerializer(typeof(EigenObjectRecognizer))).Serialize(new StringWriter(sb), recognizer);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(sb.ToString());
            xDoc.Save(filename);
        }

        /// <summary>
        /// Loads the trained Eigen Recogniser from specified location
        /// </summary>
        /// <param name="filename"></param>
        public void Load_Eigen_Recogniser(string filename)
        {
            //introduce error checking
            FileStream EigenFS = File.OpenRead(filename);
            long Eflength = EigenFS.Length;
            byte[] xmlEBs = new byte[Eflength];
            EigenFS.Read(xmlEBs, 0, (int)Eflength);
            EigenFS.Close();

            MemoryStream xStream = new MemoryStream(xmlEBs);
            recognizer = (EigenObjectRecognizer)(new XmlSerializer(typeof(EigenObjectRecognizer))).Deserialize(xStream);
            _IsTrained = true;
            //_eigenImages[Array_location] = (Image<Gray, Single>)(new XmlSerializer(typeof(Image<Gray, Single>))).Deserialize(xStream);
        }

        /// <summary>
        /// Dispose of Class call Garbage Collector
        /// </summary>
        public void Dispose()
        {
            recognizer = null;
            trainingImages = null;
            Names_List = null;
            Error = null;
            GC.Collect();
        }

        #endregion

        #region Private
        /// <summary>
        /// Loads the traing data given a (string) folder location
        /// </summary>
        /// <param name="Folder_location"></param>
        /// <returns></returns>
        private bool LoadTrainingData(string Folder_location)
        {
            if (File.Exists(Folder_location + "\\" + XmlVeriDosyasi))
            {
                try
                {
                    //message_bar.Text = "";
                    Names_List.Clear();
                    trainingImages.Clear();
                    FileStream filestream = File.OpenRead(Folder_location + "\\" + XmlVeriDosyasi);
                    long filelength = filestream.Length;
                    byte[] xmlBytes = new byte[filelength];
                    filestream.Read(xmlBytes, 0, (int)filelength);
                    filestream.Close();

                    MemoryStream xmlStream = new MemoryStream(xmlBytes);

                    using (XmlReader xmlreader = XmlTextReader.Create(xmlStream))
                    {
                        while (xmlreader.Read())
                        {
                            if (xmlreader.IsStartElement())
                            {
                                switch (xmlreader.Name)
                                {
                                    case "NAME":
                                        if (xmlreader.Read())
                                        {
                                            Names_List.Add(xmlreader.Value.Trim());
                                            NumLabels += 1;
                                        }
                                        break;
                                    case "FILE":
                                        if (xmlreader.Read())
                                        {
                                            //PROBLEM HERE IF TRAININGG MOVED
                                            trainingImages.Add(new Image<Gray, byte>(Dizin + "\\" + xmlreader.Value.Trim()));
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    ContTrain = NumLabels;

                    if (trainingImages.ToArray().Length != 0)
                    {
                        //Eigen face recognizer
                        recognizer = new EigenObjectRecognizer(trainingImages.ToArray(),
                        Names_List.ToArray(), 5000, ref termCrit); //5000 default
                        return true;
                    }
                    else return false;
                }
                catch (Exception ex)
                {
                    Error = ex.ToString();
                    return false;
                }
            }
            else return false;
        }

        #endregion
    }
}
