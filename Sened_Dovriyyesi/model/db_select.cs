using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;
using Syncfusion.XlsIORenderer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Renci.SshNet;

namespace Sened_Dovriyyesi.model
{
    public class db_select
    {
        static readonly string CryptoKey = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private readonly string ConnectionString;
        public IConfiguration Configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public db_select(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetSection("ConnectionStrings").GetSection("DefaultConnectionString").Value;
            _hostingEnvironment = hostingEnvironment;

        }


        public static string Encrypt(string pass)
        {

            byte[] clearBytes = Encoding.Unicode.GetBytes(pass);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(CryptoKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    pass = Convert.ToBase64String(ms.ToArray());
                }
            }
            return pass;
        }

        public static string Decrypt(string cipherText)
        {

            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(CryptoKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public List<User> log_in(string username, string password)
        {


            List<User> user = new List<User>();
            SqlConnection connection = new SqlConnection(ConnectionString);


            connection.Open();

            SqlCommand com = new SqlCommand("mobile.spUser", connection);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@login", username);
            com.Parameters.AddWithValue("@pass", Encrypt(password));

            SqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {


                while (reader.Read())
                {

                    User usr = new User();
                    usr.username = reader["UserName"].ToString();
                    usr.name = reader["PersonnelName"].ToString();
                    usr.dad_name = reader["PersonnelLastname"].ToString();
                    usr.surname = reader["PersonnelSurname"].ToString();
                    usr.role = reader["RoleId"].ToString();
                    usr.department = reader["DepartmentName"].ToString();
                    usr.position = reader["DepartmentPositionName"].ToString();
                    usr.password = Decrypt(reader["UserPassword"].ToString());
                    usr.workplaceID = reader["WorkplaceID"].ToString();


                    user.Add(usr);


                }
                connection.Close();
                return user;

            }
            else
            {
                return user;
            }

        }

        public List<docs> getDocs(string username, string password, int workplaceID, int pageIndex, int sendStatusId, string docNo, string docEnterNo,
            string docEnterDate, string docDocDate, string entryFromWhere, string docDescription, int documentStatusId, int docTypeId)
        {


            List<docs> docs_list = new List<docs>();
            SqlConnection connection = new SqlConnection(ConnectionString);


            connection.Open();

            SqlCommand com = new SqlCommand("mobile.spgetdocs", connection);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@login", username);
            com.Parameters.AddWithValue("@pass", Encrypt(password));
            com.Parameters.AddWithValue("@WorkplaceID", workplaceID);

            if (pageIndex > 0) com.Parameters.AddWithValue("@pageIndex", pageIndex);
            if (sendStatusId > 0) com.Parameters.AddWithValue("@sendStatusId", sendStatusId);
            if (!string.IsNullOrEmpty(docNo)) com.Parameters.AddWithValue("@docdocno", docNo);
            if (!string.IsNullOrEmpty(docEnterNo)) com.Parameters.AddWithValue("@docEnterNo", docEnterNo);
            if (!string.IsNullOrEmpty(docEnterDate)) com.Parameters.AddWithValue("@docEnterDate", docEnterDate); //qeydiyyattarixi
            if (!string.IsNullOrEmpty(docDocDate)) com.Parameters.AddWithValue("@docDocDate", docDocDate); //senedin tarixi
            if (!string.IsNullOrEmpty(entryFromWhere)) com.Parameters.AddWithValue("@entryFromWhere", entryFromWhere); // muellif melumatlari
            if (!string.IsNullOrEmpty(docDescription)) com.Parameters.AddWithValue("@docdescription ", docDescription); // qisa mezmun
            if (documentStatusId > 0) com.Parameters.AddWithValue("@documentStatusId", documentStatusId); // senedin statusu
            if (docTypeId > 0) com.Parameters.AddWithValue("@docTypeId", docTypeId); // senedin tipi


            SqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {


                while (reader.Read())
                {

                    docs doc = new docs();
                    doc.ID = Convert.ToInt32(reader["docid"]);
                    doc.DocNo = reader["DocDocNo"].ToString();
                    if (!string.IsNullOrEmpty(reader["SendStatusId"].ToString())) doc.SendStatusId = Convert.ToInt32(reader["SendStatusId"]);
                    doc.DocEnterNo = reader["DocEnterNo"].ToString();
                    doc.CreaterPersonnelName = reader["CreaterPersonnelName"].ToString();
                    doc.DocEnterdate = reader["DocEnterdate"].ToString();
                    doc.ExecuteRule = reader["ExecuteRule"].ToString();
                    doc.DocTypeId = Convert.ToInt32(reader["DocDoctypeId"]);
                    doc.DocControlStatusID = Convert.ToInt32(reader["ExecutorControlStatus"]);

                    docs_list.Add(doc);

                }
                connection.Close();
                return docs_list;

            }
            else
            {
                return docs_list;
            }

        }
        public List<menu> menu()
        {


            List<menu> menu = new List<menu>();
            SqlConnection connection = new SqlConnection(ConnectionString);


            connection.Open();

            SqlCommand com = new SqlCommand("mobile.spMenu", connection);
            com.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {


                while (reader.Read())
                {

                    menu appMenu = new menu();
                    switch (reader["id"] is int)
                    {
                        case false:
                            appMenu.id = 0;
                            break;
                        default:
                            appMenu.id = Convert.ToInt32(reader["id"]);
                            break;
                    }
                    switch (reader["parentID"] is int)
                    {
                        case false:
                            appMenu.parentID = 0;
                            break;
                        default:
                            appMenu.parentID = Convert.ToInt32(reader["parentID"]);
                            break;
                    }
                    switch (reader["docTypeID"] is int)
                    {
                        case false:
                            appMenu.docTypeID = 0;
                            break;
                        default:
                            appMenu.docTypeID = Convert.ToInt32(reader["docTypeID"]);
                            break;
                    }
                    appMenu.iconClass = reader["iconClass"].ToString();
                    appMenu.caption = reader["caption"].ToString();

                    menu.Add(appMenu);

                }
                connection.Close();
                return menu;

            }
            else
            {
                return menu;
            }

        }

        public List<categoties> categories(string username, string pass, int workplaceId)
        {


            List<categoties> _categories = new List<categoties>();
            SqlConnection connection = new SqlConnection(ConnectionString);


            connection.Open();

            SqlCommand com = new SqlCommand("mobile.spGetDocCount", connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@login", username);
            com.Parameters.AddWithValue("@pass", Encrypt(pass));
            com.Parameters.AddWithValue("@WorkplaceID", workplaceId);


            SqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {


                while (reader.Read())
                {

                    categoties docCategory = new categoties();

                    docCategory.SendStatusId = Convert.ToInt32(reader["SendStatusID"]);
                    docCategory.DocNo = Convert.ToInt32(reader["DocNo"]);
                    docCategory.SendStatusName = reader["SendStatusName"].ToString();
                    _categories.Add(docCategory);

                }
                connection.Close();
                return _categories;

            }
            else
            {
                return _categories;
            }

        }
        public List<DocumentStatus> DocStatus()
        {


            List<DocumentStatus> DocStatusList = new List<DocumentStatus>();
            SqlConnection connection = new SqlConnection(ConnectionString);


            connection.Open();

            SqlCommand com = new SqlCommand("SELECT  dd.DocumentstatusId, dd.DocumentstatusName FROM dbo.DOC_DOCUMENTSTATUS dd WHERE dd.DocumentstatusId in(" +

                "  1, 12, 31, 33, 36, 30, 14, 15, 35, 28, 8)", connection);




            SqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {


                while (reader.Read())
                {

                    DocumentStatus docStatus = new DocumentStatus();

                    docStatus.DocStatusId = Convert.ToInt32(reader["DocumentStatusId"]);
                    docStatus.DocStatusName = reader["DocumentStatusName"].ToString();

                    DocStatusList.Add(docStatus);

                }
                connection.Close();
                return DocStatusList;

            }
            else
            {
                return DocStatusList;
            }

        }




        public List<Files> getFiles(string username, string pass, int workplaceId, int docId)
        {
            List<Files> filesList = new List<Files>();
            SqlConnection connection = new SqlConnection(ConnectionString);


            connection.Open();



            SqlCommand com = new SqlCommand("mobile.spGetFiles", connection);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@login", username);
            com.Parameters.AddWithValue("@pass", Encrypt(pass));
            com.Parameters.AddWithValue("@docid", docId);
            com.Parameters.AddWithValue("@workplaceId", workplaceId);



            SqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {


                while (reader.Read())
                {

                    Files file = new Files();

                    file.docId = Convert.ToInt32(reader["FileDocId"]);
                    file.fileId = Convert.ToInt32(reader["FileInfoId"]);
                    file.filePath = reader["FileInfoPath"].ToString();
                    file.fileName = Regex.Replace(reader["FileInfoName"].ToString(), @"(\+|\%|\&|\@)", String.Empty); 
                    file.type = reader["FileInfoExtention"].ToString();
                    file.main = Convert.ToInt32(reader["FileIsMain"]);

                    filesList.Add(file);

                }
                connection.Close();
                return filesList;

            }
            else
            {
                return filesList;
            }


        }
        public List<Files> PdfConverter(string username, string pass, int workplaceId, int fileId)
        {
            try
            {

                List<Files> filesList = new List<Files>();
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                SqlCommand com = new SqlCommand("mobile.spGetFiles", connection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@login", username);
                com.Parameters.AddWithValue("@pass", Encrypt(pass));
                com.Parameters.AddWithValue("@workplaceId", workplaceId);
                com.Parameters.AddWithValue("@fileInfoId", fileId);
                SqlDataReader reader = com.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {

                        Files file = new Files();

                        file.docId = Convert.ToInt32(reader["FileDocId"]);
                        file.filePath = reader["FileInfoPath"].ToString();
                        file.fileName = reader["FileInfoName"].ToString();
                        file.type = reader["FileInfoExtention"].ToString();
                        file.main = Convert.ToInt32(reader["FileIsMain"]);

                        filesList.Add(file);

                    }
                    connection.Close();



                    //SFTP Connection

                    //string host = "";
                    //string SftpUsername = "";
                    //string SftpPassword = "";
                    //string localFileName = System.IO.Path.GetFileName("a.docx");
                    //string remoteFileName = "ftp://192.52.242.121/www.qwertytest.somee.com/temp/docx.docx";
                    ////string remoteFileName = host + "/" + filesList[0].filePath;
                    ////  string XlsRemoteFileName = "ftp://192.52.242.121/www.qwertytest.somee.com/temp/excel.xlsx";


                    string filename = Regex.Replace(filesList[0].fileName, @"(\+|\%|\&|\@)", String.Empty);

                    string Filepath = $"{_hostingEnvironment.ContentRootPath}/wwwroot/temp/{filename}";
                    string host = "10.10.7.124";
                    string SftpUsername = "esd";
                    string SftpPassword = "mhm@)!*123";
                    using (var sftp = new SftpClient(host, SftpUsername, SftpPassword))
                    {
                        sftp.Connect();

                        using (var pp = System.IO.File.OpenWrite(Filepath))
                        {
                            try
                            {


                                sftp.DownloadFile(@"/datadir/DMS_FILE/" + filesList[0].filePath, pp);
                            }
                            catch (Exception ex)
                            {
                                return null;
                            }
                        }

                        sftp.Disconnect();
                    }
                    if (filesList[0].type.ToLower() == ".doc" || filesList[0].type.ToLower() == ".docx")
                    {


                        //            string remoteFileName = "ftp://192.52.242.121/www.qwertytest.somee.com/temp/docx.docx";

                        //            FtpWebRequest request =
                        //(FtpWebRequest)WebRequest.Create(remoteFileName);
                        //            request.Credentials = new NetworkCredential("asadzade99", "Asadov99");
                        //            request.Method = WebRequestMethods.Ftp.DownloadFile;

                        //            using (Stream ftpStream = request.GetResponse().GetResponseStream())
                        //            using (Stream fileStream = System.IO.File.Create(Filepath))
                        //            {
                        //                ftpStream.CopyTo(fileStream);

                        //                ftpStream.Dispose();
                        //                fileStream.Dispose();
                        //            }



                        //SFTP Connection

                        //using (var sftp = new SftpClient(host, SftpUsername, SftpPassword))
                        //{
                        //    sftp.Connect();

                        //    using (var pp = System.IO.File.OpenWrite(Filepath))
                        //    {
                        //        sftp.DownloadFile(remoteFileName, pp);
                        //    }

                        //    sftp.Disconnect();
                        //}


                        //Word File Convert to PDF

                        //Open the file as Stream
                        FileStream docStream = new FileStream(Filepath, FileMode.Open, FileAccess.Read);
                        //Loads file stream into Word document
                        WordDocument wordDocument = new WordDocument(docStream, Syncfusion.DocIO.FormatType.Automatic);
                        docStream.Close();

                        //Instantiation of DocIORenderer for Word to PDF conversion
                        DocIORenderer render = new DocIORenderer();
                        //Sets Chart rendering Options.
                        render.Settings.ChartRenderingOptions.ImageFormat = Syncfusion.OfficeChart.ExportImageFormat.Jpeg;
                        //Converts Word document into PDF document
                        PdfDocument pdfDocument = render.ConvertToPDF(wordDocument);
                        //Releases all resources used by the Word document and DocIO Renderer objects
                        render.Dispose();
                        wordDocument.Dispose();
                        //Saves the PDF file
                        //MemoryStream outputStream = new MemoryStream();
                        //pdfDocument.Save(outputStream);
                        string filenamePdf = $"{workplaceId}_{Path.GetFileNameWithoutExtension(filename)}.pdf";
                        string FilepathPdf = _hostingEnvironment.ContentRootPath + "/wwwroot/temp/";
                        using (Stream fileStream = System.IO.File.Create(FilepathPdf + filenamePdf))
                        {
                            pdfDocument.Save(fileStream);

                        }


                        //Closes the instance of PDF document object
                        pdfDocument.Close();
                        //docStream.Close();
                        System.IO.File.Delete(Filepath);


                    }

                    else if (filesList[0].type.ToLower() == ".xls" || filesList[0].type.ToLower() == ".xlsx")
                    {

                        //Excell File Convert to PDF

                        string remoteFileName = "ftp://192.52.242.121/www.qwertytest.somee.com/temp/excell.xlsx";




                        FtpWebRequest XlsRequest =
                (FtpWebRequest)WebRequest.Create(remoteFileName);
                        XlsRequest.Credentials = new NetworkCredential("asadzade99", "Asadov99");
                        XlsRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                        using (Stream ftpStream = XlsRequest.GetResponse().GetResponseStream())
                        using (FileStream fileStream = System.IO.File.Create(Filepath))
                        {
                            ftpStream.CopyTo(fileStream);

                            ftpStream.Dispose();
                            fileStream.Dispose();
                        }

                        using (ExcelEngine excelEngine = new ExcelEngine())
                        {
                            IApplication application = excelEngine.Excel;
                            FileStream excelStream = new FileStream(Filepath, FileMode.Open, FileAccess.Read);
                            IWorkbook workbook = application.Workbooks.Open(excelStream);
                            excelStream.Dispose();
                            //Initialize XlsIO renderer.
                            XlsIORenderer renderer = new XlsIORenderer();

                            //Convert Excel document into PDF document 
                            PdfDocument pdfDocument = renderer.ConvertToPDF(workbook);

                            Stream stream = new FileStream($"{_hostingEnvironment.ContentRootPath}/wwwroot/temp/{workplaceId}_{Path.GetFileNameWithoutExtension(filename)}.pdf", FileMode.Create, FileAccess.ReadWrite);
                            pdfDocument.Save(stream);


                            stream.Dispose();
                        }

                        //using (var sftp = new SftpClient(host, SftpUsername, SftpPassword))
                        //{
                        //    sftp.Connect();

                        //    using (var pp = System.IO.File.OpenWrite(Filepath))
                        //    {
                        //        sftp.DownloadFile(remoteFileName, pp);
                        //    }

                        //    sftp.Disconnect();
                        //}


                        //try
                        //{
                        //    using (ExcelEngine excelEngine = new ExcelEngine())
                        //    {
                        //        IApplication application = excelEngine.Excel;
                        //        FileStream excelStream = new FileStream(Filepath, FileMode.Open, FileAccess.Read);
                        //        IWorkbook workbook = application.Workbooks.Open(excelStream);

                        //        //Initialize XlsIO renderer.
                        //        XlsIORenderer renderer = new XlsIORenderer();

                        //        //Convert Excel document into PDF document 
                        //        PdfDocument pdfDocument = renderer.ConvertToPDF(workbook);

                        //        Stream stream = new FileStream(Filepath + ".pdf", FileMode.OpenOrCreate, FileAccess.ReadWrite);

                        //        pdfDocument.Save(stream);

                        //        excelStream.Dispose();
                        //        stream.Dispose();
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    File.AppendAllText("error.log", $"{Environment.NewLine}{ex}{Environment.NewLine}------------------------------------------------");
                        //}
                        //}
                        System.IO.File.Delete(Filepath);
                    }

                    else
                    {

                        string remoteFileName = "ftp://192.52.242.121/www.qwertytest.somee.com/temp/pdf-sample.pdf";


                        FtpWebRequest XlsRequest =
                (FtpWebRequest)WebRequest.Create(remoteFileName);
                        XlsRequest.Credentials = new NetworkCredential("asadzade99", "Asadov99");
                        XlsRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                        using (Stream ftpStream = XlsRequest.GetResponse().GetResponseStream())
                        using (FileStream fileStream = System.IO.File.Create($"{_hostingEnvironment.ContentRootPath}/wwwroot/temp/{workplaceId}_{ Path.GetFileNameWithoutExtension(filename)}.pdf"))
                        {
                            ftpStream.CopyTo(fileStream);

                            ftpStream.Dispose();
                            fileStream.Dispose();
                        }


                        //using (var sftp = new SftpClient(host, SftpUsername, SftpPassword))
                        //{
                        //    sftp.Connect();

                        //    using (var pp = System.IO.File.OpenWrite(Filepath))
                        //    {
                        //        sftp.DownloadFile(remoteFileName, pp);
                        //    }

                        //    sftp.Disconnect();
                        //}



                    }

                  
                    filesList[0].fileName = filename;
                    return filesList;

                }
                else
                {
                    return filesList;
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("error.log", $"{Environment.NewLine}{ex}{Environment.NewLine}------------------------------------------------");
                return null;
            }
        }


        public string getDocView(string username, string pass, int workplaceId, int docId, int docTypeId)
        {
            string docViewJson = "";
            //StringBuilder docViewJson = new StringBuilder();

            SqlConnection connection = new SqlConnection(ConnectionString);

            if (log_in(username, pass).Count > 0)
            {
                connection.Open();



                SqlCommand com = new SqlCommand("dbo.GetDocView", connection);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@docid", docId);
                com.Parameters.AddWithValue("@workplaceId", workplaceId);
                com.Parameters.AddWithValue("@docTypeId", docTypeId);



                SqlDataReader reader = com.ExecuteReader();



                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        docViewJson += reader[0].ToString();
                    }
                    connection.Close();
                    return docViewJson;

                }


            }

            return docViewJson.ToString();

        }

    }
}
