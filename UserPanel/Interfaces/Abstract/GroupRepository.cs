using UserPanel.Models.Group;

namespace UserPanel.Interfaces.Abstract
{
    public abstract class GroupRepository<T>
    {
        public abstract List<T> GetGroupsByCampId(Guid id);
        public abstract List<T> GetGroupsByUserId(int id);
        public abstract void UpdateGroup(Guid id, T model);
        public abstract T GetGroupById(Guid id);
        public abstract void CreateGroup(Guid id, T model);
        public abstract GroupAdvertJoin GroupJoinAdvert(Guid id);
        public abstract void DeleteGroup(Guid id);
    }
}
