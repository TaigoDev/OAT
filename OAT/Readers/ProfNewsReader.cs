using MySqlConnector;
using Recovery.Tables;
using RepoDb;

namespace OAT.Readers
{
    public class ProfNewsReader
    {
        public static List<ProfNews> news = new List<ProfNews>();
        public static IEnumerable<IEnumerable<ProfNews>> pages = new List<List<ProfNews>>();

        public static async void init()
        {
            try
            {
                using var connection = new MySqlConnection(Utils.GetConnectionString());
                var records = await connection.QueryAllAsync<ProfNews>();
                news = records.ToList();
                news.Reverse();
                pages = news.PagesSplit(1);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

    }
}
