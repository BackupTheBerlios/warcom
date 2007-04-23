using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace SONStock
{
    [Serializable]
    class ElmansNetwork
    {
        private double[] entryValues;
        private double[] contextValues;
        private double[] hiddenValues;
        private double[] exitValues;
        private double[,] entryHiddenWeights;
        private double[,] contextHiddenWeights;
        private double[,] hiddenExitWeights;
        private int numberOfEntryNeurons;
        private int realNumberOfEntryNeurons;
        private int numberOfContextNeurons;
        private int numberOfHiddenNeurons;
        private int numberOfExitNeurons;
        private int numberOfLearningSets = 0;
        private double[] errorValues;
        private double eps = 0.005;
        private double ni = 0.9;
        private int MAX_IT = 50000;
        private double delta = 0.1;
        private List<double> errors = new List<double>();
        private bool useTechnicalAnalysis = false;

        public ElmansNetwork(int numberOfEntryNeurons,int numberOfHiddenNeurons, int numberOfExitNeurons, bool useTechnicalAnalysis)
        {
            realNumberOfEntryNeurons = numberOfEntryNeurons + 1;
            if (useTechnicalAnalysis)
                realNumberOfEntryNeurons += 2;
            this.useTechnicalAnalysis = useTechnicalAnalysis;
            entryValues = new double[realNumberOfEntryNeurons];
            contextValues = new double[numberOfHiddenNeurons];
            hiddenValues = new double[numberOfHiddenNeurons + 1];
            exitValues = new double[numberOfExitNeurons];
            errorValues = new double[numberOfExitNeurons];
            this.numberOfExitNeurons = numberOfExitNeurons;
            this.numberOfHiddenNeurons = numberOfHiddenNeurons;
            this.numberOfEntryNeurons = numberOfEntryNeurons;
            this.numberOfContextNeurons = numberOfHiddenNeurons;
            Random rand = new Random();
            entryHiddenWeights = new double[this.realNumberOfEntryNeurons, this.numberOfHiddenNeurons];
            contextHiddenWeights = new double[this.numberOfContextNeurons, this.numberOfHiddenNeurons];
            hiddenExitWeights = new double[this.numberOfHiddenNeurons + 1, this.numberOfExitNeurons];
            entryValues[entryValues.Length - 1] = 1;
            hiddenValues[hiddenValues.Length - 1] = 1;
            for (int i = 0; i < realNumberOfEntryNeurons; i++)
                for (int j = 0; j < numberOfHiddenNeurons; j++)
                    entryHiddenWeights[i, j] = rand.NextDouble();
            for (int i = 0; i < numberOfContextNeurons; i++)
                for (int j = 0; j < numberOfHiddenNeurons; j++)
                    contextHiddenWeights[i, j] = rand.NextDouble();
            for (int i = 0; i < numberOfHiddenNeurons + 1; i++)
                for (int j = 0; j < numberOfExitNeurons; j++)
                    hiddenExitWeights[i, j] = rand.NextDouble();
        }

        private double CountInputSignalForHiddenNeuron(int hiddeNeuronIndex, bool learning)
        {
            if (hiddeNeuronIndex == numberOfHiddenNeurons)
                return 1;
            if (hiddeNeuronIndex < 0 || hiddeNeuronIndex > (NumberOfHiddenNeurons - 1))
                throw new Exception("Invalid neuron index");
            double val = 0;
            for (int i = 0; i < realNumberOfEntryNeurons; ++i)
                val += entryValues[i] * entryHiddenWeights[i, hiddeNeuronIndex];
            //if (learning)
            if (true)
                for (int i = 0; i < numberOfContextNeurons; ++i)
                    val += contextValues[i] * contextHiddenWeights[i, hiddeNeuronIndex];
            return val;
        }

        private double CountInputSignalForExitNeuron(int exitNeuronIndex)
        {
            if (exitNeuronIndex < 0 || exitNeuronIndex > (numberOfExitNeurons - 1))
                throw new Exception("Invalid neuron index");
            double val = 0;
            for (int i = 0; i < NumberOfHiddenNeurons + 1; ++i)
                val += hiddenValues[i] * hiddenExitWeights[i, exitNeuronIndex];
            return val;
        }

        public double[] ComputeExitValues(double[] entryValues)
        {
            if (entryValues.Length != this.numberOfEntryNeurons)
                throw new Exception("Z³y rozmiar danych wejœiowych: jest " + entryValues.Length + " powinno byæ " +
                    this.entryValues.Length);
            else
            {
                if (useTechnicalAnalysis)
                {
                    this.entryValues[entryValues.Length - 3] = CountSimpleAverage(entryValues);
                    this.entryValues[entryValues.Length - 2] = CountComplexAverage(entryValues);
                }
                for (int i = 0; i < entryValues.Length; ++i)
                    this.entryValues[i] = entryValues[i];
                CountHiddenNeuronsExits(false);
                CountExitNeuronsExits();
                return exitValues;
            }
        }

        public void ModifyNumberOfHiddenNeurons(int newNumber, bool saveWeights, bool saveStatistics)
        {
            int oldNumber = this.numberOfHiddenNeurons;
            if (newNumber < 1)
                throw new Exception("Nieprawid³owa iloœæ neuronów");
            if (newNumber <= this.numberOfHiddenNeurons)
            {
                this.hiddenValues[newNumber] = 1;
                this.numberOfHiddenNeurons = newNumber;
            }
            else
            {
                this.hiddenValues = new double[newNumber + 1];
                this.hiddenValues[newNumber] = 1;
                this.numberOfHiddenNeurons = newNumber;
                entryHiddenWeights = new double[this.realNumberOfEntryNeurons, this.numberOfHiddenNeurons];
                contextHiddenWeights = new double[this.numberOfContextNeurons, this.numberOfHiddenNeurons];
            }
            if (!saveWeights || newNumber > oldNumber)
                SetRandomWeights(0);
            if (!saveStatistics)
                CleanStatistics();
        }

        private void SetRandomWeights(int start)
        {
            Random rand = new Random();
            for (int i = 0; i < numberOfContextNeurons; i++)
                for (int j = start; j < numberOfHiddenNeurons; j++)
                    contextHiddenWeights[i, j] = rand.NextDouble();
            for (int i = 0; i < numberOfEntryNeurons; i++)
                for (int j = start; j < numberOfHiddenNeurons; j++)
                    entryHiddenWeights[i, j] = rand.NextDouble();
        }

        private void CleanStatistics()
        {
            this.numberOfLearningSets = 0;
            this.errors.Clear();
        }

        private double CountHiddenNeuronExit(double inputValue)
        {
            return 1 / (1 + Math.Exp(delta * inputValue));
        }

        private double CountHiddenNeuronExitPrim(double inputValue)
        {
            return -(delta * Math.Exp(delta * inputValue)) / Math.Pow((1 + Math.Exp(delta * inputValue)), 2);
        }

        private double CountExitNeuronExitPrim(double inputValue)
        {
            return -(delta * Math.Exp(delta * inputValue)) / Math.Pow((1 + Math.Exp(delta * inputValue)), 2);
        }

        private double CountExitNeuronExit(double inputValue)
        {
            return 1 / (1 + Math.Exp(delta * inputValue));
        }

        private void CountHiddenNeuronsExits(bool learning)
        {
            for (int i = 0; i < this.numberOfHiddenNeurons; ++i)
            {
                double inputSignal = CountInputSignalForHiddenNeuron(i, learning);
                hiddenValues[i] = CountHiddenNeuronExit(inputSignal);
            }
        }

        private void CountExitNeuronsExits()
        {
            for (int i = 0; i < exitValues.Length; i++)
            {
                double inputSignal = CountInputSignalForExitNeuron(i);
                exitValues[i] = CountExitNeuronExit(inputSignal);
            }
        }

        private double CountErrorsValues(double[] correctExits)
        {
            for (int i = 0; i < correctExits.Length; ++i)
                errorValues[i] = correctExits[i] - exitValues[i];
            double sum = 0;
            for (int i = 0; i < errorValues.Length; ++i)
                sum += errorValues[i] * errorValues[i];
            return sum / 2.0;
        }

        private double CountHiddenExitWeightsChange(int hiddenNeuronIndex, int exitNeuronIndex)
        {
            return ni * hiddenValues[hiddenNeuronIndex] * CountErrorForExit(exitNeuronIndex);
        }

        private double CountErrorForExit(int exitNeuronIndex)
        {
            double val = CountExitNeuronExitPrim(CountInputSignalForExitNeuron(exitNeuronIndex));
            return val * errorValues[exitNeuronIndex];
        }

        private double CountErrorForHidden(int hiddenNeuronIndex)
        {
            double error = 0;
            for (int i = 0; i < NumberOfExitNeurons; ++i)
                error += HiddenExitWeights[hiddenNeuronIndex, i] * CountErrorForExit(i);
            return error;
        }

        private double CountR(int k, int l, int j, double prev)
        {
            double input = CountInputSignalForHiddenNeuron(j, true);
            double fprim = CountHiddenNeuronExitPrim(input);
            double sum = 0;
            for (int i = 0; i < numberOfContextNeurons; i++)
                sum += ContextHiddenWeights[i, j] * prev;
            if (k != l)
                sum += (l < numberOfEntryNeurons) ? entryValues[l] : contextValues[l - numberOfEntryNeurons];
            return sum * fprim;           
        }

        private double CountHiddenWeightsChange(int neuronIndex, int hiddenNeuronIndex, double[, ,] prevVals,
            double[,,] nextVals)
        {
            double sum = 0;
            for (int j = 0; j < numberOfHiddenNeurons ; ++j)
            {
                double err = CountErrorForHidden(j);
                double temp = CountR(hiddenNeuronIndex, neuronIndex, j, prevVals[hiddenNeuronIndex, neuronIndex, j]);
                nextVals[hiddenNeuronIndex, neuronIndex, j] = temp;
                sum += err * temp;
            }
            return ni * sum;
        }

        private double[,,] ModifyEntryHiddenAndContextHiddenWeights(double[,,] prevVals)
        {
            double[, ,] nextVals = new double[numberOfHiddenNeurons + 1, numberOfEntryNeurons + numberOfContextNeurons, numberOfHiddenNeurons + 1];
            for (int i = 0; i < realNumberOfEntryNeurons; i++)
                for (int j = 0; j < NumberOfHiddenNeurons; j++)
                    entryHiddenWeights[i, j] += CountHiddenWeightsChange(i, j, prevVals, nextVals);
            for (int i = 0; i < numberOfContextNeurons; ++i)
                for (int j = 0; j < NumberOfHiddenNeurons; ++j)
                    contextHiddenWeights[i, j] += CountHiddenWeightsChange(i + numberOfEntryNeurons,
                        j, prevVals, nextVals);
            return nextVals;
        }

        private void ModifyHiddenExitWeights()
        {
            for (int i = 0; i < NumberOfHiddenNeurons + 1; i++)
                for (int j = 0; j < numberOfExitNeurons; j++)
                    hiddenExitWeights[i, j] += CountHiddenExitWeightsChange(i, j);
        }

        private double CountSimpleAverage(double[] entryValues)
        {
            double avg = 0;
            for (int i = 0; i < entryValues.Length; ++i)
                avg += entryValues[i];
            return avg / entryValues.Length;
        }
 
        private double CountComplexAverage(double[] entryValues)
        {
            double avg = 0;
            double k = 2.0 / ( entryValues.Length + 1);
            for(int i=0;i<entryValues.Length;++i)
            {
                avg = entryValues[i] * k + avg * (1 - k);
            }
            return avg;
        } 

        public void Learn(double[] entryValues, double[] correctExits)
        {
            if (entryValues.Length != this.entryValues.Length -1 ||
                correctExits.Length != this.exitValues.Length)
                throw new Exception("Z³y rozmiar danych wejœciowych lub danych wyjœciowych");
            
            if (useTechnicalAnalysis)
            {
                this.entryValues[entryValues.Length - 3] = CountSimpleAverage(entryValues);
                this.entryValues[entryValues.Length - 2] = CountComplexAverage(entryValues);
            }
            this.numberOfLearningSets++;
            for (int i = 0; i < entryValues.Length; i++)
                this.entryValues[i] = entryValues[i];
            double error = Int32.MaxValue;
            double[,,] startVals = new double[numberOfHiddenNeurons + 1,numberOfEntryNeurons + numberOfContextNeurons,numberOfHiddenNeurons + 1];
            double[,,] temp;
            int it = 0;
            while (error > this.eps && it < this.MAX_IT)
            {
                it++;
                //wyliczenie wyjœcia warstwy ukrytej
                CountHiddenNeuronsExits(true);
                //przepisanie wyjœcia warstwy ukrytej na kontekstow¹
                for (int i = 0; i < this.numberOfHiddenNeurons; ++i)
                    contextValues[i] = hiddenValues[i];
                //wyliczenie wyjœcia uk³adu
                CountExitNeuronsExits();
                //okreœlenie wektora ró¿nicy wyjœcia wyliczonego oraz poprawnego, a tak¿e wyliczenie normy ró¿nicy
                error = CountErrorsValues(correctExits);
                //modyfikacja wag miêdy warstwami ukryt¹ a wyjœciow¹
                ModifyHiddenExitWeights();
                //modyfikacja wag miêdy warstwami wejœciow¹ a ukryt¹
                temp = ModifyEntryHiddenAndContextHiddenWeights(startVals);
                for (int i = 0; i < numberOfHiddenNeurons + 1; ++i)
                    for(int k = 0; k< numberOfEntryNeurons + numberOfContextNeurons; ++k)
                        for (int j = 0; j < numberOfHiddenNeurons + 1; ++j)
                            startVals[i,k,j] = temp[i,k,j];
            }
            for (int i = 0; i < correctExits.Length; ++i)
                errors.Add(correctExits[i] - exitValues[i]);
        }

        public double CountError()
        {
            double error = 0;
            for (int i = 0; i < errors.Count; ++i)
                error += errors[i] * errors[i];
            return error / errors.Count;
        }

        #region Getters
        public double[,] EntryHiddenWeights
        {
            get { return entryHiddenWeights; }
        }

        public double[,] ContextHiddenWeights
        {
            get { return contextHiddenWeights; }
        }

        public double[,] HiddenExitWeights
        {
            get { return hiddenExitWeights; }
        }

        public double Eps
        {
            get { return eps; }
            set { eps = value; }
        }

        public int NumberOfLearningSets
        {
            get { return numberOfLearningSets; }
        }

        public int NumberOfHiddenNeurons
        {
            get { return numberOfHiddenNeurons; }
        }

        public int NumberOfEntryNeurons
        {
            get { return numberOfEntryNeurons; }
        }

        public int NumberOfExitNeurons
        {
            get { return numberOfExitNeurons; }
        }

        public double Ni
        {
            get { return ni; }
            set { ni = value; }
        }
        #endregion
    }
}
