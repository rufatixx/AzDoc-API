
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Sened_Dovriyyesi.model;
using Sened_Dovriyyesi.SignalR;
using System;
using System.Collections.Generic;


namespace Sened_Dovriyyesi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class androidmobileappController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public IConfiguration Configuration;
        public androidmobileappController(IConfiguration configuration, IHubContext<ChatHub> hubContext, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hubContext = hubContext;
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpGet]
        [Route("checkVersion")]
        [EnableCors("AllowOrigin")]
        public ActionResult checkVersion(string version, string build)
        {
            string updateInfo;
           

            if (version != Configuration.GetSection("appVersion").GetSection("version").Value || build != Configuration.GetSection("appVersion").GetSection("buildNumber").Value)
            {
                updateInfo = $"Versiya: {Configuration.GetSection("appVersion").GetSection("version").Value} (Quraşdırma nömrəsi: {Configuration.GetSection("appVersion").GetSection("buildNumber").Value})\n{Configuration.GetSection("appVersion").GetSection("features").Value}";
                return Ok(updateInfo);
            }
            return Ok();
        }

        [HttpGet]
        [Route("getDocs")]
        [EnableCors("AllowOrigin")]
        public ActionResult<List<docs>> getDocs(string username, string pass, int workplaceID, int pageIndex, int sendStatusId, string docNo,
            string docEnterNo, string docEnterDate, string docDocDate, string entryFromWhere, string docDescription, int documentStatusId, int docTypeId)
        {
            List<model.docs> not_read = new List<model.docs>();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(pass) && workplaceID > 0)
            {
                model.db_select select = new model.db_select(Configuration, _hostingEnvironment);
                not_read = select.getDocs(username, pass, workplaceID, pageIndex, sendStatusId, docNo, docEnterNo, docEnterDate, docDocDate, entryFromWhere, docDescription, documentStatusId, docTypeId);
                return not_read;


            }
            else
            {
                return not_read;

            }

        }




        [HttpGet]
        [Route("user/login")]
        [EnableCors("AllowOrigin")]
        public ActionResult<List<model.User>> log_in(string username, string pass)
        {
            List<model.User> user = new List<model.User>();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pass))
            {

                return user;
            }
            else
            {



                model.db_select select = new model.db_select(Configuration, _hostingEnvironment);
                user = select.log_in(username, pass);

                return user;
            }
        }
        [HttpGet]
        [Route("getMenu")]
        [EnableCors("AllowOrigin")]
        public ActionResult<List<model.menu>> get_Menu()
        {



            List<model.menu> menu = new List<model.menu>();
            model.db_select select = new model.db_select(Configuration, _hostingEnvironment);
            menu = select.menu();
            return menu;

        }
        [HttpGet]
        [Route("user/getCategories")]
        [EnableCors("AllowOrigin")]
        public ActionResult<List<model.categoties>> getCategories(string username, string pass, int WorkPlaceId)
        {
            List<model.categoties> _categories = new List<model.categoties>();
            try
            {
                model.db_select select = new model.db_select(Configuration, _hostingEnvironment);
                _categories = select.categories(username, pass, WorkPlaceId);
                return _categories;
            }
            catch (Exception)
            {
                return _categories;

            }
        }


        [HttpGet]
        [Route("getDocStatus")]
        [EnableCors("AllowOrigin")]
        public ActionResult<List<model.DocumentStatus>> getCategories()
        {
            //List<string> docs = new List<string>();
            //docs.Add(msg);

            List<model.DocumentStatus> docStatusList = new List<model.DocumentStatus>();
            try
            {
                model.db_select select = new model.db_select(Configuration, _hostingEnvironment);
                docStatusList = select.DocStatus();
                return docStatusList;
            }
            catch (Exception)
            {
                return docStatusList;

            }


        }

        [HttpGet]
        [Route("user/testAsync")]
        [EnableCors("AllowOrigin")]
        public async void TestAsync(string msg)
        {
            //List<string> docs = new List<string>();
            //docs.Add(msg);


            await _hubContext.Clients.All.SendAsync("ReciveList", msg);

        }

        [HttpGet]
        [Route("user/getFiles")]
        [EnableCors("AllowOrigin")]
        public ActionResult<List<Files>> getFiles(string username, string pass, int workplaceId, int docId)
        {
            List<Files> files = new List<Files>();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(pass) && workplaceId > 0 && docId > 0)
            {

                db_select dbSelect = new db_select(Configuration, _hostingEnvironment);
                files = dbSelect.getFiles(username, pass, workplaceId, docId);
                return files;
            }
            else
            {
                return files;
            }








        }
        [HttpGet]
        [Route("user/convertFile")]
        [EnableCors("AllowOrigin")]
        public ActionResult<List<Files>> convertFile(string username, string pass, int workplaceId, int fileId)
        {


            List<Files> convertedFile = new List<Files>();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(pass) && workplaceId > 0 && fileId > 0)
            {

                db_select dbSelect = new db_select(Configuration, _hostingEnvironment);
                convertedFile = dbSelect.PdfConverter(username, pass, workplaceId, fileId);

                return convertedFile;
            }
            else
            {
                return convertedFile;

            }







        }


        [HttpGet]
        [Route("user/getDocView")]
        [EnableCors("AllowOrigin")]
        public ActionResult<string> getDocView(string username, string pass, int workplaceId, int docId,int docTypeId)
        {


            string docViewJson = ""; 
                
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(pass) && workplaceId > 0 && docId > 0 && docTypeId > 0)
            {

                db_select dbSelect = new db_select(Configuration, _hostingEnvironment);
                docViewJson = dbSelect.getDocView(username, pass, workplaceId,docId, docTypeId);

                return docViewJson;
            }
            else
            {
                return docViewJson;

            }







        }

    }
}