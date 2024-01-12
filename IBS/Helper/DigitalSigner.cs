using System.Security.Cryptography;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf.security;
using System.Security.Cryptography.X509Certificates;

namespace IBS.Helper
{
    public sealed class DigitalSigner
    {
        public static string sha256_hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public static void CreateSignatureField(string filename)
        {
            Document document = new Document();
            // step 2: Create a PdfWriter

            FileStream os = new FileStream(filename, FileMode.CreateNew);

            PdfWriter writer = PdfWriter.GetInstance(document, os);
            // step 3: Open the Document
            document.Open();
            // step 4: Add content
            document.Add(new Paragraph("Please Sign Below and upload the document to Map your digital Certificate!"));
            // create a signature form field
            PdfFormField field = PdfFormField.CreateSignature(writer);
            field.FieldName = "SIGNME";
            // set the widget properties
            field.SetPage();
            float xPos, yPos, xPos2, yPos2;
            xPos = PageSize.A4.Width - 10;
            yPos = PageSize.A4.Height - 830;
            xPos2 = PageSize.A4.Width - 180;
            yPos2 = PageSize.A4.Height - 740;
            field.SetWidget(new iTextSharp.text.Rectangle((int)xPos, (int)yPos, (int)xPos2, (int)yPos2), PdfAnnotation.HIGHLIGHT_NONE);
            field.SetFieldFlags(PdfAnnotation.FLAGS_PRINT);
            // add it as an annotation
            writer.AddAnnotation(field);
            // maybe you want to define an appearance
            PdfAppearance tp = PdfAppearance.CreateAppearance(writer, 72, 48);
            tp.SetColorStroke(BaseColor.LIGHT_GRAY);
            tp.SetColorFill(BaseColor.LIGHT_GRAY);
            tp.Rectangle(0.5f, 0.5f, 71.5f, 47.5f);
            tp.FillStroke();
            tp.SetColorFill(BaseColor.BLUE);
            ColumnText.ShowTextAligned(tp, Element.ALIGN_CENTER, new Phrase(""), 36, 24, 25);
            field.SetAppearance(PdfAnnotation.APPEARANCE_NORMAL, tp);
            // step 5: Close the Document
            document.Close();
        }

