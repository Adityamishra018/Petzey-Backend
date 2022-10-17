namespace Petzey.Appointments.Business
{
    public interface INotification
    {
        void Send(string addr,string msg);
    }

}
