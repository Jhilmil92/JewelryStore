using JewelryStore.BLL.Interfaces;
using JewelryStore.Common.Exceptions;
using JewelryStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JewelryStoreApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsBLL _itemsBLL;
        public ItemsController(IItemsBLL itemsBLL)
        {
            _itemsBLL = itemsBLL;
        }

        [HttpGet]
        [Route("items/get/{id}")]
        public async Task<ActionResult<ItemModel>> Get(int id)
        {
            var item = await _itemsBLL.GetItemById(id);
            return item == null ? NotFound() : (ActionResult<ItemModel>)item;
        }

        [HttpGet]
        [Route("items/get")]
        public async Task<ActionResult<IList<ItemModel>>> GetAll()
        {
            var items = await _itemsBLL.GetItems();
            return items.ToList();
        }

        [HttpGet]
        [Route("items/get/totalPrice")]
        public async Task<double> ComputePrice(double metalPrice, double weight)
        {
            return await _itemsBLL.ComputeTotalPriceAsync(metalPrice,weight);
        }

        [HttpPost]
        [Route("items/create")]
        public async Task<ActionResult<ItemModel>> Create(ItemModel item)
        {
            try
            {
              await _itemsBLL.CreateItem(item);
            }
            catch(Exception ex)
            {
                //log.
            }

            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }


        [HttpPut]
        [Route("items/update/{id}")]
        public async Task<ActionResult<ItemModel>> Update(int id, ItemModel itemModel)
        {
            if(id != itemModel.Id)
            {
                return BadRequest();
            }

            var item = _itemsBLL.GetItemById(id);

            if(item == null)
            {
                return NotFound();
            }

            try
            {
                await _itemsBLL.UpdateItem(itemModel);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            catch (Exception)
            {
                //log here also.
                throw; 
            }
            return NoContent();
        }

        [HttpDelete]
        [Route("items/delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _itemsBLL.GetItemById(id);
            if(item == null)
            {
                return NotFound();
            }

           try
            {
                await _itemsBLL.DeleteItem(item);
            }
            catch (Exception ex)
            {
                //log.
            }
           
            return NoContent();
        }
    }
}
