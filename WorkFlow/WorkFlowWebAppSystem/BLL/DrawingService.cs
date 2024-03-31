using System;
using System.Collections.Generic;
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
    public class DrawingService
    {
        private readonly WFS_2590Context? _WFS_2590Context;
        internal DrawingService(WFS_2590Context WFS_2590Context)
        {
            _WFS_2590Context = WFS_2590Context;
        }

        public DrawingView GetByPackageID(int packageID)
        {
            var ret = _WFS_2590Context.Drawings
                .Where(x => x.PackageID == packageID)
                .Select(s => new DrawingView() {
                    ID = s.ID,
                    PackageID = s.PackageID,
                    RegistDate = s.RegistDate,
                    Detail = s.Detail,
                })
                .FirstOrDefault();
            return ret;
        }

        public bool Upload(DrawingView drawingView)
        {
            var OldDraw = _WFS_2590Context.Drawings
                            .Where(x => x.PackageID == drawingView.PackageID)
                            .FirstOrDefault();
            if (OldDraw != null) {
                //. update
                Drawing DrawUpdate = _WFS_2590Context.Drawings.Find(OldDraw.ID);
                DrawUpdate.RegistDate = DateTime.Now;
                DrawUpdate.Detail = drawingView.DetailInfo.ToString();
            } else {
                //. new
                Drawing newDraw = new Drawing();
                newDraw.PackageID = drawingView.PackageID;
                newDraw.RegistDate = DateTime.Now;
                newDraw.Detail = drawingView.DetailInfo.ToString();
                _WFS_2590Context.Drawings.Add(newDraw);
            }
            return true;
        }

        public async Task<bool> UploadAsync(DrawingView drawingView)
        {
            var OldDraw = _WFS_2590Context.Drawings
                            .Where(x => x.PackageID == drawingView.PackageID)
                            .FirstOrDefault();
            if (OldDraw != null) {
                //. update
                Drawing DrawUpdate = await _WFS_2590Context.Drawings.FindAsync(OldDraw.ID);
                DrawUpdate.RegistDate = DateTime.Now;
                DrawUpdate.Detail = drawingView.DetailInfo.ToString();
            } else {
                //. new
                Drawing newDraw = new Drawing();
                newDraw.PackageID = drawingView.PackageID;
                newDraw.RegistDate = DateTime.Now;
                newDraw.Detail = drawingView.DetailInfo.ToString();
                _WFS_2590Context.Drawings.Add(newDraw);
            }

            try {
                await _WFS_2590Context.SaveChangesAsync();
                return true;
            } catch (Exception ex) {
                return false;
            }
        }
    }
}
