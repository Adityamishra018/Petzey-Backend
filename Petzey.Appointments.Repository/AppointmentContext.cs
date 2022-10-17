using System.Data.Entity;

namespace Petzey.Appointments.Repository
{
    public class AppointmentContext : DbContext
    {
        public AppointmentContext() : base("name=Default") { }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Prescription> Prescriptions { get; set; }
        public virtual DbSet<Medicine> Medicines { get; set; }
    }


}
