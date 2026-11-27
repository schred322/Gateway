namespace Gateway.Interfaces
{
    public interface IJobService
    {
        public dynamic DbDataList { get; set; }
        public dynamic FileParams { get; set; }
        public dynamic FileInfo { get; set; }
        public dynamic Task { get; set; } 
    }
}