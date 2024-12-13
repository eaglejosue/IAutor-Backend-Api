using IAutor.Api.Data.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace IAutor.Api.Services;

public interface IPDFService
{
    Task<PdfFileInfo> GenerateBookPDF(Book book, List<Chapter> chapters);
}

public sealed class PDFService(
    IAutorDb db,
    INotificationService notification, IAzureBlobServiceClient azureClient) : IPDFService
{
    public async Task<PdfFileInfo> GenerateBookPDF(Book book, List<Chapter> chapters)
    {
        var document = Document.Create(c =>
        {
            // Capítulos

            //var query = (from chapter in chapters
            //             from question in chapter.Questions
            //             where question.QuestionUserAnswers != null
            //             select question);

            //foreach(var question in query)
            //{
            //    var questionUserAnswer = question?.QuestionUserAnswers?.FirstOrDefault()?.Answer;
            //    if (string.IsNullOrEmpty(questionUserAnswer)) continue;

            //    var questionUser = question?.QuestionUserAnswers?.FirstOrDefault();
            //    c.Page(page =>
            //    {
            //        if (book.Type.HasValue && book.Type == BookType.Size210X297)
            //            page.Size(PageSizes.A4);

            //        if ((book?.Type ?? BookType.Size148X210) == BookType.Size148X210)
            //            page.Size(PageSizes.A5);

            //        page.Margin(1, Unit.Centimetre);
            //        page.PageColor(Colors.White);
            //        page.DefaultTextStyle(x => x.FontSize(5));

            //        page.Header()
            //            .Column(c =>
            //            {
            //                c.Item().AlignCenter().Text(string.Concat("Capítulo", " ", "XXXXXXX")).FontSize(8).FontColor(Colors.Black);
            //                c.Item().AlignCenter().Text(question.Subject).SemiBold().FontSize(18).FontColor(Colors.Black);
            //            });

            //        //Tratar Img aqui
            //        //TODO
                   
            //       // page.Content().PaddingTop(20, Unit.Point).Text(questionUserAnswer).FontSize(8);

            //        page.Content().Column(column =>
            //         {
            //             column.Item().Text(questionUserAnswer).FontSize(8);
            //             if (!string.IsNullOrEmpty(questionUser.ImagePhotoUrl))
            //             {
            //                 var fileImg = questionUser.ImagePhotoUrl.Substring(questionUser.ImagePhotoUrl.LastIndexOf('/'),
            //                     (questionUser.ImagePhotoUrl.Length) - questionUser.ImagePhotoUrl.LastIndexOf('/'));

            //                 var img = azureClient.DownloadFileBytesAsync("photos", fileImg);

            //                 column.Item().AlignCenter().PaddingTop(12).Height(200).Image(img.Result).WithCompressionQuality(ImageCompressionQuality.High) ;
            //             }
            //         });


            //        page.Footer().AlignCenter()
            //            .Text(x =>
            //            {
            //                x.CurrentPageNumber();
            //            });
            //    });
            //}


            foreach (var (chapter, question) in
            from chapter in chapters
            from question in chapter.Questions
            where question.QuestionUserAnswers != null
            select (chapter, question))
            {
                var questionUserAnswer = question?.QuestionUserAnswers?.FirstOrDefault()?.Answer;
                if (string.IsNullOrEmpty(questionUserAnswer)) continue;

                var questionUser = question?.QuestionUserAnswers?.FirstOrDefault();
                c.Page(page =>
                {
                    if (book.Type.HasValue && book.Type == BookType.Size210X297)
                        page.Size(PageSizes.A4);

                    if ((book?.Type ?? BookType.Size148X210) == BookType.Size148X210)
                        page.Size(PageSizes.A5);

                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(5));

                    page.Header()
                        .Column(c =>
                        {
                            c.Item().AlignCenter().Text(string.Concat("Capítulo", " ", chapter.ChapterNumber)).FontSize(8).FontColor(Colors.Black);
                            c.Item().AlignCenter().Text(question.Subject).SemiBold().FontSize(18).FontColor(Colors.Black);
                        });

                    //Tratar Img aqui
                    //TODO
                    page.Content().Column(column =>
                    {
                        column.Item().Text(questionUserAnswer).FontSize(8);
                        if (!string.IsNullOrEmpty(questionUser.ImagePhotoUrl))
                        {
                            var fileImg = questionUser.ImagePhotoUrl.Substring(questionUser.ImagePhotoUrl.LastIndexOf('/'),
                                (questionUser.ImagePhotoUrl.Length) - questionUser.ImagePhotoUrl.LastIndexOf('/'));

                            var img = azureClient.DownloadFileBytesAsync("photos", fileImg);

                            column.Item().AlignCenter().PaddingTop(12).Height(200).Image(img.Result).WithCompressionQuality(ImageCompressionQuality.High);
                        }
                    });

                    page.Footer().AlignCenter()
                        .Text(x =>
                        {
                            x.CurrentPageNumber();
                        });
                });
            }
        });

        var pdfBytes = document.GeneratePdf();

        return new PdfFileInfo(pdfBytes, "application/pdf", $"{book.Title}.pdf");
    }
}
