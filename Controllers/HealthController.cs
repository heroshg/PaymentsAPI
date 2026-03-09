using Microsoft.AspNetCore.Mvc;

namespace PaymentsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>Returns service health status</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get() =>
        Ok(new
        {
            Status = "Healthy",
            Service = "PaymentsAPI",
            Description = "Consumes OrderPlacedEvent and publishes PaymentProcessedEvent via RabbitMQ",
            Timestamp = DateTime.UtcNow
        });
}
