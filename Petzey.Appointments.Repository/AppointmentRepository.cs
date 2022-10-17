using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Petzey.Appointments.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        AppointmentContext _context = new AppointmentContext();
        public Appointment AddAppointment(Appointment appointment)
        {
            var res = _context.Appointments.Where(a => a.DoctorId == appointment.DoctorId).AsEnumerable().Where(a=>DateTime.Compare(appointment.Date,a.Date)==0).ToList();
            if (res.Count != 0)
                throw new ApplicationException("Cannot Create");

            Prescription p = new Prescription() { Temp = 98, BPM = 72, BreathesPerMin = 15, Symptoms = "", Tests = "" };
            appointment.Prescription = p;
            _context.Appointments.Add(appointment);
            if (_context.SaveChanges() >= 1)
                return appointment;
            throw new ApplicationException("Cannot Create");
        }
        public List<Appointment> GetAllAppointments()
        {
            return _context.Appointments.Include("Prescription").ToList();
        }

        public List<Appointment> GetAppointmentsByDate(DateTime dt)
        {
            var apps = _context.Appointments.Include("Prescription").ToList();
            var res = from a in apps
                      where a.Date.Date == dt.Date
                      select a;
            return res.ToList();
        }

        public List<Appointment> GetAppointmentsByDate(DateTime dt, long docId)
        {
            var apps = _context.Appointments.Include("Prescription").ToList();
            var res = from a in apps
                      where a.Date.Date == dt.Date && a.DoctorId == docId
                      select a;
            return res.ToList();
        }

        public List<Appointment> GetAppointmentsByDoctorId(long docId)
        {
            var apps = _context.Appointments.Include("Prescription").ToList();
            var res = from a in apps
                      where a.DoctorId == docId
                      select a;
            return res.ToList();
        }

        public List<Appointment> GetAppointmentsByOwnerId(long ownerId)
        {
            var apps = _context.Appointments.Include("Prescription").ToList();
            var res = from a in apps
                      where a.OwnerId == ownerId
                      select a;
            return res.ToList();
        }

        public List<Appointment> GetAppointmentsByPetId(long petId)
        {
            var apps = _context.Appointments.Include("Prescription").ToList();
            var res = from a in apps
                      where a.PetId == petId
                      select a;
            return res.ToList();
        }

        public Prescription UpdatePrescription(Prescription p)
        {
            _context.Entry(p).State = EntityState.Modified;
            _context.SaveChanges();
            var per = _context.Prescriptions.Find(p.PrescriptionId);
            if (per != null)
                return per;
            throw new ApplicationException("Prescription not valid");
        }

        public List<Prescription> GetAllPrescriptionByPetId(long petId)
        {
            var apps = _context.Appointments.Include("Prescription").ToList();
            var res = from a in apps
                      where a.PetId == petId
                      select a.Prescription.PrescriptionId;
            var idlist = res.ToList();

            return _context.Prescriptions.Include("Medicines").Where(p => idlist.Contains(p.PrescriptionId)).ToList();
        }

        public Prescription GetRecentPrescriptionByPetId(long petId)
        {
            var apps = _context.Appointments.Include("Prescription").ToList();
            var res = from a in apps
                      where a.PetId == petId
                      orderby a.Date
                      select a.Prescription.PrescriptionId;

            return _context.Prescriptions.Include("Medicines").Where(p => p.PrescriptionId == res.FirstOrDefault()).FirstOrDefault();
        }

        public Medicine GetMedicineById(long medId)
        {
            return _context.Medicines.Where(m => m.Id == medId).FirstOrDefault();
        }

        public List<Medicine> GetMedicinesByPrescriptionId(long prescriptionId)
        {
            return _context.Prescriptions.Include("Medicines").Where(p => p.PrescriptionId == prescriptionId).FirstOrDefault().Medicines;
        }

        public Prescription GetPrescriptionById(long Id)
        {
            return _context.Prescriptions.Where(p => p.PrescriptionId == Id).Include("Medicines").FirstOrDefault();
        }

        public bool DeleteAppointmentById(long id)
        {
            var app = _context.Appointments.Where(a => a.AppointmentId == id).Include("Prescription").FirstOrDefault();
            if(app != null)
            {
                _context.Database.ExecuteSqlCommand($"delete from medicines where medicines.Prescription_PrescriptionId = {app.Prescription.PrescriptionId}");

                _context.Entry(app.Prescription).State = EntityState.Deleted;
                _context.SaveChanges();
                _context.Entry(app).State = EntityState.Deleted;
                if(_context.SaveChanges() >=1)
                    return true;
            }

            throw new ApplicationException("Appointment not found");
        }

        public Appointment GetAppointmentById(long id)
        {
            var res = _context.Appointments.Where(a=>a.AppointmentId==id).Include("Prescription").FirstOrDefault();
            if (res != null)
                return res;
            throw new ApplicationException("Not found");
        }

        public Appointment EditAppointment(Appointment ap)
        {
            var app = _context.Appointments.Find(ap.AppointmentId);

            if (app != null)
            {
                app.Issue = ap.Issue;
                app.Reason = ap.Reason;
                app.Status = ap.Status;

                _context.Entry(app).State = EntityState.Modified;
                _context.SaveChanges();
                return _context.Appointments.Where(a=>a.AppointmentId == app.AppointmentId).Include("Prescription").First();
            }
            else
                throw new ApplicationException("Error");
        }
    }


}
