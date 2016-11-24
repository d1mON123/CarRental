using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;

namespace Program
{
    public class Session
    {
        private System.Windows.Forms.NotifyIcon nicon;
        private string Server;
        private const string DataBase = "car_db1";
        private string User;
        private string Password;
        private MySqlConnection serverconn;
        private const string ShortCarQuery = "SELECT car.id, Company, Model, Year, Category, Type, Seats, Price, Pledge FROM car_db1.car, car_db1.company, car_db1.type, " +
                                                "car_db1.category WHERE car.Company_ID = company.ID AND car.Type_ID = type.ID AND car.Category_ID = category.ID AND Repair = 0 AND Availability = 1";
        private const string LongCarQuery = "SELECT car.id, Company, Model, Year, Category, Type, Fuel, Transmission, Drive, Engine, DischargeS, DischargeO, DischargeM, Power, MaxSpeed, Seats, " +
                                                "Doors, Acceleration, Price, Pledge FROM car_db1.car, car_db1.company, car_db1.category, car_db1.type, car_db1.fuel, " +
                                                "car_db1.transmission, car_db1.drive WHERE car.Company_ID = company.ID AND car.Category_ID = category.ID AND car.Type_ID = type.ID " +
                                                "AND car.Fuel_ID = fuel.ID AND car.Transmission_ID = transmission.ID AND car.Drive_ID = drive.ID";
        private const string RentedCarQuery = "SELECT rented_car.Id, Company, Model, Year, SName, FName, TName, Passport, ID_Number, License, FDay, LDay, TotalHours, rented_car.Price " +
                                                "FROM car_db1.rented_car, car_db1.car, car_db1.company WHERE car.ID = rented_car.Car_ID AND car.Company_ID = company.ID AND Status = ";
        private const string CatalogQuery = "SELECT * FROM car_db1.";
        private const string ArchiveQuery = "SELECT archive.ID, Company, Model, SName, FName, TName, FDay, LDay, TotalHours, archive.Price FROM car_db1.archive, car_db1.car, car_db1.company WHERE car.ID = archive.Car_ID AND car.Company_ID = company.ID";
        private const string ReportQuery = "SELECT report.ID, Company, Model, Mileage, TotalHours, Income FROM car_db1.report, car_db1.car, car_db1.company WHERE car.ID = report.Car_ID AND car.Company_ID = company.ID";
        private const string RepairQuery = "SELECT repair.ID, Company, Model, Reason, repair.Price FROM car_db1.repair, car_db1.car, car_db1.company WHERE car.ID = repair.Car_ID AND car.Company_ID = company.ID";
        private const string RepairedQuery = "SELECT repairarchive.ID, Company, Model, Reason, repairarchive.Price FROM car_db1.repairarchive, car_db1.car, car_db1.company WHERE car.ID = repairarchive.Car_ID AND car.Company_ID = company.ID";
        private const string RepairArchiveQuery = "SELECT repairarchive.ID, Company, Model, Reason, repairarchive.Price FROM car_db1.repairarchive, car_db1.car, car_db1.company WHERE car.ID = repairarchive.Car_ID AND car.Company_ID = company.ID";
        private const string RepairReporQuery = "SELECT repair_report.ID, Company, Model, Count, Loss FROM car_db1.repair_report, car_db1.car, car_db1.company WHERE car.ID = repair_report.Car_ID AND car.Company_ID = company.ID";
        public Session(string Server, string User, string Password)
        {
            this.Server = Server;
            this.User = User;
            this.Password = Password;
            nicon = new System.Windows.Forms.NotifyIcon();
            nicon.Icon = Properties.Resources.icon;
            nicon.Visible = true;
        }

        public bool ConnectToServer()
        {
            Boolean isConnected = true;
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = Server;
            builder.UserID = User;
            builder.Password = Password;

            String connection = builder.ToString();
            serverconn = new MySqlConnection(connection);
            try
            {
                serverconn.Open();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Неправильний логін або пароль");
                        break;
                    case 1042:
                        MessageBox.Show("Немає доступу до серверу або даний сервер відсутній");
                        break;
                }
                isConnected = false;
            }
            finally
            {
                serverconn.Close();
            }
            return isConnected;
        }

