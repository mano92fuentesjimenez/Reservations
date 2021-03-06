using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Models;
using WebApplication.Utils.Extensions;
using WebApplication.Utils.Models;

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

      if (client == null)
      {
        return NotFound();
      }
      
      return client;
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateClient(Client client)
    {
      var validMessage = await isValid(client);
      if (validMessage != null)
        return BadRequest(validMessage);
      
      var clientFromDb = await this.getClientByNameHelper(client.Name);

      if (clientFromDb != null)
      {
        return BadRequest("Already exists a client with name " + client.Name);
      }
      
      await this._reservationContext.Clients.AddAsync(client);
      await this._reservationContext.SaveChangesAsync();
      return CreatedAtAction(nameof(CreateClient), client);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateClient(int id, Client client)
    {
      var clientById = await _reservationContext.Clients.FindAsync(id);
      var validMessage = await isValid(client);
      if (validMessage != null)
        return BadRequest(validMessage);

      clientById.Name = client.Name;
      clientById.PhoneNumber = client.PhoneNumber;
      clientById.BirthDate = client.BirthDate;
      clientById.ContactType = client.ContactType;
      
      await _reservationContext.SaveChangesAsync();

      return this.Ok(clientById);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteClient(int id)
    {
      var client = await this._reservationContext.Clients.FindAsync(id);

      if (client == null)
      {
        return NotFound();
      }

      this._reservationContext.Clients.Remove(client);
      await this._reservationContext.SaveChangesAsync();
      return Ok();
    }

    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResponse<Client>>> ListClients([FromQuery]ListFilter filters)
    {
      var paging = filters.Paging;
      var ordering = filters.Ordering;

      if (!this.IsOrderingValid<Reservation>(ordering))
        return BadRequest("Column \"" + ordering.GetOrderingColumn() + "\" doesn't exist");

      var count = await this._reservationContext.Clients.CountAsync();
      var query = this._reservationContext.Clients
        .Skip(paging.OffSet())
        .Take(paging.PerPage);

      query = (IQueryable<Client>)ordering.SetOrderQuery(query);
      var results = await query.ToListAsync();

      return new PagedResponse<Client>()
      {
        Data = results,
        Ordering = filters.Ordering,
        Paging = paging.CreatePaging(count),
      };
    }

    private Task<Client> getClientByNameHelper(string name)
    {
      return this._reservationContext.Clients.Where(c => c.Name == name).FirstOrDefaultAsync();
    }

    private async Task<string?> isValid(Client client)
    {
      if (client.Name == null)
      {
        return "Client name must have a value";
      }

      //todo: More validations could be added

      return null;
    }
  }
}