﻿namespace UserPanel.Services
{
    public abstract class Observer<T>
    {
        protected Subject<T> subject;
        public Observer(Subject<T> _subject) {
            subject = _subject;
        }
        public abstract void notify(T context);

        public void remove()
        {
            subject.detach(this);
        }

    }
}
