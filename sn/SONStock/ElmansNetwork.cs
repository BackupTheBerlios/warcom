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

        public ElmansNetwork(int numberOfEntryNeurons,int numberOfHiddenNeurons, int numberOfExitNeurons)
        {
            entryValues = new double[numberOfEntryNeurons + 1];
            contextValues = new double[numberOfHiddenNeurons];
            hiddenValues = new double[numberOfHiddenNeurons + 1];
            exitValues = new double[numberOfExitNeurons];
            errorValues = new double[numberOfExitNeurons];
            this.numberOfExitNeurons = numberOfExitNeurons;
            this.numberOfHiddenNeurons = numberOfHiddenNeurons;
            this.numberOfEntryNeurons = numberOfEntryNeurons;
            this.numberOfContextNeurons = numberOfHiddenNeurons;
            Random rand = new Random();
            entryHiddenWeights = new double[this.numberOfEntryNeurons + 1, this.numberOfHiddenNeurons];
            contextHiddenWeights = new double[this.numberOfContextNeurons, this.numberOfHiddenNeurons];
            hiddenExitWeights = new double[this.numberOfHiddenNeurons + 1, this.numberOfExitNeurons];
            entryValues[entryValues.Length - 1] = 1;
            hiddenValues[hiddenValues.Length - 1] = 1;
            for (int i = 0; i < numberOfEntryNeurons + 1; i++)
                for (int j = 0; j < numberOfHiddenNeurons; j++)
                    entryHiddenWeights[i, j] = rand.NextDouble();
            for (int i = 0; i < numberOfContextNeurons; i++)
                for (int j = 0; j < numberOfHiddenNeurons; j++)
                    contextHiddenWeights[i, j] = rand.NextDouble();
            for (int i = 0; i < numberOfHiddenNeurons + 1; i++)
                for (int j = 0; j < numberOfExitNeurons; j++)
                    hiddenExitWeights[i, j] = rand.NextDouble();
        }

        private double CountInputSignalForHiddenNeuron(int hiddeNeuronIndex)
        {
            if (hiddeNeuronIndex < 0 || hiddeNeuronIndex > (NumberOfHiddenNeurons - 1))
                throw new Exception("Invalid neuron index");
            double val = 0;
            for (int i = 0; i < numberOfEntryNeurons + 1; ++i)
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
            for (int i = 0; i < NumberOfHiddenNeurons + 1; ++i)
                val += hiddenValues[i] * hiddenExitWeights[i, exitNeuronIndex
                    ];
            return val;
        }

        public double[] ComputeExitValues(double[] entryValues)
        {
            if (entryValues.Length != this.entryValues.Length - 1)
                throw new Exception("Z³y rozmiar danych wejœiowych: jest " + entryValues.Length + " powinno byæ " +
                    this.entryValues.Length);
            else
            {
                for (int i = 0; i < entryValues.Length; ++i)
                    this.entryValues[i] = entryValues[i];
                CountHiddenNeuronsExits();
                CountExitNeuronsExits();
                return exitValues;
            }
        }

        public void ModifyNumberOfHiddenNeurons(int newNumber, bool saveWeights)
        {
            if (newNumber < 1)
                throw new Exception("Nieprawid³owa iloœæ neuronów");
            if (newNumber < this.numberOfHiddenNeurons)
            {
               // double[] tempHiddenValues = new double[newNumber]();
                //for (int i = 0; i < newNumber; ++i)
                    
                
                //TODO kiedys to skonczyc
            }
            if (!saveWeights)
            {
                Random rand = new Random();
                for (int i = 0; i < numberOfContextNeurons; i++)
                    for (int j = 0; j < numberOfHiddenNeurons; j++)
                        contextHiddenWeights[i, j] = rand.NextDouble();
                for (int i = 0; i < numberOfEntryNeurons; i++)
                    for (int j = 0; j < numberOfHiddenNeurons; j++)
                        entryHiddenWeights[i, j] = rand.NextDouble();
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
            for (int i = 0; i < this.numberOfHiddenNeurons; ++i)
            {
                double inputSignal = CountInputSignalForHiddenNeuron(i);
                if (inputSignal > 1)
                    inputSignal = 1;
                else if (inputSignal < 0)
                    inputSignal = 0;
                hiddenValues[i] = CountHiddenNeuronExit(inputSignal);
            }
        }

        private void CountExitNeuronsExits()
        {
            for (int i = 0; i < exitValues.Length; i++)
            {
                double inputSignal = CountInputSignalForExitNeuron(i);
                if (inputSignal > 1)
                    inputSignal = 1;
                else if (inputSignal < 0)
                    inputSignal = 0;
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
            return sum / 2.0;
        }

        private double CountHiddenExitWeightsChange(int hiddenNeuronIndex, int exitNeuronIndex)
        {
            return ni * errorValues[exitNeuronIndex] * hiddenValues[hiddenNeuronIndex];
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
                    double delta = ( j == hiddenNeuronIndex ) ? 1 : 0;
                    double tempLocalSum = (neuronIndex < numberOfEntryNeurons) ? 
                        delta * entryValues[neuronIndex] : 
                        delta * contextValues[neuronIndex - numberOfEntryNeurons];
                    for (int k = 0; k < NumberOfHiddenNeurons; ++k)
                        tempLocalSum += prevVals[k] * ContextHiddenWeights[k, j];
                    nextVals[neuronIndex] = tempLocalSum;
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
            if (entryValues.Length != this.entryValues.Length -1 ||
                correctExits.Length != this.exitValues.Length)
                throw new Exception("Z³y rozmiar danych wejœciowych lub danych wyjœiowych");
            this.numberOfLearningSets++;
            for (int i = 0; i < entryValues.Length; i++)
                this.entryValues[i] = entryValues[i];
            double error = 99999;
            double[] startVals = new double[numberOfEntryNeurons + numberOfContextNeurons];
            double[] temp;
            while (error > this.eps)
            {
                //wyliczenie wyjœcia warstwy ukrytej
                CountHiddenNeuronsExits();
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

        //wspó³czynnik uczenia ??
        public double Ni
        {
            get { return ni; }
            set { ni = value; }
        }
        #endregion
    }
}
