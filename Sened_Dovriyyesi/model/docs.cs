namespace Sened_Dovriyyesi.model
{
    public class docs
    {
        public int ID { get; set; }

        public string DocEnterNo { get; set; }
        public int SendStatusId { get; set; } = 0;
        public string DocNo { get; set; }
        public string DoDocNo { get; set; }
        public string CreaterPersonnelName { get; set; }
        public string DocEnterdate { get; set; }
        public int DocumentStatusID { get; set; }
        public string DocumentStatusName { get; set; }
        public string ExecuteRule { get; set; }
        public int DocTypeId { get; set; }

        public int DocControlStatusID { get; set; }

    }
}
