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
      var client = await this.getClientByNameHelper(name);

      if (client is null)
      {
        return NotFound();
      }
      
      return client;
    }

    [HttpPost("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateClient(Client client)
    {
      if (client.Name == null)
      {
        return BadRequest("Client name must have a value");
      }

      var clientFromDb = await this.getClientByNameHelper(client.Name);

      if (clientFromDb != null)
      {
        return BadRequest("Already exists a client with name " + client.Name);
      }
      
      //todo: More validations could be added

      await this._reservationContext.Clients.AddAsync(client);
      await this._reservationContext.SaveChangesAsync();
      return CreatedAtAction(nameof(CreateClient), client);

    }

    private Task<Client> getClientByNameHelper(string name)
    {
      return this._reservationContext.Clients.Where(c => c.Name == name).FirstOrDefaultAsync();
    }
  }
}