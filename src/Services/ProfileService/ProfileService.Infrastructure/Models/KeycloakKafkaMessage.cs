using System.Text.Json.Serialization;
using ProfileService.Infrastructure.Models.Enums;

namespace ProfileService.Infrastructure.Models;

public class KeycloakKafkaMessage
{
    [JsonPropertyName("eventId")]
    public string EventId { get; set; } = String.Empty;
    
    [JsonPropertyName("eventType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public KeycloakEventType EventType { get; set; } = KeycloakEventType.Nome;
    
    [JsonPropertyName("userId")]
    public string UserId  { get; set; } = String.Empty;
    
    [JsonPropertyName("username")]
    public string Username { get; set; } = String.Empty;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = String.Empty;
    
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = String.Empty;
    
    [JsonPropertyName("middleName")]
    public string MiddleName { get; set; } = String.Empty;
    
    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = String.Empty;
    
    [JsonPropertyName("emailVerified")]
    public bool EmailVerified { get; set; } =  false;
}