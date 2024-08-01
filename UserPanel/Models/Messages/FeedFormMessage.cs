
using Microsoft.IdentityModel.Tokens;
using UserPanel.Helpers;

namespace UserPanel.Models.Messages
{
    public class FeedFormMessage : FormMessages
    {
        public string err_parse {  get; set; }
        public string err_feed_url {  set; get; }
        public string suc_feed_refresh { set; get; }
        public string err_feed_refresh { set; get; }
        public string err_validation { set; get; }
        public override string GetValue(Enum key)
        {
            string value = base.GetValue(key);

            if(!value.IsNullOrEmpty())
            {
                return value;
            }

            switch(key.GetStringValue())
            {
                case "err_parse":
                    return err_parse;
                case "err_feed_url":
                    return err_feed_url;
                case "err_validation":
                    return err_validation;
                case "suc_feed_refresh":
                    return suc_feed_refresh;
                case "err_feed_refresh":
                    return err_feed_refresh;
                default:
                    return "";
            }
        }
    }
}