        public void InitializeDB()
        {
            String query = "CREATE SCHEMA IF NOT EXISTS `car_db1`; CREATE TABLE IF NOT EXISTS `car_db1`.`company` (`ID` INT NOT NULL AUTO_INCREMENT, `Company` VARCHAR(45) NOT NULL, UNIQUE INDEX `Company_UNIQUE` (`Company` ASC), PRIMARY KEY(`ID`)); " +
                "CREATE TABLE IF NOT EXISTS `car_db1`.`type` (`ID` INT NOT NULL AUTO_INCREMENT, `Type` VARCHAR(45) NOT NULL, UNIQUE INDEX `Type_UNIQUE` (`Type` ASC), PRIMARY KEY(`ID`));" +
                "CREATE TABLE IF NOT EXISTS `car_db1`.`category` (`ID` INT NOT NULL AUTO_INCREMENT, `Category` VARCHAR(45) NOT NULL, UNIQUE INDEX `Category_UNIQUE` (`Category` ASC), PRIMARY KEY(`ID`));" +
                "CREATE TABLE IF NOT EXISTS `car_db1`.`fuel` (`ID` INT NOT NULL AUTO_INCREMENT, `Fuel` VARCHAR(45) NOT NULL, UNIQUE INDEX `Fuel_UNIQUE` (`Fuel` ASC), PRIMARY KEY(`ID`));" +
                "CREATE TABLE IF NOT EXISTS `car_db1`.`transmission` (`ID` INT NOT NULL AUTO_INCREMENT, `Transmission` VARCHAR(45) NOT NULL, UNIQUE INDEX `Transmission_UNIQUE` (`Transmission` ASC), PRIMARY KEY(`ID`));" +
                "CREATE TABLE IF NOT EXISTS `car_db1`.`drive` (`ID` INT NOT NULL AUTO_INCREMENT, `Drive` VARCHAR(45) NOT NULL, UNIQUE INDEX `Drive_UNIQUE` (`Drive` ASC), PRIMARY KEY(`ID`));" +
                "CREATE TABLE IF NOT EXISTS `car_db1`.`car` (`ID` INT NOT NULL AUTO_INCREMENT, `Company_ID` INT NOT NULL, `Model` VARCHAR(45) NOT NULL, `Year` YEAR NOT NULL, `Category_ID` INT NOT NULL, " +
                "`Type_ID` INT NOT NULL, `Fuel_ID` INT NOT NULL, `Transmission_ID` INT NOT NULL, `Drive_ID` INT NOT NULL, `Engine` INT NOT NULL, `DischargeS` DOUBLE NOT NULL, `DischargeO` DOUBLE NOT NULL, `DischargeM` DOUBLE NOT NULL, " +
                "`Power` INT NOT NULL, `MaxSpeed` INT NOT NULL, `Seats` INT NOT NULL, `Doors` INT NOT NULL, `Acceleration` DOUBLE NOT NULL, `Price` INT NOT NULL, `Pledge` INT NOT NULL, `Repair` TINYINT NOT NULL DEFAULT 0, " +
                "`Availability` TINYINT NOT NULL DEFAULT 1, PRIMARY KEY (`ID`), INDEX `company_idx` (`Company_ID` ASC), INDEX `category_idx` (`Category_ID` ASC), INDEX `type_idx` (`Type_ID` ASC), INDEX `fuel_idx` (`Fuel_ID` ASC), " +
                "INDEX `transmission_idx` (`Transmission_ID` ASC), INDEX `drive_idx` (`Drive_ID` ASC), CONSTRAINT `company` FOREIGN KEY (`Company_ID`) REFERENCES `car_db1`.`company` (`ID`) " +
                "ON DELETE NO ACTION ON UPDATE NO ACTION, CONSTRAINT `category` FOREIGN KEY (`Category_ID`) REFERENCES `car_db1`.`category` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION, " +
                "CONSTRAINT `type` FOREIGN KEY (`Type_ID`) REFERENCES `car_db1`.`type` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION, CONSTRAINT `fuel` FOREIGN KEY (`Fuel_ID`) REFERENCES `car_db1`.`fuel` (`ID`) " +
                "ON DELETE NO ACTION ON UPDATE NO ACTION, CONSTRAINT `transmission` FOREIGN KEY (`Transmission_ID`) REFERENCES `car_db1`.`transmission` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION, " +
                "CONSTRAINT `drive` FOREIGN KEY (`Drive_ID`) REFERENCES `car_db1`.`drive` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION); CREATE TABLE IF NOT EXISTS `car_db1`.`rented_car` (`ID` INT NOT NULL AUTO_INCREMENT, `Car_ID` INT NOT NULL, " +
                "`SName` VARCHAR(45) NOT NULL, `FName` VARCHAR(45) NOT NULL, `TName` VARCHAR(45) NOT NULL, `Passport` VARCHAR(45) NOT NULL, `ID_Number` VARCHAR(45) NOT NULL, `License` VARCHAR(45) NOT NULL, " +
                "`FDay` DATETIME NOT NULL, `LDay` DATETIME NOT NULL, `TotalHours` INT NOT NULL, `Price` INT NOT NULL, `Status` TINYINT NOT NULL DEFAULT 0, PRIMARY KEY (`ID`), INDEX `car_idx` (`Car_ID` ASC), CONSTRAINT `car` FOREIGN KEY (`Car_ID`) " +
                "REFERENCES `car_db1`.`car` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION); CREATE TABLE IF NOT EXISTS `car_db1`.`report` (`ID` INT NOT NULL AUTO_INCREMENT, `Car_ID` INT NOT NULL, `Mileage` DOUBLE NOT NULL, " +
                "`TotalHours` INT NOT NULL, `Income` INT NOT NULL, PRIMARY KEY (`ID`), INDEX `carid_idx` (`Car_ID` ASC), CONSTRAINT `carid` FOREIGN KEY (`Car_ID`) REFERENCES `car_db1`.`car` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION); " +
                "CREATE TABLE IF NOT EXISTS `car_db1`.`archive` (`ID` INT NOT NULL AUTO_INCREMENT, `Car_ID` INT NOT NULL, `SName` VARCHAR(45) NOT NULL, `FName` VARCHAR(45) NOT NULL, `TName` VARCHAR(45) NOT NULL, `FDay` DATETIME NOT NULL, `LDay` DATETIME NOT NULL, " +
                "`TotalHours` INT NOT NULL, `Price` INT NOT NULL, PRIMARY KEY (`ID`), INDEX `carid_idx` (`Car_ID` ASC), CONSTRAINT `cid` FOREIGN KEY (`Car_ID`) REFERENCES `car_db1`.`car` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION); "+
                "CREATE TABLE IF NOT EXISTS `car_db1`.`repair` (`ID` INT NOT NULL AUTO_INCREMENT, `Car_ID` INT NOT NULL, `Reason` VARCHAR(200) NOT NULL, `Price` DOUBLE NOT NULL, PRIMARY KEY (`ID`), INDEX `carid_idx` (`Car_ID` ASC), CONSTRAINT `carid1` "+
                "FOREIGN KEY (`Car_ID`) REFERENCES `car_db1`.`car` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION); CREATE TABLE IF NOT EXISTS `car_db1`.`repairarchive` (`ID` INT NOT NULL AUTO_INCREMENT, `Car_ID` INT NOT NULL, `Reason` VARCHAR(200) NOT NULL, " +
                "`Loss` INT NOT NULL, PRIMARY KEY (`ID`), INDEX `carid2_idx` (`Car_ID` ASC), CONSTRAINT `carid2` FOREIGN KEY (`Car_ID`) REFERENCES `car_db1`.`car` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION); CREATE TABLE IF NOT EXISTS `car_db1`.`repair_report` ( "+
                "`ID` INT NOT NULL AUTO_INCREMENT, `Car_ID` INT NOT NULL, `Count` INT NOT NULL DEFAULT 0, `Loss` INT NOT NULL, PRIMARY KEY (`ID`), UNIQUE INDEX `Car_ID_UNIQUE` (`Car_ID` ASC)); ";
            MySqlCommand cmd = new MySqlCommand(query, serverconn);

            try
            {
                serverconn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {

                }
            }
            finally
            {
                serverconn.Close();
            }
        }

