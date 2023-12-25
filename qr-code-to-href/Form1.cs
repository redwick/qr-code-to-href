using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using Path = System.IO.Path;
using Dotnet = System.Drawing.Image;
using iTextSharp.text.pdf;
using Docnet.Core.Models;
using Docnet.Core;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace qr_code_to_href
{
    public partial class Form1 : Form
    {
        public string selectedFile = "C:\\Users\\Redzor\\Desktop\\test.pdf";
        public Form1()
        {
            InitializeComponent();
        }

        private void button_selectFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK ) { selectedFile = ofd.FileName; }
            textBox_selectedFile.Text = selectedFile;
        }
        public void Convert()
        {

            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


            var bitmapImage = new Bitmap(Image.FromFile(desktop + "/test.jpg"));
            LuminanceSource source = new BitmapLuminanceSource(bitmapImage);
            BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(source));
            var reader = new BarcodeReaderGeneric();
            reader.Options.PossibleFormats = new List<BarcodeFormat>() { BarcodeFormat.QR_CODE };
            reader.Options.TryHarder = true;
            reader.Options.TryInverted = true;
            var mlt = reader.DecodeMultiple(source).ToList();

            string pdfPath = desktop + "/test.pdf";
            string pdfPathRes = desktop + "/test1.pdf";
            PdfReader pdfReader = new PdfReader(pdfPath);
            PdfStamper stamper = new PdfStamper(pdfReader, File.OpenWrite(pdfPathRes));

            mlt.ForEach(p =>
            {
                var size = pdfReader.GetPageSize(1);

                var xValues = p.ResultPoints.ToList().OrderBy(x => x.X).ToList().ConvertAll(x => x.X);
                var yValues = p.ResultPoints.ToList().OrderBy(x => x.Y).ToList().ConvertAll(x => x.Y);


                var x0 = xValues.First() / bitmapImage.Width * size.Width;
                var y0 = (1 - yValues.First() / bitmapImage.Height) * size.Height;

                var x1 = xValues.Last() / bitmapImage.Width * size.Width;
                var y1 = (1 - yValues.Last() / bitmapImage.Height) * size.Height;

                var coef = 0.15;
                var xDiff = x1 - x0;
                var yDiff = y1 - y0;

                x0 = (float)(x0 - coef * xDiff);
                y0 = (float)(y0 - coef * yDiff);

                x1 = (float)(x1 + coef * xDiff);
                y1 = (float)(y1 + coef * yDiff);

                iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(x0, y0, x1, y1);
                PdfDestination destination = new PdfDestination(PdfDestination.FIT);
                PdfAnnotation link = PdfAnnotation.CreateLink(stamper.Writer, rectangle, PdfAnnotation.HIGHLIGHT_NONE, new PdfAction(mlt[0].Text));
                link.Border = new PdfBorderArray(0, 0, 0);
                stamper.AddAnnotation(link, 1);

            });

            stamper.Close();



        }

        private void button_proceed_Click(object sender, EventArgs e)
        {
            if (textBox_selectedFile.Text.Length == 0)
            {
                MessageBox.Show("no file selected");
                return;
            }

            string pdfPath = textBox_selectedFile.Text;
            string pdfPathRes = pdfPath.Replace(".pdf", "-res.pdf");
            while (File.Exists(pdfPathRes))
            {
                pdfPathRes = pdfPathRes.Replace(".pdf", "#.pdf");
            }
            string temp = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()) + Path.DirectorySeparatorChar;
            Directory.CreateDirectory(temp);

            pdfToImages(pdfPath, temp);

            PdfReader pdfReader = new PdfReader(pdfPath);
            PdfStamper stamper = new PdfStamper(pdfReader, File.OpenWrite(pdfPathRes));

            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                using (var image = Image.FromFile(temp + page + ".png"))
                {
                    using (var bitmapImage = new Bitmap(image))
                    {
                        try
                        {
                            LuminanceSource source = new BitmapLuminanceSource(bitmapImage);
                            BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(source));
                            var reader = new BarcodeReaderGeneric();
                            reader.Options.PossibleFormats = new List<BarcodeFormat>() { BarcodeFormat.QR_CODE };
                            reader.Options.TryHarder = true;
                            reader.Options.TryInverted = true;
                            var mlt = reader.DecodeMultiple(source).ToList();

                            mlt.ForEach(p =>
                            {
                                var size = pdfReader.GetPageSize(page);

                                var xValues = p.ResultPoints.ToList().OrderBy(x => x.X).ToList().ConvertAll(x => x.X);
                                var yValues = p.ResultPoints.ToList().OrderBy(x => x.Y).ToList().ConvertAll(x => x.Y);


                                var x0 = xValues.First() / bitmapImage.Width * size.Width;
                                var y0 = (1 - yValues.First() / bitmapImage.Height) * size.Height;

                                var x1 = xValues.Last() / bitmapImage.Width * size.Width;
                                var y1 = (1 - yValues.Last() / bitmapImage.Height) * size.Height;

                                var xDiff = x1 - x0;
                                var yDiff = y1 - y0;
                                var coef = 0.25;

                                x0 = (float)(x0 - coef * xDiff);
                                y0 = (float)(y0 - coef * yDiff);

                                x1 = (float)(x1 + coef * xDiff);
                                y1 = (float)(y1 + coef * yDiff);

                                iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(x0, y0, x1, y1);
                                PdfAnnotation link = PdfAnnotation.CreateLink(stamper.Writer, rectangle, PdfAnnotation.HIGHLIGHT_NONE, new PdfAction(p.Text));
                                link.Border = new PdfBorderArray(0, 0, 0);
                                stamper.AddAnnotation(link, page);
                            });
                        }
                        catch
                        {

                        }
                    }
                }
            }
            
            stamper.Close();
            pdfReader.Close();
            Directory.Delete(temp, true);

            Process.Start(pdfPathRes);
        }

        private void pdfToImages(string pdfPath, string tempDirRes)
        {
            //using docnet
            //using (var docReader = DocLib.Instance.GetDocReader(pdfPath, new PageDimensions(1080, 1920)))
            using (var docReader = DocLib.Instance.GetDocReader(pdfPath, new PageDimensions(2160, 4096)))
            {
                for (int page = 0; page < docReader.GetPageCount(); page++)
                {
                    //open pdf file
                    using (var pageReader = docReader.GetPageReader(page))
                    {
                        var rawBytes = pageReader.GetImage();
                        var width = pageReader.GetPageWidth();
                        var height = pageReader.GetPageHeight();
                        var characters = pageReader.GetCharacters();

                        //using bitmap to create a png image
                        using (var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                        {
                            AddBytes(bmp, rawBytes);

                            using (var stream = new MemoryStream())
                            {
                                //saving and exporting
                                bmp.Save(stream, ImageFormat.Png);
                                File.WriteAllBytes(tempDirRes + (page + 1).ToString() + ".png", stream.ToArray());
                            };
                        };
                    };
                }
               
            };

            //extra methods
            void AddBytes(Bitmap bmp, byte[] rawBytes)
            {
                var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                var bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                var pNative = bmpData.Scan0;

                Marshal.Copy(rawBytes, 0, pNative, rawBytes.Length);
                bmp.UnlockBits(bmpData);
            }

        }


    }
}
