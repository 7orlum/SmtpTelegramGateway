using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace SmtpTelegramGateway;

internal sealed class Configuration
{
    public ushort SmtpPort { get; set; } = 25;
    [Required]
    [MinLength(45)]
    public required string TelegramBotToken { get; set; }
    [Required]
    [MinLength(1)]
    [ValidateEnumeratedItems]
    public required IEnumerable<Route> Routing { get; set; }

    internal sealed class Route
    {
        [Required]
        [MinLength(1)]
        public required string Email { get; set; }
        [Required]
        [MinLength(1)]
        public required string TelegramChat { get; set; }
    }
}
