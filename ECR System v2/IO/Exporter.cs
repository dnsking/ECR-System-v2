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


        public async void SendEmailAddressAndFile(String path)
        {
            try
            {
                await Task.Run(() => {
                    String lastAddress = "marychiwala.mc@gmail.com";
                    ArrayList sents = new ArrayList();
                    

                    XSSFWorkbook workbook = new XSSFWorkbook(new FileInfo(path));

                    //ICreationHelper CreateHelper = workBookExport.GetCreationHelper();
                    //IFormulaEvaluator evaluator = CreateHelper.CreateFormulaEvaluator();


                    XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(3);
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
                                && !mCellValue.Equals("Name") && !mCellValue.Equals("Total")) { Name = mCellValue; }

                            else if ((mCellIndex == 6 && mRowIndex < 16 && mCellValue.Contains("@"))
                            || (mCellIndex == 7 && mRowIndex > 16 && mCellValue.Contains("@"))) { Email = mCellValue; }
                            else if (mCellIndex == 3) { MemberId = mCellValue; }

                            else if (mCellIndex == 5) { Amount = mCellValue; }
                            else if (mCellIndex == 6) { Units = mCellValue; }



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
                        if (Name.Replace(" ", "").Length > 4 && Email.Length>0)
                        {
                            //Console.WriteLine("date: " + date + " Name: " + Name + " Amount: " + Amount + " Units: " + Units);
                            //Console.WriteLine("templatesheet " + templatesheet.SheetName);
                            String newsheetName = Name.Replace("/", " ").Replace(".", " ");
                            if (newsheetName.Length > 30)
                                newsheetName = newsheetName.Substring(0, 29);
                            sents.Add(Email);
                            String statementPath = GetExportPdf(newsheetName);
                            if (statementPath.Length > 0 && sents.Contains(lastAddress))
                            {

                                Console.WriteLine("Sending to  " + Name + " Email " + Email);
                                SendMail(Name, statementPath, Email);
                            }
                            else
                            {

                                //Console.WriteLine("Failed for  " + Name + " Email " + Email);
                            }


                        }


                    }

                });
            }
            catch(Exception e) { }
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
                    style.Alignment = HorizontalAlignment.Center;
                    style.VerticalAlignment = VerticalAlignment.Bottom;
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
                            cell.SetCellValue(DateTime.ParseExact(mItem.Date,"dd/M/yyyy",null));
                        }

                        else if (rowIndex == 9 && mCellIndex == 3)
                        {
                            cell.SetCellValue(Double.Parse(mItem.Amount));
                        }
                        else if (rowIndex == 10 && mCellIndex == 1)
                        {
                            cell.SetCellValue(DateTime.ParseExact("30/04/2019", "dd/M/yyyy", null));
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
