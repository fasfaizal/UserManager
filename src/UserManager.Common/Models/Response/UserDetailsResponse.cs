using UserManager.Repo.Entities;

namespace UserManager.Common.Models.Response
{
    public class UserDetailsResponse
    {
        public UserDetailsResponse(UserDetails userDetails)
        {
            FirstName = userDetails?.FirstName;
            LastName = userDetails?.LastName;
            Phone = userDetails?.Phone;
            Address = userDetails?.Address;
            DateOfBirth = userDetails?.DateOfBirth;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
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
