﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.IO;

namespace Homework_12.Model
{
    /// <summary>
    /// List of clients
    /// </summary>
    public class ClientList : ObservableCollection<Client>
    {
        /// <summary>
        /// Reading file
        /// </summary>
        public void Get()
        {
            var load = JsonConvert.DeserializeObject<ObservableCollection<ClientJSON>>(
                File.ReadAllText("./clients_db.json"));

            if (load.Count > 0)
            {
                foreach (var item in load)
                {
                    this.Add(new Client(item.FullName, item.TaxId, item.PhoneNumber, item.Accounts));
                }
            }
            else
            {
                MessageBox.Show("BD is empty");
            }
        }

        /// <summary>
        /// Adding client
        /// </summary>
        /// <param name="FullName">Full name</param>
        /// <param name="TaxId">Tax id</param>
        /// <param name="PhoneNumber">Phone number</param>
        public void Add(string FullName, string TaxId, string PhoneNumber)
        {
            if (!FindClient(TaxId, false))
            {
                this.Add(new Client(FullName, TaxId, PhoneNumber));
            }
            else
            {
                MessageBox.Show("Client with this TaxId already exists");
            }
        }

        /// <summary>
        /// Updating clients database
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="field">Field</param>
        /// <param name="value">New value</param>
        public void Update(
            ref ClientList clients,
            Client client,
            string field,
            object value)
        {
            foreach (var item in typeof(Client).GetProperties())
            {
                if (item.Name == field)
                {
                    client.GetType()
                        .GetProperty(field)
                        .SetValue(client, value);
                    break;
                }
            }
        }

        /// <summary>
        /// Deleting client by tax id
        /// </summary>
        /// <param name="TaxId">TaxId</param>
        public void Remove(string TaxId)
        {
            if (FindClient(TaxId))
            {
                this.Remove(this.First(x => x.TaxId == TaxId));
            }
        }

        /// <summary>
        /// Adding account to selected client
        /// </summary>
        /// <typeparam name="T">Account type</typeparam>
        /// <param name="TaxId">TaxId</param>
        /// <param name="account">Account info</param>
        public void AddAccount<T>(string TaxId, T account) where T : Account
        {
            if (FindClient(TaxId))
            {
                this.First(x => x.TaxId == TaxId).AddAccount<T>(account);
            }
        }

        /// <summary>
        /// Searchin cliend by tax id
        /// </summary>
        /// <param name="TaxId">TaxId</param>
        /// <returns>true if client exists</returns>
        private bool FindClient(string TaxId, bool MessageShow = true)
        {
            if (this.Any(x => x.TaxId == TaxId))
            {
                return true;
            }
            else
            {
                if (MessageShow)
                {
                    MessageBox.Show($"Client with TaxId: \"{TaxId}\" was not found!");
                }
                return false;
            }
        }

        /// <summary>
        /// Saving changes
        /// </summary>
        public void SaveChanges()
        {
            string serialize = JsonConvert.SerializeObject(this);
            File.WriteAllText("./clients_db.json", serialize);
        }
    }
}
