using Newtonsoft.Json;
using Petzey.Appointments.Business;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Petzey.Appointments.API.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AppointmentController : ApiController
    {
        IAppointmentManager _appointmentsManager;

        static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = ConfigurationManager.AppSettings["RedisCache"];
            return ConnectionMultiplexer.Connect(cacheConnection);
        });
        static IDatabase cache = lazyConnection.Value.GetDatabase();

        public AppointmentController()
        {
            _appointmentsManager = new AppointmentManager();
        }

        public AppointmentController(IAppointmentManager mgr)
        {
            _appointmentsManager = mgr;
        }

        //get domain/api/appointment/doctor/id/date/dd-mm-yyyy
        [Route("api/appointment/doctor/{did}/date/{date}")]
        public IHttpActionResult GetAppointmentByDate(long did, DateTime date)
        {
            var res = _appointmentsManager.GetAppointmentsByDate(date, did);
            if (res.Count >= 1)
                return Ok(res);
            else
                return BadRequest();
        }

        //get domain/api/appointment/date/dd-mm-yyyy
        [Route("api/appointment/date/{date}")]
        public IHttpActionResult GetAppointmentByDate(DateTime date)
        {
            var res = _appointmentsManager.GetAppointmentsByDate(date);
            if (res.Count >= 1)
                return Ok(res);
            else
                return BadRequest();
        }

        //get domain/api/appointment/doctor/id
        [Route("api/appointment/doctor/{id}")]
        public IHttpActionResult GetAppointmentByDoctor(long id)
        {
            var res = _appointmentsManager.GetAppointmentsByDoctorId(id);
            if (res.Count >= 1)
                return Ok(res);
            else
                return BadRequest();
        }

        //get domain/api/appointment/owner/id
        [Route("api/appointment/owner/{id}")]
        public IHttpActionResult GetAppointmentByOwner(long id)
        {
            var res = _appointmentsManager.GetAppointmentsByOwnerId(id);
            if (res.Count >= 1)
                return Ok(res);
            else
                return BadRequest();
        }

        //get domain/api/appointment/pet/id
        [Route("api/appointment/pet/{id}")]
        public IHttpActionResult GetAppointmentByPetId(long id)
        {
            var res = _appointmentsManager.GetAppointmentsByPetId(id);
            if (res.Count >= 1)
                return Ok(res);
            else
                return BadRequest();
        }

        //get domain/api/appointment/id
        [Route("api/appointment/{id}")]
        public IHttpActionResult GetAppointmentById(long id)
        {
            string data = (cache.StringGet($"Appointment : {id}"));
            if(data!=null)
                return Ok(JsonConvert.DeserializeObject<DTOAppointment>(data));
            try
            {
                var res = _appointmentsManager.GetAppointmentById(id);
                cache.StringSet($"Appointment : {res.AppointmentId}", JsonConvert.SerializeObject(res));
                return Ok(res);
            }
            catch
            {
                return BadRequest();
            }
           
        }

        //post domain/api/appointment
        public IHttpActionResult PostAppointment([FromBody] DTOAppointment dto)
        {
            try
            {
                return Ok(_appointmentsManager.AddAppointment(dto));
            }
            catch
            {
                return BadRequest();
            }
        }
        //put domain/api/appointment
        public IHttpActionResult PutAppointment([FromBody] DTOAppointment dto)
        {

            try
            {
                var res = _appointmentsManager.EditAppointment(dto);
                cache.StringSet($"Appointment : {dto.AppointmentId}", JsonConvert.SerializeObject(res));
                return Ok(_appointmentsManager.EditAppointment(dto));

            }
            catch
            {
                return BadRequest();
            }
        }

        //delete domain/api/appointment/id
        [Route("api/appointment/{id}")]
        public IHttpActionResult DeleteAppointment(long id)
        {
            try
            {
                return Ok(_appointmentsManager.DeleteAppointment(id));

            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
