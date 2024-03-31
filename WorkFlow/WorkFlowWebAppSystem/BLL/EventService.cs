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
using Microsoft.JSInterop;
using WorkFlowSystem.DAL;
using WorkFlowSystem.Entities;
using WorkFlowSystem.ViewModels;

namespace WorkFlowSystem.BLL
{

    public class EventService
    {
        private readonly WFS_2590Context? _WFS_2590Context;
        internal EventService(WFS_2590Context WFS_2590Context)
        {
            _WFS_2590Context = WFS_2590Context;
        }

        public async Task<bool> UploadEvent(
            int UserID,
            string Role,
            string Type,
            int Target,
            string Username,
            string PackageNo,
            string WPSNo
            )
        {
            EventItem newItem;

            newItem = new EventItem();

            newItem.ClientID = UserID;
            newItem.Role = Role;
            newItem.Type = Type;
            newItem.TargetID = Target;
            newItem.Regist = DateTime.Now;
            newItem.Username = Username;
            newItem.PackageNo = PackageNo;
            newItem.WPSNo = WPSNo;

            _WFS_2590Context.Events.Add(newItem);

            try {
                await _WFS_2590Context.SaveChangesAsync();
            } catch {
                return false;
            }
            return true;
        }

        public async Task<int> GetNewEventCount(int UserID, string Role, string Type)
        {
            DateTime dtStart;
            var LastEvent = _WFS_2590Context.Events
                    .Where(x => x.ClientID == UserID && x.Role == Role && x.Type=="Clear")
                    .OrderBy(x=>x.Regist)
                    .LastOrDefault();
            if (LastEvent == null) {
                dtStart = new DateTime();
            } else {
                dtStart = LastEvent.Regist;
            }

            if (Type == "") {
                return _WFS_2590Context.Events
                    .Where(x => !(x.ClientID == UserID && x.Role == Role) && x.Regist > dtStart && x.Type != "Clear")
                    .Count();
            } else {
                return _WFS_2590Context.Events
                    .Where(x => !(x.ClientID == UserID && x.Role == Role) && x.Regist > dtStart && x.Type==Type)
                    .Count();
            }
        }

        public async Task<List<EventView>> GetEventsAsync(int UserID, string Role, int maxID)
        {
            try {
                return await _WFS_2590Context.Events
                            .Where(x => !(x.ClientID == UserID && x.Role == Role) && x.ID > maxID && x.Type != "Clear")
                            .Select(x => new EventView {
                                ID = x.ID,
                                ClientID = x.ClientID,
                                TargetID = x.TargetID,
                                Role = x.Role,
                                Username = x.Username,
                                PackageNo = x.PackageNo,
                                WPSNo = x.WPSNo,
                                Regist = x.Regist,
                                Type = x.Type,
                            }).ToListAsync();
            }catch(Exception ex) {
                return new List<EventView>();
            }
        }
    }
}
