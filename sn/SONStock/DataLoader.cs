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
                            //MessageBox.Show("pora�ka lub koniec danych:)");
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
            ofd.Title = "Wybierz plik z sieci�";
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
                    MessageBox.Show("Deserializacja zako�czy�a si� niepowodzeniem");
                }
            }
            return elmanNet;
        }

        public void SaveNetwork(ElmansNetwork elmanNet)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Pliki xml (*.xml)|*.xml";
            sfd.Title = "Wybierz plik z sieci�";
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
            ofd.Title = "Wybierz plik zawieraj�cy dane";
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
                    MessageBox.Show("Niestety nie znaleziono �adnych plik�w *.csv");
            }
        }

        public ElmansNetwork Learn(ElmansNetwork elmanNet)
        {
            if (data == null || data.Count == 0)
            {
                MessageBox.Show("Wczytaj zestaw ucz�cy");
                return elmanNet;
            }

            IEnumerator enumerator = data.Keys.GetEnumerator();
            double[] values = new double[data.Count];
            int i = 0;

            int entryLayerSize = Properties.Settings.Default.entryLayerSize;
            int exitLayerSize = Properties.Settings.Default.estimationTime;
            int hiddenLayerSize = Properties.Settings.Default.hiddenLayerSize;

            while (enumerator.MoveNext())
            {
                DateTime dt = (DateTime)enumerator.Current;
                values[i++] = data[dt];
            }

            if (elmanNet == null ||
                elmanNet.NumberOfEntryNeurons != entryLayerSize ||
                elmanNet.NumberOfExitNeurons != exitLayerSize)
                elmanNet = new ElmansNetwork(entryLayerSize, hiddenLayerSize, exitLayerSize);

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
                elmanNet.Learn(val, correct);
            }
            return elmanNet;
        }
    }
}