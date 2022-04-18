using System;

namespace Client_Management_2._1.Orders
{
    class Order
    {
        private int id;
        private string name;
        private string phone;
        private string cpnumber;
        private string orders;
        private DateTime dateTime;

        
        public Order()
        {

        }

        public Order(int id, string name, string phone, string cpnumber, string orders, DateTime dateTime)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.Cpnumber = cpnumber;
            this.Orders = orders;
            this.DateTime = dateTime;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Cpnumber { get => cpnumber; set => cpnumber = value; }
        public string Orders { get => orders; set => orders = value; }
        public DateTime DateTime { get => dateTime; set => dateTime = value; }
    }
}
