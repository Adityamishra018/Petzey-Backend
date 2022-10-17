using System;
using System.Collections.Generic;

namespace Petzey.Appointments.Repository
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAllAppointments();
        Appointment GetAppointmentById(long id);
        List<Appointment> GetAppointmentsByDoctorId(long docId);
        List<Appointment> GetAppointmentsByOwnerId(long ownerId);
        List<Appointment> GetAppointmentsByPetId(long petId);
        List<Appointment> GetAppointmentsByDate(DateTime dt);
        List<Appointment> GetAppointmentsByDate(DateTime dt, long docId);
        Appointment EditAppointment(Appointment ap);
        bool DeleteAppointmentById(long id);
        Appointment AddAppointment(Appointment appointment);
        Prescription UpdatePrescription(Prescription p);
        List<Prescription> GetAllPrescriptionByPetId(long petId);
        Prescription GetRecentPrescriptionByPetId(long petId);
        Prescription GetPrescriptionById(long presId);
        Medicine GetMedicineById(long medId);
        List<Medicine> GetMedicinesByPrescriptionId(long prescriptionId);

    }


}
