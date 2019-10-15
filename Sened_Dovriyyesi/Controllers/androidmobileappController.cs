using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sened_Dovriyyesi.Controllers
{
    public class users {
      public  string name { get; set; }
       public string surname { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]

    public class androidmobileappController : ControllerBase
    {
        [HttpGet]
        [Route("users")]
        public ActionResult<List<users>> getUsers(string name,string surname)
        {
            List<users> users = new List<users>
        {
            new users { name="Rufat", surname="Asadov" },
            new users { name="Sabina", surname="Aghayeva" },
            new users { name=name, surname=surname }
        };
            
            return  users;
        }
    }
}