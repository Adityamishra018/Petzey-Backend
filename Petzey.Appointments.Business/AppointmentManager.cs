using System;
using System.Collections.Generic;
using System.Linq;
using Petzey.Appointments.Repository;
using System.Threading.Tasks;
using System.Net.Http;
using ZeroFormatter;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Petzey.Appointments.Business
{
    public class AppointmentManager : IAppointmentManager
    {
        private IAppointmentRepository _repo;
        private List<INotification> _notifications = new List<INotification>();
        private DTOMapper _mapper = new DTOMapper();
        public AppointmentManager(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        public AppointmentManager()
        {
            _repo = new AppointmentRepository();
            _notifications.Add(new SMSNotification());
            _notifications.Add(new EmailNotification());

        }
        public DTOAppointment AddAppointment(DTOAppointment dto)
        {
            var newapp = _repo.AddAppointment(_mapper.DTOToAppointment(dto));

            var client = new HttpClient();
            //var doc = client.GetAsync($"https://doctorflowuipetzy.azurewebsites.net/api/doctorsmailid/{newapp.DoctorId}").Result;
            var own = client.GetAsync($"https://patientwebapi20221013165438.azurewebsites.net/api/owner/email/{newapp.OwnerId}").Result;

            //var docStr = doc.Content.ReadAsStringAsync().Result.Trim('\'', '"');
            var ownStr = own.Content.ReadAsStringAsync().Result.Trim('\'', '"');
            foreach (var n in _notifications)
            {
                if (n.GetType() == typeof(EmailNotification))
                {
                    n.Send(ownStr,
                   $"Appointment Booked ! \n" +
                   $"Date and Time : {newapp.Date.ToLocalTime().ToString()} \n" +
                   $"Wish you a happy visit.");
            }
        }

            return _mapper.AppointmentToDTO(newapp);
        }

        public List<DTOPrescription> GetAllPrescriptionByPetId(long petId)
        {
            return _repo.GetAllPrescriptionByPetId(petId).Select(p => _mapper.PrescriptionToDTO(p)).ToList();
        }

        public DTOPrescription GetPrescriptionById(long presId)
        {
            return _mapper.PrescriptionToDTO(_repo.GetPrescriptionById(presId));
        }

        public List<DTOAppointment> GetAppointmentsByDate(DateTime dt)
        {
            return _repo.GetAppointmentsByDate(dt).Select(a => _mapper.AppointmentToDTO(a)).ToList();
        }

        public List<DTOAppointment> GetAppointmentsByDate(DateTime dt, long docId)
        {
            return _repo.GetAppointmentsByDate(dt, docId).Select(a => _mapper.AppointmentToDTO(a)).ToList();
        }

        public List<DTOAppointment> GetAppointmentsByDoctorId(long docId)
        {
            return _repo.GetAppointmentsByDoctorId(docId).Select(a => _mapper.AppointmentToDTO(a)).ToList();

        }

        public List<DTOAppointment> GetAppointmentsByOwnerId(long ownerId)
        {
            return _repo.GetAppointmentsByOwnerId(ownerId).Select(a => _mapper.AppointmentToDTO(a)).ToList();

        }

        public List<DTOAppointment> GetAppointmentsByPetId(long petId)
        {
            return _repo.GetAppointmentsByPetId(petId).Select(a => _mapper.AppointmentToDTO(a)).ToList();
        }

        public DTOPrescription GetRecentPrescriptionByPetId(long petId)
        {
            return _mapper.PrescriptionToDTO(_repo.GetRecentPrescriptionByPetId(petId));
        }

        public DTOPrescription UpdatePrescription(DTOPrescription p)
        {
            return _mapper.PrescriptionToDTO(_repo.UpdatePrescription(_mapper.DTOToPrescription(_repo, p)));
        }

        public bool DeleteAppointment(long id)
        {
            return _repo.DeleteAppointmentById(id);
        }

        public DTOAppointment EditAppointment(DTOAppointment dto)
        {
            return _mapper.AppointmentToDTO(_repo.EditAppointment(_mapper.DTOToAppointment(dto)));
        }

        public DTOAppointment GetAppointmentById(long id)
        {
            return _mapper.AppointmentToDTO(_repo.GetAppointmentById(id));
        }
    }
}
