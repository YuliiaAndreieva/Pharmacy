using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace WepPha2.Models.DocumentModels
{
    public class InvoiceEmployeeDocument : IDocument
    {
        private IEnumerable<Employee> _employees;
        public InvoiceEmployeeDocument(IEnumerable<Employee> employees)
        {
            _employees = employees;
        }
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);
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
                column.Item().AlignCenter().Text("Employees").FontSize(20).SemiBold();

                column.Item().Element(ComposeTable);

                var totalSum = _employees.Count();
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
                    columns.ConstantColumn(20);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Text("#");
                    header.Cell().Text("Employee").Style(headerStyle);
                    header.Cell().AlignRight().Text("Phone").Style(headerStyle);
                    header.Cell().AlignRight().Text("Email").Style(headerStyle);
                    header.Cell().AlignRight().Text("Start Date").Style(headerStyle);

                    header.Cell().ColumnSpan(5).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
                });

                var index = 1;

                foreach (var item in _employees)
                {
                    table.Cell().Element(CellStyle).Text($"{index++}");
                    table.Cell().Element(CellStyle).Text($"{item.FirstName} {item.LastName}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Phone}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Email}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.StartDate}");

                    static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }

            });
        }
        
    }
    public class EmployeeListComponent : IComponent
    {
        private string Title { get; }
        private Employee _employee { get; }

        public EmployeeListComponent(string title, Employee employee)
        {
            Title = title;
            _employee = employee;
        }

        public void Compose(IContainer container)
        {
            container.ShowEntire().Column(column =>
            {
                column.Spacing(2);

                column.Item().PaddingBottom(5).LineHorizontal(1);

                column.Item().Text(_employee.FirstName + _employee.LastName);
                column.Item().Text(_employee.Phone);
                column.Item().Text(_employee.Email);
                column.Item().Text(_employee.StartDate);
            });
        }
    }
}
