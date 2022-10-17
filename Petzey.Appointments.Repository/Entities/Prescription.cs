using System.Collections.Generic;

namespace Petzey.Appointments.Repository
{
    public class Prescription
    {
        public long PrescriptionId { get; set; }
        public short BPM { get; set; }
        public short Temp { get; set; }
        public short BreathesPerMin { get; set; }
        public string Symptoms { get; set; } = string.Empty;
        public string Tests { get; set; } = string.Empty;
        public List<Medicine> Medicines { get; set; } = new List<Medicine>();
    }


}
