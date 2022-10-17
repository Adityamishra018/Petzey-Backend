using System;

namespace Petzey.Appointments.Repository
{
    public class Appointment
    {
        public long AppointmentId { get; set; }
        public long DoctorId { get; set; }
        public long OwnerId { get; set; }
        public long PetId { get; set; }
        public AppointMentStatus Status { get; set; }
        public string Reason { get; set; }
        public string Issue { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public Prescription Prescription { get; set; }
    }


}
