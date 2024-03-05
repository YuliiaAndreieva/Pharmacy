using Microsoft.AspNetCore.Identity;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace WepPha2.Models.DocumentModels
{
    public class InvoisePurchaseDocument : IDocument
    {
        private IEnumerable<Purchase> _purchases;

        public InvoisePurchaseDocument(IEnumerable<Purchase> purchases)
        {
            _purchases = purchases;
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
                column.Item().AlignCenter().Text("Reqistered Purchases").FontSize(20).SemiBold();
                column.Item().Element(ComposeTable);

                var totalSum = _purchases.Sum(x => x.UnitPurchasePrice);
                column.Item().PaddingRight(5).AlignRight().Text($"Total Price: {totalSum}").SemiBold();
                //if (!string.IsNullOrWhiteSpace(Model.Comments))
                //    column.Item().PaddingTop(25).Element(ComposeComments);
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
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn();
                    //columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Text("#");
                    header.Cell().Text("Purchase date").Style(headerStyle);
                    header.Cell().AlignRight().Text("By employee").Style(headerStyle);
                    header.Cell().AlignRight().Text("Price").Style(headerStyle);
                    //header.Cell().AlignRight().Text("ExpiryDate").Style(headerStyle);

                    header.Cell().ColumnSpan(4).PaddingTop(4).BorderBottom(1).BorderColor(Colors.Black);
                });

                var index = 1;

                foreach (var item in _purchases)
                {
                    table.Cell().Element(CellStyle).Text($"{index++}");
                    table.Cell().Element(CellStyle).Text(item.PurchaseDate);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Employee.FirstName} {item.Employee.LastName}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.UnitPurchasePrice}");
                    //table.Cell().Element(CellStyle).AlignRight().Text($"{item.}");
                    //table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price * item.Quantity:C}");

                    static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }

            });
        }
        
    }

    public class PurchaseListComponent : IComponent
    {
        private string Title { get; }
        private Purchase _purchase { get; }

        public PurchaseListComponent(string title, Purchase purchase)
        {
            Title = title;
            _purchase = purchase;
        }

        public void Compose(IContainer container)
        {
            container.ShowEntire().Column(column =>
            {
                column.Spacing(2);

                column.Item().PaddingBottom(4).LineHorizontal(1);

                column.Item().Text(_purchase.PurchaseDate);
                column.Item().Text(_purchase.Employee.LastName);
                column.Item().Text(_purchase.UnitPurchasePrice);

            });
        }
    }
    
    
}
