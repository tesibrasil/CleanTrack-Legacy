namespace CleanTrackPi.Models
{
    public class Device
    {
        public int ID { get; set; }
        public string Matricola { get; set; }
        public string Serial { get; set; }
        public string Description { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }
        public string Tag { get; set; }
    }
}
