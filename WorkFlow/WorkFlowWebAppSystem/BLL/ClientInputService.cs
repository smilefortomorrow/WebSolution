using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowSystem.DAL;
using WorkFlowSystem.Entities;
using WorkFlowSystem.ViewModels;

namespace WorkFlowSystem.BLL
{
    public class ClientInputService
    {
        private readonly WFS_2590Context? _WFS_2590Context;
        public ClientInputService(WFS_2590Context WFS_2590Context)
        {
            _WFS_2590Context = WFS_2590Context;
        }
        public async Task<bool> CreatePackageAsync(PackageView packageView)
        {
            var package = new Package
            {
                ClientID = packageView.ClientID,
                TypeOfRequest = packageView.TypeOfRequest,
                DateSubmitted = packageView.DateSubmitted,
                Deadline = packageView.Deadline,
                Priority = packageView.Priority,
                PackageNumber = packageView.PackageNumber,
                Status = packageView.Status,
                

            };

            _WFS_2590Context.Packages.Add(package);
            await _WFS_2590Context.SaveChangesAsync();
            return true;
        }
    }
}
