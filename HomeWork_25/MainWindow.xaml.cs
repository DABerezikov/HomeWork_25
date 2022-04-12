using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace HomeWork_25
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<IClient> dbclients;
        string Path;


        public MainWindow()
        {


            InitializeComponent();

            dbclients = new ObservableCollection<IClient>();
            Path = @"dbclients.json";
            ReadOnli_RefillBox(false);
            dbclients = LoadSave.LoadDB(Path);
            Clients.ItemsSource = dbclients;


        }

        private void Clients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (GetTypeAccount())
            {
                case null:
                    ClearAccountTextBox();
                    break;

                case "Депозит":                    
                    SetAccountTextBox();
                    break;
            }
            OpenDateBox_Text();

        }
        private void RefillButton_Click(object sender, RoutedEventArgs e)
        {
            if (OpenDateBox.Text != "Нет счета")
            {
                var t = dbclients[Clients.SelectedIndex];
                t.Deposit.Refill(double.Parse(RefillBox.Text));

                LoadSave.SaveDB(Path, dbclients);
                Refresh();
            }
        }

        private void OpenAccount_Click(object sender, RoutedEventArgs e)
        {
            var t = dbclients[Clients.SelectedIndex];
            if (t is IndividualClient)
            {

            }

            if (RefillBox.Text != "") t.AddDepositAcount(new Account<IndividualClient>(double.Parse(RefillBox.Text), t));
            else t.AddDepositAcount(new Account<IndividualClient>(0, t));

            LoadSave.SaveDB(Path, dbclients);
            Refresh();
            OpenAccount.Visibility = Visibility.Collapsed;
        }
        private void ClientAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ClientName.Text != "" && ClientName.Text != "Введите имя клиента")
            {
                switch (ClientType.Text)
                {
                    case "Частное лицо":
                        var newClient = new IndividualClient(ClientName.Text);

                        if (ClientAmount.Text != "Сумма зачисления" && ClientAmount.Text != "")
                        {
                            newClient.AddDepositAcount(new Account<IndividualClient>(double.Parse(ClientAmount.Text), newClient));
                        }
                        dbclients.Add(newClient);
                        break;

                    case "Юридическое лицо":
                        var newEClient = new EntityClient(ClientName.Text);

                        if (ClientAmount.Text != "Сумма зачисления" && ClientAmount.Text != "")
                        {
                            newEClient.AddDepositAcount(new Account<EntityClient>(double.Parse(ClientAmount.Text), newEClient));
                        }
                        dbclients.Add(newEClient);
                        break;

                    case "VIP клиент":
                        var newVClient = new VIPClient(ClientName.Text);

                        if (ClientAmount.Text != "Сумма зачисления" && ClientAmount.Text != "")
                        {
                            newVClient.AddDepositAcount(new Account<VIPClient>(double.Parse(ClientAmount.Text), newVClient));
                        }
                        dbclients.Add(newVClient);
                        break;
                    case null:
                        break;

                }

                LoadSave.SaveDB(Path, dbclients);
                ClientName.Text = "Введите имя клиента";
                ClientAmount.Text = "Сумма зачисления";

            }


        }

        private bool isFocused = false;
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var TB = sender as TextBox;
            isFocused = true;
            if (TB.Text == "")
            {
                TB.Text = " ";
                TB.SelectAll();
            }
        }
        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {

            var TB = sender as TextBox;
            if (isFocused && TB.IsReadOnly == false)
            {
                isFocused = false;
                TB.SelectAll();
            }

        }
        private void ClientDelete_Click(object sender, RoutedEventArgs e)
        {
            dbclients.Remove(Clients.SelectedItem as IClient);
            LoadSave.SaveDB(Path, dbclients);
        }
        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {

            if (double.Parse(RefillBox.Text) <= double.Parse(AmmountBox.Text))
            {
                var a = dbclients[Transfer.SelectedIndex];
                var i = dbclients[Clients.SelectedIndex];

                i.Deposit.TransferAccount(i.Deposit, a.Deposit, double.Parse(RefillBox.Text));

                LoadSave.SaveDB(Path, dbclients);
                Transfer.ItemsSource = null;
                Refresh();
                ViewTransferButton();
            }
            else
            {
                MessageBox.Show("Недопустимая сумма");
            }

        }
        private void OpenTransfer_Click(object sender, RoutedEventArgs e)
        {
            Transfer.ItemsSource = dbclients;
            OpenTransfer.Visibility = Visibility.Collapsed;
            CancelTransfer.Visibility = Visibility.Visible;

        }
        private void Transfer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Clients.SelectedItem != Transfer.SelectedItem && GetTransferAccount())
            {
                RefillButton.Visibility = Visibility.Collapsed;
                TransferButton.Visibility = Visibility.Visible;
            }
            else
            {
                RefillButton.Visibility = Visibility.Visible;
                TransferButton.Visibility = Visibility.Collapsed;
            }


        }
        private void CancelTransfer_Click(object sender, RoutedEventArgs e)
        {
            Transfer.ItemsSource = null;
            ViewTransferButton();

        }

        private void CloseAccount_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show($"К выплате {dbclients[Clients.SelectedIndex].Deposit.CloseAccount()} руб.");
            dbclients[Clients.SelectedIndex].Deposit = null;

            Clients_SelectionChanged();
            LoadSave.SaveDB(Path, dbclients);
            CloseAccount.Visibility = Visibility.Collapsed;
            ClientDelete.Visibility = Visibility.Visible;
        }

        private void Clients_SelectionChanged()
        {
            ReadOnli_RefillBox(false);
            SetAccountTextBox();
        }

        private void SetAccountTextBox()
        {
            var t = dbclients[Clients.SelectedIndex];
            AmmountBox.Text = t.Deposit != null ? t.Deposit.Amount.ToString() : default;
            InterestBox.Text = t.Deposit != null ? (t.Deposit.Interest + t.Deposit.TempInterest).ToString() : default;
            RefillDateBox.Text = t.Deposit != null && t.Deposit.RefillDate != null ? t.Deposit.RefillDate.ToString() : default;
            OpenDateBox.Text = t.Deposit != null && t.Deposit.OpeningDate != null ? t.Deposit.OpeningDate.ToString() : "Нет счета";
            ClosingDateBox.Text = t.Deposit != null && t.Deposit.ClosingDate != null ? t.Deposit.ClosingDate.ToString() : default;
            CloseAccount.Visibility = t.Deposit == null ? Visibility.Collapsed : Visibility.Visible;
            ClientDelete.Visibility = t.Deposit == null ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ClearAccountTextBox()
        {
            AmmountBox.Text = "";
            InterestBox.Text = "";
            OpenDateBox.Text = "";
            RefillDateBox.Text = "";
            ClosingDateBox.Text = "";
        }
        private string GetTypeAccount()
        {
            return (ChoiceAccount.SelectedValue != null) ? ((ChoiceAccount.SelectedValue) as ComboBoxItem).Content.ToString() : default;
        }
        
        private void ReadOnli_RefillBox(bool Choice)
        {
            RefillBox.IsReadOnly = Choice;
        }
        private void Refresh()
        {
            RefillBox.Text = null;
            Clients_SelectionChanged();
        }
        private void OpenDateBox_Text()
        {
            OpenAccount.Visibility = OpenDateBox.Text != "Нет счета" ? Visibility.Collapsed : Visibility.Visible;
            OpenTransfer.Visibility = ChoiceAccount.SelectedValue != null
                ? OpenDateBox.Text != "Нет счета" ? Visibility.Visible : Visibility.Collapsed
                : Visibility.Collapsed;

        }
       

        private void ViewTransferButton()
        {
            OpenTransfer.Visibility = Visibility.Visible;
            CancelTransfer.Visibility = Visibility.Collapsed;
            RefillButton.Visibility = Visibility.Visible;
            TransferButton.Visibility = Visibility.Collapsed;
        }

        private bool GetTransferAccount()
        {
            try
            {
                return dbclients[Transfer.SelectedIndex].Deposit != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

       


    }
}
