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
using Excel = Microsoft.Office.Interop.Excel;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data;

namespace wpfsport
{
    /// <summary>
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Orders : Window
    {
        SportdbContext sport = new SportdbContext();
        Order order_s = new Order();
        public Order currentorder { get; set; }
        int ord1;
        public Orders(int ord, Order order)
        {
            InitializeComponent();
            point.ItemsSource = sport.PickupPoints.ToList();
            ord1=ord;
            currentorder = order;
            Update_datagrid();
        }

        public void ImageFromFolder()
        {
            
        }

        private void orders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public void Update_datagrid() //обновление таблицы
        {
            int idd = (from dt in sport.Users where dt.UserSurname + dt.UserName + dt.UserPatronymic == fio.Content.ToString() select dt.UserId).FirstOrDefault();
            int oid = Convert.ToInt32((from dt in sport.Orders where dt.UserId == idd select dt.OrderId).FirstOrDefault());
            orders.DataContext = sport.OrderProducts.Where(x => x.OrderId == oid).ToList();

            var p = (from ut in sport.OrderProducts from dt in sport.Orders where ut.OrderId == dt.OrderId select ut.ProductId).ToList();
            foreach (int a in p)
            {   summ_t.Text = (from ut in sport.Products where ut.ProductId == a select ut.ProductCost).ToList().Sum().ToString();
                discount_t.Text = (from ut in sport.Products where ut.ProductId == a select ut.ProductDiscountAmount).ToList().Max().ToString();
            }
        }
        private void delete_Click(object sender, RoutedEventArgs e) //удаление товара из заказа
        {

            using (var db = new SportdbContext())
            {
                var deleted = db.OrderProducts.ToList().Find(pr => pr.ProductId.ToString() == id_prod.Text);
                db.OrderProducts.Remove(deleted);
                db.SaveChanges();

            }
            MessageBox.Show("Товар удален");
            Update_datagrid();
        }

        private void pdf_Click(object sender, RoutedEventArgs e)
        {
            PrintOrderCard();
        }
        private void PrintOrderCard() //создание пдф-файла
        {
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            PdfWriter.GetInstance(doc, new FileStream("Card.pdf", FileMode.Create));
            doc.Open();
            BaseFont baseFont = BaseFont.CreateFont("C:/Windows/Fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Талон на получение", font));
            table.AddCell("Order number" + currentorder.OrderId);
            var fullProductList = string.Empty;
            fullProductList = currentorder.OrderProducts.Aggregate(fullProductList,
                (current, product) => current + $"{product.Product.ProductName}\n");
            table.AddCell("product list" + fullProductList);
            table.AddCell("Total cost" + currentorder.OrderProducts.Sum(p => p.Product.ProductCost));
            doc.Add(table);
            doc.Close();
            MessageBox.Show("Pdf-документ сохранен");

        }
        private void order_save_Click(object sender, RoutedEventArgs e) //сохранение заказа
        {
            using (SportdbContext sport = new SportdbContext())
            {
                int pid = (from ut in sport.PickupPoints where ut.Address == point.Text select ut.PickupPointId).FirstOrDefault();
                int ordd = (from ut in sport.OrderProducts where ut.OrderId == ord1 select ut.OrderId).FirstOrDefault();
                int idd = Convert.ToInt32((from ut in sport.Orders where ut.OrderId == ord1 select ut.UserId).FirstOrDefault());
                Random rnd = new Random();
                int i = rnd.Next(0, 3000);
                if (ordd == ord1)
                {
                    Order order = new Order();
                    order.OrderStatusId = 1;
                    order.PickupPointId = pid;
                    order.OrderCreateDate = DateTime.Now;
                    order.OrderDeliveryDate = DateTime.UtcNow.AddDays(5);
                    order.UserId = idd;
                    order.OrderGetCode = i;

                    sport.Orders.Update(order);
                    sport.SaveChanges();
                }
                else
                {
                    Order order = new Order();
                    order.OrderStatusId = 1;
                    order.PickupPointId = pid;
                    order.OrderCreateDate = DateTime.Now;
                    order.OrderDeliveryDate = DateTime.UtcNow.AddDays(5);
                    order.UserId = idd;
                    order.OrderGetCode = i;

                    sport.Orders.Add(order);
                    sport.SaveChanges();
                }
            }
            MessageBox.Show("Оформлено");
        }
    }
}
