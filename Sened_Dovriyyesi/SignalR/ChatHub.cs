using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using System.Timers;

namespace Sened_Dovriyyesi.SignalR
{
    public class ChatHub : Hub
    {
        int last_doc_id;


        public IConfiguration Configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ChatHub(IConfiguration _iconfiguration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = _iconfiguration;
            _hostingEnvironment = hostingEnvironment;
        }
        public void StartTimer()
        {
            Timer myTimer = new Timer();
            myTimer.Elapsed += new ElapsedEventHandler(Execute);
            myTimer.Interval = 1000; // 1000 ms is one second
            myTimer.Start();



        }
        public async void Execute(object source, ElapsedEventArgs e)
        {

            model.db_select db = new model.db_select(Configuration, _hostingEnvironment);
            List<model.docs> docs = new List<model.docs>();


            docs = db.getDocs("aida.aliyeva", "123", 3, 0, 0, "", "", "", "", "", "", 0, 0);
            await this.Clients.All.SendAsync("ReceiveNewDoc", docs);


            //bool newDoc;
            //newDoc = db.checkLastDoc();
            //switch (newDoc)
            //{
            //    case true:
            //        await this.Clients.All.SendAsync("ReciveNewDoc", docs);
            //        break;
            //    case false:

            //        break;
            //}


        }

        public async Task SendMessage(string username, string message)
        {

            await this.Clients.All.SendAsync("ReceiveMessage", username, message);
        }

        public async Task getDocs(int id)
        {
            last_doc_id = id;
            //  StartTimer();
            string msg = "";
            //List<string> docs = new List<string>();
            //docs.Add("Rufat");
            //docs.Add("bb");

            await this.Clients.All.SendAsync("ReciveList", msg);
        }

    }
}
