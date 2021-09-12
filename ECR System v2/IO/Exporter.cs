using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;
using System.IO;
using NPOI.SS.Util;
using NPOI.HSSF.UserModel;
using NPOI.SS.Converter;
using Spire.Xls;
using Spire.Pdf;
using Spire.Xls.Converter;
using System.Net.Mail;
using System.Net;
using ECR_System_v2.Loaders;
using ECR_System_v2.Utils;
using ECR_System_v2.Data;
using System.Globalization;
using org.apache.pdfbox.pdmodel.font;

using static java.awt.Color;
using static org.apache.pdfbox.pdmodel.font.PDType1Font;
using java.awt;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.pdmodel.common;
using be.quodlibet.boxable;
using be.quodlibet.boxable.datatable;

namespace ECR_System_v2.IO
{
    public class Exporter
    {
        public  void SendMail(String clientName ,String statementPath,String EmailAddress)
        {
                    var client = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential("Ecrzambialtd@gmail.com", "stocksnshares"),
                        EnableSsl = true
                    };
                MailMessage msg = new MailMessage("Ecrzambialtd@gmail.com", EmailAddress, "ZICA Property Fund Client Statement", "Good afternoon " + clientName + "," + System.Environment.NewLine + " Please find your attached ZICA Client statement principal sum invested in the ZICA property Fund." + System.Environment.NewLine + System.Environment.NewLine + " Kind regards, ECR Unit Trust Team");

            System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                contentType.Name = new FileInfo(statementPath).Name;
                msg.Attachments.Add(new Attachment(statementPath, contentType));

