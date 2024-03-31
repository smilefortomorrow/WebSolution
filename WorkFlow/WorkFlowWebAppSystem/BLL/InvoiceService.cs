using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using WorkFlowSystem.DAL;
using WorkFlowSystem.Entities;
using WorkFlowSystem.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorkFlowSystem.BLL
{
    public class InvoiceService
    {
        private readonly WFS_2590Context? _WFS_2590Context;
        internal InvoiceService(WFS_2590Context WFS_2590Context)
        {
            _WFS_2590Context = WFS_2590Context;
        }

        public async Task<int> UploadInvoice(
            string InvoiceNo,
            int UserID,
            string Username,
            string Email,
            int PackageCount,
            double Total,
            string Status,
            DateTime Regist,
            double GST,
            string City,
            string Country,
            string PostalCode,
            string BusinessNo
            )
        {
            int ret;

            // calculate weekend day of this week
            TimeSpan span = new TimeSpan(6 - (int)Regist.DayOfWeek, 0,0,0);
            DateTime dt = Regist + span;
            Invoice invoice = new Invoice();

            //. check database with username and date
            var OldInvoice = _WFS_2590Context.Invoices
                                            .Where(x => x.ClientID == UserID && x.Regist == dt)
                                            .FirstOrDefault();

            try {
                if (OldInvoice == null) {
                    invoice.InvoiceNo = InvoiceNo;
                    invoice.ClientID = UserID;
                    invoice.Username = Username;
                    invoice.Email = Email;
                    invoice.City = City;
                    invoice.Country = Country;
                    invoice.PostalCode = PostalCode;
                    invoice.BusinessNo = BusinessNo;

                    invoice.PackageCount = PackageCount;
                    invoice.Total = Total;
                    invoice.Status = Status;
                    invoice.Regist = dt;
                    invoice.GST = GST;
                    ret = (await _WFS_2590Context.Invoices.AddAsync(invoice)).Entity.ID;
                } else {
                    invoice = await _WFS_2590Context.Invoices.FindAsync(OldInvoice.ID);
                    invoice.InvoiceNo = InvoiceNo;
                    invoice.ClientID = UserID;
                    invoice.Username = Username;
                    invoice.Email = Email;
                    invoice.City = City;
                    invoice.Country = Country;
                    invoice.PostalCode = PostalCode;
                    invoice.BusinessNo = BusinessNo;
                    invoice.PackageCount = PackageCount;
                    invoice.Total = Total;
                    invoice.Status = Status;
                    invoice.Regist = dt;
                    invoice.GST = GST;
                    ret = OldInvoice.ID;
                }

                await _WFS_2590Context.SaveChangesAsync();
                ret = invoice.ID;
                return ret;
            } catch {
                return 0;
            }
        }

        public async Task<List<InvoiceView>> GetInvoiceList(string status)
        {
            DateTime dtStart;
            var invoice_list = await _WFS_2590Context.Invoices
                            .Where(x => x.Status == status)
                            .Select(x => new InvoiceView {
                                ID = x.ID,
                                InvoiceNo = x.InvoiceNo,
                                UserID = x.ClientID,
                                Username = x.Username,
                                City = x.City,
                                Email = x.Email,
                                Country = x.Country,
                                PostalCode = x.PostalCode,
                                Business = x.BusinessNo,
                                PackageCount = x.PackageCount,
                                Total = x.Total,
                                Status = x.Status,
                                Regist = x.Regist,
                                GST = x.GST
                            }).ToListAsync();
            return invoice_list;
        }

        public async Task<bool> InvoiceToHistory(InvoiceView invoiceView)
        {
            DateTime dtStart;
            var invoice = await _WFS_2590Context.Invoices
                .FindAsync(invoiceView.ID);
            invoice.Status = "Send";
            try {
                await _WFS_2590Context.SaveChangesAsync();
                return true;
            }catch(Exception ex) {
                return false;
            }
        }

        public async Task<InvoiceView> GetLastInvoice()
        {
            var invoice = await _WFS_2590Context.Invoices
                .OrderBy(x => x.Regist)
                .OrderBy(x => x.ID)
                .Select(x => new InvoiceView() {
                    ID = x.ID,
                    InvoiceNo = x.InvoiceNo,
                    City = x.City,
                    Country = x.Country,
                    PostalCode = x.PostalCode,
                    Business = x.BusinessNo,
                    UserID = x.ClientID,
                    Username = x.Username,
                    Email = x.Email,
                    PackageCount = x.PackageCount,
                    Total = x.Total,
                    Status = x.Status,
                    Regist = x.Regist,
                    GST = x.GST
                })
                .LastOrDefaultAsync();
            return invoice;
        }

        public async Task<string> GetNewInvoiceName()
        {
            DateTime now = DateTime.Now;
            DateTime monthStart = new DateTime(now.Year, now.Month, 1);
            DateTime monthEnd = DateTime.Now;
            int count = await _WFS_2590Context.Invoices
                .Where(x => x.Regist >= monthStart)
                .CountAsync();

            string ret = "PK" + now.ToString("-yyyy-MM-") + (count+1);
            return ret;
        }
    }
}
