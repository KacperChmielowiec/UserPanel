using UserPanel.Types;
using UserPanel.Models;
namespace UserPanel.Services.observable
{
    public class CampActionObserver : Observer<CampActionMessage>
    {

        public CampActionObserver(Subject<CampActionMessage> _subject) : base(_subject)
        {
        }

        public override void notify(CampActionMessage context)
        {
            throw new NotImplementedException();
        }
    }
}
