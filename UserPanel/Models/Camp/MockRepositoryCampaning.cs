using AutoMapper;
using System.Data;
using UserPanel.Services;
using UserPanel.Models;
using UserPanel.Models.Camp;
using UserPanel.Interfaces.Abstract;

namespace UserPanel.Models.User
{
    public class MockRepositoryCampaning : CampaningRepository<Campaning>
    {
        private readonly string Path = "appConfig.database.mock.campanings";
        public MockRepositoryCampaning()
        {
            
        }

        public override Campaning? getCampaningById(Guid id)
        {
            return ConfigManager.GetConfig(Path).Parse<List<Campaning>>().Where(c => c.id == id).FirstOrDefault();
        }

        public override List<Campaning>? getCampaningsByUser(int userId)
        {
            return ConfigManager.GetConfig(Path).Parse<List<Campaning>>();
        }

    }
}
