using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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
                    if (book.Type.HasValue && book.Type == BookType.Size210X297)
                        page.Size(PageSizes.A4);

                    if ((book?.Type ?? BookType.Size148X210) == BookType.Size148X210)
                        page.Size(PageSizes.A5);

                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(6));

                    page.Header()
                        .Column(c =>
                        {
                            c.Item().AlignCenter().Text(string.Concat("Capítulo", " ", chapter.ChapterNumber)).FontSize(10).FontColor(Colors.Black);
                            c.Item().AlignCenter().Text(question.Subject).SemiBold().FontSize(18).FontColor(Colors.Black);
                        });

                    //Tratar Img aqui
                    //TODO

                    page.Content().PaddingTop(20, Unit.Point).Text(questionUserAnswer).FontSize(10);

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
