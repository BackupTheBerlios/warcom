using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace SONStock
{
    [Serializable()]
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
        private int numberOfContextNeurons;
        private int numberOfHiddenNeurons;
        private int numberOfExitNeurons;
        private int numberOfLearningSets = 0;
        private double[] errorValues;
        private double eps = 0.1;
        private double ni = 0.1;
        private double delta = 0.1;

        public ElmansNetwork(int numberOfEntryNeurons,int numberOfHiddenNeurons, int numberOfExitNeurons)
        {
            entryValues = new double[numberOfEntryNeurons];
            contextValues = new double[numberOfHiddenNeurons];
            hiddenValues = new double[numberOfHiddenNeurons];
            exitValues = new double[numberOfExitNeurons];
            errorValues = new double[numberOfExitNeurons];
            this.numberOfExitNeurons = numberOfExitNeurons;
            this.numberOfHiddenNeurons = numberOfHiddenNeurons;
            this.numberOfEntryNeurons = numberOfEntryNeurons;
            this.numberOfContextNeurons = numberOfHiddenNeurons;
            Random rand = new Random();
            entryHiddenWeights = new double[this.numberOfEntryNeurons, this.numberOfHiddenNeurons];
            contextHiddenWeights = new double[this.numberOfContextNeurons, this.numberOfHiddenNeurons];
            hiddenExitWeights = new double[this.numberOfHiddenNeurons, this.numberOfExitNeurons];
            for (int i = 0; i < numberOfEntryNeurons; i++)
                for (int j = 0; j < numberOfHiddenNeurons; j++)
                    entryHiddenWeights[i, j] = rand.NextDouble();
            for (int i = 0; i < numberOfContextNeurons; i++)
                for (int j = 0; j < numberOfHiddenNeurons; j++)
                    contextHiddenWeights[i, j] = rand.NextDouble();
            for (int i = 0; i < numberOfHiddenNeurons; i++)
                for (int j = 0; j < numberOfExitNeurons; j++)
                    hiddenExitWeights[i, j] = rand.NextDouble();
        }

        private double CountInputSignalForHiddenNeuron(int hiddeNeuronIndex)
        {
            if (hiddeNeuronIndex < 0 || hiddeNeuronIndex > (numberOfHiddenNeurons - 1))
                throw new Exception("Invalid neuron index");
            double val = 0;
            for (int i = 0; i < numberOfEntryNeurons; ++i)
                val += entryValues[i] * entryHiddenWeights[i, hiddeNeuronIndex];
            for (int i = 0; i < numberOfContextNeurons; ++i)
                val += contextValues[i] * contextHiddenWeights[i, hiddeNeuronIndex];
            return val;
        }

        private double CountInputSignalForExitNeuron(int exitNeuronIndex)
        {
            if (exitNeuronIndex < 0 || exitNeuronIndex > (numberOfExitNeurons - 1))
                throw new Exception("Invalid neuron index");
            double val = 0;
            for (int i = 0; i < numberOfHiddenNeurons; ++i)
                val += hiddenValues[i] * hiddenExitWeights[i, exitNeuronIndex
                    ];
            return val;
        }

        public double[] ComputeExitValues(double[] entryValues)
        {
            if (entryValues.Length != this.entryValues.Length)
                throw new Exception("Incorrect entry data set size: " + entryValues.Length + " powinno byæ " +
                    this.entryValues.Length);
            else
            {
                this.entryValues = entryValues;
                CountHiddenNeuronsExits();
                CountExitNeuronsExits();
                return exitValues;
            }
        }

        private double CountHiddenNeuronExit(double inputValue)
        {
            return inputValue;
        }

        private double CountExitNeuronExit(double inputValue)
        {
            return inputValue;
        }

        private void CountHiddenNeuronsExits()
        {
            for (int i = 0; i < hiddenValues.Length; ++i)
            {
                double inputSignal = CountInputSignalForHiddenNeuron(i);
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
            //wyliczenie normy
            double sum = 0;
            for (int i = 0; i < errorValues.Length; ++i)
                sum += errorValues[i] * errorValues[i];
            return Math.Sqrt(sum);
        }

        private double CountHiddenExitWeightsChange(int hiddenNeuronIndex, int exitNeuronIndex)
        {
            return -ni * errorValues[exitNeuronIndex] * hiddenValues[hiddenNeuronIndex];
        }

        private double CountEntryHiddenWeightsChange(int entryNeuronIndex, int hiddenNeuronIndex, double[] prevVals,
            out double[] nextVals)
        {
            double retVal = 0;
            nextVals = new double[prevVals.Length];
            for (int i = 0; i < numberOfExitNeurons; ++i)
            {
                double tempGlobalSum = 0;
                for (int j = 0; j < numberOfHiddenNeurons; ++j)
                {
                    double tempLocalSum = delta + entryValues[entryNeuronIndex];
                    for (int k = 0; k < numberOfHiddenNeurons; ++k)
                    {
                        tempLocalSum += prevVals[k] * ContextHiddenWeights[k, j];
                        nextVals[k] = tempLocalSum;
                    }
                    tempGlobalSum += tempLocalSum * hiddenExitWeights[j, i];
                }
                retVal += errorValues[i] * tempGlobalSum;
            }
            return retVal;
        }

        private double CountContextHiddenWeightsChange(int contextNeuronIndex, int hiddenNeuronIndex, double[] prevVals,
            out double[] nextVals)
        {
            double retVal = 0;
            nextVals = new double[prevVals.Length];
            for (int i = 0; i < numberOfExitNeurons; ++i)
            {
                double tempGlobalSum = 0;
                for (int j = 0; j < numberOfHiddenNeurons; ++j)
                {
                    double tempLocalSum = delta + contextValues[contextNeuronIndex];
                    for (int k = 0; k < numberOfHiddenNeurons; ++k)
                    {
                        tempLocalSum += prevVals[k] * ContextHiddenWeights[k, j];
                        nextVals[k] = tempLocalSum;
                    }
                    tempGlobalSum += tempLocalSum * hiddenExitWeights[j, i];
                }
                retVal += errorValues[i] * tempGlobalSum;
            }
            return retVal;
        }

        private double[] ModifyEntryHiddenWeights(double[] prevVals)
        {
            double[] nextVals = new double[prevVals.Length];
            for (int i = 0; i < numberOfEntryNeurons; i++)
                for (int j = 0; j < numberOfHiddenNeurons; j++)
                    entryHiddenWeights[i, j] -= CountEntryHiddenWeightsChange(i, j, prevVals, out nextVals);
            return nextVals;
        }

        private double[] ModifyContextHiddenWeights(double[] prevVals)
        {
            double[] nextVals = new double[prevVals.Length];
            for (int i = 0; i < numberOfContextNeurons; i++)
                for (int j = 0; j < numberOfHiddenNeurons; j++)
                    contextHiddenWeights[i, j] -= CountContextHiddenWeightsChange(i, j, prevVals, out nextVals);
            return nextVals;
        }

        private void ModifyHiddenExitWeights()
        {
            for (int i = 0; i < numberOfHiddenNeurons; i++)
                for (int j = 0; j < numberOfExitNeurons; j++)
                    hiddenExitWeights[i, j] -= CountHiddenExitWeightsChange(i, j);
        }

        public void Learn(double[] entryValues, double[] correctExits)
        {
            if (entryValues.Length != this.entryValues.Length ||
                correctExits.Length != this.exitValues.Length)
                throw new Exception("Z³y rozmiar danych wejœciowych lub danych wyjœiowych");
            this.numberOfLearningSets++;
            this.entryValues = entryValues;
            double error = 99999;
            double[] startVals = new double[numberOfHiddenNeurons];
            double[] temp;
            while (error > this.eps)
            {
                //wyliczenie wyjœcia warstwy ukrytej
                CountHiddenNeuronsExits();
                //przepisanie wyjœcia warstwy ukrytej na kontekstow¹
                for (int i = 0; i < hiddenValues.Length; ++i)
                    contextValues[i] = hiddenValues[i];
                //wyliczenie wyjœcia uk³adu
                CountExitNeuronsExits();
                //okreœlenie wektora ró¿nicy wyjœcia wyliczonego oraz poprawnego, a tak¿e wyliczenie normy ró¿nicy
                error = CountErrorsValues(correctExits);
                //modyfikacja wag miêdy warstwami ukryt¹ a wyjœciow¹
                ModifyHiddenExitWeights();
                //modyfikacja wag miêdy warstwami wejœciow¹ a ukryt¹
                temp = ModifyEntryHiddenWeights(startVals);
                for (int i = 0; i < temp.Length; i++)
                    startVals[i] = temp[i];
                //modyfikacja wag miêdy warstwami kontekstow¹ a ukryt¹
                temp = ModifyContextHiddenWeights(startVals);
                for (int i = 0; i < temp.Length; i++)
                    startVals[i] = temp[i];
            }
        }

        #region Accessors
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

        public int NumberOfEntryNeurons
        {
            get { return numberOfEntryNeurons; }
        }

        public int NumberOfExitNeurons
        {
            get { return numberOfExitNeurons; }
        }

        public int NumberOfHiddenNeurons
        {
            get { return numberOfHiddenNeurons; }
        }

        //wspó³czynnik uczenia ??
        public double Ni
        {
            get { return ni; }
            set { ni = value; }
        }

        //kolejny magiczny wspolczynnik
        public double Delta
        {
            get { return delta; }
            set { delta = value; }
        }
        #endregion
    }
}
