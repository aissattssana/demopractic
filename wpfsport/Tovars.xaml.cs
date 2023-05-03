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
        public Tovars()
        {
            InitializeComponent();
            sport = new SportdbContext();
            //sport.Products.Load();
            CatalogView.ItemsSource = sport.Products.ToList();
            count.Content = CatalogView.Items.Count.ToString();

            
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
            count.Content = CatalogView.Items.Count.ToString();
        }

        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {

            CatalogView.ItemsSource = sport.Products.Where(x => x.ProductDiscountAmount >10 && x.ProductDiscountAmount<15).ToList();
            count.Content = CatalogView.Items.Count.ToString();
        }

        private void ComboBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {

            CatalogView.ItemsSource = sport.Products.Where(x => x.ProductDiscountAmount > 15 && x.ProductDiscountAmount < 100).ToList();
            count.Content = CatalogView.Items.Count.ToString();
        }

        private void ComboBoxItem_Selected_3(object sender, RoutedEventArgs e)
        {

            CatalogView.ItemsSource = sport.Products.ToList();
            count.Content = CatalogView.Items.Count.ToString();
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            CatalogView.ItemsSource = sport.Products.Where(x => x.ProductName == search.Text).ToList();
            count.Content = CatalogView.Items.Count.ToString();

        }
       
    }
}
