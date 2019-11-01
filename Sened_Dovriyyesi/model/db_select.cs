using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
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

            SqlCommand com = new SqlCommand("mobile.spUser", connection);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@login", username);
            com.Parameters.AddWithValue("@pass", password);

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
                    usr.password = reader["UserPassword"].ToString();
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
        public List<docs> get_not_read_docs(string username, string password,int workplaceID,int pageIndex)
        {


            List<docs> docs_list = new List<docs>();
            SqlConnection connection = new SqlConnection(Configuration.GetSection("ConnectionStrings").GetSection("DefaultConnectionString").Value);


            connection.Open();

            SqlCommand com = new SqlCommand("mobile.spgetdocs", connection);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@login", username);
            com.Parameters.AddWithValue("@pass", password);
            com.Parameters.AddWithValue("@WorkplaceID", workplaceID);
            com.Parameters.AddWithValue("@pageIndex", pageIndex);

            SqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {


                while (reader.Read())
                {

                    docs doc = new docs();
                    doc.ID = Convert.ToInt32(reader["docid"]);
                   
                    doc.DocEnterNo = reader["DocEnterNo"].ToString();
                    doc.Signer = reader["Signer"].ToString();
                    doc.DocEnterdate = reader["DocEnterdate"].ToString(); 
                    doc.DocumentStatusName = reader["DocumentStatusName"].ToString();
                    doc.DocControlStatusID= Convert.ToInt32(reader["ExecutorControlStatus"]);

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
            SqlConnection connection = new SqlConnection(Configuration.GetSection("ConnectionStrings").GetSection("DefaultConnectionString").Value);


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
    }
}
