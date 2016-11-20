using System;

namespace Program
{
    public class RentedCar
    {
        private int id;
        private int car_id;
        private string fname;
        private string sname;
        private string tname;
        private int hours;
        private DateTime fdate;
        private DateTime ldate;
        private string passport;
        private string id_number;
        private string license;
        private int price;
        private int status;


        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int Car_ID
        {
            get { return car_id; }
            set { car_id = value; }
        }

        public string Fname
        {
            get { return fname; }
            set { fname = value; }
        }

        public string Sname
        {
            get { return sname; }
            set { sname = value; }
        }

        public string Tname
        {
            get { return tname; }
            set { tname = value; }
        }

        public int Hours
        {
            get { return hours; }
            set { hours = value; }
        }

        public DateTime FDate
        {
            get { return fdate; }
            set { fdate = value; }
        }

        public DateTime LDate
        {
            get { return ldate; }
            set { ldate = value; }
        }

        public string Passport
        {
            get { return passport; }
            set { passport = value; }
        }

        public string Id_number
        {
            get { return id_number; }
            set { id_number = value; }
        }

        public string License
        {
            get { return license; }
            set { license = value; }
        }

        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        public int Status
        {
            get { return status; }
        }
    }
}
