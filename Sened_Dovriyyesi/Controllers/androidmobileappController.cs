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
    public class users
    {
        public string name { get; set; }
        public string surname { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class androidmobileappController : ControllerBase
    {
        private readonly IConfiguration configuration;
        [HttpGet]
        [Route("users")]
        [EnableCors("AllowOrigin")]
        public ActionResult<List<users>> getUsers(string name, string surname)
        {
            List<users> users = new List<users>
        {
            new users { name="Rufat", surname="Asadov" },
            new users { name="Sabina", surname="Aghayeva" },
            new users { name=name, surname=surname }
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
        [Route("database_test")]
        [EnableCors("AllowOrigin")]
        public ActionResult<string> database_test()
        {
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand com = new SqlCommand("Select UserName from DC_USER limit 1",connection);
            string name = (string)com.ExecuteScalar();
            connection.Close();
        return name;
           }
    }
}