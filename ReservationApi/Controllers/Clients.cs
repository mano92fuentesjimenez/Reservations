using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
  [Route("api/client")]
  [ApiController]
  public class ClientsController: ControllerBase
  {
    private readonly ReservationContext _reservationContext;

    public ClientsController(ReservationContext reservationContext)
    {
      this._reservationContext = reservationContext;
    }

    [HttpGet("byName/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Client>> GetClientByName(string name)
    {
      var client = await this._reservationContext.Clients.Where(c => c.Name == name).FirstOrDefaultAsync();

      if (client is null)
      {
        return NotFound();
      }
      
      return client;
    }
  }
}