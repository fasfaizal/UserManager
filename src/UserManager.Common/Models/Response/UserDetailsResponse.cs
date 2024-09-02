using UserManager.Common.Models.Request;
using UserManager.Repo.Entities;

namespace UserManager.Common.Models.Response
{
    public class UserDetailsResponse : UserDetailsRequest
    {
        public UserDetailsResponse(UserDetails userDetails)
        {
            FirstName = userDetails?.FirstName;
            LastName = userDetails?.LastName;
            Phone = userDetails?.Phone;
            Address = userDetails?.Address;
            DateOfBirth = userDetails?.DateOfBirth;
        }
        public string FullName
        {
            get
            {
                var firstName = FirstName ?? string.Empty;
                var lastName = LastName ?? string.Empty;
                return $"{firstName} {lastName}".Trim();
            }
        }
    }
}
