using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


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
        [Route("users")]
        [EnableCors("AllowOrigin")]
        public ActionResult<List<model.User>> getUsers(string name, string surname)
        {
            List<model.User> users = new List<model.User>
        {
            new model.User { name="Rufat", password="Asadov" },
            new model.User { name="Sabina", password="Aghayeva" },
            new model.User { name=name, password=surname }
        };

            return users;
        }
        [HttpGet]
        [Route("docs")]
        [EnableCors("AllowOrigin")]
        public ActionResult<model.docs> getDocs(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            else
            {
                try
                {
                    model.docs finded_doc = null;
                    List<model.docs> docs = new List<model.docs>
        {
            new model.docs { id =1, name="dfoewkdpoewk" },
            new model.docs { id=5, name="lmlckamlmc" },
            new model.docs { id=8, name="dslcmlwkdm" }
        };
                    foreach (var item in docs)
                    {
                        if (item.id == Convert.ToInt32(id))
                        {
                            finded_doc = item;

                        }

                    }
                    return finded_doc;
                }
                catch
                {

                    return null;
                }
            }

        }



        [HttpGet]
        [Route("user/login")]
        [EnableCors("AllowOrigin")]
        public ActionResult <List<model.User>> log_in(string username,string pass)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pass))
            {
                List<model.User> empty = new List<model.User>();
                return empty;
            }
            else
            {


                List<model.User> user = new List<model.User>();
                model.db_select select = new model.db_select(Configuration);
                user = select.log_in(username, pass);

                return user;
            }
        }
    }
}