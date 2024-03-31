using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowSystem.DAL;
using WorkFlowSystem.Entities;
using WorkFlowSystem.ViewModels;

namespace WorkFlowSystem.Services
{
    public class DrawingRequestService 
    {
        private readonly WFS_2590Context _WFS_2590Context;
        public DrawingRequestService(WFS_2590Context WFS_2590Context)
        {
            _WFS_2590Context = WFS_2590Context;
        }
        private int GenerateNewRequestId()
        {
            int maxId = _WFS_2590Context.DrawingRequests.Max(r => (int?)r.DrawingRequestID) ?? 0;
            int newId = maxId + 1;

            return newId;
        }

        public async Task SaveDrawingRequest(DrawingRequestView drawingRequest)
        {
            int newRequestId = GenerateNewRequestId();
            var entity = new DrawingRequest
            {
                DrawingRequestID = newRequestId,
                IssuedForConstruction = drawingRequest.IssuedForConstruction,
                WeldMapComplete = drawingRequest.WeldMapComplete,
                ThicknessRangeQualifiedAcceptable = drawingRequest.ThicknessRangeQualifiedAcceptable,
                CorrectWPS = drawingRequest.CorrectWPS,
                //AdditionalComments = drawingRequest.AdditionalComments
            };

            _WFS_2590Context.DrawingRequests.Add(entity);
            await _WFS_2590Context.SaveChangesAsync();
        }
    }
}
