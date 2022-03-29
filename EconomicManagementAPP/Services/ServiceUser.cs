namespace EconomicManagementAPP.Services
{
    public interface IServiceUser
    {
        int GetUserId();
    }
    public class ServiceUser : IServiceUser
    {
        public int GetUserId()
        {
            return 1;
        }
    }
}
