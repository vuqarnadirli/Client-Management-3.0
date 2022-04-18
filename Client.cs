using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Management_2._1
{
    internal class Client
    {
        private string name;
        private string phone;
        private string plate;

        public Client(string name, string phone, string plate)
        {
            this.name = name;
            this.phone = phone;
            this.plate = plate;
        }

        public string Name { get => name; set => name = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Plate { get => plate; set => plate = value; }
    }
}
