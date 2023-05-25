# demosportwpf
private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var foundAgents = services.Where(x => x.Name.ToLower().Contains(searchTextBox.Text.ToLower())).ToList();
            servicesList.ItemsSource = foundAgents;
        }
        
        case 1:
                    {
                        services = db.Services.Where(x => x.Discount >= 0 && x.Discount < 5).ToList();
                        servicesList.ItemsSource = services;
                        break;
                    }
case 0:
                    {
                        services = services.OrderBy(x => x.CostWithDiscount).ToList();
                        servicesList.ItemsSource = services;
                        break;
                    }
private void InitImage()
        {
            BitmapImage imageSource = new BitmapImage();
            imageSource.BeginInit();
            try
            {
                if (_currentService != null)
                {
                    imageSource.UriSource = new Uri(@"/DemoApp;component" + _currentService.Photo);
                    BitmapImage picture = new BitmapImage();
                    picture.BeginInit();
                    picture.UriSource = new Uri(@"/DemoApp;component/Resources/", UriKind.Relative);
                    if (imageSource.UriSource == picture.UriSource)
                    {
                        imageSource.UriSource = new Uri(@"/DemoApp;component/Resources/school_logo.png", UriKind.Relative);
                    }
                }
                else
                    imageSource.UriSource = new Uri(@"/DemoApp;component/Resources/school_logo.png");
            }
            catch
            {
                return;
            }
            imageSource.EndInit();
            Picture.Source = imageSource;
        }
if (_currentService == null)
                return;
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(NameTextBox.Text))
                MessageBox.Show("Введите название", "Ошибка");
            if (string.IsNullOrEmpty(DurationTextBox.Text))
                MessageBox.Show("Введите длительность", "Ошибка");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка");
                return;
            }
            _currentService.Name = NameTextBox.Text;
            _currentService.Cost = Convert.ToDouble(CostTextBox.Text);
            _currentService.Duration = Convert.ToInt32(DurationTextBox.Text);
            _currentService.Discount = Convert.ToInt32(DiscountTextBox.Text);
            try
            {
                db.Services.Add(_currentService);
                db.SaveChanges();
                MessageBox.Show("Данные были успешно добавлены!", "Внимание");
                var window = new ServicesWindow();
                window.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка");
            }
 private void create_Click(object sender, RoutedEventArgs e)
        {
            Type a = cost1.Text.GetType();

            if (User2Context.Services.Any(x => x.Name != name1.Text))
            {
                if (Convert.ToInt32(time1.Text) < 14400)
                {
                    Service usl = new Service { Name = name1.Text, Photo = null, Duration = Convert.ToInt32(time1.Text), Cost =Convert.ToDouble(cost1.Text), Discount = Convert.ToInt16(skidka1.Text) };
                    User2Context.Services.Add(usl);
                    User2Context.SaveChanges();
                    MessageBox.Show("Услуга добавлена");
                       }
                else { MessageBox.Show("Продолжительность не может превышать 4 часов(14400 сек)"); }
            }
            else { MessageBox.Show("Такая услуга уже существует"); }
        }
