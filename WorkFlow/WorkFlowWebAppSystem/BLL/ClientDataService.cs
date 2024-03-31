using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using WorkFlowSystem.DAL;
using WorkFlowSystem.Entities;
using WorkFlowSystem.ViewModels;

namespace WorkFlowSystem.BLL
{
    public class UserProfile
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
    }

    public class ClientDataService
    {
        private readonly WFS_2590Context? _WFS_2590Context;
        internal ClientDataService(WFS_2590Context WFS_2590Context)
        {
            _WFS_2590Context = WFS_2590Context;
        }

        public ClientSearchView GetClient(string UserName, string Password)
        {
            return _WFS_2590Context.Clients
                .Where(x => x.Username == UserName && x.Password == GetHash(Password))
                .Select(c => new ClientSearchView
                {
                    ClientID = c.ClientID,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Phone = c.Phone,
                    City = c.City
                })
                .FirstOrDefault();
        }
        public Client? GetByID(int ID)
        {
            return _WFS_2590Context.Clients
                .FirstOrDefault(x => x.ClientID == ID);
        }

        public Client? GetByUserName(string Username , string Password)
        {
            return _WFS_2590Context.Clients
                .FirstOrDefault(x => x.Username == Username && x.Password == GetHash(Password));
        }

        public Client? GetByEmail(string Email)
        {
            //            builder.ConfigurationGetConnectionString();
            //            bool bConn = IsServerConnected("Server=localhost;Database=WFS_2590;Trusted_Connection=true;encrypt=false;MultipleActiveResultSets=true");
            DbSet<Client> cList = _WFS_2590Context.Clients;
            try
            {
                return cList.FirstOrDefault(x => x.Email == Email);
            }
            catch ( Exception e )
            {
                return null;
            }
        }

        public async Task<List<ClientSearchView>> GetClients()
        {
            return await _WFS_2590Context.Clients
                .Select(c => new ClientSearchView
                {
                    ClientID = c.ClientID,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    UserName = c.Username,
                    Password = "",
                    Company = c.Company,
                    Email = c.Email,
                    Phone = c.Phone,
                    City = c.City,
                    Status = c.Status,
                })
                .ToListAsync();
        }
        public ClientEditView GetClientEdit(int ClientID)
        {

            if (ClientID == 0)
            {
                throw new ArgumentNullException("Please provide a Client");
            }

            return _WFS_2590Context.Clients
                .Where(x => (x.ClientID == ClientID))
                .Select(x => new ClientEditView
                {
                    ClientID = x.ClientID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Province = x.Province,
                    City = x.City,
                    Company = x.Company,
                    Phone = x.Phone,
                    Email = x.Email,
                    Password = "",
                    UserName = x.Username,
                    Address1 = x.Address1
                }).FirstOrDefault();
        }

        public List<ClientSearchView> GetClients(string lastName, string phone)
        {
            if (string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentNullException("Please provide either a last name and/or phone number");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                lastName = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                phone = Guid.NewGuid().ToString();
            }

            return _WFS_2590Context.Clients
                .Where(x => (x.LastName.Contains(lastName)
                             || x.Phone.Contains(phone)))
                .Select(c => new ClientSearchView
                {
                    ClientID = c.ClientID,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Phone = c.Phone,
                    City = c.City,
                })
                .ToList();
        }

        public void AddEditClient(ClientEditView editClient)
        {
            #region Business Logic and Parameter Exceptions
      
            List<Exception> errorList = new List<Exception>();
       
            if (editClient == null)
            {
                throw new ArgumentNullException("No client was supplied");
            }


            if (string.IsNullOrWhiteSpace(editClient.FirstName))
            {
                errorList.Add(new Exception("First name is required"));
            }

            if (string.IsNullOrWhiteSpace(editClient.LastName))
            {
                errorList.Add(new Exception("Last name is required"));
            }

            if (string.IsNullOrWhiteSpace(editClient.Email))
            {
                errorList.Add(new Exception("Email is required"));
            }

            if (string.IsNullOrWhiteSpace(editClient.Phone))
            {
                errorList.Add(new Exception("Phone is required"));
            }

            #endregion

            Client client = new Client();
            //client.ClientID = GenerateClientId();
            client.Username = editClient.UserName;
            client.Role = editClient.Role = "Client";
            client.FirstName = editClient.FirstName;
            client.LastName = editClient.LastName;
            client.City = editClient.City;
            client.Province = editClient.Province;
            client.Address1 = editClient.Address1;
            client.Company = editClient.Company;
            client.Email = editClient.Email;
            client.Phone = editClient.Phone;
            client.Password = GetHash(editClient.Password);

            _WFS_2590Context.Clients.Add(client);
            _WFS_2590Context.SaveChanges();
            if (errorList.Count > 0)
            {
                throw new AggregateException("Unable to add or edit client. Please check error message(s)", errorList);
            }
        }

        //private int GenerateClientId()
        //{
        //    int maxClientId = _WFS_2590Context.Clients.Max(c => (int?)c.ClientID) ?? 0;
        //    int nextClientId = maxClientId + 1;
        //    return nextClientId;
        //}

        public bool ClientExistsWithEmail(string email, int currentClientId)
        {
            return _WFS_2590Context.Clients.Any(c => c.Email == email && c.ClientID != currentClientId);
        }

        // Example method to check for duplicate phone number
        public bool ClientExistsWithPhone(string phone, int currentClientId)
        {
            return _WFS_2590Context.Clients.Any(c => c.Phone == phone && c.ClientID != currentClientId);
        }


        public async Task<bool> DeleteClientAsync(int clientId)
        {
            var client = await _WFS_2590Context.Clients.FindAsync(clientId);
            if (client == null)
            {
                return false;
            }

            _WFS_2590Context.Clients.Remove(client);
            await _WFS_2590Context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateClientStatusAsync(int clientId, string status)
        {
            var client = await _WFS_2590Context.Clients.FindAsync(clientId);
            if (client == null)
            {
                return false;
            }
            client.Status = status;

            await _WFS_2590Context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RegistClientResetPwdToken(int ClientID, string token)
        {
            Client client = await _WFS_2590Context.Clients.FindAsync(ClientID);

            client.PwdToken = token;

            try
            {
                var ret = await _WFS_2590Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool IsExistToken(string sToken, int nID, out string newPwd)
        {
            newPwd = "";
            try {
                var k = _WFS_2590Context.Clients.Where(x => x.PwdToken.Contains(sToken) && x.ClientID == nID)
                    .Select(x => x.PwdToken).ToList();
                if (k == null || k.Count() == 0) 
                    return false;
                newPwd = k.FirstOrDefault();
                newPwd = newPwd.Substring(newPwd.Length - 12);
                return true;
            } catch (Exception ex) {
                return false;
            }
        }

        public async Task<bool> SetPassword(int nID, string v)
        {
            Client client = await _WFS_2590Context.Clients.FindAsync(nID);

            client.PwdToken = "";
            client.Password = GetHash(v);

            try
            {
                var ret = await _WFS_2590Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateProfile(int nID, UserProfile profile)
        {
            Client client = await _WFS_2590Context.Clients.FindAsync(nID);

            client.FirstName = profile.FirstName;
            client.LastName = profile.LastName;

            try {
                var ret = await _WFS_2590Context.SaveChangesAsync();
                return true;
            } catch (Exception ex) {
                return false;
            }
        }

        public async Task UpdateClientInfo(ClientEditView editClient)
        {
            #region Business Logic and Parameter Exceptions

            List<Exception> errorList = new List<Exception>();

            if (editClient == null)
            {
                throw new ArgumentNullException("No client was supplied");
            }


            if (string.IsNullOrWhiteSpace(editClient.FirstName))
            {
                errorList.Add(new Exception("First name is required"));
            }

            if (string.IsNullOrWhiteSpace(editClient.LastName))
            {
                errorList.Add(new Exception("Last name is required"));
            }

            if (string.IsNullOrWhiteSpace(editClient.Email))
            {
                errorList.Add(new Exception("Email is required"));
            }

            if (string.IsNullOrWhiteSpace(editClient.Phone))
            {
                errorList.Add(new Exception("Phone is required"));
            }

            if (editClient.ClientID == 0)
            {
                bool clientExist = _WFS_2590Context.Clients
                                    .Any(x => x.FirstName == editClient.FirstName
                                            && x.LastName == editClient.LastName
                                            && x.Phone == editClient.Phone);

                if (clientExist)
                {
                    errorList.Add(new Exception("Client already exists in the database and cannot be added again"));
                }
            }
            #endregion

            try
            {
                Client client = await _WFS_2590Context.Clients.FindAsync(editClient.ClientID);
                client.Username = editClient.UserName;
                client.Role = editClient.Role = "Client";
                client.FirstName = editClient.FirstName;
                client.LastName = editClient.LastName;
                client.City = editClient.City;
                client.Province = editClient.Province;
                client.Address1 = editClient.Address1;
                client.Company = editClient.Company;
                client.Email = editClient.Email;
                client.Phone = editClient.Phone;
                if (editClient.Password != "")
                    client.Password = GetHash(editClient.Password);
                await _WFS_2590Context.SaveChangesAsync();
           }
            catch (Exception e)
            {
                errorList.Add(new Exception("Input Client ID is not correct"));
            }

            if (errorList.Count > 0)
            {
                throw new AggregateException("Unable to add or edit client. Please check error message(s)", errorList);
            }
        }

        public static string GetHash(string src)
        {
            byte[] salt = Encoding.ASCII.GetBytes("PwdSalt-CLI");

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: src!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
        }

        public bool IsValidPassword(int UserID, string password)
        {
            return _WFS_2590Context.Clients.Any(c => c.ClientID == UserID && c.Password == GetHash(password));
        }
    }
}
