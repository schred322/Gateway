namespace Gateway.Interfaces
{
    public interface IJobService
    {
        public dynamic FileParam { get; set; }
        public dynamic FileInfo { get; set; }
        public dynamic Task { get; set; } 
    }
}