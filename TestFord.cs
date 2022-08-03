using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Management_2._1
{
    internal class TestFord
    {
        public int id;
        public string distance;
        public string description;
        public DateTime dateTime;

        public TestFord(int id, string distance, string description, DateTime dateTime)
        {
            this.id = id;
            this.distance = distance;
            this.description = description;
            this.dateTime = dateTime;
        }
    }
}
