namespace Dr_Home.BackgroundJobs
{
    public interface IManageSchedules
    {
        Task DeleteExpiredSchedules();
    }
}
