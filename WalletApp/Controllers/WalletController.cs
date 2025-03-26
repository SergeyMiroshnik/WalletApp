using Microsoft.AspNetCore.Mvc;
using WalletApp.Core.Infrastructure;
using WalletApp.Extensions;
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
        public async Task<IActionResult> RegisterPlayerWallet(string playerId)
        {
            try
            {
                Guid walletId = await _walletService.RegisterNewWallet(playerId.ToGuid());
                return Ok(walletId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetBalance")]
        [GuidValidate("playerId")]
        public async Task<IActionResult> GetBalance(string playerId)
        {
            try
            {
                decimal balance = await _walletService.GetBalance(playerId.ToGuid());
                return Ok(balance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("MakeTransaction")]
        [TransactionValidate("PlayerId", "Id")]
        [EnumValidate("Type", typeof(TransactionType))]
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
        public async Task<IActionResult> GetTransactions(string playerId)
        {
            try
            {
                var transactions = await _walletService.GetTransactions(playerId.ToGuid());
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
