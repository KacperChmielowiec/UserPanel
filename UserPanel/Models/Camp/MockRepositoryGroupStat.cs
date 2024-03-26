using UserPanel.Interfaces.Abstract;
using UserPanel.Services;

namespace UserPanel.Models.Camp
{
    public class MockRepositoryGroupStat : GroupStatRepository<GroupStat>
    {
        private readonly string Path = "appConfig.database.mock.groupStats";
        public override List<GroupStat> getGroupStatByCampId(Guid id)
        {
            return ConfigManager.GetConfig(Path).Parse<List<GroupStat>>().Where(c => c.Id_Camp == id).ToList();
        }

        public override GroupStat getGroupStatById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
