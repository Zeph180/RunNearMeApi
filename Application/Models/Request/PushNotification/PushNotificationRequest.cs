using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.Models.Request.PushNotification;

public class PushNotificationRequest
{
    [Required]
    public required string Title { get; set; }
    [Required]
    public required string Body { get; set; }
    public Dictionary<string, string>? Data { get; set; }
    [Required]
    public required List<string> DeviceTokens { get; set; }
    public string? ImageUrl { get; set; }
    public string? ClickAction { get; set; }
}