using Microsoft.AspNetCore.Mvc;
using VetUberApp.Application.DTOs;
using VetUberApp.Application.Interfaces;

namespace VetUberApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    /// Crea un nuevo pago para una cita
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto)
    {
        var payment = await _paymentService.CreatePaymentAsync(dto);
        return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
    }

    /// <summary>
    /// Obtiene los detalles de un pago específico
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPayment(string id)
    {
        var payment = await _paymentService.GetPaymentByIdAsync(id);
        if (payment == null)
            return NotFound();

        return Ok(payment);
    }

    /// <summary>
    /// Obtiene los pagos asociados a una cita específica
    /// </summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentByAppointment(string appointmentId)
    {
        var payment = await _paymentService.GetPaymentByAppointmentIdAsync(appointmentId);
        if (payment == null)
            return NotFound();

        return Ok(payment);
    }

    /// <summary>
    /// Actualiza el estado de un pago
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePayment(string id, [FromBody] UpdatePaymentDto dto)
    {
        var payment = await _paymentService.UpdatePaymentAsync(id, dto);
        if (payment == null)
            return NotFound();

        return Ok(payment);
    }

    /// <summary>
    /// Procesa un reembolso para un pago específico
    /// </summary>
    [HttpPost("{id}/refund")]
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefundPayment(string id, [FromBody] string refundReason)
    {
        var payment = await _paymentService.RefundPaymentAsync(id, refundReason);
        if (payment == null)
            return NotFound();

        return Ok(payment);
    }
}