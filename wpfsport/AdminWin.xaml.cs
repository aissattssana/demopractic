using Microsoft.Win32;
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
using System.IO;
using static Azure.Core.HttpHeader;
using System.Net;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace wpfsport
{
    /// <summary>
    /// Логика взаимодействия для AdminWin.xaml
    /// </summary>
    public partial class AdminWin : Window
    {
        public SportdbContext sport = new SportdbContext();
        Product currentproduct = new Product();

        public AdminWin()
        {
            InitializeComponent();
            sport = new SportdbContext();
            ProductGrid.ItemsSource = sport.Products.ToList();
           InitComboBoxes();
          //  InitId();
        }
        private void InitComboBoxes()
        {
            prod_category_combobox.ItemsSource = sport.ProductCategories.ToList();
            prod_manufacturer_combobox.ItemsSource = sport.ProductManufacturers.ToList();
            prod_unittype_combobox.ItemsSource = sport.UnitTypes.ToList();
            prod_supplier_combobox.ItemsSource = sport.ProductSuppliers.ToList();



            ////foreach (ProductCategory category in sport.ProductCategories)
            ////{
            ////    prod_category_combobox.Items.Add(category.ProductCategoryName);

            ////    if (category.ProductCategoryId == currentproduct.ProductCategoryId)
            ////    {
            ////        prod_category_combobox.SelectedItem = category.ProductCategoryName;
            ////    }
            ////}

            //foreach (UnitType unitType in sport.UnitTypes)
            //{
            //    prod_unittype_combobox.Items.Add(unitType.UnitTypeName);

            //    if (unitType.UnitTypeId == currentproduct.UnitTypeId)
            //    {
            //        prod_unittype_combobox.SelectedItem = unitType.UnitTypeName;
            //    }
            //}
            //foreach (ProductManufacturer manufacturer in sport.ProductManufacturers)
            //{
            //    prod_manufacturer_combobox.Items.Add(manufacturer.ProductManufacturerName);

            //    if (manufacturer.ProductManufacturerId == currentproduct.ProductManufacturerId)
            //    {
            //        prod_manufacturer_combobox.SelectedItem = manufacturer.ProductManufacturerName;
            //    }
            //}
            //foreach (ProductSupplier supplier in sport.ProductSuppliers)
            //{
            //    prod_supplier_combobox.Items.Add(supplier.ProductSupplierName);

            //    if (supplier.ProductSupplierId == currentproduct.ProductSupplierId)
            //    {
            //        prod_supplier_combobox.SelectedItem = supplier.ProductSupplierName;
            //    }
            //}

        }
        private void InitId()
        {
            foreach (ProductManufacturer man in sport.ProductManufacturers)
            {
                if (man.ProductManufacturerId == currentproduct.ProductManufacturerId)
                {
                    currentproduct.ProductManufacturer = man;
                }
            }

            foreach (ProductSupplier sup in sport.ProductSuppliers)
            {
                if (sup.ProductSupplierId == currentproduct.ProductSupplierId)
                {
                    currentproduct.ProductSupplier = sup;
                }
            }
            foreach (UnitType unit in sport.UnitTypes)
            {
                if (unit.UnitTypeId == currentproduct.UnitTypeId)
                {
                    currentproduct.UnitType = unit;
                }
            }
            foreach (ProductCategory cat in sport.ProductCategories)
            {
                if (cat.ProductCategoryId == currentproduct.ProductCategoryId)
                {
                    currentproduct.ProductCategory = cat;
                }
            }
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            this.Hide();
            main.ShowDialog();
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] textBoxes = { prod_cost, prod_count, prod_description, prod_discount, prod_name, prod_article};
          
            foreach (var textBox in textBoxes)
            {
                if (String.IsNullOrEmpty(textBox.Text))
                {
                    MessageBox.Show("Введите текст");
                    return;
                }
            }
            if (Convert.ToInt32(prod_cost.Text) < 0 || Convert.ToInt32(prod_maxdiscount.Text) < 0 || Convert.ToInt32(prod_count.Text) < 0 )
            {
                MessageBox.Show("Поле не может быть отрицательным!");
                return;
            }
            if (Convert.ToInt16(prod_discount.Text) > Convert.ToInt16(prod_maxdiscount.Text))
            {
                MessageBox.Show("Скидка должна быть меньше максимальной!");
                return;
            }
            var manufacturer = sport.ProductManufacturers.Where(m => m.ProductManufacturerName == prod_manufacturer_combobox.Text).FirstOrDefault();
            var supplier = sport.ProductSuppliers.Where(s => s.ProductSupplierName == prod_supplier_combobox.Text).FirstOrDefault();
            var unit = sport.UnitTypes.Where(m => m.UnitTypeName == prod_unittype_combobox.Text).FirstOrDefault();
            var category = sport.ProductCategories.Where(s => s.ProductCategoryName == prod_category_combobox.Text).FirstOrDefault();

            if (!ValidateManufacturerAndSupplier(manufacturer, supplier, unit, category))
            {
                MessageBox.Show("Неверно заполнено поле!");
                return;
            }

            var newProduct = new Product()
            {
                ProductName = prod_name.Text,
                ProductArticleNumber = prod_article.Text,
                ProductCost = Convert.ToDecimal(prod_cost.Text),
                ProductDescription = prod_description.Text,
                ProductDiscountAmount = Convert.ToByte(prod_discount.Text),
                ProductQuantityInStock = Convert.ToByte(prod_count.Text),
                ProductMaxDiscountAmount = Convert.ToByte(prod_maxdiscount.Text),
                ProductCategoryId = ((ProductCategory)(prod_category_combobox.SelectedItem)).ProductCategoryId,
                ProductManufacturerId = ((ProductManufacturer)(prod_manufacturer_combobox.SelectedItem)).ProductManufacturerId,
                ProductSupplierId = ((ProductSupplier)(prod_supplier_combobox.SelectedItem)).ProductSupplierId,
                UnitTypeId = ((UnitType)(prod_unittype_combobox.SelectedItem)).UnitTypeId,
                ProductPhoto = string.Empty,

            };
            sport.Products.Add(newProduct);
            sport.SaveChanges();
            MessageBox.Show("Товар добавлен");
            foreach (var textBox in textBoxes)
            {
                textBox.Text = string.Empty;
            }
            ProductGrid.ItemsSource = sport.Products.ToList();
        }
        private bool ValidateManufacturerAndSupplier(ProductManufacturer man, ProductSupplier sup, UnitType unit, ProductCategory category )
        {
            if (sport.ProductManufacturers.Contains(man) && sport.ProductSuppliers.Contains(sup) && sport.UnitTypes.Contains(unit) && sport.ProductCategories.Contains(category))
            {
                return true;
            }
            return false;
        }
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            sport.Products.Remove(currentproduct);
            sport.SaveChanges();
            MessageBox.Show("Товар удален");
            ProductGrid.ItemsSource = sport.Products.ToList();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] textBoxes = { prod_cost,prod_maxdiscount, prod_count, prod_description, prod_discount, prod_name, prod_article };
            string cost = prod_cost.Text.Replace(".", ",");
            using (var sport = new SportdbContext())
                if(currentproduct != null)
            {
                    currentproduct.ProductName = prod_name.Text;
                    currentproduct.ProductArticleNumber = prod_article.Text;
                    currentproduct.ProductCost = Convert.ToDecimal(cost);
                    currentproduct.ProductDescription = prod_description.Text;
                    currentproduct.ProductDiscountAmount = Convert.ToByte(prod_discount.Text);
                    currentproduct.ProductQuantityInStock = Convert.ToByte(prod_count.Text);
                    currentproduct.ProductMaxDiscountAmount = Convert.ToByte(prod_maxdiscount.Text);
                    currentproduct.UnitTypeId = 1;
                currentproduct.ProductCategoryId = ((ProductCategory)(prod_category_combobox.SelectedItem)).ProductCategoryId;
                    currentproduct.ProductManufacturerId = ((ProductManufacturer)(prod_manufacturer_combobox.SelectedItem)).ProductManufacturerId;
                    currentproduct.ProductSupplierId = ((ProductSupplier)(prod_supplier_combobox.SelectedItem)).ProductSupplierId;
                currentproduct.ProductPhoto = string.Empty;

            };
            sport.SaveChanges();
            MessageBox.Show("Товар изменен!");
            ProductGrid.ItemsSource = sport.Products.ToList();
            foreach (var textBox in textBoxes)
            {
                textBox.Text = string.Empty;
            }
        }

        private void photo_open_Click(object sender, RoutedEventArgs e)
        {
           /* OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) { }
             //   txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
           */

        }
        public void ProductGrid_Selected(object sender, RoutedEventArgs e) { }
        public void ProductGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        public void ProductGrid_SelectedCellsChanged(object sender, SelectionChangedEventArgs e) { }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                foreach (var ut in sport.ProductCategories)
                {
                    if (ut.ProductCategoryName == prod_category_combobox.SelectedValue)
                    {
                        currentproduct.ProductCategory = ut;
                    }
                }
            }
            catch (InvalidOperationException)
            {
                return;
            }
        }

        private void prod_unittype_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                foreach (var ut in sport.UnitTypes)
                {
                    if (ut.UnitTypeName == prod_unittype_combobox.SelectedValue)
                    {
                        currentproduct.UnitType = ut;
                    }
                }
            }
            catch (InvalidOperationException)
            {
                return;
            }
        }

        private void prod_supplier_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                foreach (var ut in sport.ProductSuppliers)
                {
                    if (ut.ProductSupplierName == prod_supplier_combobox.SelectedValue)
                    {
                        currentproduct.ProductSupplier = ut;
                    }
                }
            }
            catch (InvalidOperationException)
            {
                return;
            }
        }

        private void prod_manufacturer_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                foreach (var ut in sport.ProductManufacturers)
                {
                    if (ut.ProductManufacturerName == prod_manufacturer_combobox.SelectedValue)
                    {
                        currentproduct.ProductManufacturer = ut;
                    }
                }
            }
            catch (InvalidOperationException)
            {
                return;
            }
        }

        private void clean_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
