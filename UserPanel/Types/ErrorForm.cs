using UserPanel.Attributes;

namespace UserPanel.Types
{
    public enum ErrorForm
    {
        [StringValue("err_remove")]
        err_remove,
        [StringValue("err_default")]
        err_default,
        [StringValue("suc_remove")]
        suc_remove,
        [StringValue("suc_create")]
        suc_create,
        [StringValue("err_create")]
        err_create,
        [StringValue("err_parse")]
        err_parse,
        [StringValue("err_feed_url")]
        err_feed_url,
        [StringValue("err_validation")]
        err_validation,
        [StringValue("suc_feed_refresh")]
        suc_feed_refresh,
        [StringValue("err_feed_refresh")]
        err_feed_refresh,
        [StringValue("err_edit")]
        err_edit,
        [StringValue("suc_edit")]
        suc_edit,
    }
}
