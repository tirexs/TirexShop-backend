using System.Text.RegularExpressions;

namespace ProfileService.Domain.ValueObjects;
public class EmailAddress : IEquatable<EmailAddress>
{
    public string Value { get; private set; }

    private EmailAddress() { }

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email не может быть пустым", nameof(value));

        if (!IsValid(value))
            throw new ArgumentException("Некорректный формат email", nameof(value));

        Value = value.Trim().ToLowerInvariant();
    }

    private static bool IsValid(string email)
    {
        // Упрощённая валидация
        return Regex.IsMatch(email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase);
    }

    // Сравнение по значению
    public override bool Equals(object? obj)
        => Equals(obj as EmailAddress);

    public bool Equals(EmailAddress? other) =>
        other is not null && Value == other.Value;

    public override int GetHashCode()
        => Value.GetHashCode();

    public override string ToString()
        => Value;

    public static implicit operator string(EmailAddress email)
        => email.Value;
}