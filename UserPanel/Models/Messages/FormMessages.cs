using UserPanel.Helpers;

namespace UserPanel.Models.Messages
{
    public abstract class FormMessages
    {
        public string err_remove { get; set;}

        public string suc_remove { get; set; }
        public string err_default { get; set;}
        
        public string GetValue(Enum key)
        {
            switch (key.GetStringValue())
            {
                case "err_remove":
                    return err_remove;
                case "suc_remove":
                    return suc_remove;
                case "err_default":
                    return err_default;
                default:
                    return "";
            }
        }
    }
}
