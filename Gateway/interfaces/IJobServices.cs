namespace Gateway.interfaces
{
    public interface IJobServices
    {
        public dynamic Instance { get; set; }

        void LoadJob();
    }
}