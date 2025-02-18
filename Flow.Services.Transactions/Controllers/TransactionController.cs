using Flow.Application.UseCases.Transaction;
using Flow.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Flow.Services.Transactions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TransactionDTO request, [FromServices] IExecuteTransactionUseCase useCase)
        {
            await useCase.Execute(request);
            return Ok();
        }
    }
}
