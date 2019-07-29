//*****************************************************************************************
//*                                                                                       *
//* This is an auto-generated file by Microsoft ML.NET CLI (Command-Line Interface) tool. *
//*                                                                                       *
//*****************************************************************************************

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.ML;
using SampleBinaryClassification.Model.DataModels;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SampleBinaryClassification.ConsoleApp
{
    class Program
    {
        //Machine Learning model to load and use for predictions
        private const string MODEL_FILEPATH = @"MLModel.zip";

        //Dataset to use for trainning 
        private const string DATA_FILEPATH = @"C:\Users\evsro\Documents\source\repos\titanic-mlnet\titanic\train.csv";

        //Dataset to use for tests 
        private const string TEST_FILEPATH = @"C:\Users\evsro\Documents\source\repos\titanic-mlnet\titanic\test.csv";

        // File with results of the predictions
        private const string OUTPUT_FILEPATH = @"C:\Users\evsro\Documents\source\repos\titanic-mlnet\titanic\gender_submission_mrs.csv";

        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext();

            // Training code used by ML.NET CLI and AutoML to generate the model
            //ModelBuilder.CreateModel();

            ITransformer mlModel = mlContext.Model.Load(GetAbsolutePath(MODEL_FILEPATH), out DataViewSchema inputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            List<string> result_lines = new List<string>();
            result_lines.Add("PassengerId,Survived");

            using (StreamReader sr = new StreamReader(TEST_FILEPATH))
            {
                Console.WriteLine("PassengerId,Survived");
                String line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = CSVParser.Split(line);
                    i++;
                    if(i > 1)
                    {
                        string PassengerId = parts[0];
                        string Pclass = parts[1];
                        string Name = parts[2];
                        string Sex = parts[3];
                        string Age = parts[4];
                        string SibSp = parts[5];
                        string Parch = parts[6];
                        string Ticket = parts[7];
                        string Fare = parts[8];
                        string Cabin = parts[9];
                        string Embarked = parts[10];

                        var input = new ModelInput();
                        input.PassengerId = float.Parse(PassengerId, CultureInfo.InvariantCulture.NumberFormat);
                        input.Name = Name;
                        input.Sex = Sex;
                        input.Ticket = Ticket;
                        input.Cabin = Cabin;
                        input.Embarked = Embarked;

                        if (Age.Length> 0)
                        {
                            input.Age = float.Parse(Age, CultureInfo.InvariantCulture.NumberFormat);
                        } else
                        {
                            input.Age = 0;
                        }

                        if (Fare.Length > 0)
                        {
                            input.Fare = float.Parse(Fare, CultureInfo.InvariantCulture.NumberFormat);
                        }
                        else
                        {
                            input.Fare = 0;
                        }

                        if (SibSp.Length > 0)
                        {
                            input.SibSp = float.Parse(SibSp, CultureInfo.InvariantCulture.NumberFormat);
                        }
                        else
                        {
                            input.SibSp = 0;
                        }

                        if (Parch.Length > 0)
                        {
                            input.Parch = float.Parse(Parch, CultureInfo.InvariantCulture.NumberFormat);
                        }
                        else
                        {
                            input.Parch = 0;
                        }

                        // Predict using input data.
                        ModelOutput result = predEngine.Predict(input);
                        Console.Write($"{PassengerId},");
                        string str = $"{PassengerId},";
                        if(result.Prediction)
                        {
                            Console.WriteLine("1");
                            str += "1";
                        } else
                        {
                            Console.WriteLine("0");
                            str += "0";
                        }
                        result_lines.Add(str);
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(OUTPUT_FILEPATH))
            {

                foreach (string s in result_lines)
                {
                    sw.WriteLine(s);
                }
            }

            Console.ReadKey();
        }

        
        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
    }
}
