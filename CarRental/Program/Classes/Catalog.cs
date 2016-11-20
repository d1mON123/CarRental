namespace Program
{
    public class Catalog
    {
        private int id;
        private string name;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
