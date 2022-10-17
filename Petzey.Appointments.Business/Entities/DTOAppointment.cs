using System;

namespace Petzey.Appointments.Business
{
    public class DTOAppointment
    {
        public long AppointmentId { get; set; }
        public long DoctorId { get; set; }
        public long OwnerId { get; set; }
        public long PetId { get; set; }
        public string Issue { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } = "Confirmed";
        public DateTime Date { get; set; } = DateTime.Now;
        public long PrescriptionId { get; set; }
    }


}