        public void Insert(string table, string field, string value)
        {
            String query = String.Format("INSERT INTO {0}.{1} ({2}) VALUES ('{3}') ", DataBase, table, field, value);
            MySqlCommand cmd = new MySqlCommand(query, serverconn);

            try
            {
                serverconn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1062:
                        System.Windows.MessageBox.Show("Повтор");
                        break;
                }
            }
            finally
            {
                serverconn.Close();
            }
        }

        public List<Catalog> FillBox(string table)
        {
            List<Catalog> list = new List<Catalog>();
            Catalog cmp;

            String query = String.Format("SELECT * FROM {0}.{1}", DataBase, table);
            MySqlCommand cmd = new MySqlCommand(query, serverconn);
            serverconn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cmp = new Catalog();
                cmp.ID = int.Parse(reader["ID"].ToString());
                cmp.Name = reader[table].ToString();
                list.Add(cmp);
            }
            serverconn.Close();
            return list;
        }

        public DataTable ShowTable(string table)
        {
            String query;
            DataTable dbtable = new DataTable();
            if (table == "car") query = ShortCarQuery;
            else if (table == "rented_car") query = RentedCarQuery;
            else if (table == "archive") query = ArchiveQuery;
            else if (table == "report") query = ReportQuery;
            else if (table == "repair") query = RepairQuery;
            else if (table == "repairarchive") query = RepairedQuery;
            else query = CatalogQuery + table;
            MySqlCommand cmd = new MySqlCommand(query, serverconn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, serverconn);
            try
            {
                serverconn.Open();
                adapter.Fill(dbtable);
                return dbtable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                serverconn.Close();
            }

        }

        public DataTable ShowTable(string table, int status)
        {
            String query = RentedCarQuery + status.ToString();
            DataTable dbtable = new DataTable();
            MySqlCommand cmd = new MySqlCommand(query, serverconn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, serverconn);
            try
            {
                serverconn.Open();
                adapter.Fill(dbtable);
                return dbtable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                serverconn.Close();
            }

        }

        public void Insert(Car newCar, string table)
        {
            String query = String.Format("INSERT INTO {0}.{1} (Company_ID, Model, Year, Category_ID, Type_ID, Fuel_ID, Transmission_ID, Drive_ID, Engine, DischargeS, DischargeO, DischargeM, Power, MaxSpeed, Seats, Doors, Acceleration, Price, Pledge) VALUES ({2},'{3}',{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20}) ", DataBase, table, newCar.Company_id, newCar.Model, newCar.Year, newCar.Category_id, newCar.Type_id, newCar.Fuel_id, newCar.Transmission_id, newCar.Drive_id, newCar.Engine, newCar.DischargeS.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture), newCar.DischargeO.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture), newCar.DischargeM.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture), newCar.Power, newCar.Maxspeed, newCar.Seats, newCar.Doors, newCar.Acceleration.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture), newCar.Price, newCar.Pledge);
            MySqlCommand cmd = new MySqlCommand(query, serverconn);
            try
            {
                serverconn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1062:
                        MessageBox.Show("Повтор");
                        break;
                }
            }
            finally
            {
                serverconn.Close();
            }
        }

        public DetCarInfo GetSavedCar(int ID)
        {
            DetCarInfo sc = new DetCarInfo();
            String query = String.Format("SELECT car.ID, Company, Model, Year, Category, Type, Fuel, Transmission, Drive, Engine, DischargeS, DischargeO, DischargeM, Power, MaxSpeed, Seats, Doors, Acceleration, Price, Pledge FROM {0}.car, {0}.company, {0}.drive, {0}.fuel, {0}.transmission, {0}.type, {0}.category WHERE car.Company_ID = company.ID AND car.Category_ID = category.ID AND car.Type_ID = type.ID AND car.Fuel_ID = fuel.ID AND car.Transmission_ID = transmission.ID AND car.Drive_ID = drive.ID AND car.ID = {1}", DataBase, ID);
            MySqlCommand cmd = new MySqlCommand(query, serverconn);
            serverconn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            sc.Id = int.Parse(reader["ID"].ToString());
            sc.Company = reader["Company"].ToString();
            sc.Model = reader["Model"].ToString();
            sc.Category = reader["Category"].ToString();
            sc.Year = int.Parse(reader["Year"].ToString());
            sc.Type = reader["Type"].ToString();
            sc.Fuel = reader["Fuel"].ToString();
            sc.Transmission = reader["Transmission"].ToString();
            sc.Drive = reader["Drive"].ToString();
            sc.Engine = int.Parse(reader["Engine"].ToString());
            sc.DischargeS = double.Parse(reader["DischargeS"].ToString());
            sc.DischargeO = double.Parse(reader["DischargeO"].ToString());
            sc.DischargeM = double.Parse(reader["DischargeM"].ToString());
            sc.Power = int.Parse(reader["Power"].ToString());
            sc.Maxspeed = int.Parse(reader["MaxSpeed"].ToString());
            sc.Seats = int.Parse(reader["Seats"].ToString());
            sc.Doors = int.Parse(reader["Doors"].ToString());
            sc.Acceleration = double.Parse(reader["Acceleration"].ToString());
            sc.Price = int.Parse(reader["Price"].ToString());
            sc.Pledge = int.Parse(reader["Pledge"].ToString());
            serverconn.Close();
            return sc;
        }

        public void Update(Car newCar, int ID)
        {
            String query = String.Format("UPDATE {0}.car SET Company_ID = {1}, Model = '{2}', Year = {3}, Category_ID = {4}, Type_ID = {5}, Fuel_ID = {6}, Transmission_ID = {7}, Drive_ID = {8}, Engine = {9}, DischargeS = {10}, DischargeO = {11}, DischargeM = {12}, Power = {13}, MaxSpeed = {14}, Seats = {15}, Doors = {16}, Acceleration = {17}, Price = {18}, Pledge = {19} WHERE ID = {20}", DataBase, newCar.Company_id, newCar.Model, newCar.Year, newCar.Category_id, newCar.Type_id, newCar.Fuel_id, newCar.Transmission_id, newCar.Drive_id, newCar.Engine, newCar.DischargeS.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture), newCar.DischargeO.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture), newCar.DischargeM.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture), newCar.Power, newCar.Maxspeed, newCar.Seats, newCar.Doors, newCar.Acceleration.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture), newCar.Price, newCar.Pledge, ID);
            MySqlCommand cmd = new MySqlCommand(query, serverconn);
            try
            {
                serverconn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                serverconn.Close();
            }
        }

        public void Delete(string table, int Id)
        {
            String query = String.Format("DELETE FROM {0}.{1} WHERE ID = {2} ", DataBase, table, Id);
            MySqlCommand cmd = new MySqlCommand(query, serverconn);
            try
            {
                serverconn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1451:
                        System.Windows.MessageBox.Show("Видалення неможливе");
                        break;
                }
            }
            finally
            {
                serverconn.Close();
            }
        }

        public void RentCar(RentedCar rc)
        {
            String firstQuery = String.Format("INSERT INTO {0}.rented_car (Car_ID, SName, FName, TName, Passport, ID_Number, License, FDay, LDay, TotalHours, Price) VALUES ({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10}, {11}) ", DataBase, rc.Car_ID, rc.Sname, rc.Fname, rc.Tname, rc.Passport, rc.Id_number, rc.License, rc.FDate.ToString("yyyy-MM-dd H:mm:ss"), rc.LDate.ToString("yyyy-MM-dd H:mm:ss"), rc.Hours, rc.Price);
            MySqlCommand firstCmd = new MySqlCommand(firstQuery, serverconn);
            try
            {
                serverconn.Open();
                firstCmd.ExecuteNonQuery();
                //secondCmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1062:
                        System.Windows.MessageBox.Show("Повтор");
                        break;
                }
            }
            finally
            {
                serverconn.Close();
            }

        }

        public Car GetCarInfo(int ID)
        {
            Car car = new Car();
            String query = String.Format("SELECT car.ID, Company, Model, Price FROM {0}.car, {0}.company WHERE car.company_ID = company.ID AND car.ID = {1}", DataBase, ID);
            MySqlCommand cmd = new MySqlCommand(query, serverconn);
            serverconn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            car.Id = int.Parse(reader["ID"].ToString());
            car.Company = reader["Company"].ToString();
            car.Model = reader["Model"].ToString();
            car.Price = int.Parse(reader["Price"].ToString());
            serverconn.Close();
            return car;
        }

        public void CarRented(DateTime dt)
        {
            string title = "Прокат";
            string text = "";

            List<RentedCar> activeList = new List<RentedCar>();
            List<RentedCar> finishedList = new List<RentedCar>();
            RentedCar rc = new RentedCar();
            String query = String.Format("SELECT * FROM {0}.rented_car", DataBase);
            MySqlCommand cmd = new MySqlCommand(query, serverconn);
            serverconn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                rc.ID = int.Parse(reader["ID"].ToString());
                rc.Fname = reader["FName"].ToString();
                rc.Sname = reader["SName"].ToString();
                rc.Tname = reader["TName"].ToString();
                rc.FDate = Convert.ToDateTime(reader["FDay"].ToString());
                rc.LDate = Convert.ToDateTime(reader["Lday"].ToString());
                if (rc.LDate <= dt)
                {
                    text = String.Format("Прокат закінчився ({0} {1} {2})", rc.Sname, rc.Fname, rc.Tname);
                    finishedList.Add(rc);
                }
                else if (rc.FDate <= dt)
                {
                    text = String.Format("Прокат почався ({0} {1} {2})", rc.Sname, rc.Fname, rc.Tname);
                    activeList.Add(rc);
                }
                nicon.ShowBalloonTip(5000, title, text, System.Windows.Forms.ToolTipIcon.Info);
                
            }
            serverconn.Close();
            ChangeStatus(activeList, 1);
            ChangeStatus(finishedList, 2);
        }

        public void CheckStatus(DateTime dt)
        {
            string title = "Статус";
            string text = "";
            List<RentedCar> activeList = new List<RentedCar>();
            List<RentedCar> finishedList = new List<RentedCar>();
            RentedCar rc = new RentedCar();
            int activeCount = 0;
            int finishedCount = 0;
            String query = String.Format("SELECT * FROM {0}.rented_car", DataBase);
            MySqlCommand cmd = new MySqlCommand(query, serverconn);
            serverconn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                rc.ID = int.Parse(reader["ID"].ToString());
                rc.FDate = Convert.ToDateTime(reader["FDay"].ToString());
                rc.LDate = Convert.ToDateTime(reader["Lday"].ToString());
                if (rc.LDate <= dt)
                {
                    finishedCount++;
                    finishedList.Add(rc);
                }
                else if (rc.FDate <= dt)
                {
                    activeCount++;
                    activeList.Add(rc);
                }
                if (activeCount > 0)
                {
                    text = "К-ть активних оренд : " + activeCount.ToString();
                    nicon.ShowBalloonTip(5000, title, text, System.Windows.Forms.ToolTipIcon.Info);
                }
                if (finishedCount > 0)
                {
                    text = "К-ть завершених оренд : " + finishedCount.ToString();
                    nicon.ShowBalloonTip(5000, title, text, System.Windows.Forms.ToolTipIcon.Info);
                }
            }
            serverconn.Close();
            ChangeStatus(activeList, 1);
            ChangeStatus(finishedList, 2);
        }

        private void ChangeStatus(List<RentedCar> rc, int status)
        {
            foreach (RentedCar item in rc)
            {
                String query = String.Format("UPDATE {0}.rented_car SET Status = {1} WHERE ID = {2}", DataBase, status, item.ID);
                MySqlCommand cmd = new MySqlCommand(query, serverconn);
                try
                {
                    serverconn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    serverconn.Close();
                }
            }
            
        }

        public bool CheckTime(RentedCar newCar)
        {
            RentedCar rc = new RentedCar();
            String query = String.Format("SELECT * FROM {0}.rented_car", DataBase);
            MySqlCommand cmd = new MySqlCommand(query, serverconn);
            try
            {
                serverconn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rc.FDate = Convert.ToDateTime(reader["FDay"].ToString());
                    rc.LDate = Convert.ToDateTime(reader["Lday"].ToString());

                    if (!((newCar.FDate >= rc.LDate) || (newCar.LDate <= rc.FDate)))
                    {
                        MessageBox.Show("Час зайнятий");
                        return true;
                    }
                }
                return false;

            }
            catch (MySqlException ex)
            {

                throw;
            }
            finally
            {
                serverconn.Close();

            }
        }

        public void Report(RentedCar rc, double mileage)
        {
            String firstQuery = String.Format("INSERT INTO {0}.archive (Car_ID, SName, FName, TName, FDay, LDay, TotalHours, Price) VALUES ({1}, '{2}', '{3}', '{4}', '{5}', '{6}', {7}, {8}) ", DataBase, rc.Car_ID, rc.Sname, rc.Fname, rc.Tname, rc.FDate.ToString("yyyy-MM-dd H:mm:ss"), rc.LDate.ToString("yyyy-MM-dd H:mm:ss"), rc.Hours, rc.Price);
            MySqlCommand firstCmd = new MySqlCommand(firstQuery, serverconn);
            String secondQuery = String.Format("INSERT INTO {0}.report (Car_ID, Mileage, TotalHours, Income) VALUES ({1}, {2}, {3}, {4}) ON DUPLICATE KEY UPDATE Mileage = Mileage + {2}, TotalHours = TotalHours + {3}, Income = Income + {4} ", DataBase, rc.Car_ID, mileage, rc.Hours, rc.Price);
            MySqlCommand secondCmd = new MySqlCommand(secondQuery, serverconn);
            try
            {
                serverconn.Open();
                firstCmd.ExecuteNonQuery();
                secondCmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1062:
                        MessageBox.Show("Повтор");
                        break;
                }
            }
            finally
            {
                serverconn.Close();
            }
        }

        public RentedCar GetRentedCarInfo(int ID)
        {
            RentedCar rc = new RentedCar();
            String query = String.Format("SELECT Car_ID, SName, FName, TName, FDay, LDay, TotalHours, Price FROM {0}.rented_car WHERE ID = {1}", DataBase, ID);
            MySqlCommand cmd = new MySqlCommand(query, serverconn);
            serverconn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            rc.ID = ID;
            rc.Car_ID = int.Parse(reader["Car_ID"].ToString());
            rc.Sname = reader["SName"].ToString();
            rc.Fname = reader["FName"].ToString();
            rc.Tname = reader["TName"].ToString();
            rc.FDate = Convert.ToDateTime(reader["FDay"].ToString());
            rc.LDate = Convert.ToDateTime(reader["Lday"].ToString());
            rc.Hours = int.Parse(reader["TotalHours"].ToString());
            rc.Price = int.Parse(reader["Price"].ToString());
            serverconn.Close();
            return rc;
        }

        public void Repair(RepairCar rpc)
        {
            String Query = String.Format("INSERT INTO {0}.repair (Car_ID, Reason, Price) VALUES ({1},'{2}',{3}) ", DataBase, rpc.Carid, rpc.Reason, rpc.Price.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
            MySqlCommand Cmd = new MySqlCommand(Query, serverconn);
            try
            {
                serverconn.Open();
                Cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1062:
                        MessageBox.Show("Повтор");
                        break;
                }
            }
            finally
            {
                serverconn.Close();
            }
        }

        public void UnRepair(int ID)
        {
            String firstQuery = String.Format("INSERT INTO {0}.repairarchive (Car_ID, Reason, Loss) SELECT Car_ID, Reason, Price FROM {0}.repair WHERE ID = {1}  ", DataBase, ID);
            MySqlCommand firstCmd = new MySqlCommand(firstQuery, serverconn);
            String secondQuery = String.Format("INSERT INTO {0}.repair_report (Car_ID, Loss) SELECT Car_ID, Price FROM {0}.repair WHERE ID = {1} ON DUPLICATE KEY UPDATE Count = Count + 1, Loss = Loss + repair.Price ", DataBase, ID);
            MySqlCommand secondCmd = new MySqlCommand(secondQuery, serverconn);
            String thirdQuery = String.Format("DELETE FROM {0}.repair WHERE ID = {1}", DataBase, ID);
            MySqlCommand thirdCmd = new MySqlCommand(thirdQuery, serverconn);
            try
            {
                serverconn.Open();
                firstCmd.ExecuteNonQuery();
                secondCmd.ExecuteNonQuery();
                thirdCmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1062:
                        MessageBox.Show("Повтор");
                        break;
                }
            }
            finally
            {
                serverconn.Close();
            }
        }
    }
}