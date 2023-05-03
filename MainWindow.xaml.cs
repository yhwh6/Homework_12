﻿using Homework_12.Interface;
using Homework_12.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace Homework_12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Page
    {
        public static ClientList? clients;
        public static ITransfer<SideAccount, SideAccount> transfer = new Transfer();
        public static string? CurrentClientTaxId;
        public static object? CurrentAccountNumber;

        public MainWindow()
        {
            if (clients == null)
            {
                clients = new ClientList();
                clients.Get();
            }
            if (clients.Count > 0)
            {
                InitializeComponent();
                dgClients.ItemsSource = clients;
                dgAccounts.ItemsSource = clients.First().Accounts;
                CurrentClientTaxId = clients.First().TaxId;
            }
            clients.SaveChanges();
        }

        private void MenuAddClient(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("pAddClient.xaml", UriKind.Relative));
        }

        private void MenuDeleteClient(object sender, RoutedEventArgs e)
        {
            if (dgClients.SelectedItems.Count > 0)
            {
                Client client = dgClients.SelectedItems[0] as Client;
                clients.Remove(client.TaxId);
                clients.SaveChanges();
            }
            else
            {
                MessageBox.Show("Select client to delete");
            }
        }

        private void dgClients_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            CurrentClientTaxId = ((Client)e.Row.DataContext).TaxId;
            clients.Update(ref clients, (Client)e.Row.DataContext, e.Column.SortMemberPath, (e.EditingElement as TextBox).Text);
            clients.SaveChanges();
        }

        private void dgClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CurrentClientTaxId = ((Client)e.AddedItems[0]).TaxId;
                dgAccounts.ItemsSource = ((Client)e.AddedItems[0]).Accounts;
            }
            catch
            {
                ;
            }
        }

        private void MenuAddAccount(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("pAddAccount.xaml", UriKind.Relative));
        }

        private void MenuDeleteAccount(object sender, RoutedEventArgs e)
        {
            Client client;
            if (dgClients.SelectedItems.Count > 0)
            {
                client = dgClients.SelectedItems[0] as Client;
            }
            else
            {
                MessageBox.Show("Select client");
                return;
            }

            if (dgAccounts.SelectedItems.Count > 0)
            {
                client.Accounts.Remove(dgAccounts.SelectedItems[0] as IAccount<Account>);
                clients.SaveChanges();
            }
            else
            {
                MessageBox.Show("Select client's account you want to delete");
            }
        }

        private void MenuTopUpBalance(object sender, RoutedEventArgs e)
        {
            if (dgAccounts.SelectedItems.Count > 0)
            {
                CurrentAccountNumber = (dgAccounts.SelectedItems[0] as Account).Number;
                NavigationService.Navigate(new Uri("pTransferFunds.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Select account to top up");
            }
        }

        private void MenuTransfer(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("pTransfer.xaml", UriKind.Relative));
        }
    }
}
