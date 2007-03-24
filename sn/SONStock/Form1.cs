using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

namespace SONStock
{
    public partial class Form1 : Form
    {
        private SortedDictionary<DateTime, double> data = new SortedDictionary<DateTime, double>();
        private ElmansNetwork elmanNet;
        private MatrixPreviewForm matrixPreviewForm;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //NetworkTest();
        }

        private void NetworkTest()
        {
            elmanNet = new ElmansNetwork(5, 2, 1);
            double[] learn = new double[] { 1, 2, 3, 4, 5 };
            double[] correct = new double[] { 6 };
            double[] test = new double[] { 7, 8, 9, 10, 11 };
            elmanNet.Learn(learn, correct);
            double[] exit = elmanNet.ComputeExitValues(test);
            this.exitValuesMatrixPreview.BuildControl(exit);
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

        #region NetworkToolStripMenu
        private void loadNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

                    this.elmanNet = (ElmansNetwork)formatter.Deserialize(stream);
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
        }

        private void saveNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.elmanNet != null)
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
            else
            {
                MessageBox.Show("Brak sieci zapisanej w programie");
            }
        }

        private void learnNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (data == null || data.Count==0)
            {
                MessageBox.Show("Wczytaj zestaw ucz¹cy");
                return;
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

            if (data != null && elmanNet != null && data.Count >= elmanNet.NumberOfEntryNeurons)
                this.performEstimationToolStripMenuItem.Enabled = true;
        }

        private void networkMatrixPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (elmanNet == null)
            {
                MessageBox.Show("Nie ma nauczonej sieci!");
                return;
            }

            if (this.matrixPreviewForm == null)
                matrixPreviewForm = new MatrixPreviewForm(elmanNet.EntryHiddenWeights,
                    elmanNet.ContextHiddenWeights, elmanNet.HiddenExitWeights);

            matrixPreviewForm.ShowDialog();
        }
        #endregion

        #region EstimateToolStripMenu
        private void loadDataFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Pliki csv (*.csv)|*.csv";
            ofd.Title = "Wybierz plik zawieraj¹cy dane";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.AddNewDataFromFile(ofd.FileName, this.addDataToExistingToolStripMenuItem.Enabled);
                this.addDataToExistingToolStripMenuItem.Enabled = true;
                if(elmanNet != null && data != null && data.Count>=elmanNet.NumberOfEntryNeurons)
                    this.performEstimationToolStripMenuItem.Enabled = true;
            }
        }

        private void loadDataFromManyFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDirectoryDialog odd = new OpenDirectoryDialog();
            string directory;
            if ((directory = odd.GetFolder()) != null && directory != "")
            {
                string[] files = Directory.GetFiles(directory, "*.csv");
                if (files.Length > 0)
                {
                    if (!this.addDataToExistingToolStripMenuItem.Enabled)
                        this.data.Clear();
                    for (int i = 0; i < files.Length; ++i)
                    {
                        this.AddNewDataFromFile(files[i], false);
                    }
                    this.addDataToExistingToolStripMenuItem.Enabled = true;
                    if (data.Count >= elmanNet.NumberOfEntryNeurons)
                        this.performEstimationToolStripMenuItem.Enabled = true;
                }
                else
                    MessageBox.Show("Niestety nie znaleziono ¿adnych plików *.csv");
            }
        }

        private void performEstimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (elmanNet != null && data != null && data.Count >= elmanNet.NumberOfEntryNeurons)
            {
                double[] values = new double[data.Count];
                int counter = 0;
                IEnumerator enumerator = data.Keys.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DateTime dt = (DateTime)enumerator.Current;
                    values[counter++] = data[dt];
                }
                
                double[] entryValues = new double[elmanNet.NumberOfEntryNeurons];
                for (int i = 0; i < entryValues.Length; i++)
                    entryValues[i] = values[values.Length - elmanNet.NumberOfEntryNeurons + i];


                double[] exit = elmanNet.ComputeExitValues(entryValues);
                this.exitValuesMatrixPreview.BuildControl(exit);
            }
        }

        private void clearDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            data.Clear();
            this.performEstimationToolStripMenuItem.Enabled = false;
        }
        #endregion

        #region SettingsToolStripMenu
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //network parameters changed
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                elmanNet = null;
            }
        }
        #endregion

    }
}

#region Arch
/*private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "pliki csv (*.csv)|*.csv";
            ofd.Title = "Wybierz plik z danymi";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.AddNewDataFromFile(ofd.FileName, !this.checkBox1.Checked);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenDirectoryDialog odd = new OpenDirectoryDialog();
            string dir;
            if ((dir = odd.GetFolder()) != null && dir != "")
            {
                string[] files = Directory.GetFiles(dir, "*.csv");
                if (files.Length > 0)
                {
                    if (!this.checkBox1.Checked)
                        this.data.Clear();
                    for (int i = 0; i < files.Length; ++i)
                    {
                        this.AddNewDataFromFile(files[i], false);
                    }
                }
                else
                    MessageBox.Show("Niestety nie znaleziono ¿adnych plików *.csv");
                
                Learn();
            }
        }*/

/*
private void button3_Click(object sender, EventArgs e)
{
    if (this.elman != null)
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "pliki csv (*.xml)|*.xml";
        sfd.Title = "Wybierz plik z sieci¹";
        if (sfd.ShowDialog() == DialogResult.OK)
        {
            //Opens a file and serializes the object into it in binary format.
            Stream stream = File.Open(sfd.FileName, FileMode.Create);
            SoapFormatter formatter = new SoapFormatter();

            //BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, elman);
            stream.Close();
        }
    }
    else
    {
        MessageBox.Show("Brak sieci zapisanej w programie");
    }
}

private void button4_Click(object sender, EventArgs e)
{
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

            this.elman = (ElmansNetwork)formatter.Deserialize(stream);
            stream.Close();
        }
        catch (Exception)
        {
            MessageBox.Show("Deserializacja zakoñczy³a siê niepowodzeniem");
        }
    }
}
        private void Learn()
        {
            IEnumerator enumerator = data.Keys.GetEnumerator();
            double[] values = new double[data.Count];
            int i = 0, MAX_ENTRY = 10, MAX_EXIT = 1;
            while (enumerator.MoveNext())
            {
                DateTime dt = (DateTime)enumerator.Current;
                //MessageBox.Show(dt.ToString());
                values[i++] = data[dt];
            }

            ElmansNetwork en = new ElmansNetwork(10, 2, 1);
            double[] val = new double[MAX_ENTRY];
            double[] correct = new double[MAX_EXIT];
            for (i = 0; i < values.Length - 11; ++i)
            {
                for (int j = 0; j < MAX_ENTRY; ++j)
                {
                    val[j] = values[i + j];
                }
                for (int j = 0; j < MAX_EXIT; ++j)
                {
                    correct[j] = values[i + j + MAX_ENTRY];
                }
                en.Learn(val, correct);
            }
            double[] exit = en.ComputeExitValues(val);
            for (int j = 0; j < exit.Length; ++j)
                label1.Text = exit[j].ToString();
        }
 
 */

#endregion