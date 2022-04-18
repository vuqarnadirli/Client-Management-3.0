using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Management_2._1
{
    class Owner
    {
        private string name;
        private string surname;
        private string phone;

        public Owner()
        {

        }

        public Owner(string name, string surname, string phone)
        {
            this.name = name;
            this.surname = surname;
            this.phone = phone;
        }

        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Phone { get => phone; set => phone = value; }
    }
}
