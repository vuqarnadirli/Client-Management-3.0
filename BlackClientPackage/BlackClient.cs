using System;

namespace Client_Management_2._1.BlackClientPackage
{
    class BlackClient
    {
        private int id;
        private string name;
        private string phone;
        private string vin;
        private string cpnumber;
        private string description;
        private DateTime dateTime;

        public BlackClient()
        {

        }

        public BlackClient(int id, string name, string phone, string vin, string cpnumber, string description, DateTime dateTime)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.Vin = vin;
            this.Cpnumber = cpnumber;
            this.Description = description;
            this.DateTime = dateTime;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Vin { get => vin; set => vin = value; }
        public string Cpnumber { get => cpnumber; set => cpnumber = value; }
        public string Description { get => description; set => description = value; }
        public DateTime DateTime { get => dateTime; set => dateTime = value; }
    }
}
