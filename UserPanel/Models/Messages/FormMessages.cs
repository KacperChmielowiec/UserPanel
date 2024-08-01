using System.Diagnostics.Contracts;
using UserPanel.Helpers;

namespace UserPanel.Models.Messages
{
    public abstract class FormMessages
    {
        public string err_remove { get; set;}
        public string err_create { get; set;}
        public string suc_create { get; set;}
        public string suc_remove { get; set; }
        public string err_default { get; set;}
        public string suc_edit { get; set; }
        public string err_edit { get; set; }
        public virtual string GetValue(Enum key)
        {
            switch (key.GetStringValue())
            {
                case "err_remove":
                    return err_remove;
                case "suc_remove":
                    return suc_remove;
                case "err_default":
                    return err_default;
                case "err_create":
                    return err_create;
                case "suc_create":
                    return suc_create;
                case "suc_edit":
                    return suc_edit;
                case "err_edit":
                    return err_edit;
                default:
                    return "";

            }
        }
    }
}
