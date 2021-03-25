using JeweleryStore.Common.Enums;
using JewelryStore.BLL.Interfaces;
using JewelryStore.Common.Exceptions;
using JewelryStore.Domain.Models;
using JewelryStoreApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JewelryStoreApp.Controllers
{
    /// <summary>
    /// Controller for getting discounts and other things related to item such as Gold and Silver
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsBLL _itemsBLL;
        private readonly IHttpServices _httpServices;

        /// <summary>
        /// Creates an instance of <see cref="ItemsController"/>.
        /// </summary>
        /// <param name="itemsBLL"></param>
        public ItemsController(IItemsBLL itemsBLL, IHttpServices httpServices)
        {
            _itemsBLL = itemsBLL;
            _httpServices = httpServices;
        }

        /// <summary>
        /// Gets Item by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("items/get/{id}")]
        public async Task<ActionResult<ItemModel>> Get(int id)
        {
            var item = await _itemsBLL.GetItemById(id);
            return item == null ? NotFound() : (ActionResult<ItemModel>)item;
        }

        /// <summary>
        /// Gets All the items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("items/get")]
        public async Task<ActionResult<IList<ItemModel>>> GetAll()
        {
            var items = await _itemsBLL.GetItems();
            return items.ToList();
        }

        /// <summary>
        /// Gets the Total price of the item depending on the user type.
        /// </summary>
        /// <param name="metalPrice"></param>
        /// <param name="weight"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("items/get/totalPrice")]
        public async Task<double> ComputePrice(double metalPrice, double weight,int itemId)
        {
            var claim = _httpServices.GetClaimsIdentity(HttpContext);
            return await _itemsBLL.ComputeTotalPriceAsync(metalPrice,weight, itemId, claim.UserType, claim.UserName);
        }

        /// <summary>
        /// Creates an item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("items/create")]
        public async Task<ActionResult<ItemModel>> Create(ItemModel item)
        {
            try
            {
              var result = await _itemsBLL.CreateItem(item);
              if (result)
              {
                    return Ok();
              }

              return BadRequest();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Updates the Item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemModel"></param>
        /// <returns></returns>
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
            catch (Exception ex)
            {
                Log.Fatal(ex, ex.Message);
                throw; 
            }
            return NoContent();
        }

        /// <summary>
        /// Deletes an item by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                 var result = await _itemsBLL.DeleteItem(item);
                if (result)
                {
                   return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, ex.Message);
            }
           
            return NoContent();
        }
    }
}
