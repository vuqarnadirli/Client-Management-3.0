using System;
namespace Client_Management_2._1
{
    class Description
    {

        private int carId;
        private string desc;
        private DateTime dateTime;
        private string filePath;
        private string fileName;
        private string extension;

        public Description()
        {

        }

        public Description(int carId, string desc, DateTime dateTime, string filePath, string fileName, string extension)
        {
            this.carId = carId;
            this.desc = desc;
            this.dateTime = dateTime;
            this.filePath = filePath;
            this.fileName = fileName;
            this.extension = extension;
        }

        public int CarId { get => carId; set => carId = value; }
        public string Desc { get => desc; set => desc = value; }
        public DateTime DateTime { get => dateTime; set => dateTime = value; }
        public string FilePath { get => filePath; set => filePath = value; }
        public string FileName { get => fileName; set => fileName = value; }
        public string Extension { get => extension; set => extension = value; }
    }
}
