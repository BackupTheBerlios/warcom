using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

namespace SONStock
{
    public partial class Form1 : Form
    {
        private ElmansNetwork elmanNet;
        private MatrixPreviewForm matrixPreviewForm;
        private DataLoader data = new DataLoader();

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
            //double[] test = new double[] { 7, 7, 7, 7, 7 };
            double[] test = new double[] { 1, 2, 3, 4, 5 };
            double oMin, oMax;
            this.data.Normalize(ref learn, ref correct, out oMin, out oMax);
            //for(int i=0;i<100;++i)
                elmanNet.Learn(learn, correct);
            learn = new double[] { 6, 7, 8, 9, 10 };
            correct = new double[] { 11 };
            this.data.Normalize(ref learn, ref correct, out oMin, out oMax);
            elmanNet.Learn(learn, correct);
            this.data.Normalize(ref test, out oMin, out oMax);
            double[] exit = elmanNet.ComputeExitValues(test);
            this.data.DeNormalize(ref exit, oMin, oMax);
            this.exitValuesMatrixPreview.BuildControl(exit);
        }

        #region NetworkToolStripMenu
        private void loadNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO
            this.elmanNet = this.data.LoadNetwork();
        }

        private void saveNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.elmanNet != null)
            {
                //TODO
                this.data.SaveNetwork(this.elmanNet);
            }
            else
            {
                MessageBox.Show("Brak sieci zapisanej w programie");
            }
        }

        private void learnNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
//TODO
            this.elmanNet = this.data.Learn(this.elmanNet);
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
            this.data.LoadDataFromFile(this.addDataToExistingToolStripMenuItem.Enabled);
//TODO
            if (this.data.Count > 0)
                this.addDataToExistingToolStripMenuItem.Enabled = true;
            if (elmanNet != null && data != null && data.Count >= elmanNet.NumberOfEntryNeurons)
            {
                this.performEstimationToolStripMenuItem.Enabled = true;
                this.UpdateDataGraph();
            }
        }

        private void loadDataFromManyFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.data.LoadDataFromFiles(this.addDataToExistingToolStripMenuItem.Enabled);
            if(this.data.Count>0)
                this.addDataToExistingToolStripMenuItem.Enabled = true;
            if (elmanNet != null && data.Count >= elmanNet.NumberOfEntryNeurons)
                this.performEstimationToolStripMenuItem.Enabled = true;
        }

        private void performEstimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortedDictionary<DateTime, double> d=null;
            if (this.data != null)
                d = this.data.Data;
            if (elmanNet != null && d != null && d.Count >= elmanNet.NumberOfEntryNeurons)
            {
                double[] values = new double[d.Count];
                int counter = 0;
                IEnumerator enumerator = d.Keys.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DateTime dt = (DateTime)enumerator.Current;
                    values[counter++] = d[dt];
                }
                
                double[] entryValues = new double[elmanNet.NumberOfEntryNeurons];
                for (int i = 0; i < entryValues.Length; i++)
                    entryValues[i] = values[values.Length - elmanNet.NumberOfEntryNeurons + i];
                
                double oMin, oMax;
                this.data.Normalize(ref entryValues, out oMin, out oMax);
                
                double[] exit = elmanNet.ComputeExitValues(entryValues);
                this.data.DeNormalize(ref exit, oMin, oMax);
                this.exitValuesMatrixPreview.BuildControl(exit);
                
                //this.dataGraph1.ClearData();
                //List<double> v = this.data.ListDoubleData;
                //v.AddRange(exit);
                
                //this.dataGraph1.AddDataSeries(v.ToArray());

                this.exitValuesMatrixPreview.Visible = true;
                this.dataGraph1.Refresh();
            }
        }

        private void clearDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            data.ClearData();
            this.performEstimationToolStripMenuItem.Enabled = false;
        }

        private void UpdateDataGraph()
        {
            double[] dataArray = this.data.ListDoubleData.ToArray();
            if (dataArray != null)
            {
                this.dataGraph1.AddDataSeries(dataArray);
                this.dataGraph1.Refresh();
            }

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