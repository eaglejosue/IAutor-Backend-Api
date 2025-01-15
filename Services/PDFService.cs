using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace IAutor.Api.Services;

public interface IPDFService
{
    Task<PdfFileInfo> GenerateBookPDFv1Async(Book book, List<Chapter> chapters, bool isPreview = true);
    Task<PdfFileInfo> GenerateBookPDFv2Async(Book book, List<Chapter> chapters, bool isPreview = true);
}

public sealed class PDFService(
    INotificationService notification,
    //IAzureBlobServiceClient azureBlobServiceClient//,
    IAmazonS3StorageManager amazonS3
    ) : IPDFService
{
    public async Task<PdfFileInfo> GenerateBookPDFv1Async(Book book, List<Chapter> chapters, bool isPreview = true)
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
                var questionUserAnswer = question?.QuestionUserAnswers?.FirstOrDefault();
                if (string.IsNullOrEmpty(questionUserAnswer?.Answer)) continue;

                c.Page(async page =>
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
                            c.Item().AlignCenter().Text(string.Concat("Capítulo", " ", chapter.ChapterNumber)).FontSize(14).FontColor(Colors.Black);
                            c.Item().AlignCenter().Text(question.Subject).SemiBold().FontSize(20).FontColor(Colors.Black);
                        });

                    page.Content().Column(c =>
                    {
                        if (!string.IsNullOrEmpty(questionUserAnswer.ImagePhotoUrl))
                        {
                            try
                            {
                                var fileName = questionUserAnswer.ImagePhotoUrl[questionUserAnswer.ImagePhotoUrl.LastIndexOf('/')..];
                                //var img = azureBlobServiceClient.DownloadFileBytesAsync(Folders.Photos, fileName);
                                var img = amazonS3.GetFileContainerAsync(Folders.Photos, fileName);

                                c.Item().AlignCenter().PaddingTop(20, Unit.Point).Height(200).Image(img.Result).WithCompressionQuality(ImageCompressionQuality.Best);

                                if (!string.IsNullOrEmpty(questionUserAnswer.ImagePhotoLabel))
                                    c.Item().AlignCenter().PaddingTop(5, Unit.Point).Text(questionUserAnswer.ImagePhotoLabel).FontSize(12);
                                
                                c.Item().PaddingTop(10, Unit.Point).Text(questionUserAnswer.Answer).FontSize(12);
                            }
                            catch (Exception ex)
                            {
                                notification.AddNotification(new Notification("PDF Img Error", ex.Message));
                            }
                        }
                        else
                        {
                            c.Item().PaddingTop(20, Unit.Point).Text(questionUserAnswer.Answer).FontSize(12);
                        }

                        if (isPreview)
                            c.Item().AlignCenter().PaddingTop(5, Unit.Point).Text("*** EM EDIÇÃO ***").FontSize(30);
                    });

                    page.Footer().AlignCenter().Text(x => x.CurrentPageNumber());
                });
            }
        });

        var pdfBytes = document.GeneratePdf();

        return new PdfFileInfo(pdfBytes, "application/pdf", $"{book.Title}.pdf");
    }

    public async Task<PdfFileInfo> GenerateBookPDFv2Async(Book book, List<Chapter> chapters, bool isPreview = true)
    {
        var document = Document.Create(c =>
        {
            foreach (var chapter in chapters)
            {
                var chapterQuestions = chapter.Questions
                    .Where(q => q.QuestionUserAnswers != null
                             && q.QuestionUserAnswers.Any(qa => !string.IsNullOrEmpty(qa.Answer)))
                    .ToList();

                if (chapterQuestions.Count == 0) continue;

                // Add chapter header only once
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
                        .Column(col =>
                        {
                            col.Item().AlignCenter().Text($"Capítulo {chapter.ChapterNumber}").FontSize(14).FontColor(Colors.Black);
                        });

                    page.Content().Column(col =>
                    {
                        var isFirstAnswer = true;

                        foreach (var question in chapterQuestions)
                        {
                            var questionUserAnswer = question?.QuestionUserAnswers?.FirstOrDefault();
                            if (string.IsNullOrEmpty(questionUserAnswer?.Answer)) continue;

                            if (!string.IsNullOrEmpty(questionUserAnswer.ImagePhotoUrl))
                            {
                                try
                                {
                                    var fileName = questionUserAnswer.ImagePhotoUrl[questionUserAnswer.ImagePhotoUrl.LastIndexOf('/')..].Remove(0, 1);
                                    var img = amazonS3.GetFileContainerAsync(string.Concat(Folders.Public,"/",Folders.Photos), fileName); 

                                    if (!isFirstAnswer)
                                        col.Item().PageBreak();

                                    col.Item().AlignCenter().Text(question!.Subject).SemiBold().FontSize(20).FontColor(Colors.Black);
                                    col.Item().PaddingTop(10, Unit.Point).AlignCenter().Height(150).Image(img.Result).WithCompressionQuality(ImageCompressionQuality.Best);

                                    if (!string.IsNullOrEmpty(questionUserAnswer.ImagePhotoLabel))
                                        col.Item().AlignCenter().PaddingTop(5, Unit.Point).Text(questionUserAnswer.ImagePhotoLabel).FontSize(12);

                                    col.Item().PaddingTop(10, Unit.Point).Text(questionUserAnswer.Answer).FontSize(12);
                                }
                                catch (Exception ex)
                                {
                                    notification.AddNotification(new Notification("PDF Img Error", ex.Message));
                                }
                            }
                            else
                            {
                                if (isFirstAnswer)
                                    col.Item().AlignCenter().Text(question!.Subject).SemiBold().FontSize(20).FontColor(Colors.Black);
                                else
                                    col.Item().PaddingTop(10, Unit.Point).AlignCenter().Text(question!.Subject).SemiBold().FontSize(20).FontColor(Colors.Black);

                                col.Item().PaddingTop(10, Unit.Point).Text(questionUserAnswer.Answer).FontSize(12);
                            }

                            isFirstAnswer = false;
                        }

                        //if (isPreview)
                        //    col.Item().AlignCenter().PaddingTop(5, Unit.Point).Text("*** EM EDIÇÃO ***").FontSize(30);
                    });

                    page.Footer().AlignCenter().Text(x => x.CurrentPageNumber());
                });
            }
        });
        var pdfBytes = document.GeneratePdf();
        return new PdfFileInfo(pdfBytes, "application/pdf", $"{book.Title}.pdf");
    }

}
