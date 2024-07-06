using UserPanel.Models.Camp;
using UserPanel.Models.Group;
using UserPanel.Models.Adverts;

namespace UserPanel.Models
{
    public class PermissionContextUserPanel : PermissionContext<Guid>
    {
        public int UserId { get; set; }
        public override PermissionContext<Guid> SetupContext(Func<FullContext> fetch)
        {
            try
            {
                FullContext context = fetch();
                if (context is FullUserContext user)
                {

                    foreach (Campaning campaning in user.Campanings)
                    {
                        if (!MapContext.ContainsKey(campaning.id))
                        {
                            MapContext[campaning.id] = CampaningContextMap(campaning);
                        }
                    }

                    foreach (GroupModel groupModel in user.Groups)
                    {
                        if (!MapContext.ContainsKey(groupModel.id))
                        {
                            MapContext[groupModel.id] = GroupContextMap(groupModel, groupModel.Parent);
                        }
                    }

                    foreach (Advert adModel in user.Adverts)
                    {
                        if (!MapContext.ContainsKey(adModel.Id))
                        {
                            MapContext[adModel.Id] = AdContextMap(adModel, adModel.Parent);
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Bad Type of FullContext !");
                }

            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Error while fetching FullContext Data", ex);
            }

            return this;
        }

        private ContextNode<Guid> GroupContextMap(GroupModel groupModel, Guid parent)
        {
            var Value = new ContextElement<Guid>() { ID = groupModel.id, ElementType = ContextNodeType.Group };
            var Parent = new ContextElement<Guid>() { ID = parent, ElementType = ContextNodeType.Camp };

            if (parent == Guid.Empty) throw new ArgumentException("Empty Parent Guid");

            if(MapContext.ContainsKey(parent))
            {
                MapContext[parent].Children.Add(Value);
            }
            else
            {
                throw new InvalidDataException("Parent does not exists");
            }

            return new ContextNode<Guid>(Value, Parent, MapContext);
        }

        private ContextNode<Guid> CampaningContextMap(Campaning campaning)
        {
            var Value = new ContextElement<Guid>() { ID = campaning.id, ElementType = ContextNodeType.Camp };

            return new ContextNode<Guid>(Value, MapContext);
        }

        private ContextNode<Guid> AdContextMap(Advert adModel, Guid parent)
        {
            var Value = new ContextElement<Guid>() { ID = adModel.Id, ElementType = ContextNodeType.ADVERT };
            var Parent = new ContextElement<Guid>() { ID = parent, ElementType = ContextNodeType.Group };

            if (parent == Guid.Empty) throw new ArgumentException("Empty Parent Guid");

            if (MapContext.ContainsKey(parent))
            {
                MapContext[parent].Children.Add(Value);
            }
            else
            {
                throw new InvalidDataException("Parent does not exists");
            }

            return new ContextNode<Guid>(Value, Parent, MapContext);
        }
    }
}
