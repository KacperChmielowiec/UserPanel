using UserPanel.Models.Camp;
using UserPanel.Models.Group;
using UserPanel.Models.Adverts;
using UserPanel.Interfaces.Abstract;
using Microsoft.AspNetCore.Mvc.Formatters;

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

                foreach (var item in context.f_camp)
                {
                    if(!MapContext.ContainsKey(item.id))
                    {
                        MapContext[item.id] = MapFullContextCamp(item);
                    }
                }

                foreach (var item in context.f_group)
                {
                    if (!MapContext.ContainsKey(item.id))
                    {
                        MapContext[item.id] = MapFullContextGroup(item);
                    }
                }


                foreach (var item in context.f_advert)
                {
                    if (!MapContext.ContainsKey(item.id))
                    {
                        MapContext[item.id] = MapFullContextAdvert(item);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Error while fetching FullContext Data", ex);
            }

            return this;
        }
        public ContextNode<Guid> MapFullContextCamp(FullContextCampaning full_c)
        {
            ContextElement<Guid> value = new ContextElement<Guid>() { ElementType = ContextNodeType.Camp, ID = full_c.id };
            List<ContextElement<Guid>> children = full_c.Children.Select(c => new ContextElement<Guid>() { ElementType = ContextNodeType.Group, ID = c}).ToList();

            return new ContextNode<Guid>(value, children, MapContext);
        }
        public ContextNode<Guid> MapFullContextGroup(FullContextGroup full_g)
        {
            ContextElement<Guid> value = new ContextElement<Guid>() { ElementType = ContextNodeType.Group, ID = full_g.id };
            List<ContextElement<Guid>> children = full_g.Adverts.Select(a => new ContextElement<Guid>() { ElementType = ContextNodeType.Advert,ID = a}).ToList();
            List<ContextElement<Guid>> parents = new List<ContextElement<Guid>>() { new ContextElement<Guid>() { ElementType = ContextNodeType.Camp, ID = full_g.Campaning } };

            return new ContextNode<Guid>(value, parents, children, MapContext);
        }
        public ContextNode<Guid> MapFullContextAdvert(FullContextAdvert full_a)
        {
            ContextElement<Guid> value = new ContextElement<Guid>() { ElementType = ContextNodeType.Advert, ID = full_a.id };
            List<ContextElement<Guid>> groups = full_a.Groups.Select(c => new ContextElement<Guid>() { ElementType = ContextNodeType.Group, ID = c }).ToList();


            return new ContextNode<Guid>(value, groups, MapContext);
        }
    }
   
}
