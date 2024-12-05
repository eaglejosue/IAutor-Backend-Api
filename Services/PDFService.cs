using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;

namespace IAutor.Api.Services;

public interface IPDFService
{
    PdfFileInfo GenerateBookPDF(Book book, List<Chapter> chapters);
}

public sealed class PDFService(
    IAutorDb db,
    INotificationService notification) : IPDFService
{
    public PdfFileInfo GenerateBookPDF(Book book, List<Chapter> chapters)
    {
        var document = Document.Create(c =>
        {
            // Capa
            c.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.Red.Darken4);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Content()
                    .Text(book.Title)
                    .FontSize(32);
            });

            // Capítulos
            foreach (var (chapter, question) in
            from chapter in chapters
            from question in chapter.Questions
            where question.QuestionUserAnswers != null
            select (chapter, question))
            {
                var questionUserAnswer = question?.QuestionUserAnswers?.FirstOrDefault()?.Answer;
                if (string.IsNullOrEmpty(questionUserAnswer)) continue;

                c.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().AlignCenter().Text(chapter.Title).FontSize(18).FontColor(Colors.Black);
                    page.Header().AlignCenter().Text(question.Subject).SemiBold().FontSize(24).FontColor(Colors.Black);

                    //Tratar Img aqui
                    //TODO

                    page.Content().Text(questionUserAnswer).FontSize(14);

                    page.Footer().AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Página ");
                            x.CurrentPageNumber();
                        });
                });
            }
        });

        var pdfBytes = document.GeneratePdf();

        return new PdfFileInfo(pdfBytes, "application/pdf", $"{book.Title}.pdf");
    }
}
