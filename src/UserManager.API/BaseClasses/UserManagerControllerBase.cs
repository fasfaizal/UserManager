using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UserManager.API.BaseClasses
{
    [Authorize]
    public abstract class UserManagerControllerBase : ControllerBase
    {
        private UserManagerContext _userManagerReqContext;
        public UserManagerContext UserManagerReqContext
        {
            get
            {
                if (_userManagerReqContext == null)
                {
                    _userManagerReqContext = new UserManagerContext();
                    if (this.User != null)
                    {
                        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                        if (userIdClaim != null)
                        {
                            _userManagerReqContext.UserId = Int32.Parse(userIdClaim.Value);
                        }

                    }
                }
                return _userManagerReqContext;
            }
        }
    }

    public class UserManagerContext
    {
        public int UserId { get; set; }
    }
}
