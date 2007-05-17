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
        private ChooseLearningDataForm chooseLearningDataForm = new ChooseLearningDataForm();

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
            elmanNet = new ElmansNetwork(5, 2, 1, Properties.Settings.Default.useTechnicalAnalysis);
            double[] learn = new double[] { 1, 2, 3, 4, 5 };
            double[] correct = new double[] { 6 };
            //double[] test = new double[] { 7, 7, 7, 7, 7 };
            double[] test = new double[] { 1, 2, 3, 4, 5 };
            double oMin, oMax;
            this.data.Normalize(ref learn, ref correct, out oMin, out oMax);
            for(int i=0;i<100;++i)
            {   elmanNet.Learn(learn, correct);
                }
            learn = new double[] { 6, 7, 8, 9, 10 };
            correct = new double[] { 11 };
            this.data.Normalize(ref learn, ref correct, out oMin, out oMax);
            //elmanNet.Learn(learn, correct);
            this.data.Normalize(ref test, out oMin, out oMax);
            double[] exit = elmanNet.ComputeExitValues(test);
           // MessageBox.Show(exit[0].ToString());
            this.data.DeNormalize(ref exit, oMin, oMax);
            this.exitValuesMatrixPreview.BuildControl(exit);
        }

        #region NetworkToolStripMenu
        private void newNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.data != null)
                this.data.ClearData();
            this.dataGraph1.ClearData();
            this.elmanNet = null;
            this.elmanNetErrorPanel.Visible = false;
            this.exitValuesMatrixPreview.Visible = false;
        }

        private void loadNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO
            this.elmanNet = this.data.LoadNetwork();
            this.elmanNetErrorPanel.Visible = false;
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
                MessageBox.Show("Brak sieci zapisanej w programie!");
            }
        }

        private void learnNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
//TODO
            if (data == null || data.Count == 0)
            {
                MessageBox.Show("Wczytaj zestaw ucz¹cy");
                return;
            }

            this.elmanNet = this.data.Learn(this.elmanNet);
            if (data != null && elmanNet != null && data.Count >= elmanNet.NumberOfEntryNeurons)
            {
                this.performEstimationToolStripMenuItem.Enabled = true;
                double error = elmanNet.CountError();
                MessageBox.Show("Sieæ nauczona. B³¹d œredniokwadratowy: " + error);
                this.elmanNetErrorTextBox.Text = error.ToString();
                this.elmanNetErrorPanel.Visible = true;
            }
        }

        private void shrinkLearninDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (data == null || data.Count == 0)
            {
                MessageBox.Show("Wczytaj zestaw ucz¹cy");
                return;
            }

            this.chooseLearningDataForm.LoadData(data.Data);
            if (chooseLearningDataForm.ShowDialog() == DialogResult.OK)
            {
                int initialDateIndex = chooseLearningDataForm.InitialDateIndex;
                int learningDataSetSize = chooseLearningDataForm.LearningDataSetSize;
                
                List<DateTime> toRemove = new List<DateTime>();
                int counter = 0;
                foreach (DateTime dt in data.Data.Keys)
                {
                    if (counter < initialDateIndex || counter >= initialDateIndex + learningDataSetSize)
                        toRemove.Add(dt);
                    counter++;
                }

                data.RemoveData(toRemove);
                if (Properties.Settings.Default.entryLayerSize > data.Count)
                {
                    Properties.Settings.Default.entryLayerSize = data.Count;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void modifyNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (elmanNet == null)
            {
                MessageBox.Show("Nie ma nauczonej sieci!");
                return;
            }

            if (modifyNetworkForm.ShowDialog() == DialogResult.OK)
            {
                elmanNet.ModifyNumberOfHiddenNeurons(modifyNetworkForm.NewHiddenLayerSize,
                    modifyNetworkForm.SaveWeights,
                    modifyNetworkForm.SaveStatistics);

                if (!modifyNetworkForm.SaveStatistics)
                    elmanNetErrorPanel.Show();
            }
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
                
                this.dataGraph1.ClearData();
                List<double> v = this.data.ListDoubleData;
                v.AddRange(exit);
                this.dataGraph1.XValuesCounter = 15;
                this.dataGraph1.AddDataSeries(v.ToArray());

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

        private void garchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GarchSettingsForm g = new GarchSettingsForm();
            if (g.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Garch.GarchModel garch = new Garch.GarchModel(g.P, g.Q, g.Gamma);
                    garch.Theta = g.Theta;
                    double[] test = this.data.ListDoubleData.ToArray();
                    double min, max;
                    this.dataGraph1.ClearData();
                    List<double> v = new List<double>();
                    v.AddRange(test);
                    this.data.Normalize(ref test, out min, out max);
                    double[] exit = garch.ComputeGarchModel(test, Properties.Settings.Default.estimationTime);
                    this.data.DeNormalize(ref exit, min, max);

                    v.AddRange(exit);
                    this.dataGraph1.XValuesCounter = 15;
                    this.dataGraph1.AddDataSeries(v.ToArray());

                    this.exitValuesMatrixPreview.Visible = true;
                    this.dataGraph1.Refresh();
                }
                catch (Exception) { }

            }
        }

    }
}