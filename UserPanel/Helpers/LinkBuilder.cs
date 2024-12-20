namespace UserPanel.Helpers
{
    public class LinkBuilder
    {
        private readonly HttpContext _context;
        private readonly LinkGenerator _generator;

        public LinkBuilder(IHttpContextAccessor accessor, LinkGenerator generator)
        {
            _context = accessor.HttpContext;
            _generator = generator;
        }
        public LinkBuilder(HttpContext context)
        {
           _context = context;
           _generator = context.RequestServices.GetRequiredService<LinkGenerator>();
        }
        public string GenerateConfirmEmailLink(string token, string email)
        {

          var callbackLink = _generator.GetUriByAction(_context,
                action: "Confirmation",
                controller: "Register",
                values: new {email = email, code = token});

            return callbackLink;
        }

        public string GenerateResetPasswordLink(string token, string key, string id)
        {
            var callbackLink = _generator.GetUriByAction(_context,
                action: "ResetUpdate",
                controller: "Login",
                values: new { key = key, uid = id, token = token });

            return callbackLink;
        }
    }
}
