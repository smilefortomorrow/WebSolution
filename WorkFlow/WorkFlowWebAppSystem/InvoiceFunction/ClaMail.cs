using EmailService;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowSystem.BLL;
using WorkFlowSystem.ViewModels;

namespace WorkFlowSystem.InvoiceFunction
{
    public class ClaMail
    {
        public ClaMail(EmailSender emailSender)
        {
            this.emailSender = emailSender;
        }
        public EmailSender emailSender { get; set; }

        public async Task<bool> send(string email, InvoiceView invoiceView, List<PackageView> l)
        {
            string body = GenerateBody(invoiceView, l);
            try {
                await emailSender.Send(email, "WorkFlow Invoice", body);
            } catch (Exception ex) {
                return false;
            }
            return true;
        }

        protected static string Main = "<!DOCTYPE html>\r\n<html lang=\"en, id\">\r\n  <head>\r\n    <meta charset=\"UTF-8\" />\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />\r\n    <title>\r\n      Tina King Invoice\r\n    </title>\r\n    <link rel=\"preconnect\" href=\"https://fonts.gstatic.com\" crossorigin />\r\n    <link\r\n      href=\"https://fonts.googleapis.com/css2?family=Inter:wght@100;200;300;400;500;600;700;800;900&display=swap\"\r\n      rel=\"stylesheet\"\r\n    />\r\n    <link rel=\"stylesheet\" href=\"assets/css/invoice.css\" />\r\n  </head>\r\n  <body>\r\n    <section class=\"wrapper-invoice\">\r\n      <!-- switch mode rtl by adding class rtl on invoice class -->\r\n      <div class=\"invoice\">\r\n        <div class=\"invoice-information\">\r\n          <p><b>Invoice #</b> : {INO}</p>\r\n          <p><b>Created Date </b>: {DATE}</p>\r\n        </div>\r\n        <!-- logo brand invoice -->\r\n        <div class=\"invoice-logo-brand\">\r\n          <!-- <h2>Tampsh.</h2> -->\r\n          <h1 style=\"color: #4567A5\">Tina King</h1>\r\n        </div>\r\n        <!-- invoice body-->\r\n        <div class=\"invoice-body\">\r\n          <table class=\"table\">\r\n            <thead>\r\n              <tr>\r\n                <th>Client</th>\r\n                <th>No</th>\r\n                <th>Start</th>\r\n                <th>Finish</th>\r\n                <th>Dead Line</th>\r\n                <th>Type</th>\r\n                <th>Work-on</th>\r\n                <th>Rate</th>\r\n                <th>Total</th>\r\n              </tr>\r\n            </thead>\r\n            <tbody>\r\n            \t{PACKAGE}\r\n            </tbody>\r\n          </table>\r\n          <div class=\"flex-table\">\r\n            <div class=\"flex-column\" style=\"width:500px\"></div>\r\n            <div class=\"flex-column\">\r\n              <table class=\"table-subtotal\">\r\n                <tbody>\r\n                  <tr>\r\n                    <td>Subtotal</td>\r\n                    <td>{TOTAL}</td>\r\n                  </tr>\r\n                  <tr>\r\n                    <td>GST # </td>\r\n                    <td>{GST}</td>\r\n                  </tr>\r\n                </tbody>\r\n              </table>\r\n            </div>\r\n          </div>\r\n          <!-- invoice total  -->\r\n          <div class=\"invoice-total-amount\">\r\n            <p>Grand Total : {GTOTAL}</p>\r\n          </div>\r\n        </div>\r\n        <!-- invoice footer -->\r\n      </div>\r\n    </section>\r\n    <div class=\"copyright\">\r\n      <p>Created by ❤ Reejv ....</p>\r\n    </div>\r\n  </body>\r\n  \r\n  <style type=\"text/css\">\r\n  \t@import \"https://fonts.googleapis.com/css2?family=Inter:wght@100;200;300;400;500;600;700;800;900&display=swap\";\r\n* {\r\n  margin: 0 auto;\r\n  padding: 0 auto;\r\n  user-select: none;\r\n}\r\n\r\nbody {\r\n  padding: 20px;\r\n  font-family: \"Inter\", -apple-system, BlinkMacSystemFont, \"Segoe UI\", Roboto, Oxygen, Ubuntu, Cantarell, \"Open Sans\", \"Helvetica Neue\", sans-serif;\r\n  -webkit-font-smoothing: antialiased;\r\n  background-color: #dcdcdc;\r\n}\r\n\r\n.wrapper-invoice {\r\n  display: flex;\r\n  justify-content: center;\r\n}\r\n.wrapper-invoice .invoice {\r\n  height: auto;\r\n  background: #fff;\r\n  padding: 35px;\r\n  margin-top: 35px;\r\n  max-width: 1024px;\r\n  width: 100%;\r\n  box-sizing: border-box;\r\n  border: 1px solid #dcdcdc;\r\n}\r\n.wrapper-invoice .invoice .invoice-information {\r\n  float: right;\r\n  text-align: right;\r\n}\r\n.wrapper-invoice .invoice .invoice-information b {\r\n  color: \"#0F172A\";\r\n}\r\n.wrapper-invoice .invoice .invoice-information p {\r\n  font-size: 14;\r\n  color: gray;\r\n}\r\n.wrapper-invoice .invoice .invoice-logo-brand h2 {\r\n  text-transform: uppercase;\r\n  font-family: \"Inter\", -apple-system, BlinkMacSystemFont, \"Segoe UI\", Roboto, Oxygen, Ubuntu, Cantarell, \"Open Sans\", \"Helvetica Neue\", sans-serif;\r\n  font-size: 20px;\r\n  color: \"#0F172A\";\r\n}\r\n.wrapper-invoice .invoice .invoice-logo-brand img {\r\n  max-width: 100px;\r\n  width: 100%;\r\n  object-fit: fill;\r\n}\r\n.wrapper-invoice .invoice .invoice-head {\r\n  display: flex;\r\n  margin-top: 24px;\r\n}\r\n.wrapper-invoice .invoice .invoice-head .head {\r\n  width: 100%;\r\n  box-sizing: border-box;\r\n}\r\n.wrapper-invoice .invoice .invoice-head .client-info {\r\n  text-align: left;\r\n}\r\n.wrapper-invoice .invoice .invoice-head .client-info h2 {\r\n  font-weight: 500;\r\n  letter-spacing: 0.3px;\r\n  font-size: 14px;\r\n  color: \"#0F172A\";\r\n}\r\n.wrapper-invoice .invoice .invoice-head .client-info p {\r\n  font-size: 14px;\r\n  color: gray;\r\n}\r\n.wrapper-invoice .invoice .invoice-head .client-data {\r\n  text-align: right;\r\n}\r\n.wrapper-invoice .invoice .invoice-head .client-data h2 {\r\n  font-weight: 500;\r\n  letter-spacing: 0.3px;\r\n  font-size: 14px;\r\n  color: \"#0F172A\";\r\n}\r\n.wrapper-invoice .invoice .invoice-head .client-data p {\r\n  font-size: 14px;\r\n  color: gray;\r\n}\r\n.wrapper-invoice .invoice .invoice-body {\r\n  margin-top: 56px;\r\n}\r\n.wrapper-invoice .invoice .invoice-body .table {\r\n  border-collapse: collapse;\r\n  width: 100%;\r\n}\r\n.wrapper-invoice .invoice .invoice-body .table thead tr th {\r\n  font-size: 14px;\r\n  border: 1px solid #dcdcdc;\r\n  text-align: left;\r\n  padding: 7px;\r\n  background-color: #eeeeee;\r\n}\r\n.wrapper-invoice .invoice .invoice-body .table tbody tr td {\r\n  font-size: 14px;\r\n  border: 1px solid #dcdcdc;\r\n  text-align: left;\r\n  padding: 7px;\r\n  background-color: #fff;\r\n}\r\n.wrapper-invoice .invoice .invoice-body .table tbody tr td:nth-child(2) {\r\n  text-align: right;\r\n}\r\n.wrapper-invoice .invoice .invoice-body .flex-table {\r\n  display: flex;\r\n}\r\n.wrapper-invoice .invoice .invoice-body .flex-table .flex-column {\r\n  width: 100%;\r\n  box-sizing: border-box;\r\n}\r\n.wrapper-invoice .invoice .invoice-body .flex-table .flex-column .table-subtotal {\r\n  border-collapse: collapse;\r\n  box-sizing: border-box;\r\n  width: 100%;\r\n  margin-top: 14px;\r\n}\r\n.wrapper-invoice .invoice .invoice-body .flex-table .flex-column .table-subtotal tbody tr td {\r\n  font-size: 14px;\r\n  border-bottom: 1px solid #dcdcdc;\r\n  text-align: left;\r\n  padding: 14px;\r\n  background-color: #fff;\r\n}\r\n.wrapper-invoice .invoice .invoice-body .flex-table .flex-column .table-subtotal tbody tr td:nth-child(2) {\r\n  text-align: right;\r\n}\r\n.wrapper-invoice .invoice .invoice-body .invoice-total-amount {\r\n  margin-top: 1rem;\r\n}\r\n.wrapper-invoice .invoice .invoice-body .invoice-total-amount p {\r\n  font-weight: bold;\r\n  color: \"#0F172A\";\r\n  text-align: right;\r\n  font-size: 14px;\r\n}\r\n.wrapper-invoice .invoice .invoice-footer {\r\n  margin-top: 28px;\r\n}\r\n.wrapper-invoice .invoice .invoice-footer p {\r\n  font-size: 13px;\r\n  color: gray;\r\n}\r\n\r\n.copyright {\r\n  margin-top: 2rem;\r\n  text-align: center;\r\n}\r\n.copyright p {\r\n  color: gray;\r\n  font-size: 13px;\r\n}\r\n\r\n@media print {\r\n  .table thead tr th {\r\n    -webkit-print-color-adjust: exact;\r\n    background-color: #eeeeee !important;\r\n  }\r\n\r\n  .copyright {\r\n    display: none;\r\n  }\r\n}\r\n.rtl {\r\n  direction: rtl;\r\n  font-family: \"Inter\", -apple-system, BlinkMacSystemFont, \"Segoe UI\", Roboto, Oxygen, Ubuntu, Cantarell, \"Open Sans\", \"Helvetica Neue\", sans-serif;\r\n}\r\n.rtl .invoice-information {\r\n  float: left !important;\r\n  text-align: left !important;\r\n}\r\n.rtl .invoice-head .client-info {\r\n  text-align: right !important;\r\n}\r\n.rtl .invoice-head .client-data {\r\n  text-align: left !important;\r\n}\r\n.rtl .invoice-body .table thead tr th {\r\n  text-align: right !important;\r\n}\r\n.rtl .invoice-body .table tbody tr td {\r\n  text-align: right !important;\r\n}\r\n.rtl .invoice-body .table tbody tr td:nth-child(2) {\r\n  text-align: left !important;\r\n}\r\n.rtl .invoice-body .flex-table .flex-column .table-subtotal tbody tr td {\r\n  text-align: right !important;\r\n}\r\n.rtl .invoice-body .flex-table .flex-column .table-subtotal tbody tr td:nth-child(2) {\r\n  text-align: left !important;\r\n}\r\n.rtl .invoice-body .invoice-total-amount p {\r\n  text-align: left !important;\r\n}\r\n\r\n/*# sourceMappingURL=invoice.css.map */\r\n  </style>\r\n</html>";
        protected static string Package = "              <tr>\r\n                <td>{CLIENT}</td>\r\n                <td>{NO}</td>\r\n                <td>{START}</td>\r\n                <td>{END}</td>\r\n                <td>{LIMIT}</td>\r\n                <td>{TYPE}</td>\r\n                <td>{SPEND}</td>\r\n                <td>{RATE}</td>\r\n                <td>{TOTAL}</td>\r\n              </tr>";
        public static string GenerateBody(InvoiceView invoiceView, List<PackageView> l)
        {
            string body = Main;
            body = body.Replace("{INO}", invoiceView.InvoiceNo);
            body = body.Replace("{DATE}", invoiceView.Regist.ToString());
/*            body = body.Replace("{CITY}", invoiceView.City);
            body = body.Replace("{COUNTRY}", invoiceView.Country);
            body = body.Replace("{PCODE}", invoiceView.PostalCode);
            body = body.Replace("{EMAIL}", invoiceView.Email);
            body = body.Replace("{BUSINESSNO}", invoiceView.Business);*/
            

            string packages = "";
            double total = 0, gst = 0, gtotal = 0;
            foreach (var package in l) {
                string t = Package;
                t = t.Replace("{CLIENT}", package.ClientName);
                t = t.Replace("{NO}", package.PackageNumber);
                t = t.Replace("{START}", package.DateSubmitted.ToString("dd MMM "));
                t = t.Replace("{END}", package.EndDate.ToString("dd MMM "));
                t = t.Replace("{LIMIT}", package.Deadline.ToString("dd MMM "));
                t = t.Replace("{TYPE}", package.TypeOfRequest);
                t = t.Replace("{SPEND}", package.SpendTime.ToString());
                t = t.Replace("{RATE}", package.Rate.ToString());
                t = t.Replace("{TOTAL}", (package.SpendTime* package.Rate).ToString());
                total += package.SpendTime * package.Rate;
                packages += t;
            }

            gst = total * 0.05;
            gtotal = total + gst;
            body = body.Replace("{PACKAGE}", packages);
            body = body.Replace("{TOTAL}", total.ToString());
            body = body.Replace("{GST}", gst.ToString());
            body = body.Replace("{GTOTAL}", gtotal.ToString());

            return body;
        }
    }
}
