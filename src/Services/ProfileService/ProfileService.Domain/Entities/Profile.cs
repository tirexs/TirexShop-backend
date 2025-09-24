using ProfileService.Domain.Interfaces;

namespace ProfileService.Domain.Entities;

public class Profile : Entity<Guid>, IAggregateRoot
{
    public string Email {get; set;}
    public string FirstName {get; set;}
    public string MiddleName {get; set;}
    public string LastName {get; set;}
    public bool EmailVerified {get; set;}
    
    public Profile(
        Guid id,
        string email,
        string firstName,
        string middleName,
        string lastName,
        bool emailVerified)
    {
        Id = id;
        Email = email ?? throw new ArgumentNullException(nameof(email));
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        EmailVerified = emailVerified;
        CreatedAt = DateTime.UtcNow;
    }
}