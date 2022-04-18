using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Management_2._1
{
    internal class ClientAllInfos
    {
        private string name;
        private string phone;
        private string vin;
        private string model;
        private string engine;
        private string plate;

        public ClientAllInfos(string name, string phone, string vin, string model, string engine, string plate)
        {
            this.name = name;
            this.phone = phone;
            this.vin = vin;
            this.model = model;
            this.engine = engine;
            this.plate = plate;
        }

        public string Name { get => name; set => name = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Vin { get => vin; set => vin = value; }
        public string Model { get => model; set => model = value; }
        public string Engine { get => engine; set => engine = value; }
        public string Plate { get => plate; set => plate = value; }
    }
}
