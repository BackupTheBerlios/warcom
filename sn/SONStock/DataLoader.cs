using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Collections;
using System.Windows.Forms;

namespace SONStock
{
    class DataLoader
    {
        private SortedDictionary<DateTime, double> data = new SortedDictionary<DateTime, double>();

        public SortedDictionary<DateTime, double> Data
        {
            get { return data; }
        }

        public List<double> ListDoubleData
        {
            get
            {
                IEnumerator enumerator = data.Keys.GetEnumerator();
                List<double> values = new List<double>();
                while (enumerator.MoveNext())
                {
                    DateTime dt = (DateTime)enumerator.Current;
                    values.Add(data[dt]);
                }
                return values;
            }
        }

        public void ClearData()
        {
            data.Clear();
        }

        public int Count
        {
            get 
            {
                if (data != null)
                    return data.Count;
                else
                    return 0;
            }
        }

        private void AddNewDataFromFile(String filename, bool clear)
        {
            if (clear)
                data.Clear();
            using (TextReader rd = new StreamReader(filename))
            {
                String str = null;
                string flo = "[+|-]?[0-9]*[,[0-9]*]?";
                string date = "[0-9]*-[0-9][0-9]-[0-9][0-9]";
                string wzorzec = "\"(?<data>" + date + "?)\",\"" +
                    "(?<fl1>" + flo + "?)\",\"" +
                    "(?<fl2>" + flo + "?)\",\"" +
                    "(?<fl3>" + flo + "?)\",\"" +
                    "(?<fl4>" + flo + "?)\",\"" +
                    "(?<fl5>" + flo + "?)\"";
                Regex wyr = new Regex(wzorzec, RegexOptions.IgnoreCase);
                while ((str = rd.ReadLine()) != null)
                {
                    try
                    {
                        str = str.Trim();
                        Match elem = wyr.Match(str);
                        string[] tablica = wyr.GetGroupNames();
                        if (elem.Success)
                        {
                            string d = elem.Groups["data"].Value;
                            double zamk = (double)Double.Parse(elem.Groups["fl4"].Value);
                            DateTime dt = new DateTime(Int32.Parse(d.Substring(0, 4)),
                                                        Int32.Parse(d.Substring(5, 2)),
                                                        Int32.Parse(d.Substring(8, 2)));
                            data.Add(dt, zamk);
                        }
                        else
                        {
                            //MessageBox.Show("pora¿ka lub koniec danych:)");
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        public ElmansNetwork LoadNetwork()
        {
            ElmansNetwork elmanNet = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "pliki csv (*.xml)|*.xml";
            ofd.Title = "Wybierz plik z sieci¹";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //Opens file "data.xml" and deserializes the object from it.
                    Stream stream = File.Open(ofd.FileName, FileMode.Open);
                    SoapFormatter formatter = new SoapFormatter();

                    //formatter = new BinaryFormatter();

                    elmanNet = (ElmansNetwork)formatter.Deserialize(stream);
                    stream.Close();

                    Properties.Settings.Default.estimationTime = elmanNet.NumberOfExitNeurons;
                    Properties.Settings.Default.hiddenLayerSize = elmanNet.NumberOfHiddenNeurons;
                    Properties.Settings.Default.entryLayerSize = elmanNet.NumberOfEntryNeurons;
                    Properties.Settings.Default.Save();
                }
                catch (Exception)
                {
                    MessageBox.Show("Deserializacja zakoñczy³a siê niepowodzeniem");
                }
            }
            return elmanNet;
        }

        public void SaveNetwork(ElmansNetwork elmanNet)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Pliki xml (*.xml)|*.xml";
            sfd.Title = "Wybierz plik z sieci¹";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                //Opens a file and serializes the object into it in binary format.
                Stream stream = File.Open(sfd.FileName, FileMode.Create);
                SoapFormatter formatter = new SoapFormatter();

                //BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(stream, elmanNet);
                stream.Close();
            }
        }

        public void LoadDataFromFile(bool clear)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Pliki csv (*.csv)|*.csv";
            ofd.Title = "Wybierz plik zawieraj¹cy dane";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.AddNewDataFromFile(ofd.FileName, clear);
                //this.addDataToExistingToolStripMenuItem.Enabled = true;
            }
        }

