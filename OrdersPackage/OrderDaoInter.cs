using Client_Management_2._1.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Management_2._1.OrdersPackage
{
    interface OrderDaoInter
    {
        void Add(Order order);
        void Update(Order order);
        void Delete(int id);
        List<Order> GetByName(string name);
        List<Order> GetByPhone(string phone);
        List<Order> GetByCPNumber(string cpnumber);
        List<Order> GetByOrders(string orders);
    }
}
