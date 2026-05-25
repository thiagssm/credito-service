using CreditoService.Application.DTOs.Common;
using CreditoService.Application.DTOs.CreditoConstituido;
using CreditoService.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CreditoService.Api.Controllers;

[ApiController]
[Route("api/creditos")]
public sealed class CreditosController : ControllerBase
{
    private readonly ICreditoApplicationService _creditoApplicationService;

    public CreditosController(ICreditoApplicationService creditoApplicationService)
    {
        _creditoApplicationService = creditoApplicationService;
    }

    [HttpPost("integrar-credito-constituido")]
    [ProducesResponseType(typeof(SuccessViewModel), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> IntegrarCreditoConstituidoAsync(
        [FromBody] IReadOnlyCollection<CreditoConstituidoInputModel> inputModels,
        CancellationToken cancellationToken)
    {
        await _creditoApplicationService.IntegrarCreditoConstituidoAsync(inputModels, cancellationToken);
        return Accepted(new SuccessViewModel(true));
    }

    [HttpGet("{numeroNfse}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<CreditoConstituidoViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorNumeroNfseAsync(
        [FromRoute] string numeroNfse,
        CancellationToken cancellationToken)
    {
        var creditos = await _creditoApplicationService.ObterPorNumeroNfseAsync(numeroNfse, cancellationToken);
        return Ok(creditos);
    }

    [HttpGet("credito/{numeroCredito}")]
    [ProducesResponseType(typeof(CreditoConstituidoViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorNumeroCreditoAsync(
        [FromRoute] string numeroCredito,
        CancellationToken cancellationToken)
    {
        var credito = await _creditoApplicationService.ObterPorNumeroCreditoAsync(numeroCredito, cancellationToken);
        return credito is null ? NotFound() : Ok(credito);
    }
}
