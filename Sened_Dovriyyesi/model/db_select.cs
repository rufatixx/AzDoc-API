using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Sened_Dovriyyesi.model
{
    public class db_select
    {
        public IConfiguration Configuration;
        public db_select(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public List<User> log_in(string username, string password)
        {

            List<User> user = new List<User>();
            SqlConnection connection = new SqlConnection(Configuration.GetSection("ConnectionStrings").GetSection("DefaultConnectionString").Value);


            connection.Open();

            SqlCommand com = new SqlCommand("Select * from DC_USER where UserStatus=1 and UserName=@login and UserPassword=@pass", connection);
            com.Parameters.AddWithValue("@login", username);
            com.Parameters.AddWithValue("@pass", password);

            SqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {


                while (reader.Read())
                {

                    User usr = new User();
                    usr.name = reader["UserName"].ToString();
                    usr.password = reader["UserPassword"].ToString();
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
    }
}
