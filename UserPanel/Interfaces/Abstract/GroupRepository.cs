﻿namespace UserPanel.Interfaces.Abstract
{
    public abstract class GroupRepository<T>
    {
        public abstract List<T> GetGroupsByCampId(Guid id);
        public abstract List<T> GetGroupsByUserId(int id);
    }
}