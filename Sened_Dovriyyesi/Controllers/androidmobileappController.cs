using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;


namespace Sened_Dovriyyesi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class androidmobileappController : ControllerBase
    {
        public IConfiguration Configuration;
        public androidmobileappController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        [Route("get_not_read_docs")]
        [EnableCors("AllowOrigin")]
        public ActionResult<List<model.docs>> getNotReadtDocs(string username, string pass, int workplaceID, int pageIndex)
        {
            List<model.docs> not_read = new List<model.docs>();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pass) || workplaceID<1||pageIndex<1)
            {

                return not_read;
            }
            else
            {

                model.db_select select = new model.db_select(Configuration);
                not_read = select.get_not_read_docs(username, pass, workplaceID,pageIndex);
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



                model.db_select select = new model.db_select(Configuration);
                user = select.log_in(username, pass);

                return user;
            }
        }
        [HttpGet]
        [Route("get_Menu")]
        [EnableCors("AllowOrigin")]
        public ActionResult<List<model.menu>> get_Menu()
        {



            List<model.menu> menu = new List<model.menu>();
            model.db_select select = new model.db_select(Configuration);
            menu = select.menu();
            return menu;

        }

    }
}