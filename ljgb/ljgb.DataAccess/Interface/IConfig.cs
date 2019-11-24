using ljgb.DataAccess.Model;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface IConfig
    {
        Task<string> GetValue(Config config);
    }
}
