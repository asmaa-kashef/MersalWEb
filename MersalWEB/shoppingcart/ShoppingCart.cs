using MersalWEB;
using MersalWEB.Controllers;
using MersalWEB.Data;
using MersalWEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MersalWEB.shoppingcart
{
    public class ShoppingCart
    {
        public string ShoppingCartId { get; set; }
        private readonly ApplicationDbContext db;
        public ShoppingCart(ApplicationDbContext context)
        {

            db = context;
        }

        public static ShoppingCart GetShoppingCart(IServiceProvider service)
        {
            var session = service.GetRequiredService<IHttpContextAccessor>().HttpContext.Session;
            var context = service.GetService<ApplicationDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }
        //Get All Item in Shopping Cart
        public List<MersalWEB.Models.Cart> GetShoppingCartItems()
        {
            return db.Carts.Where(x => x.CartId == Convert.ToInt32(ShoppingCartId)).Include(x => x.Product).ToList();
        }




        //Calculate Total Amount in Shopping Cart Item
        public double GetShoppingCartTotal()

        => (double)db.Carts.Where(x => x.CartId == Convert.ToInt32(ShoppingCartId)).Select(x => x.Price * x.Qty).Sum();
        public int GetShoppingCartTotalAmount()
            => (int)db.Carts.Where(x => x.CartId == Convert.ToInt32(ShoppingCartId)).Select(x => x.Qty).Sum();

        public async Task AddItemToShoppingCart(Product product)
        {
            var shoppingCartItem = await db.Carts.FirstOrDefaultAsync(x => x.ProductId == product.ProductId && x.CartId == Convert.ToInt32(ShoppingCartId));
            if (shoppingCartItem == null)
            {
                shoppingCartItem = new MersalWEB.Models.Cart()
                {
                    CartId = Convert.ToInt32(ShoppingCartId),
                    Product = product,
                    Qty = 1
                };
                await db.Carts.AddAsync(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Qty++;
            }
            await db.SaveChangesAsync();

        }
        public async Task RemoveItemFromShoppingCart(Product product)
        {
            var shoppingCartItem = await db.Carts.FirstOrDefaultAsync(x => x.CartId ==(Convert.ToInt32 (ShoppingCartId)) && x.Product.ProductId == product.ProductId);
            if (shoppingCartItem!=null)
            {
                if (shoppingCartItem.Qty>1)
                {
                    shoppingCartItem.Qty--;
                }
                else
                {
                    db.Carts.Remove(shoppingCartItem);                    
                }
                await db.SaveChangesAsync();
            }
        }

        //public void ClearShoppingCart()
        //{
        //    var items = db.Carts.Where(x => x.CartId == (ShoppingCartId).ToList();
        //    db.Carts.RemoveRange(items);
        //    db.SaveChanges();
        //}
    }
}
