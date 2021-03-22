using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_DT102G.Models
{
    public class SessionCart : Cart
    {
        public static ISession session;

        public static Cart GetCart(IServiceProvider services)
        {
            session = services.GetRequiredService<IHttpContextAccessor>().HttpContext.Session;

            var json = session.GetString("Cart");
            SessionCart cart = json == null ? new SessionCart() : JsonConvert.DeserializeObject<SessionCart>(json);

            if (json == null)
            {
                json = JsonConvert.SerializeObject(cart);
                session.SetString("Cart", json);
            }

            return cart;
        }

        public override void AddOrderItem(Book book, int quantity)
        {
            base.AddOrderItem(book, quantity);
        
            var json = JsonConvert.SerializeObject(this); //this is Cart
            
            session.SetString("Cart", json);

            var json2 = session.GetString("Cart");
            SessionCart cart = json == null ? new SessionCart() : JsonConvert.DeserializeObject<SessionCart>(json2);
        }

        public override void RemoveOrderItem(Book book)
        {
            base.RemoveOrderItem(book);
            var json = JsonConvert.SerializeObject(this);
            session.SetString("Cart", json);
        }

        public override void ClearCart()
        {
            base.ClearCart();
            session.Remove("Cart");
        }
    }
}
