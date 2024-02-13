using Microsoft.AspNetCore.Identity;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Net;
using WepPha2.Interfaces;
using WepPha2.ViewModels;

namespace WepPha2.Models.DocumentModels
{
    public class InvoiceMedicineDocument : IDocument
    {
        private IEnumerable<Medicine> _medicines;
     
        public InvoiceMedicineDocument(IEnumerable<Medicine> medicines)
        {
            _medicines = medicines;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);

                    //page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });
                });
        }
        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(20);
                column.Item().AlignCenter().Text("Available Medicine").FontSize(20).SemiBold();

                column.Item().Element(ComposeTable);

                var totalSum = _medicines.Sum(x => x.Quantity);
                column.Item().PaddingRight(5).AlignRight().Text($"Total count: {totalSum}").SemiBold();
            });
        }

        void ComposeTable(IContainer container)
        {
            var headerStyle = TextStyle.Default.SemiBold();


            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Text("#");
                    header.Cell().Text("Medicine").Style(headerStyle);
                    header.Cell().AlignRight().Text("Quantity").Style(headerStyle);
                    header.Cell().AlignRight().Text("UnitsInStock").Style(headerStyle);
                    header.Cell().AlignRight().Text("ExpiryDate").Style(headerStyle);

                    header.Cell().ColumnSpan(5).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
                });

                var index = 1;

                foreach (var item in _medicines)
                {
                    table.Cell().Element(CellStyle).Text($"{index++}");
                    table.Cell().Element(CellStyle).Text(item.MedicineName);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Quantity}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.UnitsInStock}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.ExpiryDate}");

                    static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }

            });
        }
    }

    public class MedicineListComponent : IComponent
    {
        private string Title { get; }
        private Medicine _medicine { get; }

        public MedicineListComponent(string title, Medicine medicine)
        {
            Title = title;
            _medicine = medicine;
        }

        public void Compose(IContainer container)
        {
            container.ShowEntire().Column(column =>
            {
                column.Spacing(2);

                column.Item().PaddingBottom(5).LineHorizontal(1);

                column.Item().Text(_medicine.MedicineName);
                column.Item().Text(_medicine.Quantity);
                column.Item().Text(_medicine.UnitsInStock);
                column.Item().Text(_medicine.ExpiryDate);
            });
        }
    }
}

