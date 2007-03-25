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
            if (hiddeNeuronIndex < 0 || hiddeNeuronIndex > (NumberOfHiddenNeurons - 1))
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
            for (int i = 0; i < NumberOfHiddenNeurons; ++i)
                val += hiddenValues[i] * hiddenExitWeights[i, exitNeuronIndex
                    ];
            return val;
        }

        public double[] ComputeExitValues(double[] entryValues)
        {
            if (entryValues.Length != this.entryValues.Length)
                throw new Exception("Z�y rozmiar danych wej�iowych: jest " + entryValues.Length + " powinno by� " +
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

        private double CountHiddenWeightsChange(int neuronIndex, int hiddenNeuronIndex, double[] prevVals,
            double[] nextVals)
        {
            double retVal = 0;
            for (int i = 0; i < numberOfExitNeurons; ++i)
            {
                double tempGlobalSum = 0;
                for (int j = 0; j < NumberOfHiddenNeurons; ++j)
                {
                    double tempLocalSum = (neuronIndex < numberOfEntryNeurons) ? 
                        delta + entryValues[neuronIndex] : 
                        delta + contextValues[neuronIndex - numberOfEntryNeurons];
                    for (int k = 0; k < NumberOfHiddenNeurons; ++k)
                    {
                        tempLocalSum += prevVals[neuronIndex] * ContextHiddenWeights[k, j];
                        nextVals[neuronIndex] = tempLocalSum;
                    }
                    tempGlobalSum += tempLocalSum * hiddenExitWeights[j, i];
                }
                retVal += errorValues[i] * tempGlobalSum;
            }
            return retVal;
        }

        private double[] ModifyEntryHiddenAndContextHiddenWeights(double[] prevVals)
        {
            double[] nextVals = new double[prevVals.Length];
            for (int i = 0; i < numberOfEntryNeurons; i++)
                for (int j = 0; j < NumberOfHiddenNeurons; j++)
                    entryHiddenWeights[i, j] -= CountHiddenWeightsChange(i, j, prevVals, nextVals);
            for (int i = 0; i < numberOfContextNeurons; ++i)
                for (int j = 0; j < NumberOfHiddenNeurons; ++j)
                    contextHiddenWeights[i, j] -= CountHiddenWeightsChange(i + numberOfEntryNeurons,
                        j, prevVals, nextVals);
            return nextVals;
        }

        private void ModifyHiddenExitWeights()
        {
            for (int i = 0; i < NumberOfHiddenNeurons; i++)
                for (int j = 0; j < numberOfExitNeurons; j++)
                    hiddenExitWeights[i, j] -= CountHiddenExitWeightsChange(i, j);
        }

        public void Learn(double[] entryValues, double[] correctExits)
        {
            if (entryValues.Length != this.entryValues.Length ||
                correctExits.Length != this.exitValues.Length)
                throw new Exception("Z�y rozmiar danych wej�ciowych lub danych wyj�iowych");
            this.numberOfLearningSets++;
            this.entryValues = entryValues;
            double error = 99999;
            double[] startVals = new double[numberOfEntryNeurons + numberOfContextNeurons];
            double[] temp;
            while (error > this.eps)
            {
                //wyliczenie wyj�cia warstwy ukrytej
                CountHiddenNeuronsExits();
                //przepisanie wyj�cia warstwy ukrytej na kontekstow�
                for (int i = 0; i < hiddenValues.Length; ++i)
                    contextValues[i] = hiddenValues[i];
                //wyliczenie wyj�cia uk�adu
                CountExitNeuronsExits();
                //okre�lenie wektora r�nicy wyj�cia wyliczonego oraz poprawnego, a tak�e wyliczenie normy r�nicy
                error = CountErrorsValues(correctExits);
                //modyfikacja wag mi�dy warstwami ukryt� a wyj�ciow�
                ModifyHiddenExitWeights();
                //modyfikacja wag mi�dy warstwami wej�ciow� a ukryt�
                temp = ModifyEntryHiddenAndContextHiddenWeights(startVals);
                for (int i = 0; i < temp.Length; i++)
                    startVals[i] = temp[i];
            }
        }

        #region Geters
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

        public int NumberOfHiddenNeurons
        {
            get { return numberOfHiddenNeurons; }
            //set { numberOfHiddenNeurons = value; }
        }

        public int NumberOfExitNeurons
        {
            get { return numberOfExitNeurons; }
        }

        //wsp�czynnik uczenia ??
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
