using Microsoft.AspNetCore.Mvc;
using WalletApp.Interfaces.Services;
using WalletApp.Models;
using WalletApp.Validation;

namespace WalletApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletController : ControllerBase
    {
        private IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost("RegisterPlayerWallet")]
        [GuidValidate("playerId")]
        public async Task<IActionResult> RegisterPlayerWallet(Guid playerId)
        {
            try
            {
                Guid walletId = await _walletService.RegisterNewWallet(playerId);
                return Ok(walletId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetBalance")]
        [GuidValidate("playerId")]
        public async Task<IActionResult> GetBalance(Guid playerId)
        {
            try
            {
                decimal balance = await _walletService.GetBalance(playerId);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("MakeTransaction")]
        [TransactionValidate("PlayerId", "Id")]
        public async Task<IActionResult> MakeTransaction(InputTransaction transaction)
        {
            try
            {
                var state = await _walletService.MakeTransaction(transaction);
                return Ok(state.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetTransactions")]
        [GuidValidate("playerId")]
        public async Task<IActionResult> GetTransactions(Guid playerId)
        {
            try
            {
                var transactions = await _walletService.GetTransactions(playerId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
