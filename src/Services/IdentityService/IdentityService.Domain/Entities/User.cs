namespace IdentityService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; }
        public string Email { get; }
        public string UserName { get; }        
        public IReadOnlyList<string> Roles { get; }

        public User(Guid id, string email, string userName, List<string> roles)
        {
            Id = id;
            Email = email;
            UserName = userName;
            Roles = roles.AsReadOnly();
        }
    }
}
