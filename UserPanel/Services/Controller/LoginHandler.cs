using Microsoft.AspNetCore.Mvc;
using UserPanel.Models.User;

namespace UserPanel.Services.Controller
{
    public static class LoginHandler
    {
        public enum StageActionLogin
        {
            MODEL_VALID,
            USER_EXISTS,
            USER_VALID,
            REDIRECT_TO_PAGE,
        }
        
       
        

    }
}