        public static void SetSignField(byte[] byteArray, string PDFForSignField, bool IsMultipleSign, bool isLeft, string SearchText, out int counter, int PageNo = 0, int Level = 0, decimal x1 = 0, decimal y1 = 0, decimal x2 = 0, decimal y2 = 0)
        {
            counter = 1;
            try
            {
                PdfReader reader = new PdfReader(byteArray);
                using (FileStream fout = new FileStream(PDFForSignField, FileMode.Create, FileAccess.ReadWrite))
                {
                    // appearance
                    PdfStamper stamper = new PdfStamper(reader, fout, '\0', true);
                    PdfSignatureAppearance appearance = stamper.SignatureAppearance;
                    int NumberOfPages = PageNo > 0 ? PageNo : reader.NumberOfPages;
                    PdfFormField field = PdfFormField.CreateSignature(stamper.Writer);
                    decimal xPos, yPos, xPos2, yPos2;
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        var txtstrategy = new MyLocationTextExtractionStrategy(SearchText);
                        var ex = PdfTextExtractor.GetTextFromPage(reader, PageNo, txtstrategy);

                        //var strategy = new LocationTextExtractionStrategy();
                        //var ex1 = PdfTextExtractor.GetTextFromPage(reader, PageNo, strategy);

                        //iTextSharp.text.pdf.parser.ITextExtractionStrategy txtstrategy = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                        //var currentText = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, PageNo, txtstrategy);

                        if (txtstrategy.myPoints.Count == 0)
                        {
                            //return PDFForSignField; // Show error msg
                        }

                        xPos = (decimal)txtstrategy.myPoints[txtstrategy.myPoints.Count - 1].Rect.Left;
                        yPos = (decimal)(txtstrategy.myPoints[txtstrategy.myPoints.Count - 1].Rect.Top + 5);
                        xPos2 = xPos + 72;
                        yPos2 = yPos + 48;
                    }
                    else
                    {
                        if (isLeft)
                        {
                            xPos = (decimal)reader.GetPageSize(PageNo).Width - 450;
                            yPos = (decimal)reader.GetPageSize(PageNo).Height - 800;
                            xPos2 = (decimal)reader.GetPageSize(PageNo).Width - 550;
                            yPos2 = (decimal)reader.GetPageSize(PageNo).Height - 750;
                        }
                        else
                        {
                            xPos = x1;
                            yPos = y1; //792
                            xPos2 = x2;
                            yPos2 = y2;
                        }
                    }

                    field.SetWidget(new iTextSharp.text.Rectangle((float)(xPos * PageNo), (float)(yPos * PageNo), (float)(xPos2 * PageNo), (float)(yPos2 * PageNo)), PdfAnnotation.HIGHLIGHT_NONE);

                    field.SetFieldFlags(PdfAnnotation.FLAGS_PRINT);

                    field.FieldName = "SIGNME_" + counter; //+ ij.ToString();
                    field.SetWidget(new iTextSharp.text.Rectangle((float)xPos, (float)yPos, (float)xPos2, (float)yPos2), PdfAnnotation.APPEARANCE_NORMAL);
                    field.Flags = PdfAnnotation.FLAGS_PRINT;

                    stamper.AddAnnotation(field, PageNo);

                    stamper.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void SignPDF(X509Certificate2 cert, string PDFToSign, string SignReason, string SignLocation, int counter, int pageNo = 0)
        {
            Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
            Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] { cp.ReadCertificate(cert.RawData) };

            //IExternalSignature externalSignature = new X509Certificate2Signature(cert, "SHA1");

            RSACng rsa = (RSACng)cert.PrivateKey;

            IExternalSignature externalSignature = new RSACngSignature(rsa);

            string pathToSignatureImage = PDFToSign.Replace(".pdf", "_Signed.pdf");

            PdfReader reader = new PdfReader(PDFToSign);
            using (FileStream fout = new FileStream(pathToSignatureImage, FileMode.Create, FileAccess.ReadWrite))
            {
                PdfStamper stamper = PdfStamper.CreateSignature(reader, fout, '\0', null, true);
                PdfSignatureAppearance appearance = stamper.SignatureAppearance;

                appearance.Reason = SignReason;
                appearance.Location = SignLocation;

                //appearance.Layer2Font
                if (!string.IsNullOrEmpty(SignReason)) appearance.Reason = SignReason;
                if (!string.IsNullOrEmpty(SignLocation)) appearance.Location = SignLocation;

                appearance.SignDate = DateTime.Now;
                float xPos, yPos, xPos2, yPos2;

                var fieldPositions = reader.AcroFields.GetFieldPositions("SIGNME_" + counter);
                iTextSharp.text.Rectangle rectangle = fieldPositions.FirstOrDefault().position;

                xPos = rectangle.Right; //txtstrategy.myPoints[txtstrategy.myPoints.Count - 1].Rect.Left;
                yPos = rectangle.Bottom; //txtstrategy.myPoints[txtstrategy.myPoints.Count - 1].Rect.Top - 60;
                xPos2 = rectangle.Left; //xPos + 72;
                yPos2 = rectangle.Top; //yPos1 + 48;

                appearance.SetVisibleSignature(new iTextSharp.text.Rectangle(xPos - 10, yPos - 10, xPos2 - 10, yPos2 - 10), pageNo, null);

                reader.AcroFields.RemoveField("SIGNME_" + counter);

                MakeSignature.SignDetached(appearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);
            }
        }

        public class RectAndText
        {
            public iTextSharp.text.Rectangle Rect;
            public string Text;
            public RectAndText(iTextSharp.text.Rectangle rect, string text)
            {
                this.Rect = rect;
                this.Text = text;
            }
        }

        public class MyLocationTextExtractionStrategy : LocationTextExtractionStrategy
        {
            //Hold each coordinate
            public List<RectAndText> myPoints = new List<RectAndText>();

            //The string that we're searching for
            public String TextToSearchFor { get; set; }

            //How to compare strings
            public System.Globalization.CompareOptions CompareOptions { get; set; }

            public MyLocationTextExtractionStrategy(String textToSearchFor, System.Globalization.CompareOptions compareOptions = System.Globalization.CompareOptions.None)
            {
                this.TextToSearchFor = textToSearchFor;
                this.CompareOptions = compareOptions;
            }

            //Automatically called for each chunk of text in the PDF
            public override void RenderText(TextRenderInfo renderInfo)
            {
                base.RenderText(renderInfo);

                //See if the current chunk contains the text
                var startPosition = System.Globalization.CultureInfo.CurrentCulture.CompareInfo.IndexOf(renderInfo.GetText(), this.TextToSearchFor, this.CompareOptions);

                //If not found bail
                if (startPosition < 0)
                {
                    return;
                }

                //Grab the individual characters
                var chars = renderInfo.GetCharacterRenderInfos().Skip(startPosition).Take(this.TextToSearchFor.Length).ToList();

                //Grab the first and last character
                var firstChar = chars.First();
                var lastChar = chars.Last();


                //Get the bounding box for the chunk of text
                var bottomLeft = firstChar.GetDescentLine().GetStartPoint();
                var topRight = lastChar.GetAscentLine().GetEndPoint();

                //Create a rectangle from it
                var rect = new iTextSharp.text.Rectangle(
                                                        bottomLeft[Vector.I1],
                                                        bottomLeft[Vector.I2],
                                                        topRight[Vector.I1],
                                                        topRight[Vector.I2]
                                                        );

                //Add this to our main collection
                this.myPoints.Add(new RectAndText(rect, this.TextToSearchFor));
            }
        }

        public List<int> ReadPdfFile(string fileName, String searthText)
        {
            List<int> pages = new List<int>();
            if (File.Exists(fileName))
            {
                PdfReader pdfReader = new PdfReader(fileName);
                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

                    string currentPageText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                    if (currentPageText.Contains(searthText))
                    {
                        pages.Add(page);
                    }
                }
                pdfReader.Close();
            }
            return pages;
        }

