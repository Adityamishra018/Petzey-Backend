using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Petzey.Appointments.Repository;

namespace Petzey.Appointments.Business
{
    class DTOMapper
    {
        public Prescription DTOToPrescription(IAppointmentRepository _repo, DTOPrescription dto)
        {
            Prescription p = _repo.GetPrescriptionById(dto.PrescriptionId);
            p.PrescriptionId = dto.PrescriptionId;
            p.BPM = dto.BPM;
            p.BreathesPerMin = dto.BreathesPerMin;
            p.Symptoms = dto.Symptoms.Count >= 1 ? dto.Symptoms.Aggregate((first, second) => first + "," + second) : "";
            p.Tests = dto.Tests.Count >= 1 ? dto.Tests.Aggregate((first, second) => first + "," + second) : "";
            p.Temp = dto.Temp;

            foreach (var med in dto.Medicines)
            {
                Medicine m = _repo.GetMedicineById(med.Id);
                if (m == null)
                {
                    p.Medicines.Add(DTOToMedicine(med));
                }
                else
                {
                    m.Dosage = med.Dosage;
                    m.Instruction = med.Instruction;
                    m.Days = med.Days;
                    m.Id = med.Id;
                    m.Name = med.Name;
                    p.Medicines.Add(m);
                }
            }
            return p;
        }

        public DTOPrescription PrescriptionToDTO(Prescription p)
        {
            var dto = new DTOPrescription();
            dto.PrescriptionId = p.PrescriptionId;
            dto.BPM = p.BPM;
            dto.BreathesPerMin = p.BreathesPerMin;
            dto.Symptoms = p.Symptoms.Split(',').ToList();
            dto.Tests = p.Tests.Split(',').ToList();
            dto.Temp = p.Temp;

            dto.Medicines = new List<DTOMedicine>();

            foreach (var med in p.Medicines)
            {
                dto.Medicines.Add(MedicineToDTO(med));
            }
            return dto;
        }

        public Medicine DTOToMedicine(DTOMedicine dto)
        {
            var m = new Medicine();
            m.Dosage = dto.Dosage;
            m.Instruction = dto.Instruction;
            m.Days = dto.Days;
            m.Id = dto.Id;
            m.Name = dto.Name;
            return m;
        }
        public DTOMedicine MedicineToDTO(Medicine m)
        {
            var dto = new DTOMedicine();
            dto.Dosage = m.Dosage;
            dto.Instruction = m.Instruction;
            dto.Days = m.Days;
            dto.Id = m.Id;
            dto.Name = m.Name;
            return dto;
        }

        public Appointment DTOToAppointment(DTOAppointment dto)
        {
            Appointment ap = new Appointment();
            ap.DoctorId = dto.DoctorId;
            ap.OwnerId = dto.OwnerId;
            ap.PetId = dto.PetId;
            ap.Issue = dto.Issue;
            ap.AppointmentId = dto.AppointmentId;
            if (dto.Status.ToLower() == "confirmed")
                ap.Status = AppointMentStatus.Confirmed;
            else if (dto.Status.ToLower() == "closed")
                ap.Status = AppointMentStatus.Closed;
            else
                ap.Status = AppointMentStatus.Cancelled;

            ap.Reason = dto.Reason;
            ap.Date = dto.Date;

            return ap;
        }

        public DTOAppointment AppointmentToDTO(Appointment ap)
        {
            DTOAppointment dto = new DTOAppointment();
            dto.DoctorId = ap.DoctorId;
            dto.OwnerId = ap.OwnerId;
            dto.PetId = ap.PetId;
            dto.AppointmentId = ap.AppointmentId;
            dto.Issue = ap.Issue;   
            if (ap.Status == AppointMentStatus.Confirmed)
                dto.Status = "Confirmed";
            else if (ap.Status == AppointMentStatus.Closed)
                dto.Status = "Closed";
            else
                dto.Status = "Cancelled";

            dto.Reason = ap.Reason;
            dto.Date = ap.Date;
            dto.PrescriptionId = ap.Prescription.PrescriptionId;
            return dto;
        }

        
    }


}
