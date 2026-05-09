using System;
using System.Windows;
using System.IO; 
using CodeAnalyzer.Core;

namespace CodeAnalyzer.WPF
{
    public partial class MainWindow : Window
    {
        private ProgramHelper _helper = new ProgramHelper();

        private readonly string _historyFile = "history.txt";

        public MainWindow()
        {
            InitializeComponent();

            if (File.Exists(_historyFile))
            {
                OutputBox.Text = File.ReadAllText(_historyFile);
            }
        }

        private void BtnConvertToCSharp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OutputBox.Text += $"\n{_helper.ConvertToCSharp(InputCodeBox.Text)}\n";
            }
            catch (Exception ex)
            {
                OutputBox.Text += $"\nПомилка: {ex.Message}\n";
            }
        }

        private void BtnConvertToVB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OutputBox.Text += $"\n{_helper.ConvertToVB(InputCodeBox.Text)}\n";
            }
            catch (Exception ex)
            {
                OutputBox.Text += $"\nПомилка: {ex.Message}\n";
            }
        }

        // Demonstrates polymorphism and runtime interface resolution using a heterogeneous collection of converter implementations
        private void BtnRunArrayTest_Click(object sender, RoutedEventArgs e)
        {
            OutputBox.Clear();
            OutputBox.Text = "=== Демонстрація поліморфізму ===\n\n";

            // Collection of base-type references pointing to different implementations
            ProgramConverter[] converters = new ProgramConverter[]
            {
                new ProgramConverter(),
                new ProgramHelper()
            };

            foreach (var item in converters)
            {
                OutputBox.Text += $"[Об'єкт: {item.GetType().Name}]\n";

                // Runtime check for extended behavior 
                if (item is ICodeChecker checker)
                {
                    OutputBox.Text += "  -> Підтримує перевірку синтаксису\n";

                    bool isValid = checker.CodeCheckSyntax("var a = 5;", "C#");
                    OutputBox.Text += $"  -> Результат перевірки: {(isValid ? "OK" : "Error")}\n\n";
                }
                else
                {
                    OutputBox.Text += "  -> Підтримується лише конвертація\n\n";
                }
            }
        }

        // Save the data to a file before closing the program
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            File.WriteAllText(_historyFile, OutputBox.Text);
            base.OnClosing(e);
        }
    }
}