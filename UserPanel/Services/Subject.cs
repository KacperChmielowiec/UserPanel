using UserPanel.Interfaces.Abstract;

namespace UserPanel.Services
{
    public class Subject<T>
    {
        public Subject() { }
        private List<Observer<T>> _observers = new List<Observer<T>>();
        public Subject<T> attach(Observer<T> observer)
        {
            if(_observers.FirstOrDefault(obs => obs.GetHashCode() == observer.GetHashCode()) == null)
                _observers.Add(observer);
            return this;
        }
        public Subject<T> detach(Observer<T> observer)
        {
            _observers.RemoveAll(obj => obj.GetHashCode() == observer.GetHashCode());

            return this;
        }
        public void notify(T message)
        {
            foreach (var observer in _observers)
            {
                observer.notify(message);
            }
        }
    }
}
