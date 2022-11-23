using System;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarParking.Tests
{
    [TestClass]
    public class CarParkingTests
    {
        [TestMethod]
        public void Prnd_Srnd_Drnd()
        {
            //arrange
            Random rnd = new Random();
            int p = rnd.Next(-1000, 1000);
            int s = rnd.Next(-1000, 1000);
            int d = rnd.Next(-1000, 1000);
            int expected = 1;
            int actual;
            //act
            Methods x = new Methods();
            int Z = x.TotalPrice(p, s, d);
            if (Z >= 0)
            {
                actual = 1;
            }
            else
            {
                actual = 0;
            }
            //assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void P350_S400_D0_Return0()
        {
            //arrange
            int p = 350;
            int s = 400;
            int d = 0;
            int expected = 0;
            //act
            Methods x = new Methods();
            int actual = x.TotalPrice(p, s, d);
            //assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void SaveForm_Return1()
        {
            //arrange
            string Surename = "Иксов";
            string Name = "Икс";
            string Patr = "Иксович";
            string Car = "BMW";
            string DateTime = "01/11/2022";
            string Price = "350";
            string Sale = "50";
            string Dolg = "0";
            SqlConnection sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\!Files\Documents\ProgLabsVisualStudio\Labs\Sem_5\C#\CarParking\CarParking\CarParkingDB.mdf;Integrated Security=True");
            sqlConnection.Open();
            int expected = 1;
            //act
            Methods x = new Methods();
            int actual = x.SaveForm(Surename, Name, Patr, Car, DateTime, Price, Sale, Dolg, sqlConnection);
            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}