                client.Send(msg);
                Console.WriteLine("Sent to "+ EmailAddress);
               
        }
        public void SendMail( String EmailAddress)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("Ecrzambialtd@gmail.com", "stocksnshares"),
                EnableSsl = true
            };
            client.Timeout = Int32.MaxValue;
            MailMessage msg = new MailMessage("Ecrzambialtd@gmail.com", EmailAddress, "ECRUT AGM NOTICE", "Good afternoon SIR/MADAM"+ "," + System.Environment.NewLine + " Please be informed that we are still awaiting your response to the AGM meeting notice" + System.Environment.NewLine + System.Environment.NewLine + " Kind regards, ECR Unit Trust Team");
            String[] attachemtnets = new String[] {
            @"C:\Users\pc\Desktop\Current Project Resources\ECR\ECRUT\ECR AGM NOTICE AND PROXY FORM 2019.pdf",
            @"C:\Users\pc\Desktop\Current Project Resources\ECR\ECRUT\ECRUT Audited Finacial 2018.PDF"};
            foreach(var statementPath in attachemtnets)
            {

                System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                contentType.Name = new FileInfo(statementPath).Name;
               // msg.Attachments.Add(new Attachment(statementPath, contentType));
            }

            client.Send(msg);
            Console.WriteLine("Sent to " + EmailAddress);

        }
        private  String GetExportPdf(String client)
        {
            DirectoryInfo d = new DirectoryInfo(@"C:\Users\pc\Desktop\Current Project Resources\ECR\Zica Clients\pdfs");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*xlsecr.xls.pdf"); //Getting Text files
            string str = ""; 
            double n = Files.Length;
            double i = 0;
            foreach (FileInfo file in Files)
            {
                if(file.FullName.EndsWith(client+ ".xlsecr.xls.pdf"))
                {
                    str= file.FullName;

                    return file.FullName;
                }
                i++;

            }
            return str;



        }
        public async void LoadClients()
        {
            await Task.Run(() =>
            {

                var path = @"C:\Users\pc\Desktop\Current Project Resources\ECR\ECRUT\ECR UT Register 2018.xlsx";
                XSSFWorkbook workbook = new XSSFWorkbook(new FileInfo(path));

                var nCount = workbook.NumberOfSheets;
                for (int i = 0; i < nCount; i++)
                {
                    XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(i);
                    String sheetName = sheet.SheetName;
                    IEnumerator rowIterator = sheet.GetEnumerator();
                    List<Object> data = new List<Object>();
                    String preDate = "";
                    Boolean first = true;
                    List<String> Saved = new List<string>();
                    while (rowIterator.MoveNext())
                    {


                        IRow row = (IRow)rowIterator.Current;
                        IEnumerator<ICell> cellIterator = row.GetEnumerator();
                        while (cellIterator.MoveNext())
                        {
                            ICell cell = cellIterator.Current;
                            int mCellIndex = cell.ColumnIndex;
                            int mRowIndex = cell.RowIndex;
                            String mCellValue = CellVal(cell);
                            if (mCellValue.Contains("@")&&!mCellValue.Contains("zsiclife"))
                            {
                                mCellValue = mCellValue.Replace(" ", "");
                                if (mCellValue.Contains("/"))
                                {
                                    var mails = mCellValue.Split('/');
                                    foreach(var mail in mails)
                                    {

                                        Console.WriteLine("email " + mail);
                                        SendMail(mail);
                                    }
                                }
                                else
                                {

                                    Console.WriteLine("email " + mCellValue);
                                    SendMail(mCellValue);
                                }
                            }
                            

                        }

                    }
                }
            });
        }
        public async static Task<String[]> FindClients(String path,int sheetIndex,int column) {
            var Names = new List<String>();
            try
            {
              await  Task.Run(() => {
                  ArrayList sents = new ArrayList();


                  XSSFWorkbook workbook = new XSSFWorkbook(new FileInfo(path));

                  XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(sheetIndex);
                  String sheetName = sheet.SheetName;
                  IEnumerator rowIterator = sheet.GetEnumerator();
                  List<Object> data = new List<Object>();
                  String preDate = "";
                  Boolean first = true;
                  List<String> Saved = new List<string>();
                  while (rowIterator.MoveNext())
                  {


                      IRow row = (IRow)rowIterator.Current;
                      IEnumerator<ICell> cellIterator = row.GetEnumerator();
                      String date = "";
                      String Name = "";
                      String Amount = "";
                      String MemberId = "";
                      String Units = "";
                      String Email = "";
                      while (cellIterator.MoveNext())
                      {
                          ICell cell = cellIterator.Current;
                          int mCellIndex = cell.ColumnIndex;
                          int mRowIndex = cell.RowIndex;
                          String mCellValue = CellVal(cell);

                          if (mCellIndex == column && mCellValue.Length > 0 && !mCellValue.Equals("ZICA PROPERTY FUND REGISTER")
                             && !mCellValue.Contains("Name") && !mCellValue.Equals("Total")
                             && !mCellValue.ToLower().Contains("dollar") && !mCellValue.ToLower().Contains("paid")
                             && !mCellValue.ToLower().Contains("amount")
                             && mCellValue.Split(' ').Length > 0 && mCellValue.Replace(" ", "").Length > 4)
                          {
                              Name = StringUtils.RemoveNumbers(mCellValue);
                              Console.WriteLine("mCellValue " + Name + " mCellIndex " + mCellIndex + " column " + column);
                              Names.Add(Name);
                          }




                      }


                  }

              });
                           
            }
            catch (Exception e)
            {
                Console.WriteLine("Import error " + e.Message);
            }
            return Names.ToArray();

        }
        public async static Task<String[]> FindUnitsOrAmount(String path, int sheetIndex, int column)
        {
            var Names = new List<String>();
            try
            {
                await Task.Run(() => {
                    ArrayList sents = new ArrayList();


                    XSSFWorkbook workbook = new XSSFWorkbook(new FileInfo(path));

                    XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(sheetIndex);
                    String sheetName = sheet.SheetName;
                    IEnumerator rowIterator = sheet.GetEnumerator();
                    List<Object> data = new List<Object>();
                    String preDate = "";
                    Boolean first = true;
                    List<String> Saved = new List<string>();
                    while (rowIterator.MoveNext())
                    {


                        IRow row = (IRow)rowIterator.Current;
                        IEnumerator<ICell> cellIterator = row.GetEnumerator();
                        String date = "";
                        String Value = "";
                        String Amount = "";
                        String MemberId = "";
                        String Units = "";
                        String Email = "";
                        while (cellIterator.MoveNext())
                        {
                            ICell cell = cellIterator.Current;
                            int mCellIndex = cell.ColumnIndex;
                            int mRowIndex = cell.RowIndex;
                            String mCellValue = CellVal(cell);
                            double n;

                            if (mCellIndex == column && mCellValue.Length > 0 && double.TryParse(mCellValue, out n))
                            {
                                Names.Add(mCellValue);
                            }

                        }


                    }

                });

            }
            catch (Exception e)
            {
                Console.WriteLine("Import error " + e.Message);
            }
            return Names.ToArray();

        }

        private DataLoader mDataLoader;
        public async static void ImportClients(String fundName,String[] mImportClientsNames, String[] mImportClientId, String[] mImportEmailAddresses
            , String[] mImportDate, String[] mImportAmount,double unitPrice)
        {

            DataLoader mDataLoader = new DataLoader();
            for (int i=0;i< mImportClientsNames.Length; i++)
            {
                var mNow = DateUtils.TicksToMillis(DateTime.Now.Ticks);
                Client mClient = new Client();
                mClient.Fund = fundName;
                mClient.ClientName = mImportClientsNames[i];
                if (mImportEmailAddresses != null)
                    mClient.ClientEmailAdress = mImportEmailAddresses[i];
                else
                    mClient.ClientEmailAdress = "";
                mClient.ClientPhysicalAdress = "";
                mClient.DateCreated = mNow;
                mClient.ClientId = mImportClientId[i];
                mClient.Open = App.Open;
                await mDataLoader.addFundUnitClients(mClient);

                mNow = DateUtils.TicksToMillis(DateTime.ParseExact(mImportDate[i].Replace('.','/').Replace(' ', '/'), "dd/MM/yyyy", null).Ticks);
                FundUnitTrans mFundUnitTrans = new FundUnitTrans();
                mFundUnitTrans.Amount = Double.Parse(mImportAmount[i]);
                mFundUnitTrans.Client = mImportClientId[i];
                mFundUnitTrans.DateInMillis = mNow;
                mFundUnitTrans.Fund = fundName;
                mFundUnitTrans.Units = Double.Parse(mImportAmount[i])/ unitPrice;
                mFundUnitTrans.TransactionType = 0;
                await mDataLoader.addFundUnitTransItem(mFundUnitTrans);
            }
        }
        public async void ImportData(String path)
        {
            mDataLoader = new DataLoader();
            try
            {
                await Task.Run(async () =>
                {
                    String lastAddress = "marychiwala.mc@gmail.com";
                    ArrayList sents = new ArrayList();

                    var lines = File.ReadAllLines(IoUtils.GetMainDirectory()+"/sent.txt");
                    foreach (var line in lines)
                        sents.Add(line);

                    XSSFWorkbook workbook = new XSSFWorkbook(new FileInfo(path));

                    //ICreationHelper CreateHelper = workBookExport.GetCreationHelper();
                    //IFormulaEvaluator evaluator = CreateHelper.CreateFormulaEvaluator();


                    XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0);
                    String sheetName = sheet.SheetName;
                    IEnumerator rowIterator = sheet.GetEnumerator();
                    List<Object> data = new List<Object>();
                    String preDate = "";
                    Boolean first = true;
                    List<String> Saved = new List<string>();
                    while (rowIterator.MoveNext())
                    {


                        IRow row = (IRow)rowIterator.Current;
                        IEnumerator<ICell> cellIterator = row.GetEnumerator();
                        String date = "";
                        String Name = "";
                        String Amount = "";
                        String MemberId = "";
                        String Units = "";
                        String Email = "";
                        while (cellIterator.MoveNext())
                        {
                            ICell cell = cellIterator.Current;
                            int mCellIndex = cell.ColumnIndex;
                            int mRowIndex = cell.RowIndex;
                            String mCellValue = CellVal(cell);

                            if (mCellIndex == 0 && mCellValue.ToLower().StartsWith("paid on"))
                            {
                                date = mCellValue.ToLower().Replace("paid on ", "").Replace(".", "/");
                            }

                            else if (mCellIndex == 2 && mCellValue.Length > 0 && !mCellValue.Equals("ZICA PROPERTY FUND REGISTER")
                                && !mCellValue.Contains("Name") && !mCellValue.Equals("Total")) { Name = mCellValue; }
                            /*
                            else if ((mCellIndex == 6 && mRowIndex < 16 && mCellValue.Contains("@"))
                            || (mCellIndex == 7 && mRowIndex > 16 && mCellValue.Contains("@"))) { Email = mCellValue; }*/
                            else if (mCellIndex == 7) { Email = mCellValue; }
                            else if (mCellIndex == 3) { MemberId = mCellValue; }

                            else if (mCellIndex == 4) { Amount = mCellValue; }
                            else if (mCellIndex == 5) { Units = mCellValue; }



                        }
                        //date = preDate;
                        if (date.Length == 0)
                        {
                            date = preDate;
                        }
                        else
                        {
                            preDate = date;
                        }
                        Name = StringUtils.RemoveNumbers(Name);
                        if (Name.Replace(" ", "").Length > 4 && MemberId.Length > 0)
                        {
                            /*var mNow = DateUtils.TicksToMillis(DateTime.Now.Ticks);
                            Client mClient = new Client();
                            mClient.Fund = "ZICA";
                            mClient.ClientName = Name;
                            mClient.ClientEmailAdress = Email;
                            mClient.ClientPhysicalAdress = "";
                            mClient.DateCreated = mNow;
                            mClient.ClientId = MemberId;
                            mClient.Open = App.Open;
                            await mDataLoader.addFundUnitClients(mClient);*/
                            var mNow = DateUtils.TicksToMillis(DateTime.ParseExact("03/04/2019", "dd/MM/yyyy",null).Ticks);
                            FundUnitTrans mFundUnitTrans = new FundUnitTrans();
                            mFundUnitTrans.Amount =Double.Parse(Amount);
                            mFundUnitTrans.Client = MemberId;
                            mFundUnitTrans.DateInMillis = mNow;
                            mFundUnitTrans.Fund = "ZICA";
                            mFundUnitTrans.Units = Double.Parse(Units);
                            mFundUnitTrans.TransactionType = 0;
                            await mDataLoader.addFundUnitTransItem(mFundUnitTrans);
                            Console.WriteLine("Added Client " + Name+" amount "+Amount);
                        }
                        /*if (Name.Replace(" ", "").Length > 4 && Email.Length>0)
                        {
                            //Console.WriteLine("date: " + date + " Name: " + Name + " Amount: " + Amount + " Units: " + Units);
                            //Console.WriteLine("templatesheet " + templatesheet.SheetName);
                            String newsheetName = Name.Replace("/", " ").Replace(".", " ");
                            if (newsheetName.Length > 30)
                                newsheetName = newsheetName.Substring(0, 29);
                            String statementPath = GetExportPdf(newsheetName);
                            sents.Add("chabum2003@gmail.com");
                            sents.Add("martin.mukendwa@yahoo.com");
                            sents.Add("alexkapam@gmail.com");
                            sents.Add("achabooka@zesco.co.zm");
                            sents.Add("chishimba49@yahoo.com");
                            sents.Add("fredm@southernsun.co.zm");
                            sents.Add("harrykamanga47@yahoo.com");
                            sents.Add("kenny.kapusu@gmail.com");
                            sents.Add("kachingwe2011@gmail.com");
                            sents.Add("sikawala90@gmail.com");
                            sents.Add("bryan@gammazenith.co.zm");
                            sents.Add("nainip@vopzambia.adventist.org");
                           
                            File.AppendAllText(@"C:\Users\pc\Desktop\Current Project Resources\ECR\Zica Clients\all2.txt", Name+" : "+ Email + Environment.NewLine);
                            File.AppendAllText(@"C:\Users\pc\Desktop\Current Project Resources\ECR\Zica Clients\no_emails.txt", Name + " Email" + Email + Environment.NewLine);

                            if (statementPath.Length > 0 && sents.Contains(Email))
                            {

                                Console.WriteLine("Sending to  " + Name + " Email " + Email);
                           //   SendMail(Name, statementPath, Email);
                             File.AppendAllText(@"C:\Users\pc\Desktop\Current Project Resources\ECR\Zica Clients\sent_name.txt",Name +" Email"+ Email + Environment.NewLine);

                             //   System.IO.File.WriteAllText(@"C:\sent.txt", Email);
                            }
                            else if (statementPath.Length > 0 && sents.Contains(lastAddress)&&sents.Contains(Email))
                            {
                               // File.AppendAllText(@"C:\Users\pc\Desktop\Current Project Resources\ECR\Zica Clients\repeated.txt", Name + " : " + Email + Environment.NewLine);

                            }
                            else
                            {
                            //    File.AppendAllText(@"C:\Users\pc\Desktop\Current Project Resources\ECR\Zica Clients\failed.txt", Email + Environment.NewLine);

                                //Console.WriteLine("Failed for  " + Name + " Email " + Email);
                            }


                            sents.Add(Email);
                        }
                        else if(Name.Replace(" ", "").Length > 4) {
                            File.AppendAllText(@"C:\Users\pc\Desktop\Current Project Resources\ECR\Zica Clients\none.txt", Name + " : " + Email + Environment.NewLine);
                        }*/


                    }

                });
            }
            catch(Exception e) {
                Console.WriteLine("Import error "+e.Message);
            }
        }
        public async void ExportPdfs()
        {
            try {
                await Task.Run(() => {
                    DirectoryInfo d = new DirectoryInfo(@"C:\Users\pc\Desktop\Current Project Resources\ECR\Zica Clients\pdfs");//Assuming Test is your Folder
                    FileInfo[] Files = d.GetFiles("*ecr.xls"); //Getting Text files
                    string str = "";
                    double n = Files.Length;
                    double i = 0;
                    foreach (FileInfo file in Files)
                    {
                        Workbook workbook = new Workbook();
           workbook.LoadFromFile(file.FullName);
                        PdfDocument pdfDocument = new PdfDocument();

           PdfConverter pdfConverter = new PdfConverter(workbook);
           PdfConverterSettings settings = new PdfConverterSettings();
           settings.TemplateDocument = pdfDocument;
                        settings.FitSheetToOnePage = FitToPageType.ScaleWithSameFactor; ;
           pdfDocument = pdfConverter.Convert(settings);
  
           pdfDocument.SaveToFile(file.DirectoryName+"/"+ file.Name+".pdf");

                        /*
                        
                        HSSFWorkbook workbookTemp;
                        try
                        {
                            using (FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                            {
                                workbookTemp = new HSSFWorkbook(fs);



                                ICreationHelper CreateHelper = workbookTemp.GetCreationHelper();
                                IFormulaEvaluator evaluator = CreateHelper.CreateFormulaEvaluator();
                                evaluator.EvaluateAll();

                                using (FileStream fsp = new FileStream(file.FullName+"ecr.xls", FileMode.Create, FileAccess.Write))
                                {
                                    workbookTemp.Write(fsp);
                                }
                                
                                //   File.Delete(file);
                            }
                        }
                        catch (Exception e) { }*/
                        Console.WriteLine(((i/n)*(double)100)+" "+file.Name);
                        i++;

                    }

                });
            }
            catch(Exception e) { }
        }

        public static  void ExportPdf( FundUnitTrans[] mFundUnitTrans, Fund mFund,String clientName,String dir) {
           Console.WriteLine("Exporting PDF for "+ clientName);

                
                

                float PADDING = 50f;

                PDDocument document = new PDDocument();
                PDPage page = new PDPage(PDRectangle.A2);

                float startY = page.getMediaBox().getHeight() - PADDING;

                PDPageContentStream contentStream = new PDPageContentStream(document, page);

                float margin = 50;
                float yStartNewPage = page.getMediaBox().getHeight() - (2 * margin);
                float tableWidth = page.getMediaBox().getWidth() - (2 * margin);
                float yStart = yStartNewPage;
                float bottomMargin = 70;
                float yPosition = 550;

                java.util.List data = new java.util.ArrayList();

                data.add(new java.util.ArrayList(
                               java.util.Arrays.asList("Date","Description", "Transaction Value", "Acc. Transaction Value"
                               , "Change in Units", "Acc. No. of Units", "Unit Price"
                               , "Total Capital Gains", "Gross Investment Amount")));
                if (mFundUnitTrans != null)
                {
                    double previousAmount = 0;
                    double previousUnits = 0;
                double previosunitprice = 0;
                    foreach (FundUnitTrans mFundUnitTran in mFundUnitTrans)
                    {
                        String description = (mFundUnitTran.TransactionType== App.Purchase)
                        ?"Purchase of Units": (mFundUnitTran.TransactionType == App.Redemption) ? "Redemption of Units" : "N/A";
                        previousAmount = (mFundUnitTran.TransactionType == App.Purchase) ? previousAmount+mFundUnitTran.Amount 
                        : (mFundUnitTran.TransactionType == App.Redemption) ? previousAmount - mFundUnitTran.Amount:
                        mFundUnitTran.Amount;
                    previousUnits = (mFundUnitTran.TransactionType == App.Purchase) ? previousUnits + mFundUnitTran.Units 
                        : (mFundUnitTran.TransactionType == App.Redemption) ? previousUnits - mFundUnitTran.Units:
                        previousUnits;
                    double unitprice = 0;
                    if (mFund.Name.Equals("ZICA") && mFundUnitTran.DateInMillis < App.ZicaDefaults.DefaultZicaDate)
                        unitprice = 10;
                    else
                     unitprice = mFundUnitTran.Amount / mFundUnitTran.Units;
                    var tansValue = (mFundUnitTran.TransactionType == App.All) ? "" : mFundUnitTran.Amount + " " + mFund.Currency;
                    var chaingInUnits = (mFundUnitTran.TransactionType == App.All) ? "" : mFundUnitTran.Units.ToString();
                      double capitalGain = (unitprice- previosunitprice)*previousUnits;

                    previosunitprice = unitprice;
                        data.add(new java.util.ArrayList(
                                       java.util.Arrays.asList(mFundUnitTran.DateInMillisFormted,description, tansValue, previousAmount + " " + mFund.Currency
                                       , chaingInUnits, previousUnits.ToString(), unitprice.ToString()
                                       ,MathUtils.round( capitalGain,2), MathUtils.round(previousAmount,2))));
                    }
                }
            Console.WriteLine("img path " + System.IO.Directory.GetCurrentDirectory() + "/Resources/erc_start_logo.png");
            be.quodlibet.boxable.image.Image image = new be.quodlibet.boxable.image.Image(javax.imageio.ImageIO.read(new java.io.File(System.IO.Directory.GetCurrentDirectory() + "/Resources/erc_start_logo_out.png")));
            // imagine we have pretty big logo and we want scale that a little bit
            float imageWidth = 175;
            image = image.scaleByWidth(imageWidth);


                BaseTable dataTable = new BaseTable(yStart, yStartNewPage, bottomMargin, tableWidth, margin, document, page, true, true);

            be.quodlibet.boxable.Row headerRow = dataTable.createRow(15f);
            for(int i=0;i< ((java.util.ArrayList)data.get(0) ).size(); i++)
            {
               var value = ((java.util.ArrayList)data.get(0)).get(i).ToString();

                be.quodlibet.boxable.Cell cell = headerRow.createCell(100f/9f, value);
                cell.setFont(PDType1Font.HELVETICA);
                cell.setFillColor(Color.decode("#ff9800"));


            }

            dataTable.addHeaderRow(headerRow);
            for (int i=1;i< data.size();i++)
            {
                be.quodlibet.boxable.Row row = dataTable.createRow(10f);

                for (int k = 0; k < ((java.util.ArrayList)data.get(i)).size(); k++)
                {
                    var value = ((java.util.ArrayList)data.get(i)).get(k).ToString();

                    be.quodlibet.boxable.Cell cell = row.createCell(100f / 9f, value);
                    cell.setFont(PDType1Font.HELVETICA);
                    cell.setFillColor(Color.decode("#fafafa"));

                }
                
            }

            //   DataTable t = new DataTable(dataTable, page);

            //   t.addListToTable(data, DataTable.HASHEADER);

            dataTable.draw();

            image.draw(document, contentStream, 50, page.getMediaBox().getHeight()-25);

            contentStream.close();
                document.addPage(page);
                document.save(dir+"/" + clientName+".pdf");
                document.close();

                Console.WriteLine("Complete Exporting PDF");
           
            }
        public static async void ExportChartValuesNampak(List<ItemValue> mItemValues, Security[] mSecurities, String outpath)
        {
            await Task.Run(() =>
            {

                XSSFWorkbook templateworkbook;
                var path = System.IO.Directory.GetCurrentDirectory() + "/Resources/monthly_template.xlsx";
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    templateworkbook = new XSSFWorkbook(fs);


                }
                var newsheetName = "sheet1";
                //  XSSFWorkbook workBookExport = new XSSFWorkbook();

                //((XSSFSheet)templateworkbook.GetSheetAt(0)).CopyTo(workBookExport, newsheetName, true, true);
                XSSFSheet Sheet = (XSSFSheet)templateworkbook.GetSheetAt(0);
                //replaceInSheet(Sheet, new Item(date, Name, Amount, Units, MemberId, Email), first);

                IEnumerator rowIterator = Sheet.GetEnumerator();
                List<Object> data = new List<Object>();
                String preDate = "";
                while (rowIterator.MoveNext())
                {
                    IRow row = (IRow)rowIterator.Current;
                    IEnumerator<ICell> cellIterator = row.GetEnumerator();

                    while (cellIterator.MoveNext())
                    {
                        ICell cell = cellIterator.Current;
                        int mCellIndex = cell.ColumnIndex;
                        int rowIndex = cell.RowIndex;

                        if (rowIndex == 2 && mCellIndex == 1)
                        {
                            double value = 0;
                            foreach (var itemValue in mItemValues)
                            {
                                if (itemValue.Name.Equals("Bonds") || itemValue.Name.Equals("Treasury Bills"))
                                {
                                    value += Double.Parse(itemValue.Value);
                                }
                            }

                            cell.SetCellValue(value);
                        }

                        else if (rowIndex == 3 && mCellIndex == 1)
                        {
                            double value = 0;
                            foreach (var itemValue in mItemValues)
                            {
                                if (itemValue.Name.Equals("Equities"))
                                {
                                    value += Double.Parse(itemValue.Value);
                                }
                            }

                            cell.SetCellValue(value);
                        }


                        else if (rowIndex == 4 && mCellIndex == 1)
                        {
                            double value = 0;
                            foreach (var itemValue in mItemValues)
                            {
                                if (itemValue.Name.Equals("Cash"))
                                {
                                    value += Double.Parse(itemValue.Value);
                                }
                            }

                            cell.SetCellValue(value);
                        }

                        else if (rowIndex == 5 && mCellIndex == 1)
                        {
                            double value = 0;
                            foreach (var itemValue in mItemValues)
                            {
                                if (itemValue.Name.Equals("Term Deposits"))
                                {
                                    value += Double.Parse(itemValue.Value);
                                }
                            }

                            cell.SetCellValue(value);
                        }


                    }


                }


                String[] equitColms = new String[] {  "Stock","No. of shares","Monthly Price","Opening Value"
                    ,"Sold/Purchased","Dividens Applicable",
                "Monthly Change","Month end prices","Closing value","Portfolio performance"};
                var columCount = equitColms.Length;
                int currentRowIndex = 15;
                currentRowIndex++;
                IRow secRow = Sheet.CreateRow(currentRowIndex);
                for (int i = 0; i < equitColms.Length; i++)
                {
                    XSSFFont fontHeader = secRow.Sheet.Workbook.CreateFont() as XSSFFont;
                    fontHeader.FontHeightInPoints = ((short)11);
                    fontHeader.FontName = ("Arial Narrow");
                    fontHeader.IsBold = true;

                    XSSFCellStyle style = secRow.Sheet.Workbook.CreateCellStyle() as XSSFCellStyle;
                    style.SetFont(fontHeader);
                    ICell cell = secRow.CreateCell(i);
                    cell.CellStyle = style;
                    cell.SetCellValue(equitColms[i]);


                }
                foreach (Security mSecurity in mSecurities)
                {
                    if (mSecurity.Type.Equals(App.Types.ListedEquityType))
                    {
                        currentRowIndex++;
                        secRow = Sheet.CreateRow(currentRowIndex);
                        for (int i=0;i< equitColms.Length; i++)
                        {ICell cell = secRow.CreateCell(i);
                            if (i == 0)
                                cell.SetCellValue(mSecurity.Name);
                            else if (i == 1)
                                cell.SetCellValue(mSecurity.Nshares);

                            else if (i == 2)
                                cell.SetCellValue(mSecurity.Value);

                            else if (i == 3)
                                cell.SetCellValue(mSecurity.Value * mSecurity.Nshares);
                            else if (i == 6)
                                cell.SetCellValue((mSecurity.EndValue ) - (mSecurity.Value * mSecurity.Nshares));
                            else if (i == 7)
                                cell.SetCellValue(mSecurity.EndValue / mSecurity.Nshares);
                            else if (i == 8)
                                cell.SetCellValue(mSecurity.EndValue );
                        }
                    }
                }

                equitColms = new String[] {  "FIXED INCOME","COST","Accrued Interest","Market Value"
                    ,"Interest Received","Monthly Interest",
                "Monthly ROI%","Holding Return"};
                currentRowIndex++;
                currentRowIndex++;
                secRow = Sheet.CreateRow(currentRowIndex);
                // currentRowIndex = 15;
                for (int i = 0; i < equitColms.Length; i++)
                {
                    XSSFFont fontHeader = secRow.Sheet.Workbook.CreateFont() as XSSFFont;
                    fontHeader.FontHeightInPoints = ((short)11);
                    fontHeader.FontName = ("Arial Narrow");
                    fontHeader.IsBold = true;

                    XSSFCellStyle style = secRow.Sheet.Workbook.CreateCellStyle() as XSSFCellStyle;
                    style.SetFont(fontHeader);
                    ICell cell = secRow.CreateCell(i);
                    cell.CellStyle = style;
                    cell.SetCellValue(equitColms[i]);


                }



                foreach (Security mSecurity in mSecurities)
                {
                    if (!mSecurity.Type.Equals(App.Types.ListedEquityType))
                    {
                        currentRowIndex++;
                        secRow = Sheet.CreateRow(currentRowIndex);
                        for (int i = 0; i < equitColms.Length; i++)
                        {
                            ICell cell = secRow.CreateCell(i);
                            if (i == 0)
                                cell.SetCellValue(mSecurity.Name);
                            else if (i == 1)
                                cell.SetCellValue(mSecurity.Value);

                            else if (i == 2)
                                cell.SetCellValue(mSecurity.AccuredInterest);

                            else if (i == 3)
                                cell.SetCellValue(mSecurity.CurrentValue);
                            else if (i == 4)
                                cell.SetCellValue((mSecurity.InterestReceived));
                            else if (i == 5)
                                cell.SetCellValue(mSecurity.DailyInterest *30);
                            else if (i == 6)
                            {
                                
                                
                            XSSFCellStyle style = secRow.Sheet.Workbook.CreateCellStyle() as XSSFCellStyle;
                            style.SetDataFormat(secRow.Sheet.Workbook.CreateDataFormat().GetFormat("0.000%"));

                                cell.SetCellValue(((mSecurity.DailyInterest * 30) / mSecurity.Value) );
                                cell.CellStyle = style;

                            }
                            else if (i == 7)
                            {


                                XSSFCellStyle style = secRow.Sheet.Workbook.CreateCellStyle() as XSSFCellStyle;
                                style.SetDataFormat(secRow.Sheet.Workbook.CreateDataFormat().GetFormat("0.000%"));

                                cell.SetCellValue(((mSecurity.AccuredInterest) / mSecurity.Value));
                                cell.CellStyle = style;

                            }
                        }
                    }
                }


                for (int i = 0; i < columCount; i++)
                {
                    Sheet.AutoSizeColumn(i);

                }
                XSSFFormulaEvaluator.EvaluateAllFormulaCells(templateworkbook);

                using (FileStream file = new FileStream(outpath, FileMode.Create, FileAccess.Write))
                {
                    templateworkbook.Write(file);
                    file.Close();
                }

            });

        }
        public async void Import(String path)
        {

            try
            {
                await Task.Run(() =>
                {

                    XSSFWorkbook workbook = new XSSFWorkbook(new FileInfo(path));

                    //ICreationHelper CreateHelper = workBookExport.GetCreationHelper();
                    //IFormulaEvaluator evaluator = CreateHelper.CreateFormulaEvaluator();


                    XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(5);
                    String sheetName = sheet.SheetName;
                    IEnumerator rowIterator = sheet.GetEnumerator();
                    List<Object> data = new List<Object>();
                    String preDate = "06/03/2019";
                    Boolean first = true;
                    List<String> Saved = new List<string>();
                    while (rowIterator.MoveNext())
                    {


                        IRow row = (IRow)rowIterator.Current;
                        IEnumerator<ICell> cellIterator = row.GetEnumerator();
                        String date = "";
                        String Name = "";
                        String Amount = "";
                        String MemberId = "";
                        String Units = "";
                        String Email = "";
                        while (cellIterator.MoveNext())
                        {
                                ICell cell = cellIterator.Current;
                                int mCellIndex = cell.ColumnIndex;
                                int mRowIndex = cell.RowIndex;
                                String mCellValue = CellVal(cell);

                                if (mCellIndex == 0 && mCellValue.ToLower().StartsWith("paid on")) {
                                    date = mCellValue.ToLower().Replace("paid on ", "").Replace(".","/");
                                }

                                else if (mCellIndex == 2 && mCellValue.Length > 0 && !mCellValue.Equals("ZICA PROPERTY FUND REGISTER")
                                    && !mCellValue.Equals("Name") && !mCellValue.Equals("Total")) { Name = mCellValue; }
                                else if (mCellIndex == 3) { MemberId = mCellValue; }
                                
                                else if ( mCellIndex == 5) { Amount = mCellValue; }
                                else if ( mCellIndex == 6) { Units = mCellValue; }

                            

                        }
                        //date = preDate;
                        if (date.Length == 0)
                        {
                            date = preDate;
                        }
                        else
                        {
                            preDate = date;
                        }
                        if (Name.Replace(" ", "").Length > 4)
                        {
                            Console.WriteLine("Client " + Name);

                            HSSFWorkbook templateworkbook;

                            using (FileStream fs = new FileStream(@"C:\Users\pc\Desktop\Current Project Resources\ECR\Zica Clients\Book1.xls", FileMode.Open, FileAccess.Read))
                            {
                                templateworkbook = new HSSFWorkbook(fs);


                            }

                            HSSFSheet templatesheet = (HSSFSheet)templateworkbook.GetSheetAt(0);
                            //Console.WriteLine("date: " + date + " Name: " + Name + " Amount: " + Amount + " Units: " + Units);
                            //Console.WriteLine("templatesheet " + templatesheet.SheetName);
                            String newsheetName = Name.Replace("/", " ").Replace("."," ");
                            if (newsheetName.Length > 30)
                                newsheetName = newsheetName.Substring(0, 29);
                            // XSSFSheet Sheet = WorkBookExport.CreateSheet(newsheetName) as XSSFSheet;
                            try
                            {
                                
                                HSSFWorkbook workBookExport = new HSSFWorkbook();
                                templatesheet.CopyTo(workBookExport, newsheetName, true, true);

                                HSSFSheet Sheet = (HSSFSheet)workBookExport.GetSheet(newsheetName);
                                replaceInSheet(Sheet, new Item(date, Name, Amount, Units, MemberId, Email), first);
                                using (FileStream file = new FileStream(path.Replace("ZICA Register New.xlsx", newsheetName + ".xls"), FileMode.Create, FileAccess.Write))
                                {
                                    workBookExport.Write(file);
                                    file.Close();
                                }
                                Saved.Add(path.Replace("ZICA Register New.xlsx", newsheetName + ".xls"));


                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Failed for " + newsheetName + " Amount " + Amount + " Units " + Units);
                            }

                            //   HSSFSheet Sheet = (HSSFSheet)workBookExport.GetSheet(newsheetName);
                            //  replaceInSheet(Sheet, new Item(date, Name, Amount, Units, MemberId), first);
                            // first = false;
                        }


                    }
                    /*
                    double k = 0;
                    double len = Saved.Count();
                    foreach (String file in Saved)
                    {

                        HSSFWorkbook workBookExportfinal = new HSSFWorkbook();
                        if (!first)
                        {
                            Console.WriteLine("progress " + ((k/len)*(double)100));
                            Console.WriteLine("File " + file);
                            workBookExportfinal = new HSSFWorkbook(new FileStream(path.Replace("ZICA Register.xlsx", "statements" + ".xls"), FileMode.Open, FileAccess.Read));
                        }
                        HSSFWorkbook workbookTemp;
                        try
                        {
                            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                            {
                                workbookTemp = new HSSFWorkbook(fs);

                                if (first)
                                {
                                    ((HSSFSheet)workbookTemp.GetSheetAt(0)).CopyTo(workBookExportfinal, workbookTemp.GetSheetName(0), first, true);
                                    first = false;

                                }
                                else {
                                    ((HSSFSheet)workbookTemp.GetSheetAt(0)).CopyTo(workBookExportfinal, workbookTemp.GetSheetName(0), first, true);

                                    CopyStyle(workBookExportfinal);
                                  //  File.Delete(path.Replace("ZICA Register.xlsx", "statements" + (k - 1) + ".xls"));
                                }

                                
                                using (FileStream fsp = new FileStream(path.Replace("ZICA Register.xlsx", "statements"+ ".xls"), FileMode.Create, FileAccess.Write))
                                {
                                    workBookExportfinal.Write(fsp);
                                }

                                k++;
                                //   File.Delete(file);
                            }
                        }
                        catch (Exception e) { }
        }*/
                  //  CopyStyle(workBookExportfinal);

                    //evaluator.EvaluateAll();

                });
            }
            catch (Exception ex)
            {
            }


        }
        class Item
        {
            public Item(String Date, String Name, String Amount, String Units, String MemberID, String Email) {
                this.Date = Date;
                this.Name = Name;
                this.Amount = Amount;
                this.Units = Units;
                this.MemberID = MemberID;
                this.Email = Email;
            }
            public String Date { set; get; }
            public String Name { set; get; }
            public String Amount { set; get; }
            public String Units { set; get; }
            public String MemberID { set; get; }
            public String Email { set; get; }
        }
        private HSSFCellStyle getStyleForPosition(IRow Row,int rowIndex,int mCellIndex,Boolean isFirst)
        {
            Row.Sheet.SetColumnWidth(mCellIndex, 26);
            if (isFirst==true)
            {
                if (rowIndex == 1 && mCellIndex == 1 || rowIndex == 3 && mCellIndex == 1 || rowIndex == 4 && mCellIndex == 1 || rowIndex == 1 && mCellIndex == 9 || rowIndex == 2 && mCellIndex == 9)
                {

                    HSSFFont fontHeader = Row.Sheet.Workbook.CreateFont() as HSSFFont;
                    fontHeader.FontHeightInPoints = ((short)11);
                    fontHeader.FontName = ("Arial Narrow");
                    fontHeader.IsBold = true;

                    HSSFCellStyle style = Row.Sheet.Workbook.CreateCellStyle() as HSSFCellStyle;
                    style.SetFont(fontHeader);
                    return style;
                }
                else if (rowIndex == 2 && mCellIndex == 5 || rowIndex == 3 && mCellIndex == 5 || rowIndex == 4 && mCellIndex == 5)
                {

                    HSSFFont fontHeader = Row.Sheet.Workbook.CreateFont() as HSSFFont;
                    fontHeader.FontHeightInPoints = ((short)11);
                    fontHeader.FontName = ("Arial Narrow");
                    fontHeader.IsBold = true;

                    HSSFCellStyle style = Row.Sheet.Workbook.CreateCellStyle() as HSSFCellStyle;
                    style.SetFont(fontHeader);
                    style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Bottom;
                    return style;
                }

                else if (rowIndex == 8 && mCellIndex == 2 || rowIndex == 8 && mCellIndex == 3 || rowIndex == 8 && mCellIndex == 4
                    || rowIndex == 8 && mCellIndex == 5 || rowIndex == 8 && mCellIndex == 6 || rowIndex == 8 && mCellIndex == 7
                    || rowIndex == 8 && mCellIndex == 8)
                {

                    HSSFFont fontHeader = Row.Sheet.Workbook.CreateFont() as HSSFFont;
                    fontHeader.FontHeightInPoints = ((short)11);
                    fontHeader.FontName = ("Arial Narrow");
                    fontHeader.IsBold = false;

                    HSSFCellStyle style = Row.Sheet.Workbook.CreateCellStyle() as HSSFCellStyle;
                    style.SetFont(fontHeader);
                    style.BorderTop = (BorderStyle.Thin);
                    style.TopBorderColor = (IndexedColors.Black.Index);

                    style.BorderBottom = (BorderStyle.Thin);
                    style.BottomBorderColor = (IndexedColors.Black.Index);
                    return style;
                }
                else if (rowIndex == 8 && mCellIndex == 1)
                {

                    HSSFFont fontHeader = Row.Sheet.Workbook.CreateFont() as HSSFFont;
                    fontHeader.FontHeightInPoints = ((short)11);
                    fontHeader.FontName = ("Arial Narrow");
                    fontHeader.IsBold = false;

                    HSSFCellStyle style = Row.Sheet.Workbook.CreateCellStyle() as HSSFCellStyle;
                    style.SetFont(fontHeader);
                    style.BorderTop = (BorderStyle.Thin);
                    style.TopBorderColor = (IndexedColors.Black.Index);

                    style.BorderLeft = (BorderStyle.Thin);
                    style.LeftBorderColor = (IndexedColors.Black.Index);
                    style.BorderBottom = (BorderStyle.Thin);
                    style.BottomBorderColor = (IndexedColors.Black.Index);
                    return style;
                }

                else if (rowIndex == 9 && mCellIndex == 1 || rowIndex == 10 && mCellIndex == 1)
                {

                    HSSFFont fontHeader = Row.Sheet.Workbook.CreateFont() as HSSFFont;
                    fontHeader.FontHeightInPoints = ((short)11);
                    fontHeader.FontName = ("Arial Narrow");
                    fontHeader.IsBold = false;

                    HSSFCellStyle style = Row.Sheet.Workbook.CreateCellStyle() as HSSFCellStyle;
                    style.SetFont(fontHeader);

                    style.BorderLeft = (BorderStyle.Thin);
                    style.LeftBorderColor = (IndexedColors.Black.Index);
                    return style;
                }

                else if (rowIndex == 11 && mCellIndex == 1)
                {

                    HSSFFont fontHeader = Row.Sheet.Workbook.CreateFont() as HSSFFont;
                    fontHeader.FontHeightInPoints = ((short)11);
                    fontHeader.FontName = ("Arial Narrow");
                    fontHeader.IsBold = false;

                    HSSFCellStyle style = Row.Sheet.Workbook.CreateCellStyle() as HSSFCellStyle;
                    style.SetFont(fontHeader);

                    style.BorderTop = (BorderStyle.Thin);
                    style.TopBorderColor = (IndexedColors.Black.Index);
                    style.BorderLeft = (BorderStyle.Thin);
                    style.LeftBorderColor = (IndexedColors.Black.Index);
                    return style;
                }
                else if (rowIndex == 11 && mCellIndex == 2 || rowIndex == 11 && mCellIndex == 3 || rowIndex == 11 && mCellIndex == 4
                    || rowIndex == 11 && mCellIndex == 5 || rowIndex == 11 && mCellIndex == 6 || rowIndex == 11 && mCellIndex == 7
                    || rowIndex == 11 && mCellIndex == 8)
                {

                    HSSFFont fontHeader = Row.Sheet.Workbook.CreateFont() as HSSFFont;
                    fontHeader.FontHeightInPoints = ((short)11);
                    fontHeader.FontName = ("Arial Narrow");
                    fontHeader.IsBold = false;

                    HSSFCellStyle style = Row.Sheet.Workbook.CreateCellStyle() as HSSFCellStyle;
                    style.SetFont(fontHeader);

                    style.BorderTop = (BorderStyle.Thin);
                    style.TopBorderColor = (IndexedColors.Black.Index);
                    return style;
                }
                else if (rowIndex == 12 && mCellIndex == 1)
                {

                    HSSFFont fontHeader = Row.Sheet.Workbook.CreateFont() as HSSFFont;
                    fontHeader.FontHeightInPoints = ((short)11);
                    fontHeader.FontName = ("Arial Narrow");
                    fontHeader.IsBold = false;

                    HSSFCellStyle style = Row.Sheet.Workbook.CreateCellStyle() as HSSFCellStyle;
                    style.SetFont(fontHeader);
                    style.BorderBottom = (BorderStyle.Thin);
                    style.BottomBorderColor = (IndexedColors.Black.Index);

                    style.BorderLeft = (BorderStyle.Thin);
                    style.LeftBorderColor = (IndexedColors.Black.Index);
                    return style;
                }

                else if (rowIndex == 12 && mCellIndex == 2 || rowIndex == 12 && mCellIndex == 3 || rowIndex == 12 && mCellIndex == 4
                    || rowIndex == 12 && mCellIndex == 5 || rowIndex == 12 && mCellIndex == 6 || rowIndex == 12 && mCellIndex == 7
                    || rowIndex == 12 && mCellIndex == 8)
                {

                    HSSFFont fontHeader = Row.Sheet.Workbook.CreateFont() as HSSFFont;
                    fontHeader.FontHeightInPoints = ((short)11);
                    fontHeader.FontName = ("Arial Narrow");
                    fontHeader.IsBold = false;

                    HSSFCellStyle style = Row.Sheet.Workbook.CreateCellStyle() as HSSFCellStyle;
                    style.SetFont(fontHeader);
                    style.BorderBottom = (BorderStyle.Thin);
                    style.BottomBorderColor = (IndexedColors.Black.Index);
                    return style;

                }
                else if (rowIndex == 8 && mCellIndex == 9)
                {

                    HSSFFont fontHeader = Row.Sheet.Workbook.CreateFont() as HSSFFont;
                    fontHeader.FontHeightInPoints = ((short)11);
                    fontHeader.FontName = ("Arial Narrow");
                    fontHeader.IsBold = false;

                    HSSFCellStyle style = Row.Sheet.Workbook.CreateCellStyle() as HSSFCellStyle;
                    style.SetFont(fontHeader);
                    style.BorderTop = (BorderStyle.Thin);
                    style.TopBorderColor = (IndexedColors.Black.Index);

                    style.BorderRight = (BorderStyle.Thin);
                    style.RightBorderColor = (IndexedColors.Black.Index);
                    style.BorderBottom = (BorderStyle.Thin);
                    style.BottomBorderColor = (IndexedColors.Black.Index);
                    return style;
                }

                else if (rowIndex == 9 && mCellIndex == 9 || rowIndex == 10 && mCellIndex == 9 || rowIndex == 11 && mCellIndex == 9)
                {

                    HSSFFont fontHeader = Row.Sheet.Workbook.CreateFont() as HSSFFont;
                    fontHeader.FontHeightInPoints = ((short)11);
                    fontHeader.FontName = ("Arial Narrow");
                    fontHeader.IsBold = false;

                    HSSFCellStyle style = Row.Sheet.Workbook.CreateCellStyle() as HSSFCellStyle;
                    style.SetFont(fontHeader);
                    style.BorderTop = (BorderStyle.Thin);
                    style.TopBorderColor = (IndexedColors.Black.Index);

                    style.BorderRight = (BorderStyle.Thin);
                    style.RightBorderColor = (IndexedColors.Black.Index);
                    style.BorderBottom = (BorderStyle.Thin);
                    style.BottomBorderColor = (IndexedColors.Black.Index);
                    return style;
                }
                else
                {

                    HSSFFont fontHeader = Row.Sheet.Workbook.CreateFont() as HSSFFont;
                    fontHeader.FontHeightInPoints = ((short)11);
                    fontHeader.FontName = ("Arial Narrow");
                    fontHeader.IsBold = true;

                    HSSFCellStyle style = Row.Sheet.Workbook.CreateCellStyle() as HSSFCellStyle;
                    style.SetFont(fontHeader);
                    return style;
                }
            }
            else {
                return (HSSFCellStyle)Row.Sheet.Workbook.GetSheetAt(0).GetRow(rowIndex).GetCell(mCellIndex).CellStyle;
            }
        }

        private void CopyStyle(HSSFWorkbook workBookExportfinal)
        {
            for(int i=1;i< workBookExportfinal.NumberOfSheets; i++) {
                HSSFSheet Sheet =(HSSFSheet) workBookExportfinal.GetSheetAt(i);

                IEnumerator rowIterator = Sheet.GetEnumerator();
                List<Object> data = new List<Object>();
                String preDate = "";
                while (rowIterator.MoveNext())
                {


                    IRow row = (IRow)rowIterator.Current;
                    IEnumerator<ICell> cellIterator = row.GetEnumerator();

                    while (cellIterator.MoveNext())
                    {
                        ICell cell = cellIterator.Current;
                        int mCellIndex = cell.ColumnIndex;
                        int rowIndex = cell.RowIndex;
                        cell.CellStyle = workBookExportfinal.GetSheetAt(0).GetRow(rowIndex).GetCell(mCellIndex).CellStyle;
                    }


                }
            }

        }
        private void replaceInSheet(HSSFSheet Sheet, Item mItem, Boolean isFirst)
        {
            IEnumerator rowIterator = Sheet.GetEnumerator();
            List<Object> data = new List<Object>();
            String preDate = "";
            while (rowIterator.MoveNext())
            {


                IRow row = (IRow)rowIterator.Current;
                IEnumerator<ICell> cellIterator = row.GetEnumerator();
                
                String date = "";
                String Name = "";
                String Amount = "";
                String Units = "";
                while (cellIterator.MoveNext())
                {
                        ICell cell = cellIterator.Current;
                        int mCellIndex = cell.ColumnIndex;
                        int rowIndex = cell.RowIndex;
                    //cell = row.CreateCell(mCellIndex);

                   // cell.CellStyle = getStyleForPosition(row, rowIndex, mCellIndex, isFirst);
                   // cell.CellStyle.FillBackgroundColor = IndexedColors.Black.Index;
                    if (rowIndex==1 && mCellIndex == 1)
                        {
                            cell.SetCellValue("Name: " + mItem.Name);
                        }
                        else if (rowIndex == 1 && mCellIndex == 9)
                        {
                            cell.SetCellValue("ECRUT No: EUT18- " + mItem.MemberID);
                        }

                        else if (rowIndex == 4 && mCellIndex == 1)
                        {
                            cell.SetCellValue(DateTime.ParseExact(mItem.Date,"dd/MM/yyyy",null));
                        }

                        else if (rowIndex == 9 && mCellIndex == 3)
                        {
                            cell.SetCellValue(Double.Parse(mItem.Amount));
                        }
                        else if (rowIndex == 10 && mCellIndex == 1)
                        {
                            cell.SetCellValue(DateTime.ParseExact("30/04/2019", "dd/MM/yyyy", null));
                        }
                    

                }


            }

        }

        public static String CellVal(ICell cell)
        {

            if (cell.CellType == CellType.Formula)
            {
                String val = "";
                switch (cell.CachedFormulaResultType)
                {
                    case CellType.Numeric:

                        IFormulaEvaluator evaluator = cell.Sheet.Workbook.GetCreationHelper().CreateFormulaEvaluator();
                        evaluator.IgnoreMissingWorkbooks = true;
                        val = new DataFormatter().FormatRawCellContents(cell.NumericCellValue, cell.CellStyle.DataFormat
                            , cell.CellStyle.GetDataFormatString()).Replace(",", "").Replace("(", "-").Replace(")", "").Replace(" ", "");

                        break;
                    case CellType.String:
                        val = cell.StringCellValue;
                        break;
                }
                return val;
            }
            else if (cell.CellType == CellType.Numeric)
            {
                 String val = cell.NumericCellValue.ToString();
                    return val;
                
            }
            else
            {

                String val = cell.StringCellValue;
                return val;
            }
        }

        
    }
}
