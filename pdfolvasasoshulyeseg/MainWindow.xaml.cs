using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Text.RegularExpressions;

namespace pdfolvasasoshulyeseg
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadReport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
                Title = "Select a Vulnerability Scan Report"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string reportContent = ReadPdf(openFileDialog.FileName);
                    var vulnerabilities = ParseReport(reportContent);
                    VulnerabilityDataGrid.ItemsSource = vulnerabilities;

                    // Debug output
                    DebugTextBox.Text = "Parsed Vulnerabilities:\n";
                    foreach (var vulnerability in vulnerabilities)
                    {
                        DebugTextBox.Text += $"{vulnerability.ID} - {vulnerability.Synopsis}\n";
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Error reading file: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                    DebugTextBox.Text += $"Error: {ex.Message}\n";
                }
            }
        }

        private string ReadPdf(string filePath)
        {
            StringBuilder text = new StringBuilder();

            using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(filePath)))
            {
                for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentPageText = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(i), strategy);
                    text.Append(currentPageText);
                }
            }

            return text.ToString();
        }

        private List<Vulnerability> ParseReport(string reportContent)
        {
            List<Vulnerability> vulnerabilities = new List<Vulnerability>();

            // Output the raw report content for debugging
            DebugTextBox.Text += "Raw Report Content:\n";
            DebugTextBox.Text += reportContent + "\n";
            DebugTextBox.Text += "-----------------------------------\n";

            // Regex pattern to match each vulnerability entry
            string vulnerabilityPattern = @"(?<ID>\d{5})\s+-\s+(?<Synopsis>.+?)\s+Synopsis\s+([\s\S]*?)Description\s+(?<Description>[\s\S]*?)See Also\s+(?<SeeAlso>[\s\S]*?)Solution\s+(?<Solution>[\s\S]*?)Risk Factor\s+(?<RiskFactor>.+?)\s+CVSS v3\.0 Base Score\s+(?<CVSS3BaseScore>[\d.]+).*?CVSS Base Score\s+(?<CVSSBaseScore>[\d.]+)";

            // Match the regex against the report content
            var matches = Regex.Matches(reportContent, vulnerabilityPattern, RegexOptions.Singleline);

            // Debug output for regex matches
            DebugTextBox.Text += $"Total Matches Found: {matches.Count}\n";

            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1) // Ensure we have captured groups
                {
                    vulnerabilities.Add(new Vulnerability
                    {
                        ID = match.Groups["ID"].Value.Trim(),
                        Synopsis = match.Groups["Synopsis"].Value.Trim(),
                        Description = match.Groups["Description"].Value.Trim(),
                        Solution = match.Groups["Solution"].Value.Trim(),
                        RiskFactor = match.Groups["RiskFactor"].Value.Trim(),
                        CVSS3BaseScore = match.Groups["CVSS3BaseScore"].Value.Trim(),
                        CVSSBaseScore = match.Groups["CVSSBaseScore"].Value.Trim(),
                        SeeAlso = match.Groups["SeeAlso"].Value.Trim()
                    });

                    // Debug output for each vulnerability
                    DebugTextBox.Text += $"ID: {match.Groups["ID"].Value.Trim()}\n";
                    DebugTextBox.Text += $"Synopsis: {match.Groups["Synopsis"].Value.Trim()}\n";
                    DebugTextBox.Text += $"Description: {match.Groups["Description"].Value.Trim()}\n";
                    DebugTextBox.Text += $"Solution: {match.Groups["Solution"].Value.Trim()}\n";
                    DebugTextBox.Text += $"Risk Factor: {match.Groups["RiskFactor"].Value.Trim()}\n";
                    DebugTextBox.Text += $"CVSS3 Base Score: {match.Groups["CVSS3BaseScore"].Value.Trim()}\n";
                    DebugTextBox.Text += $"CVSS Base Score: {match.Groups["CVSSBaseScore"].Value.Trim()}\n";
                    DebugTextBox.Text += $"See Also: {match.Groups["SeeAlso"].Value.Trim()}\n";
                    DebugTextBox.Text += "-----------------------------------\n";
                }
            }

            // Log unmatched lines or patterns
            string[] lines = reportContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (!Regex.IsMatch(line, vulnerabilityPattern))
                {
                    DebugTextBox.Text += $"Unmatched Line: {line}\n";
                }
            }

            return vulnerabilities;
        }
    }
public class Vulnerability
    {
        public string ID { get; set; }
        public string Synopsis { get; set; }
        public string Description { get; set; }
        public string Solution { get; set; }
        public string RiskFactor { get; set; }
        public string CVSS3BaseScore { get; set; }
        public string CVSSBaseScore { get; set; }
        public string SeeAlso { get; set; }
    }
}