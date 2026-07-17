using System;
using System.Data;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AMS.Services;

namespace AMS.Services
{
    public class ReportService
    {
        private static ReportService _instance;
        public static ReportService Instance => _instance ?? (_instance = new ReportService());
        private ReportService() { }

        public FlowDocument BuildReport(string title, string companyName, DataTable data)
        {
            var doc = new FlowDocument
            {
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 11,
                PagePadding = new Thickness(40),
                PageWidth = 850
            };

            // Header
            var header = new Paragraph(new Run(companyName))
            {
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromRgb(15, 23, 42))
            };
            doc.Blocks.Add(header);

            var subtitle = new Paragraph(new Run(title))
            {
                FontSize = 13,
                TextAlignment = TextAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromRgb(56, 189, 248))
            };
            doc.Blocks.Add(subtitle);

            var dateLine = new Paragraph(new Run($"Generated: {DateTime.Now:dd MMM yyyy HH:mm}"))
            {
                FontSize = 10,
                TextAlignment = TextAlignment.Right,
                Foreground = Brushes.Gray
            };
            doc.Blocks.Add(dateLine);

            if (data == null || data.Rows.Count == 0)
            {
                doc.Blocks.Add(new Paragraph(new Run("No records found for the selected criteria.")) { Foreground = Brushes.Gray });
                return doc;
            }

            // Table
            var table = new Table { CellSpacing = 0, BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(0.5) };
            int colCount = data.Columns.Count;
            for (int i = 0; i < colCount; i++)
                table.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });

            // Header row
            var headerGroup = new TableRowGroup();
            var headerRow = new TableRow { Background = new SolidColorBrush(Color.FromRgb(15, 23, 42)) };
            foreach (DataColumn col in data.Columns)
            {
                var cell = new TableCell(new Paragraph(new Bold(new Run(col.ColumnName))))
                {
                    Padding = new Thickness(6, 4, 6, 4),
                    Foreground = new SolidColorBrush(Color.FromRgb(56, 189, 248))
                };
                headerRow.Cells.Add(cell);
            }
            headerGroup.Rows.Add(headerRow);
            table.RowGroups.Add(headerGroup);

            // Data rows
            var dataGroup = new TableRowGroup();
            bool alt = false;
            double total = 0;
            int amountCol = -1;
            bool isRunningStatement = title.StartsWith("Account Statement");
            bool isProfitBreakdown = title == "Profit Breakdown";
            if (!isRunningStatement && !isProfitBreakdown)
                for (int c = 0; c < data.Columns.Count; c++)
                    if (data.Columns[c].ColumnName.Contains("Amount") || data.Columns[c].ColumnName == "Price" ||
                        data.Columns[c].ColumnName == "Current" || data.Columns[c].ColumnName == "Balance")
                        amountCol = c;

            foreach (DataRow row in data.Rows)
            {
                var tr = new TableRow { Background = alt ? new SolidColorBrush(Color.FromArgb(30, 56, 189, 248)) : Brushes.White };
                for (int c = 0; c < colCount; c++)
                {
                    object raw = row[c];
                    Type colType = data.Columns[c].DataType;
                    string text;
                    if (raw == null || raw == DBNull.Value)
                    {
                        text = "";
                    }
                    else if (colType == typeof(DateTime))
                    {
                        text = ((DateTime)raw).ToString("dd MMM yyyy");
                    }
                    else if (colType == typeof(double) || colType == typeof(decimal) || colType == typeof(float))
                    {
                        double num = Convert.ToDouble(raw);
                        text = num.ToString("N2");
                        if (c == amountCol) total += num;
                    }
                    else if (colType == typeof(int) || colType == typeof(long) || colType == typeof(short))
                    {
                        text = Convert.ToInt64(raw).ToString("N0");
                    }
                    else
                    {
                        text = raw.ToString();
                    }
                    var cell = new TableCell(new Paragraph(new Run(text))) { Padding = new Thickness(6, 3, 6, 3) };
                    if (c == amountCol) cell.TextAlignment = TextAlignment.Right;
                    tr.Cells.Add(cell);
                }
                dataGroup.Rows.Add(tr);
                alt = !alt;
            }
            table.RowGroups.Add(dataGroup);

            // Totals row
            if (amountCol >= 0)
            {
                var totalGroup = new TableRowGroup();
                var totalRow = new TableRow { Background = new SolidColorBrush(Color.FromRgb(15, 23, 42)) };
                for (int c = 0; c < colCount; c++)
                {
                    string text = c == 0 ? "TOTAL" : c == amountCol ? total.ToString("N2") : "";
                    var cell = new TableCell(new Paragraph(new Bold(new Run(text))))
                    {
                        Padding = new Thickness(6, 4, 6, 4),
                        Foreground = Brushes.White
                    };
                    if (c == amountCol) cell.TextAlignment = TextAlignment.Right;
                    totalRow.Cells.Add(cell);
                }
                totalGroup.Rows.Add(totalRow);
                table.RowGroups.Add(totalGroup);
            }

            doc.Blocks.Add(table);
            return doc;
        }
    }
}
