using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace fiveWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateCombinationsButton_Click(object sender, RoutedEventArgs e)
        {
            string inputFilePath = inputFilePathTextBox.Text;
            string outputFilePath = outputFilePathTextBox.Text;

            List<string> words = ReadWordsFromFile(inputFilePath);
            words = FilterWords(words);

            if (words.Count < 5)
            {
                statusTextBlock.Text = "There are not enough valid 5-letter words in the file to form combinations.";
                return;
            }

            statusTextBlock.Text = $"Valid words count: {words.Count} (out of {words.Count})";
            statusTextBlock.Text += "\nGenerating and printing possible combinations with real words...";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Perform combination generation here
            var combinations = GenerateWordCombinations(words, 5);

            // Display the results
            statusTextBlock.Text += $"\nTotal combinations generated: {combinations.Count}";
            statusTextBlock.Text += $"\nElapsed time: {stopwatch.ElapsedMilliseconds} ms";
        }

        public static List<string> ReadWordsFromFile(string filePath)
        {
            List<string> words = new List<string>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string word;
                while ((word = reader.ReadLine()) != null)
                {
                    words.Add(word);
                }
            }
            return words;
        }

        public List<string> FilterWords(List<string> words)
        {
            return words
                .Where(word => word.Length == 5 && word.Distinct().Count() == 5)
                .GroupBy(word => string.Concat(word.OrderBy(c => c)))
                .Select(group => group.First()) // Remove anagrams
                .ToList();
        }

        // Implement combination generation methods
        private List<List<string>> GenerateWordCombinations(List<string> words, int combinationLength)
        {
            List<List<string>> result = new List<List<string>>();
            GenerateCombinationsHelper(words, combinationLength, new List<string>(), result);
            return result;
        }

        private void GenerateCombinationsHelper(List<string> words, int combinationLength, List<string> currentCombination, List<List<string>> result)
        {
            if (currentCombination.Count == combinationLength)
            {
                result.Add(new List<string>(currentCombination));
                return;
            }

            for (int i = 0; i < words.Count; i++)
            {
                string word = words[i];

                if (!currentCombination.Contains(word))
                {
                    currentCombination.Add(word);
                    GenerateCombinationsHelper(words, combinationLength, currentCombination, result);
                    currentCombination.RemoveAt(currentCombination.Count - 1);
                }
            }
        }
    }
}
