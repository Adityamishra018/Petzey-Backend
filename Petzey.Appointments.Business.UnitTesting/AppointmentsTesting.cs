using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using Petzey.Appointments.Business;
using Petzey.Appointments.Repository;

namespace Petzey.Appointments.Business.UnitTesting
{
    [TestClass]
    public class AppointmentsTesting
    {
        [TestMethod]
        public void AddAppointmentSuccessTest()
        {
            var _repoMock = new Mock<IAppointmentRepository>();
            _repoMock.Setup(repo => repo.AddAppointment(It.IsAny<Appointment>())).Returns(new Appointment() { Prescription=new Prescription { PrescriptionId=1} });  
            var _mgr = new AppointmentManager(_repoMock.Object);
            var res = _mgr.AddAppointment(new DTOAppointment() { });
            Assert.IsNotNull(res);
        }

        [TestMethod,ExpectedException(typeof(NullReferenceException))]
        public void AddAppointmentFailureTest()
        {
            var _repoMock = new Mock<IAppointmentRepository>();
            _repoMock.Setup(repo => repo.AddAppointment(It.IsAny<Appointment>())).Throws(new NullReferenceException());  
            var _mgr = new AppointmentManager(_repoMock.Object);
            var res = _mgr.AddAppointment(new DTOAppointment() { });
        }

        [TestMethod]
        public void GetAppointmentByDateSuccess()
        {
            var _repoMock = new Mock<IAppointmentRepository>();
            _repoMock.Setup(repo => repo.GetAppointmentsByDate(It.IsAny<DateTime>())).Returns(new System.Collections.Generic.List<Appointment>() { new Appointment {Prescription=new Prescription { PrescriptionId=1} } });
            var _mgr = new AppointmentManager(_repoMock.Object);
            var res = _mgr.GetAppointmentsByDate(DateTime.Now);
            Assert.AreEqual(res.Count,1);
        }

        [TestMethod]
        public void GetAppointmentByPet()
        {
            var _repoMock = new Mock<IAppointmentRepository>();
            _repoMock.Setup(repo => repo.GetAppointmentsByPetId(It.IsAny<long>())).Returns(new System.Collections.Generic.List<Appointment>() { new Appointment { Prescription = new Prescription { PrescriptionId = 1 } } });
            var _mgr = new AppointmentManager(_repoMock.Object);
            var res = _mgr.GetAppointmentsByPetId(100);
            Assert.AreEqual(res.Count, 1);
        }

        [TestMethod]
        public void GetAppointmentByDoctor()
        {
            var _repoMock = new Mock<IAppointmentRepository>();
            _repoMock.Setup(repo => repo.GetAppointmentsByDoctorId(It.IsAny<long>())).Returns(new System.Collections.Generic.List<Appointment>() { new Appointment { Prescription = new Prescription { PrescriptionId = 1 } } });
            var _mgr = new AppointmentManager(_repoMock.Object);
            var res = _mgr.GetAppointmentsByDoctorId(100);
            Assert.AreEqual(res.Count, 1);
        }
    }

    [TestClass]
    public class PrescriptionsTesting
    {
        [TestMethod,ExpectedException(typeof(NullReferenceException))]
        public void UpdatePrescriptionsFailure()
        {
            var _repoMock = new Mock<IAppointmentRepository>();
            _repoMock.Setup(repo => repo.UpdatePrescription(It.IsAny<Prescription>())).Throws(new NullReferenceException());
            var _mgr = new AppointmentManager(_repoMock.Object);
            var res = _mgr.UpdatePrescription(new DTOPrescription());
        }

        [TestMethod]
        public void UpdatePrescriptionsSuccess()
        {
            var _repoMock = new Mock<IAppointmentRepository>();
            _repoMock.Setup(repo => repo.UpdatePrescription(It.IsAny<Prescription>())).Returns(new Prescription() { PrescriptionId=1 });
            _repoMock.Setup(repo => repo.GetPrescriptionById(1)).Returns(new Prescription() { PrescriptionId = 1 });
            var _mgr = new AppointmentManager(_repoMock.Object);
            var res = _mgr.UpdatePrescription(new DTOPrescription() { PrescriptionId=1});
            Assert.AreEqual(res.PrescriptionId, 1);
        }
    }
}
