using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleStore.DB;
using SimpleStore.Models;
using SimpleStore.Services;

namespace SimpleStore.Controllers
{
    // Simple CRUD operations controller for store items
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly IStoreItemsService _service;

        public ItemsController(ILogger<ItemsController> logger, IStoreItemsService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<StoreItem>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllItems());
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StoreItem), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _service.Get(id));
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StoreItem), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Put(Guid id, [FromBody] StoreItemModel model)
        {
            return Ok(await _service.Update(id, model));
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StoreItem), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] StoreItemModel model)
        {
            var item = await _service.Create(model);
            return CreatedAtAction(nameof(Get),
                new { id = item.Id },
                item);
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpDelete]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
