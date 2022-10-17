using System.Collections.Generic;

namespace Petzey.Appointments.Business
{
    public class DTOPrescription
    {
        public long PrescriptionId { get; set; }
        public short BPM { get; set; }
        public short BreathesPerMin { get; set; }
        public short Temp { get; set; }
        public List<string> Symptoms { get; set; } = new List<string>();
        public List<string> Tests { get; set; } = new List<string>();
        public List<DTOMedicine> Medicines { get; set; } = new List<DTOMedicine>();
    }


}