        public static X509Certificate2 getCertificate(string subject)
        {
            // Access Personal (MY) certificate store of current user
            X509Store my = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            my.Open(OpenFlags.ReadOnly);
            // Find the certificate we’ll use to sign
            X509Certificate2 cert2 = null;
            foreach (X509Certificate2 cert in my.Certificates)
            {
                if (cert.Subject.Contains(subject))
                {
                    // Get its associated CSP and private key
                    cert2 = cert;
                }
            }
            return cert2;
        }

        public static byte[] Sign(string text, X509Certificate2 cert)
        {
            // Access Personal (MY) certificate store of current user
            X509Store my = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            my.Open(OpenFlags.ReadOnly);
            // Find the certificate we’ll use to sign
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PrivateKey;
            if (csp == null)
            {
                throw new Exception("No valid cert was found");
            }
            // Hash the data
            SHA1Managed sha1 = new SHA1Managed();
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(text);
            byte[] hash = sha1.ComputeHash(data);
            // Sign the hash
            return csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
        }

        public static bool Verify(string text, byte[] signature, string certPath)
        {
            // Load the certificate we’ll use to verify the signature from a file
            X509Certificate2 cert = new X509Certificate2(certPath);
            // Note:
            // If we want to use the client cert in an ASP.NET app, we may use something like this instead:
            // X509Certificate2 cert = new X509Certificate2(Request.ClientCertificate.Certificate);
            // Get its associated CSP and public key
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key;
            // Hash the data
            SHA1Managed sha1 = new SHA1Managed();
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(text);
            byte[] hash = sha1.ComputeHash(data);
            // Verify the signature with the hash
            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);
        }
    }

    public class RSACngSignature : IExternalSignature
    {
        private RSACng rsa;

        public RSACngSignature(RSACng rsa)
        {
            this.rsa = rsa;
        }

        public string GetEncryptionAlgorithm()
        {
            return "RSA";
        }

        public string GetHashAlgorithm()
        {
            return "SHA-256";
        }

        public byte[] Sign(byte[] message)
        {
            // Ensure the RSA object is properly initialized
            if (rsa == null)
            {
                throw new InvalidOperationException("RSACng object not initialized.");
            }

            // Sign the message using RSACng
            byte[] signature = rsa.SignData(message, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return signature;
        }
    }

}
