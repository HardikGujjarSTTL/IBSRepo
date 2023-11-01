﻿using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface ICallListRepository
    {
        List<CallListModel> GetCallList();

        int SheduleInspection(SheduleInspectionRequestModel sheduleInspectionRequestModel);
    }
}
