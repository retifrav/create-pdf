using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace create_pdf
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating PDF...\n");

            string filename = "some.pdf";
            string filepath = Path.Combine(
                Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),
                filename
                );

            try // creating PDF
            {
                // --- font settings

                // normal font
                string normal_font = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
                    "verdana.ttf"
                    );
                FontFactory.Register(normal_font);
                BaseFont baseFont_normal = BaseFont.CreateFont(
                    normal_font,
                    BaseFont.IDENTITY_H,
                    BaseFont.NOT_EMBEDDED
                    );
                Font font_main = new Font(baseFont_normal, 10, Font.NORMAL);

                // bold font
                string bold_font = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
                    "verdanab.ttf"
                    );
                FontFactory.Register(bold_font);
                BaseFont baseFont_bold = BaseFont.CreateFont(
                    bold_font,
                    BaseFont.IDENTITY_H,
                    BaseFont.NOT_EMBEDDED
                    );
                Font font_bold = new Font(baseFont_bold, 10, Font.NORMAL);

                // italic font
                string italic_font = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
                    "verdanai.ttf"
                    );
                FontFactory.Register(italic_font);
                BaseFont baseFont_italic = BaseFont.CreateFont(
                    italic_font,
                    BaseFont.IDENTITY_H,
                    BaseFont.NOT_EMBEDDED
                    );
                Font font_italic = new Font(baseFont_italic, 10, Font.NORMAL);

                // header font
                string header_font = Path.Combine(
                   Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
                   "verdanab.ttf"
                   );
                FontFactory.Register(header_font);
                BaseFont baseFont_header = BaseFont.CreateFont(
                    bold_font,
                    BaseFont.IDENTITY_H,
                    BaseFont.NOT_EMBEDDED
                    );
                Font font_header = new Font(baseFont_header, 16, Font.NORMAL);

                List<string> name_awarded = new List<string>() { "Junkrat", "Mercy", "Symmetra" };
                List<string> name_reported = new List<string>() { "Widowmaker", "Sombra" };

                using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (Document doc = new Document(PageSize.A4, 80, 80, 70, 70))
                using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
                {
                    writer.ViewerPreferences = PdfWriter.HideMenubar;
                    doc.Open();

                    // --- meta information

                    doc.AddTitle("Attack on Numbani");
                    doc.AddSubject("Match results");
                    doc.AddKeywords("pdf, example");
                    doc.AddCreator("Overwatch");
                    doc.AddAuthor("Athena");

                    // --- document contents

                    doc.Add(createAlignedParagraph("Overwatch", font_header, Element.ALIGN_CENTER));

                    doc.Add(new Paragraph(" "));
                    doc.Add(new Paragraph(" "));
                    doc.Add(createAlignedParagraph("Resolution #3426", font_main, Element.ALIGN_RIGHT));
                    doc.Add(createAlignedParagraph(
                        string.Format(
                            "from {0}",
                            DateTime.Today.ToShortDateString()
                            ),
                        font_main, Element.ALIGN_RIGHT));

                    doc.Add(new Paragraph(" "));
                    doc.Add(new Paragraph(" "));
                    doc.Add(new Paragraph("Regarding last match results.", font_bold));
                    doc.Add(new Paragraph(" "));

                    if (name_awarded.Count != 0)
                    {
                        doc.Add(new Paragraph("For the extradionary performance", font_main));
                        doc.Add(new Paragraph(" "));
                        for (int i = 0; i < name_awarded.Count; i++)
                        {
                            Paragraph li = new Paragraph(
                                string.Format(
                                    "{0}. {1}",
                                    (i + 1).ToString(),
                                    name_awarded[i]
                                    ),
                                font_main
                                );
                            li.IndentationLeft = 30;
                            doc.Add(li);
                        }
                        doc.Add(new Paragraph(" "));
                        doc.Add(new Paragraph("are awarded with gold medals.", font_main));
                        if (name_reported.Count != 0) { doc.Add(new Paragraph(" ")); }
                    }

                    if (name_reported.Count != 0)
                    {
                        doc.Add(new Paragraph("For the lack of teamplay and stupidity in general", font_main));
                        doc.Add(new Paragraph(" "));
                        for (int i = 0; i < name_reported.Count; i++)
                        {
                            Paragraph li = new Paragraph(
                                string.Format(
                                    "{0}. {1}",
                                    (i + 1).ToString(),
                                    name_reported[i]
                                    ),
                                font_main
                                );
                            li.IndentationLeft = 30;
                            doc.Add(li);
                        }
                        doc.Add(new Paragraph(" "));
                        doc.Add(new Paragraph("are reported for being such noobs.", font_main));
                    }

                    if (name_awarded.Count == 0 && name_reported.Count == 0)
                    {
                        doc.Add(new Paragraph("Just a regular match, nothing special.", font_main));
                        doc.Add(new Paragraph("[eats banana]", font_italic));
                    }

                    doc.Add(new Paragraph(" "));
                    // "glue" that allows to have two differently aligned phrases in one paragraph
                    Chunk glue = new Chunk(new VerticalPositionMark());
                    Paragraph p = new Paragraph(new Phrase("The head of Overwatch\n", font_bold));
                    p.Add(new Phrase("mighty monkey with Tesla Cannon", font_bold));
                    p.Add(new Chunk(glue));
                    p.Add(new Phrase("Winston", font_bold));
                    doc.Add(p);

                    #region Table as an alternative to "glue"
                    //var table = new PdfPTable(3);
                    //table.SpacingBefore = 35;
                    //PdfPCell cell = null;
                    //cell = new PdfPCell(new Phrase("The head of Overwatch", font_bold));
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //cell.Colspan = 2;
                    //table.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Winston", font_bold));
                    //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //cell.Rowspan = 2;
                    //table.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("mighty monkey with Tesla Cannon", font_bold));
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //cell.Colspan = 2;
                    //table.AddCell(cell);
                    //table.WidthPercentage = 90;
                    //table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //doc.Add(table);
                    #endregion

                    doc.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Something went wrong. {0}", ex.Message));
                Console.Write("Press any key to exit");
                Console.ReadKey();
            }

            Console.WriteLine(string.Format("File \"{0}\" has been successfully created\n", filepath));
            Console.Write("Press any key to exit");
            Console.ReadKey();
        }

        /// <summary>
        /// Creates a custom Paragraph
        /// </summary>
        /// <param name="text">paragraph contents</param>
        /// <param name="font">font for the paragraph</param>
        /// <param name="alignment">alignment of the paragraph</param>
        /// <returns>created paragraph</returns>
        static Paragraph createAlignedParagraph(string text, Font font, int alignment)
        {
            Paragraph p = new Paragraph(text, font);
            p.Alignment = alignment;
            return p;
        }
    }
}
