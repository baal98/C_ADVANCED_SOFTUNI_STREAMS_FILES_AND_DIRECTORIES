using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;

namespace EvenLines
{

    public class EvenLines

    {
        static void Main(string[] args)
        {
            Even_Lines();
        }

        private static void ZipAndExtract()
        {
            string destinationFolder = @"C:\Users\baal_\OneDrive\Desktop";
            string targetFolder = @"C:\Exercises\C# ADVANCED\Streams, Files and Directories\Streams, Files and Directories\Even Lines\bin\Debug\net5.0\ExtractedFiles";
            ZipFile.CreateFromDirectory(destinationFolder, "zippedPng.zip");
            ZipFile.ExtractToDirectory(@"zippedPng.zip", targetFolder);
        }

        private static void DirectoryTraversal()
        {
            // can use REGEX or FileInfo Class:

            //string str = @"text.txt";
            //FileInfo fileInfo = new FileInfo(str);
            //string fileName = fileInfo.Extension;
            //long lentgh = fileInfo.Length;
            //long extension = fileInfo.Extesion;

            string[] report = Directory.GetFiles(".");
            Console.WriteLine(string.Join(Environment.NewLine, report));
            Regex regex = new Regex(@"(?:[\.]{1}[\\]{1})(?'fullFileName'(?'fileName'.*)(?:\.)(?'extension'[a-z]*))");

            MatchCollection matches = regex.Matches(string.Join(Environment.NewLine, report));

            Dictionary<string, Dictionary<string, double>> dicResult = new Dictionary<string, Dictionary<string, double>>();

            foreach (Match match in matches)
            {
                string fileName = match.Groups["fileName"].Value;
                string extension = match.Groups["extension"].Value;
                string fullFileName = match.Groups["fullFileName"].Value;

                double length = new System.IO.FileInfo(fullFileName).Length;

                if (!dicResult.ContainsKey(extension))
                {
                    dicResult.Add(extension, new Dictionary<string, double> { { fullFileName, length } });
                }
                else
                {
                    dicResult[extension].Add(fullFileName, length);
                }

            }

            var sortedDic = dicResult.OrderByDescending(x => x.Value.Count).ThenBy(x => x.Key);

            List<string> lines = new List<string>();

            foreach (var file in sortedDic)
            {
                //Console.WriteLine($".{file.Key}");
                lines.Add($".{file.Key}");
                foreach (var fileName in file.Value.OrderBy(x => x.Value))
                {
                    //Console.WriteLine($"--{fileName.Key} - {fileName.Value / 1000:F3}kb");
                    lines.Add($"--{fileName.Key} - {fileName.Value / 1000:F3}kb");
                }
            }
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/report.txt";

            File.AppendAllLines(path, lines);
        }

        private static void CopyBinaryFile()
        {
            using FileStream fileReader = new FileStream("copyMe.png", FileMode.Open);
            using FileStream fileWriter = new FileStream("copyMeCopy.png", FileMode.Create);

            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = fileReader.Read(buffer, 0, buffer.Length)) != 0)
            {
                fileWriter.Write(buffer, 0, bytesRead);
            }
        }

        private static void Word_Count()
        {
            string[] text = File.ReadAllLines("text.txt");

            string[] searchedWords = File.ReadAllLines("words.txt");

            Dictionary<string, int> dicSearchedWords = new Dictionary<string, int>();

            foreach (var word in searchedWords)
            {
                if (!dicSearchedWords.ContainsKey(word))
                {
                    dicSearchedWords.Add(word, 0);
                }
            }

            foreach (var line in text)
            {
                foreach (var word in dicSearchedWords)
                {
                    if (line.Contains(word.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        dicSearchedWords[word.Key]++;
                    }
                }

            }

            foreach (var word in dicSearchedWords.OrderByDescending(x => x.Value))
            {
                File.AppendAllText("actualResult.txt", $"{word.Key} - {word.Value}{Environment.NewLine}");
            }
        }

        private static void Line_Numbers()
        {
            string[] result = File.ReadAllLines("text.txt");

            for (int i = 0; i < result.Length; i++)
            {
                int letterConter = result[i].Count(symbol => Char.IsLetter(symbol));
                int punctuationCounter = result[i].Count(symbol => Char.IsPunctuation(symbol));

                File.AppendAllText("output.txt", $"Line {i + 1}: {result[i]} ({letterConter}) ({punctuationCounter}){Environment.NewLine}");

            }


        }

        private static void Even_Lines()
        {
            StreamReader streamReader = new StreamReader("text.txt");

            string result;

            string[] specialSymbols = new string[] { "-", ",", ".", "!", "?" };
            bool isEven = true;

            while ((result = streamReader.ReadLine()) != null)
            {
                if (!isEven)
                {
                    isEven = true;
                    continue;
                }
                foreach (string symbol in specialSymbols)
                {
                    result = result.Replace(symbol, "@");
                }

                Console.WriteLine(string.Join(" ", result.Split().Reverse()));
                isEven = false;
            }
        }
    }
}