        public void LoadDataFromFiles(bool clear)
        {
            OpenDirectoryDialog odd = new OpenDirectoryDialog();
            string directory;
            if ((directory = odd.GetFolder()) != null && directory != "")
            {
                string[] files = Directory.GetFiles(directory, "*.csv");
                if (files.Length > 0)
                {
                    if (clear)
                        this.data.Clear();
                    for (int i = 0; i < files.Length; ++i)
                    {
                        this.AddNewDataFromFile(files[i], false);
                    }
                    //this.addDataToExistingToolStripMenuItem.Enabled = true;
                }
                else
                    MessageBox.Show("Niestety nie znaleziono ¿adnych plików *.csv");
            }
        }

        public ElmansNetwork Learn(ElmansNetwork elmanNet)
        {
            if (data == null || data.Count == 0)
            {
                MessageBox.Show("Wczytaj zestaw ucz¹cy");
                return elmanNet;
            }

            int entryLayerSize = Properties.Settings.Default.entryLayerSize;
            int exitLayerSize = Properties.Settings.Default.estimationTime;
            int hiddenLayerSize = Properties.Settings.Default.hiddenLayerSize;

            //IEnumerator enumerator = data.Keys.GetEnumerator();
            //double[] values = new double[data.Count];
            //int i = 0;
            //while (enumerator.MoveNext())
            //{
            //    DateTime dt = (DateTime)enumerator.Current;
            //    values[i++] = data[dt];
            //}
            int i;
            double[] values = this.ListDoubleData.ToArray();
            bool useTechnicalAnalysis = Properties.Settings.Default.useTechnicalAnalysis;

            if (elmanNet == null ||
                elmanNet.NumberOfEntryNeurons != entryLayerSize ||
                elmanNet.NumberOfExitNeurons != exitLayerSize)
                elmanNet = new ElmansNetwork(entryLayerSize, hiddenLayerSize, exitLayerSize, useTechnicalAnalysis);

            double[] val = new double[entryLayerSize];
            double[] correct = new double[exitLayerSize];

            for (i = 0; i < values.Length - entryLayerSize - exitLayerSize; ++i)
            {
                for (int j = 0; j < entryLayerSize; ++j)
                {
                    val[j] = values[i + j];
                }
                for (int j = 0; j < exitLayerSize; ++j)
                {
                    correct[j] = values[i + j + entryLayerSize];
                }
                double oMin, oMax;
                this.Normalize( ref val, ref correct, out oMin, out oMax);
                elmanNet.Learn(val, correct);
            }
            return elmanNet;
        }

        public void Normalize(ref double[] val, ref double[] exit, out double oMin, out double oMax)
        {
            if (val.Length == 0)
            {
                oMin = oMax = 0;
                return;
            }
            double[] values = new double[val.Length];
            double max = val[0], min = val[0];
            for (int i = 0; i < val.Length; ++i)
            {
                max = max >= val[i] ? max : val[i];
                min = min <= val[i] ? min : val[i];
            }
            for (int i = 0; i < exit.Length; ++i)
            {
                max = max >= exit[i] ? max : exit[i];
                min = min <= exit[i] ? min : exit[i];
            }
            double diff = max - min;
            diff = diff == 0 ? 0.01 : diff;
            for (int i = 0; i < val.Length; ++i)
            {
                val[i] = 0.1 + 0.8 * ((val[i] - min) / diff);
            }
            for (int i = 0; i < exit.Length; ++i)
            {
                exit[i] = 0.1 + 0.8 * ((exit[i] - min) / diff);
            }
            oMin = min;
            oMax = max;
        }

        public void Normalize(ref double[] val, out double oMin, out double oMax)
        {
            double[] tmp = new double[0];
            this.Normalize(ref val, ref tmp,out  oMin,out oMax);
        }

        public void DeNormalize(ref double[] val, double min, double max)
        {
            double diff = max - min;
            if (diff == 0)
                for (int i = 0; i < val.Length; ++i)
                    val[i] *= val[i] * min;
            else
            {
                //val = min + (val-0.1)*(max-min)/0.8
                diff *= 1.25;
                for (int i = 0; i < val.Length; ++i)
                    val[i] = min + (val[i]-0.1)*diff;
            }
        }
    }
}
