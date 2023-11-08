using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IBS.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ModelContext context;

        public DashboardRepository(ModelContext context)
        {
            this.context = context;
        }

        public DashboardModel GetIEDDashBoardCount(int IeCd)
        {
            DashboardModel model = new();

            model.TotalCallsCount = 35;
            model.PendingCallsCount = 18;
            model.AcceptedCallsCount = 15;
            model.CancelledCallsCount = 2;
            model.UnderLabTestingCount = 40;
            model.StillUnderInspectionCount = 20;
            model.StageRejectionCount = 17;
            model.DSCExpiryDateCount = 4;
            model.NCIsuedAgainstIECount = 45;
            model.OutstandingNCCount = 22;
            model.NotRecievedCount = 19;
            model.ConsigneeCompaintCount = 6;

            return model;

        }
    }

}

