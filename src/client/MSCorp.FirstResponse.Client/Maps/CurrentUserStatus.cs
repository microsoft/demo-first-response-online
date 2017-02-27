using System;

namespace MSCorp.FirstResponse.Client.Maps
{
    public static class CurrentUserStatus
    {
        public static bool IsNavigating { get; set; }

        public static int? AttendingIncidentId { get; set; }

        public static bool IsAttendingAnIncident { get; set; }

        public static bool CanNavigate
        {
            get
            {
                return !IsNavigating && !IsAttendingAnIncident;
            }
        }

        public static void Reset()
        {
            IsNavigating = false;
            IsAttendingAnIncident = false;
            AttendingIncidentId = null;
        }
    }
}