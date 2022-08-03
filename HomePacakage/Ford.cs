namespace Client_Management_2._1
{
    class Ford
    {

        private int id;
        private Owner owner;
        private string vin;
        private string model;
        private string engine;
        private string carPlateNumber;
        private string year;
        private Description description;
        public Ford()
        {

        }      

        public Ford(int id, Owner owner, string vin, string model, string engine, string carPlateNumber, string year, Description description)
        {
            this.id = id;
            this.owner = owner;
            this.vin = vin;
            this.model = model;
            this.engine = engine;
            this.carPlateNumber = carPlateNumber;
            this.year = year;
            this.description = description;
        }

        public int Id { get => id; set => id = value; }
        public string Vin { get => vin; set => vin = value; }
        public string Model { get => model; set => model = value; }
        public string Engine { get => engine; set => engine = value; }
        public string CarPlateNumber { get => carPlateNumber; set => carPlateNumber = value; }
        public string Year { get => year; set => year = value; }
        internal Owner Owner { get => owner; set => owner = value; }
        internal Description Description { get => description; set => description = value; }
    }
}
