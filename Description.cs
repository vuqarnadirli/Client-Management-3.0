using System;
namespace Client_Management_2._1
{
    class Description
    {

        private int carId;
        private string desc;
        private DateTime dateTime;

        public Description()
        {

        }

        public Description(int carId, string desc, DateTime dateTime)
        {
            this.carId = carId;
            this.desc = desc;
            this.dateTime = dateTime;
        }

        public int GetCarId()
        {
            return this.carId;
        }

        public void SetCarId(int carId)
        {
            this.carId = carId;
        }

        public string GetDesc()
        {
            return this.desc;
        }

        public void SetDesc(string desc)
        {
            this.desc = desc;
        }

        public DateTime GetDateTime()
        {
            return this.dateTime;
        }

        public void SetDateTime(DateTime dateTime)
        {
            this.dateTime = dateTime;
        }
    }
}
