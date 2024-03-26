﻿namespace UserPanel.Interfaces.Abstract
{
    public abstract class GroupStatRepository<T>
    {
        public abstract T getGroupStatById(Guid id);
        public abstract List<T> getGroupStatByCampId(Guid id);
    }
}
