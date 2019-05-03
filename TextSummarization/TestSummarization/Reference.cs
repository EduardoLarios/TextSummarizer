using System;
using System.Collections.Generic;
using System.Linq;

namespace PerceptronLab
{
    class Perceptron
    {
        // Perceptron properties
        public List<double> Inputs { get; set; } = new List<double>();
        public List<double> Weights { get; set; } = new List<double>() { 0.0, 0.0 };
        public double LearningRate { get; set; } = 0.1;

        public void AdjustWeights(double Error)
        {
            for (int i = 0; i < Weights.Count; i++)
            {
                Weights[i] += LearningRate * Error * Inputs[i];
            }
        }

        public double DotProduct()
        {
            var weightedSum = 0.0;
            for (int i = 0; i < Inputs.Count; i++)
            {
                weightedSum += (Inputs[i] * Weights[i]);
            }

            return weightedSum;
        }

        public int ActivationFunction(double weightedSum)
        {
            return (weightedSum > 0.5) ? 1 : 0;
        }
    }

    // class Program
    // {
    //     public static bool TrainNetwork(List<IEnumerable<double>> trainingInput, List<double> expectedValues, Perceptron perceptron)
    //     {
    //         var resultValues = new List<double>();
    //         var iterations = 0;

    //         while (iterations < 100)
    //         {
    //             int errorCounter = 0;

    //             for (int i = 0; i < trainingInput.Count; i++)
    //             {
    //                 perceptron.Inputs = trainingInput[i].ToList();

    //                 var weightSum = perceptron.DotProduct();
    //                 var result = perceptron.ActivationFunction(weightSum);
    //                 var error = expectedValues[i] - result;
    //                 resultValues.Add(result);

    //                 if (error != 0.0)
    //                 {
    //                     ++errorCounter;
    //                     perceptron.AdjustWeights(error);
    //                 }
    //             }

    //             if (errorCounter == 0) break;
    //             resultValues.Clear();
    //             ++iterations;
    //         }

    //         return (iterations < 100) ? true : false;
    //     }
    //     public static List<double> TestNetwork(IEnumerable<IEnumerable<double>> testInput, Perceptron perceptron)
    //     {
    //         var resultValues = new List<double>();

    //         foreach (var entry in testInput)
    //         {
    //             perceptron.Inputs = entry.ToList();

    //             var weightSum = perceptron.DotProduct();
    //             var result = perceptron.ActivationFunction(weightSum);
    //             resultValues.Add(result);
    //         }

    //         return resultValues;
    //     }
    //     static void Main(string[] args)
    //     {
    //         var rawInput = new List<string>();
    //         string line;

    //         while ((line = Console.ReadLine()) != null)
    //         {
    //             rawInput.Add(line);
    //         }

    //         int.TryParse(rawInput[0], out int numberInputs);
    //         int.TryParse(rawInput[1], out int trainingSize);
    //         int.TryParse(rawInput[2], out int testSize);

    //         // Gets the training set as an IEnumerable of string[] -> i.e ["1"], ["1"], ["1"]
    //         var rawTrainingSet =
    //             rawInput.Skip(3)
    //                     .Take(trainingSize)
    //                     .Select(s => s.Replace(" ", string.Empty).Split(','));

    //         // Parses the IEnumerable of string[] and converts it to an IEnumerable of 
    //         // doubles -> i.e { 1.0, 1.0, 1.0 } for each entry
    //         var trainingSet = rawTrainingSet.Select(a => a.Select(double.Parse));

    //         // Gets the training set as an IEnumerable of string[] -> i.e ["1"], ["1"]
    //         var rawTestSet =
    //             rawInput.Skip(trainingSize + 3)
    //                     .Select(s => s.Replace(" ", string.Empty).Split(','));

    //         // Parses the IEnumerable of string[] and converts it to an IEnumerable of 
    //         // doubles -> i.e { 1.0, 1.0 } for each entry
    //         var testSet = rawTestSet.Select(a => a.Select(double.Parse));

    //         //Console.WriteLine("Input:\n");
    //         //foreach (var entry in rawInput)
    //         //{
    //         //    Console.WriteLine(entry);
    //         //}

    //         var trainingInput = trainingSet.Select(entry => entry.Take(numberInputs)).ToList();
    //         var expectedValues = trainingSet.Select(entry => entry.Last()).ToList();
    //         var perceptron = new Perceptron();

    //         if (TrainNetwork(trainingInput, expectedValues, perceptron))
    //         {
    //             //Console.WriteLine("\n-- Training Succesful --");
    //             //Console.WriteLine("\nResults: ");
    //             var testResult = TestNetwork(testSet, perceptron);
    //             foreach (var result in testResult)
    //             {
    //                 Console.WriteLine(result);
    //             }
    //         }

    //         else
    //         {
    //             Console.WriteLine("no solution found");
    //         }

    //         Console.ReadLine();
    //     }
    // }
}
