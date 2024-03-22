using AutoMapper;
using System.Data;
using UserPanel.Services;
using UserPanel.Models;
using UserPanel.Models.Campaning;
using UserPanel.Interfaces.Abstract;

namespace UserPanel.Models.User
{
    public class MockRepositoryCampaning : CampaningRepository<Campaning.Campaning>
    {
        private string Path = "appConfig.database.mock.campanings";
        public MockRepositoryCampaning()
        {
            
        }

        public override Campaning.Campaning? getCampaningById(Guid id)
        {
            return ConfigManager.GetConfig(Path).Parse<List<Campaning.Campaning>>().Where(c => c.id == id).FirstOrDefault();
        }

        public override List<Campaning.Campaning>? getCampaningsByUser(int userId)
        {
            return ConfigManager.GetConfig(Path).Parse<List<Campaning.Campaning>>();
        }
    }
}
