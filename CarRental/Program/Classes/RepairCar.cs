namespace Program
{
    public class RepairCar
    {
        private int id;
        private int carid;
        private string reason;
        private double price;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Carid
        {
            get { return carid; }
            set { carid = value; }
        }

        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}
