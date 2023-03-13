using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CarParking
{
    public partial class MainWindow : Window
    {
        private SqlConnection sqlConnection = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CarParkingDB"].ConnectionString);
            sqlConnection.Open();
            if (sqlConnection.State == ConnectionState.Open)
            {
                MessageBox.Show("Подключение установлено!");
            }
        }

        private void SaveForm_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand($"INSERT INTO [CarsOnParking] (Surename, Name, Patr, Car, DateTime, Price, Sale, Dolg) VALUES (@Surename, @Name, @Patr, @Car, @DateTime, @Price, @Sale, @Dolg)", sqlConnection);

            DateTime date = DateTime.Parse(DataTimeTB.Text);

            command.Parameters.AddWithValue("Surename", FizSurenameTB.Text);
            command.Parameters.AddWithValue("Name", FizNameTB.Text);
            command.Parameters.AddWithValue("Patr", FizPatrTB.Text);
            command.Parameters.AddWithValue("Car", FizCarTB.Text);
            command.Parameters.AddWithValue("DateTime", $"{date.Day}/{date.Month}/{date.Year}");
            command.Parameters.AddWithValue("Price", PriceTB.Text);
            command.Parameters.AddWithValue("Sale", SaleTB.Text);
            command.Parameters.AddWithValue("Dolg", DolgTB.Text);

            MessageBox.Show("Добавлена " + command.ExecuteNonQuery().ToString() + " запись.");
        }

        private void ShowTable_Click(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM CarsOnParking", sqlConnection);

            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet);
            Table.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dataSet.Tables[0] });
        }

        private void SaleCB_Checked(object sender, RoutedEventArgs e)
        {
            Sale.Visibility = Visibility.Visible;
            SaleTB.Visibility = Visibility.Visible;
        }

        private void SaleCB_Unchecked(object sender, RoutedEventArgs e)
        {
            SaleTB.Text = "0";
            Sale.Visibility = Visibility.Hidden;
            SaleTB.Visibility = Visibility.Hidden;
        }

        private void DolgCB_Checked(object sender, RoutedEventArgs e)
        {
            Dolg.Visibility = Visibility.Visible;
            DolgTB.Visibility = Visibility.Visible;
        }

        private void DolgCB_Unchecked(object sender, RoutedEventArgs e)
        {
            DolgTB.Text = "0";
            Dolg.Visibility = Visibility.Hidden;
            DolgTB.Visibility = Visibility.Hidden;
        }

        private void ClearForm_Click(object sender, RoutedEventArgs e)
        {
            FizSurenameTB.Clear();
            FizNameTB.Clear();
            FizPatrTB.Clear();
            FizCarTB.Clear();
            //DataTimeTB.Clear();
            PriceTB.Text = "350";
            SaleTB.Text = "0";
            DolgTB.Text = "0";
        }

        private void TotalPrice_Click(object sender, RoutedEventArgs e)
        {
            int price = Convert.ToInt32(PriceTB.Text);
            int sale = Convert.ToInt32(SaleTB.Text);
            int dolg = Convert.ToInt32(DolgTB.Text);
            if (FizSurenameTB.Text == "Break" && FizNameTB.Text == "Out")
            {
                this.Visibility = Visibility.Hidden;

                Game BreakOutGame = new Game();
                BreakOutGame.Show();
            }
            if (price < 200)
            {
                MessageBox.Show("Стоимость не может быть ниже 200.");
                price = 200;
            }
            if (sale < 0)
            {
                MessageBox.Show("Скидка не может быть ниже 0.");
                sale = 0;
            }
            if (dolg < 0)
            {
                MessageBox.Show("Долг не может быть ниже 0.");
                dolg = 0;
            }
            price = price - sale + dolg;
            if (price < 0)
            {
                price = 0;
                PriceTB.Text = Convert.ToString(price);
            }
            else
                PriceTB.Text = Convert.ToString(price);
            SaleTB.Text = "0";
            DolgTB.Text = "0";
        }

        private void TotalTable_Click(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT SUM(Price) AS 'Total price' FROM CarsOnParking", sqlConnection);

            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet);
            Table.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dataSet.Tables[0] });
        }

        private void OnlyNum(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}