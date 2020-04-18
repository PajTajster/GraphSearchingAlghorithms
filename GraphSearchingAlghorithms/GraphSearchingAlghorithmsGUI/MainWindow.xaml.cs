using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using GraphSearchingAlghorithmsGUI.Enums;

namespace GraphSearchingAlghorithmsGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isAStarSelected = false;
        private IAlghorithm currentAlgorithm;
        private string currentAlgorithmName;

        private FileLoaderResult fileLoaderResult;

        private string outputFileName = string.Empty;
        private string outputFilePath = string.Empty;

        private Dictionary<string, double> algorithmsTimes = new Dictionary<string, double>();

        public MainWindow()
        {
            InitializeComponent();
            CurrentAlgorithmComboBox.ItemsSource = Enum.GetValues(typeof(AlghorithmTypes));
        }

        private void FileLoaderButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.DefaultExt = ".txt";
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";

            var result = openFileDialog.ShowDialog();

            if (result == true)
            {
                var filePath = openFileDialog.FileName;

                FileLoaderLabel.Content = $"Loading file...";

                FileLoader fileLoader = new FileLoader();

                fileLoaderResult = fileLoader.LoadFile(filePath, isAStarSelected);

                string labelText;

                if (!fileLoaderResult.IsLoadingSuccess)
                {
                    MessageBox.Show("An error occured while loading a file, try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    AlghorithmAButton.IsEnabled = false;
                    labelText = "Loading error";
                }
                else
                {
                    var message = $"Successfully loaded file {Path.GetFileName(filePath)}.\n" +
                        $"Amount of cities: {fileLoaderResult.CityAmount}, connections: {fileLoaderResult.ConnectionsAmount}\n" +
                        $"Time taken: {fileLoaderResult.LoadTime}ms";
                    MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    outputFileName = Path.GetFileName(filePath).Replace("in", "out");
                    outputFilePath = Path.Combine(Path.GetDirectoryName(filePath), outputFileName);

                    AlghorithmAButton.IsEnabled = true;

                    labelText = "Loading complete!";
                }

                algorithmsTimes.Clear();
                FileLoaderLabel.Content = labelText;
            }
        }

        private void AlghorithmAButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentAlgorithm == null)
            {
                MessageBox.Show("No algorithm choosen!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var result = currentAlgorithm.CalculateRoute(fileLoaderResult);
            stopwatch.Stop();

            File.WriteAllText(outputFilePath, result.ToString(), Encoding.UTF8);

            algorithmsTimes[currentAlgorithmName] = stopwatch.ElapsedMilliseconds;

            MessageBox.Show($"Data written to the {outputFileName}. Time taken: {stopwatch.ElapsedMilliseconds.ToString()}ms", "Result", MessageBoxButton.OK);
        }

        private void CurrentAlgorithmComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var choosenAlgorithm = Enum.Parse(typeof(AlghorithmTypes), CurrentAlgorithmComboBox.SelectedItem.ToString());

            switch (choosenAlgorithm)
            {
                case AlghorithmTypes.ModifiedBFS:
                    {
                        currentAlgorithm = new ModifiedBFS();
                        currentAlgorithmName = nameof(ModifiedBFS);
                        isAStarSelected = false;
                        break;
                    }
                case AlghorithmTypes.Dijkstra:
                    {
                        currentAlgorithm = new Dijkstra();
                        currentAlgorithmName = nameof(Dijkstra);
                        isAStarSelected = false;
                        break;
                    }
                case AlghorithmTypes.AStar:
                    {
                        currentAlgorithm = new AStar();
                        currentAlgorithmName = nameof(AStar);
                        isAStarSelected = true;
                        break;
                    }
                default:
                    break;
            }
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (algorithmsTimes.Count == 0)
            {
                MessageBox.Show("No calculation performed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var algorithmTime in algorithmsTimes)
            {
                stringBuilder.Append($"{algorithmTime.Key}: {algorithmTime.Value.ToString()}ms{Environment.NewLine}");
            }

            double timeDifference = 0;
            if (algorithmsTimes.ContainsKey(nameof(AStar)) && algorithmsTimes.ContainsKey(nameof(Dijkstra)))
            {
                timeDifference = Math.Abs(algorithmsTimes[nameof(AStar)] - algorithmsTimes[nameof(Dijkstra)]);
            }

            stringBuilder.Append($"Time Difference[Dijkstra and A*]: {timeDifference.ToString()}ms");

            MessageBox.Show($"Results:\n{stringBuilder.ToString()}", "Time Report", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
