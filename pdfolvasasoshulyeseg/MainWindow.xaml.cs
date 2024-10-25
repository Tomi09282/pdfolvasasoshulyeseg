using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.Win32;
using pdfolvasasoshulyeseg;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace pdfolvasasoshulyeseg
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Load File button event handler
        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            if (openFileDialog.ShowDialog() == true)
            {
                string pdfPath = openFileDialog.FileName;
                List<string> pdfContent = ReadPdfIntoList(pdfPath);
                pageCountLabel.Content = $"Pages: {pdfContent.Count}";

                // Print PDF content for debugging
                Console.WriteLine("PDF Content:");
                foreach (var page in pdfContent)
                {
                    Console.WriteLine(page);
                }

                // Extract errors from PDF content
                List<Error> errors = ExtractErrors(pdfContent);

                // Check if any errors were extracted
                if (errors.Count > 0)
                {
                    errorDataGrid.ItemsSource = errors;
                }
                else
                {
                    MessageBox.Show("No errors found in the PDF.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private List<string> ReadPdfIntoList(string pdfPath)
        {
            var pdfContent = new List<string>();
            using (var pdfReader = new PdfReader(pdfPath))
            using (var pdfDocument = new PdfDocument(pdfReader))
            {
                for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                {
                    string pageText = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(i));
                    pdfContent.Add(pageText);
                }
            }
            return pdfContent;
        }

        private List<Error> ExtractErrors(List<string> pdfContent)
        {
            List<Error> errors = new List<Error>();

            // Updated regex pattern to match the error format you provided
            string errorPattern = @"(?<Id>\d+)\s*-\s*(?<ErrorName>.+?)\s*Synopsis\s*(?<Synopsis>.+?)\s*Description\s*(?<Description>.+?)\s*See Also\s*(?<SeeAlso>.+?)(?=\s*Solution|$)\s*Solution\s*(?<Solution>.+?)(?=\s*Risk Factor|$)\s*Risk Factor\s*(?<RiskFactor>.+?)(?=\s*Plugin Information|$)\s*Plugin Information\s*(?<PluginInformation>.+?)(?=\s*Plugin Output|$)\s*Plugin Output\s*(?<PluginOutput>.+?)(?=\s*$)";

            foreach (var pageContent in pdfContent)
            {
                var matches = Regex.Matches(pageContent, errorPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);

                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        var error = new Error
                        {
                            Id = match.Groups["Id"].Value.Trim(),
                            ErrorName = match.Groups["ErrorName"].Value.Trim(),
                            Synopsis = match.Groups["Synopsis"].Value.Trim(),
                            Description = match.Groups["Description"].Value.Trim(),
                            SeeAlso = match.Groups["SeeAlso"].Value.Trim(),
                            Solution = match.Groups["Solution"].Value.Trim(),
                            RiskFactor = match.Groups["RiskFactor"].Value.Trim(),
                            CVSSv3BaseScore = string.Empty, // Not present in the example
                            CVSSv3TemporalScore = string.Empty, // Not present in the example
                            CVSSBaseScore = string.Empty, // Not present in the example
                            CVSSTemporalScore = string.Empty, // Not present in the example
                            References = string.Empty, // Not present in the example
                            PluginInformation = match.Groups["PluginInformation"].Value.Trim(),
                            PluginOutput = match.Groups["PluginOutput"].Value.Trim(),
                        };
                        errors.Add(error);
                    }
                }
            }
            return errors;
        }
    }
}