using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParking
{
    public class Methods
    {
        public int TotalPrice(int price, int sale, int dolg)
        {
            if (price < 200)
            {
                price = 200;
            }
            if (sale < 0)
            {
                sale = 0;
            }
            if (dolg < 0)
            {
                dolg = 0;
            }
            price = price - sale + dolg;
            if (price < 0)
            {
                price = 0;
            }
            return price;
        }

        public int SaveForm(string Surename, string Name, string Patr, string Car, string DateTime, string Price, string Sale, string Dolg, SqlConnection connectionString)
        {
            SqlCommand command = new SqlCommand($"INSERT INTO [CarsOnParking] (Surename, Name, Patr, Car, DateTime, Price, Sale, Dolg) VALUES (@Surename, @Name, @Patr, @Car, @DateTime, @Price, @Sale, @Dolg)", connectionString);

            command.Parameters.AddWithValue("Surename", Surename);
            command.Parameters.AddWithValue("Name", Name);
            command.Parameters.AddWithValue("Patr", Patr);
            command.Parameters.AddWithValue("Car", Car);
            command.Parameters.AddWithValue("DateTime", DateTime);
            command.Parameters.AddWithValue("Price", Price);
            command.Parameters.AddWithValue("Sale", Sale);
            command.Parameters.AddWithValue("Dolg", Dolg);

            int numOfApplications = command.ExecuteNonQuery();
            return numOfApplications;
        }
    }
}