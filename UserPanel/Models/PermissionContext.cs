using Org.BouncyCastle.Bcpg;
using UserPanel.Models.Camp;
using UserPanel.References;
using UserPanel.Services.observable;

namespace UserPanel.Models
{
    public static class PermissionActionManager<T> where T : IComparable
    {
        public static PermissionContext<T>? InstanceContext { get; private set; }
        public static DataActionObserverPermission observer { get; private set; }
        public static bool Inited { get; private set; } = false;
        public static void SetupInstance(PermissionContext<T> context)
        {
            InstanceContext = context;
            observer = new DataActionObserverPermission(Subjects.dataActionSubject);
            Inited = true;
        }

        public static void AddNode(T node, T parent, ContextNodeType ElementType)
        {
            if (!Inited) throw new InvalidOperationException("Manager is not inited");
            ContextElement<T> nodeElement = new ContextElement<T>() { ElementType = ElementType, ID = node };
            ContextElement<T> parentElement = new ContextElement<T>() { ElementType = GetParentNodeType(ElementType), ID = parent };
            InstanceContext.AddContextNode(new ContextNode<T>(nodeElement,parentElement, InstanceContext.MapContext));

        }
        public static void AddNode(T node, ContextNodeType ElementType)
        {
            if (!Inited) throw new InvalidOperationException("Manager is not inited");
            if (ElementType != ContextNodeType.Camp) throw new ArgumentException("The parent argumeny has not been passed");
            ContextElement<T> nodeElement = new ContextElement<T>() { ElementType = ElementType, ID = node };
            InstanceContext.AddContextNode( new ContextNode<T>(nodeElement, InstanceContext.MapContext) );

        }
        public static void RemoveNode(T node)
        {
            InstanceContext.RemoveNode(node);
        }
        public static ContextPath<T> GetFullPath(T guid, ContextPath<T> path = null)
        {
            if(!Inited) throw new InvalidOperationException("Manager is not inited");
            if (path == null) path = new ContextPath<T>();

            if (!InstanceContext.MapContext.TryGetValue(guid,out ContextNode<T> node))
            {
                return path;
            }
            else
            {
                switch(node.Value.ElementType)
                {
                    case ContextNodeType.Camp:
                        path.Camp = guid;
                        break;
                    case ContextNodeType.Group:
                        path.Group = guid;
                        break;
                    case ContextNodeType.ADVERT:
                        path.Advert = guid;
                        break;
                    default:
                        break;

                }
                if(node.Value.ElementType == ContextNodeType.Camp) return path;
                return GetFullPath(node.Parent.ID, path);
            }
        }
        public static bool CheckPermisionAccess(T[] path)
        { 
            if (path.Length > 0 && Inited == true)
            {
                int loop = 0;
                ContextNode<T> prevNode = null;

                while(path.Length > 0)
                {
                    var el = path[0];
                    path = path.Skip(1).ToArray();

                    if (InstanceContext.MapContext.TryGetValue(el, out ContextNode<T> node))
                    {
                      
                        loop++;
                        if(loop == 1)
                        {
                            prevNode = node;
                            continue;
                        }
                        
                        if(prevNode.Value.ID.CompareTo(node.Parent.ID) == 0)
                        {
                            prevNode = node;
                            continue;
                        }

                        return false;

                    }
                    else
                    {
                        return false;
                    }


                }
                return true;
            }
            return false;
        }
        private static ContextNodeType GetParentNodeType(ContextNodeType type)
        {
            switch(type)
            {
                case ContextNodeType.Group:
                    return ContextNodeType.Camp;
                case ContextNodeType.ADVERT:
                    return ContextNodeType.Group;
                default:
                    return ContextNodeType.NULL;
                   
            }

        }
    }
    public enum ContextNodeType
    {
        Camp,
        Group,
        ADVERT,
        NULL
    }
    public class ContextElement<T>
    {
        public ContextNodeType ElementType { get; set; }
        public T ID { get; set; }

    }

    public class ContextPath<T>
    {
        public T Camp { get; set; }
        public T Group { get; set; }
        public T Advert { get; set; }
    }
    public class ContextNode<T> where T : IComparable
    {
        public Dictionary<T, ContextNode<T>> MapContextRef { get; set; }
        public ContextElement<T> Value { get; set; }
        public ContextElement<T> Parent { get; set; }
        public List<ContextElement<T>> Children { get; set; }

        public ContextNode(ContextElement<T> value, ContextElement<T> parent, Dictionary<T, ContextNode<T>> _ref) {
            
            Value = value;
            Parent = parent;
            Children = new List<ContextElement<T>>();
            MapContextRef = _ref;

        }
        public ContextNode(ContextElement<T> value, Dictionary<T, ContextNode<T>> _ref)
        {

            Value = value;
            Children = new List<ContextElement<T>>();
            Parent = null;
            MapContextRef = _ref;

        }
        public void DettachChildrenNode(T id)
        { 
           Children.RemoveAll(element => element.ID.CompareTo(id) == 0);
        }
        public void DettachFromParentNode()
        {
            if (Value.ElementType == ContextNodeType.Camp) return;
            if (MapContextRef.TryGetValue(Parent.ID, out ContextNode<T> node))
            {
                node.DettachChildrenNode(Value.ID);
            }
        }
    
    }

    public abstract class PermissionContext<T> where T : IComparable
    {
        public bool IsLogin { get; set; }
        public bool IsLoad { get; set; }
        public Dictionary<T, ContextNode<T>> MapContext { get; set; }

        public PermissionContext()
        {
            MapContext = new Dictionary<T, ContextNode<T>>();
            IsLoad = false;
            IsLogin = false;
        }

        public void RemoveNode(T node)
        {
            if (MapContext.ContainsKey(node))
            {
                MapContext[node].DettachFromParentNode();
                MapContext.Remove(node);
            }
        }
        public void AddContextNode(ContextNode<T> node)
        {
            if (node.Value.ElementType == ContextNodeType.Camp)
            {
                if (MapContext.ContainsKey(node.Value.ID))
                {
                    throw new InvalidOperationException("This Campaning already exists");
                }
                else
                {
                    node.Parent = null;
                    MapContext.Add(node.Value.ID, node);
                }
            }
            else
            {
                if (MapContext.ContainsKey(node.Parent.ID))
                {
                    MapContext[node.Parent.ID].Children.Add(node.Value);
                    MapContext.Add(node.Value.ID, node);
                }
                else
                {
                    throw new ArgumentException("Parent of node doesnt exists");
                }
            }
        }

        public void ClearContext()
        {
            MapContext = new Dictionary<T, ContextNode<T>>();
            IsLogin = false;
            IsLoad = false;
        }
        public abstract PermissionContext<T> SetupContext(Func<FullContext> fetch);
        
    }

   
}
