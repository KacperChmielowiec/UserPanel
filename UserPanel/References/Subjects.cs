﻿using UserPanel.Services.observable;

namespace UserPanel.References
{
    public static class Subjects
    {
        public static DataActionSubject dataActionSubject = new DataActionSubject();
        public static UserActionSubject userActionSubject = new UserActionSubject();
    }
}