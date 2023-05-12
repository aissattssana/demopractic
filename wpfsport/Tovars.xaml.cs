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
using System.Windows.Shapes;

namespace wpfsport
{
    /// <summary>
    /// Логика взаимодействия для Tovars.xaml
    /// </summary>
    public partial class Tovars : Window
    {
        public SportdbContext sport = new SportdbContext();
        Product currentproduct = new Product();
        public Tovars(User user)
        {
            InitializeComponent();
            sport = new SportdbContext();
            //sport.Products.Load();
            CatalogView.ItemsSource = sport.Products.ToList();
            point.ItemsSource = sport.PickupPoints.ToList();
            count.Content ="найдено "+CatalogView.Items.Count.ToString();
            if (user != null)
            {
                fio.Content = user.UserSurname + " " + user.UserName + " " + user.UserName;
                to_order.IsEnabled = true;
            }
            else
            {
                fio.Content = "Неавторизированный пользователь";
                to_order.IsEnabled= false;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (point.SelectedItem != null)
            {
                using (SportdbContext db = new SportdbContext())
                {
                    int idd = (from dt in db.Users where dt.UserSurname + dt.UserName + dt.UserPatronymic == fio.Content.ToString() select dt.UserId).FirstOrDefault();
                    int oid = Convert.ToInt32((from dt in db.Orders where dt.UserId == idd select dt.UserId).FirstOrDefault());
                    Random rnd = new Random();
                    int i = rnd.Next(0, 3000);
                    int pid = (from ut in db.PickupPoints where ut.Address == point.Text select ut.PickupPointId).FirstOrDefault();
                    if (idd != oid)
                    {
                        Order order = new Order { OrderStatusId = 1, PickupPointId = pid, OrderCreateDate = DateTime.Now, OrderDeliveryDate = DateTime.UtcNow.AddDays(6), UserId = idd, OrderGetCode = i };
                        db.Orders.Add(order);
                        int a = order.OrderId;
                        db.SaveChanges();
                        to_order.Visibility = Visibility.Visible;

                        OrderProduct op = new OrderProduct { OrderId = a, ProductId = currentproduct.ProductId, Count = 1 };
                        db.OrderProducts.Add(op);
                        db.SaveChanges();
                    }
                    else
                    {
                        int ord = (from dt in db.Orders where dt.UserId == idd select dt.OrderId).FirstOrDefault();
                        OrderProduct op = new OrderProduct { OrderId = ord, ProductId = currentproduct.ProductId, Count = 1 };
                        db.OrderProducts.Add(op);
                        db.SaveChanges();
                    }


                }
                MessageBox.Show("Товар добавлен в заказ");
            }
            else { MessageBox.Show("Выберите пунк выдачи"); }

        }
        private void entr_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Hide();
            } 

        public void ProductGrid_Selected(object sender, RoutedEventArgs e) { }
        public void ProductGrid_SelectionChanged (object sender, SelectionChangedEventArgs e) { }
        public void ProductGrid_SelectedCellsChanged (object sender, SelectionChangedEventArgs e) { }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            CatalogView.ItemsSource = sport.Products.Where(x=>x.ProductDiscountAmount <10 ).ToList();
            count.Content = "найдено" + CatalogView.Items.Count.ToString();
        }

        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {

            CatalogView.ItemsSource = sport.Products.Where(x => x.ProductDiscountAmount >10 && x.ProductDiscountAmount<15).ToList();
            count.Content = "найдено" + CatalogView.Items.Count.ToString();
        }

        private void ComboBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {

            CatalogView.ItemsSource = sport.Products.Where(x => x.ProductDiscountAmount > 15 && x.ProductDiscountAmount < 100).ToList();
            count.Content = "найдено" + CatalogView.Items.Count.ToString();
        }

        private void ComboBoxItem_Selected_3(object sender, RoutedEventArgs e)
        {

            CatalogView.ItemsSource = sport.Products.ToList();
            count.Content = "найдено" + CatalogView.Items.Count.ToString();
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            CatalogView.ItemsSource = sport.Products.Where(x => x.ProductName == search.Text).ToList();
            count.Content = "найдено" + CatalogView.Items.Count.ToString();

        }

        private void ComboBoxItem_Selected_4(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBoxItem_Selected_5(object sender, RoutedEventArgs e)
        {
            CatalogView.ItemsSource = sport.Products.OrderBy(x => x.ProductCost).ToList();
            count.Content = "найдено" + CatalogView.Items.Count.ToString();
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            CatalogView.ItemsSource = sport.Products.OrderByDescending(x => x.ProductCost).ToList();
            count.Content = "найдено" + CatalogView.Items.Count.ToString();
        }

        private void ComboBoxItem_Selected_6(object sender, RoutedEventArgs e)
        {
            CatalogView.ItemsSource = sport.Products.ToList();
            count.Content = "найдено" + CatalogView.Items.Count.ToString();
        }

        private void ComboBox_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {

        }

        private void to_order_Click(object sender, RoutedEventArgs e)
        {

            int idd = (from dt in sport.Users where dt.UserSurname + dt.UserName + dt.UserPatronymic == fio.Content.ToString() select dt.UserId).FirstOrDefault();
            int oid = Convert.ToInt32((from dt in sport.Orders where dt.UserId == idd select dt.OrderId).FirstOrDefault());

            int ord = (from ut in sport.OrderProducts where ut.OrderId == oid select ut.OrderId).FirstOrDefault();
            var p = (from ut in sport.OrderProducts from dt in sport.Orders where ut.OrderId == dt.OrderId select ut.ProductId).ToList();
            var order = sport.Orders.ToList().Find(x => x.OrderId == ord);

            Orders ord_w = new Orders(ord, order);
            ord_w.Show();
            ord_w.fio.Content = fio.Content;

            ord_w.orders.DataContext = sport.OrderProducts.Where(x => x.OrderId == oid).ToList();

            foreach (int a in p)
            {
                ord_w.summ_t.Text = (from ut in sport.Products where ut.ProductId == a select ut.ProductCost).ToList().Sum().ToString();
                ord_w.discount_t.Text = (from ut in sport.Products where ut.ProductId == a select ut.ProductDiscountAmount).ToList().Max().ToString();
            }

        }
    }
}
