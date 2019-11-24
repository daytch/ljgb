using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Repository
{
    public class ConfigRepository : IConfig
    {
        ljgbContext db;

        public ConfigRepository(ljgbContext _db)
        {
            db = _db;
        }

        public async Task<string> GetValue(Config config)
        {
            string result = string.Empty;
            try
            {
                if (db != null)
                {
                    Config c = await db.Config.FirstOrDefaultAsync(x => x.RowStatus == true && x.Name == config.Name);
                    result = c.Value;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
